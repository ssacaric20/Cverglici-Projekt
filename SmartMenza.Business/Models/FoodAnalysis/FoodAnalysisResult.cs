namespace SmartMenza.Business.Models.FoodAnalysis
{
    public sealed record FoodAnalysisResult(
        IReadOnlyList<AllergenFinding> Allergens,
        bool IsVegan,
        bool IsVegetarian,
        bool IsGlutenFree
    );

    public sealed record AllergenFinding(
        string Allergen,
        IReadOnlyList<string> Triggers
    );
}
