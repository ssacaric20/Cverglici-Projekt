package foi.cverglici.core.data.model.student.nutritiongoal

data class NutritionGoalResponse(
    val caloriesGoal: Int,
    val proteinsGoal: Double,
    val fatsGoal: Double,
    val carbohydratesGoal: Double,
    val goalSetDate: String
)
