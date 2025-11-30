package foi.cverglici.core.menu.model

data class Dish(
    val dishId: Int,
    val title: String,
    val description: String?,
    val price: Double,
    val calories: Int,
    val protein: Double,
    val fat: Double,
    val carbohydrates: Double,
    val imgPath: String?
)
