using SmartMenza.Business.Models.FoodAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface IAINutritionAnalyzerService
    {
        Task<NutritionResult> AnalyzeAsync(string text, CancellationToken ct = default);
    }
}
