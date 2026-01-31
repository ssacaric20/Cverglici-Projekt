package foi.cverglici.core.data.model.student.nutritiongoal

data class SetNutritionGoalRequest(
    val caloriesGoal: Int,
    val proteinsGoal: Double,
    val fatsGoal: Double,
    val carbohydratesGoal: Double
)
