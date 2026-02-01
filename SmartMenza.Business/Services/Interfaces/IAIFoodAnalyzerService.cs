using SmartMenza.Business.Models.FoodAnalysis;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface IAIFoodAnalyzerService
    {
        Task<FoodAnalysisResult> AnalyzeAsync(string text, CancellationToken ct = default);
    }
}
