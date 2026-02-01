package foi.cverglici.smartmenza.ui.student.reviews

import foi.cverglici.core.data.api.student.reviews.IReviewService
import foi.cverglici.core.data.model.student.reviews.CreateReviewRequest
import foi.cverglici.core.data.model.student.reviews.UpdateReviewRequest

class RetrofitReviewRepository(
    private val service: IReviewService,
    private val currentUserId: Int
) : ReviewRepository {

    override suspend fun getForDish(dishId: Int): List<ReviewUi> {
        val resp = service.getDishReviews(dishId)
        if (!resp.isSuccessful) {
            val err = resp.errorBody()?.string()
            throw Exception("HTTP ${resp.code()} $err")
        }


        return resp.body().orEmpty().map { r ->
            ReviewUi(
                id = r.dishRatingId,
                userDisplayName = r.userName,
                userId = r.userId,
                rating = r.rating,
                text = r.comment,
                dateIso = r.createdAt.take(10),
                isMine = (r.userId == currentUserId)
            )
        }
    }

    override suspend fun create(dishId: Int, rating: Int, comment: String): ReviewUi {
        val resp = service.createReview(dishId, CreateReviewRequest(rating, comment))
        if (!resp.isSuccessful) throw Exception("HTTP ${resp.code()}")

        val r = resp.body() ?: throw Exception("Empty body")
        return ReviewUi(
            id = r.dishRatingId,
            userDisplayName = r.userName,
            userId = r.userId,
            rating = r.rating,
            text = r.comment,
            dateIso = r.createdAt.take(10),
            isMine = (r.userId == currentUserId)
        )
    }

    override suspend fun update(dishId: Int, reviewId: Int, rating: Int, comment: String): ReviewUi {
        val resp = service.updateReview(dishId, reviewId, UpdateReviewRequest(rating, comment))
        if (!resp.isSuccessful) throw Exception("HTTP ${resp.code()}")

        val r = resp.body() ?: throw Exception("Empty body")
        return ReviewUi(
            id = r.dishRatingId,
            userDisplayName = r.userName,
            userId = r.userId,
            rating = r.rating,
            text = r.comment,
            dateIso = r.createdAt.take(10),
            isMine = (r.userId == currentUserId)
        )
    }

    override suspend fun delete(dishId: Int, reviewId: Int) {
        val resp = service.deleteReview(dishId, reviewId)
        if (!resp.isSuccessful) throw Exception("HTTP ${resp.code()}")
    }
}
