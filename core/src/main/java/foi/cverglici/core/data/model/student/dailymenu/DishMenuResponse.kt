package foi.cverglici.core.data.model.student.dailymenu

data class DishMenuResponse(
    val dishId: Int,
    val title: String,
    val description: String,
    val price: Double,
    val calories: Int,
    val protein: Double,
    val fat: Double,
    val carbohydrates: Double,
    val fiber: Double,
    val imgPath: String?,
    val ingredients: List<String>
)
