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
    public sealed class AINutritionAnalyzerService : IAINutritionAnalyzerService
    {
        private readonly ChatClient _chatClient;

        public AINutritionAnalyzerService(IOptions<AzureOpenAISettings> options)
        {
            var s = options.Value;

            if (string.IsNullOrWhiteSpace(s.Endpoint))
                throw new InvalidOperationException("AzureOpenAI:Endpoint is missing.");
            if (string.IsNullOrWhiteSpace(s.ApiKey))
                throw new InvalidOperationException("AzureOpenAI:ApiKey is missing.");
            if (string.IsNullOrWhiteSpace(s.ChatDeployment))
                throw new InvalidOperationException("AzureOpenAI:ChatDeployment is missing.");


            var azureClient = new AzureOpenAIClient(
                new Uri(s.Endpoint),
                new AzureKeyCredential(s.ApiKey)
                );

            _chatClient = azureClient.GetChatClient(s.ChatDeployment);
        }

        public async Task<NutritionResult> AnalyzeAsync(string text, CancellationToken ct = default)
        {
            text ??= "";

            var system = """
You are a nutrition estimation assistant.
Return ONLY valid JSON (no markdown, no code fences, no extra text).

Return JSON matching exactly:
{
  "macros": {
    "kcal": 0,
    "protein_g": 0,
    "carbs_g": 0,
    "fat_g": 0,
    "fiber_g": 0,
    "salt_g": 0
  },
  "estimatedServingSizeGrams": 0,
  "assumptions": ["..."]
}

Rules:
- If no concrete numbers are provided, use average portions.
- Make reasonable portion assumptions and list them in assumptions.
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

            var result = JsonSerializer.Deserialize<NutritionResult>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (result is null)
                throw new InvalidOperationException("Azure OpenAI response parsed to null NutritionResult.");

            return new NutritionResult(
                Macros: result.Macros ?? new NutritionMacros(null, null, null, null, null, null),
                EstimatedServingSizeGrams: result.EstimatedServingSizeGrams,
                Assumptions: result.Assumptions ?? Array.Empty<string>()
            );
        }
    }
}
