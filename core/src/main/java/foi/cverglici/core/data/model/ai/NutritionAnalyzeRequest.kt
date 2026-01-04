package foi.cverglici.core.data.model.ai

data class NutritionAnalyzeRequest(
    val text: String,
    val servings: Int = 1
)