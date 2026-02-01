package foi.cverglici.core.data.model.student.nutritiongoal

data class TodayNutritionProgressResponse(
    val caloriesConsumed: Int,
    val proteinsConsumed: Double,
    val fatsConsumed: Double,
    val carbohydratesConsumed: Double
)
