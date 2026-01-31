package foi.cverglici.core.data.model.student.dailyfoodintake

data class DailyFoodIntakeResponse(
    val dailyFoodIntakeId: Int,
    val date: String,
    val dishId: Int,
    val dishTitle: String,
    val calories: Int,
    val protein: Double,
    val fat: Double,
    val carbohydrates: Double,
    val imgPath: String?
)