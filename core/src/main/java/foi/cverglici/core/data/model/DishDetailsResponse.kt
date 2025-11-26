package foi.cverglici.core.data.model

data class DishDetailsResponse(
    val dishId: Int,
    val title: String,
    val description: String,
    val price: Double,
    val calories: Int,
    val protein: Double,
    val fat: Double,
    val carbohydrates: Double,
    val imgPath: String?,
    val ingredients: List<String>,
    val averageRating: Double?,
    val ratingsCount: Int
)
