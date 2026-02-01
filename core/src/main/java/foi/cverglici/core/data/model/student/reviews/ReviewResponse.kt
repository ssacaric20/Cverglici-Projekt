package foi.cverglici.core.data.model.student.reviews

data class ReviewResponse(
    val dishRatingId: Int,
    val rating: Int,
    val comment: String,
    val createdAt: String,
    val updatedAt: String?,
    val userId: Int,
    val userName: String
)
