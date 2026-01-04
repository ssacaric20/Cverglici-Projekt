package foi.cverglici.core.data.model.ai

data class NutritionEstimateResponse(
    val calories: Double,
    val protein: Double,
    val fat: Double,
    val carbohydrates: Double,
    val fiber: Double,
    val confidence: Double,
    val notes: String,
    val ingredients: List<IngredientEstimate>
)