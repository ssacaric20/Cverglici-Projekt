package foi.cverglici.core.data.model.ai

data class FoodAnalysisResult(
    val allergens: List<AllergenFinding>,
    val isVegan: Boolean,
    val isVegetarian: Boolean,
    val isGlutenFree: Boolean
)