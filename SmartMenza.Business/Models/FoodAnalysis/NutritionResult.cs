using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenza.Business.Models.FoodAnalysis
{
    public sealed record NutritionResult(
        NutritionMacros Macros,
        double? EstimatedServingSizeGrams,
        IReadOnlyList<string> Assumptions
    );

    public sealed record NutritionMacros(
        double? Kcal,
        double? Protein_g,
        double? Carbs_g,
        double? Fat_g,
        double? Fiber_g,
        double? Salt_g
    );
}
