using SmartMenza.Business.Models.FoodAnalysis;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface IFoodAnalyzer
    {
        Task<FoodAnalysisResult> AnalyzeAsync(string text, CancellationToken ct = default);
    }
}
