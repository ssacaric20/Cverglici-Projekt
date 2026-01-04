package foi.cverglici.core.data.model.ai

data class IngredientEstimate(
    val ingredient: String,
    val estimatedGrams: Double,
    val calories: Double,
    val protein: Double,
    val fat: Double,
    val carbohydrates: Double,
    val fiber: Double
)