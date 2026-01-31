using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using SmartMenza.Business.Models.FoodAnalysis;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Core.Settings;
using System.Text.Json;

namespace SmartMenza.Business.Services
{
    public sealed class AzureOpenAIFoodAnalyzer : IFoodAnalyzer
    {
        private readonly AzureOpenAISettings _settings;
        private readonly AzureOpenAIClient _azureClient;
        private readonly ChatClient _chatClient;

        public AzureOpenAIFoodAnalyzer(IOptions<AzureOpenAISettings> options)
        {
            _settings = options.Value;

            if (string.IsNullOrWhiteSpace(_settings.Endpoint))
                throw new InvalidOperationException("AzureOpenAI:Endpoint is missing.");

            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                throw new InvalidOperationException("AzureOpenAI:ApiKey is missing.");

            if (string.IsNullOrWhiteSpace(_settings.ChatDeployment))
                throw new InvalidOperationException("AzureOpenAI:ChatDeployment is missing.");

            _azureClient = new AzureOpenAIClient(
                new Uri(_settings.Endpoint),
                new AzureKeyCredential(_settings.ApiKey));

            _chatClient = _azureClient.GetChatClient(_settings.ChatDeployment);
        }

        public async Task<FoodAnalysisResult> AnalyzeAsync(string text, CancellationToken ct = default)
        {
            text ??= "";

            var system = """
You are a food allergen classifier.
Return ONLY valid JSON (no markdown, no code fences, no extra text).

You MUST return JSON matching this exact shape:
{
  "allergens": [
    { "allergen": "Cereals containing gluten", "triggers": ["wheat", "breadcrumbs"] }
  ],
  "isVegan": true,
  "isVegetarian": true,
  "isGlutenFree": false
}

Rules:
- Use EU allergen categories when applicable:
  Cereals containing gluten, Crustaceans, Eggs, Fish, Peanuts, Soybeans, Milk, Nuts, Celery,
  Mustard, Sesame, Sulphur dioxide and sulphites, Lupin, Molluscs.
- If allergen is not present, do not include it in the allergens list.
- Triggers must be words/phrases you used as evidence from the input text.
- If uncertain, choose the safest answer (e.g., vegan/vegetarian/glutenFree = false when ambiguous).
""";

            var user = $"""
Dish description:
"{text}"
""";

            List<ChatMessage> messages = new()
            {
                new SystemChatMessage(system),
                new UserChatMessage(user),
            };

            var requestOptions = new ChatCompletionOptions
            {
                Temperature = 0.2f
            };

            var response = await _chatClient.CompleteChatAsync(messages, requestOptions, ct);

            var content = response.Value.Content.Count > 0
                ? response.Value.Content[0].Text?.Trim()
                : null;

            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("Azure OpenAI returned an empty response.");

            FoodAnalysisResult? result;
            try
            {
                result = JsonSerializer.Deserialize<FoodAnalysisResult>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            } catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    "Azure OpenAI returned invalid JSON that could not be parsed into FoodAnalysisResult.",
                    ex
                );
            }

            if (result is null)
                throw new InvalidOperationException("Azure OpenAI response parsed to null FoodAnalysisResult.");

            return new FoodAnalysisResult(
                Allergens: result.Allergens ?? Array.Empty<AllergenFinding>(),
                IsVegan: result.IsVegan,
                IsVegetarian: result.IsVegetarian,
                IsGlutenFree: result.IsGlutenFree
            );
        }
    }
}
