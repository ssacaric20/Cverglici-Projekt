package foi.cverglici.smartmenza.ui.student.reviews

interface ReviewRepository {
    suspend fun getForDish(dishId: Int): List<ReviewUi>
    suspend fun create(dishId: Int, rating: Int, comment: String): ReviewUi
    suspend fun update(dishId: Int, reviewId: Int, rating: Int, comment: String): ReviewUi
    suspend fun delete(dishId: Int, reviewId: Int)
}
