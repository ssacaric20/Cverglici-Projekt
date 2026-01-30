package foi.cverglici.core.data.model.ai

data class NutritionEstimateResponse(
    val macros: NutritionMacros,
    val estimatedServingSizeGrams: Double? = null,
    val assumptions: List<String> = emptyList()
)

data class NutritionMacros(
    val kcal: Double? = null,
    val protein_g: Double? = null,
    val carbs_g: Double? = null,
    val fat_g: Double? = null,
    val fiber_g: Double? = null,
    val salt_g: Double? = null
)
