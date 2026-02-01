package foi.cverglici.core.data.api.student.reviews

import foi.cverglici.core.data.model.student.reviews.CreateReviewRequest
import foi.cverglici.core.data.model.student.reviews.DeleteReviewResponse
import foi.cverglici.core.data.model.student.reviews.ReviewResponse
import foi.cverglici.core.data.model.student.reviews.UpdateReviewRequest
import retrofit2.Response
import retrofit2.http.*

interface IReviewService {

    @GET("api/dishes/{dishId}/reviews")
    suspend fun getDishReviews(
        @Path("dishId") dishId: Int
    ): Response<List<ReviewResponse>>

    @POST("api/dishes/{dishId}/reviews")
    suspend fun createReview(
        @Path("dishId") dishId: Int,
        @Body body: CreateReviewRequest
    ): Response<ReviewResponse> // POST vraća objekt

    @PUT("api/dishes/{dishId}/reviews/{reviewId}")
    suspend fun updateReview(
        @Path("dishId") dishId: Int,
        @Path("reviewId") reviewId: Int,
        @Body body: UpdateReviewRequest
    ): Response<ReviewResponse> // PUT vraća objekt

    @DELETE("api/dishes/{dishId}/reviews/{reviewId}")
    suspend fun deleteReview(
        @Path("dishId") dishId: Int,
        @Path("reviewId") reviewId: Int
    ): Response<DeleteReviewResponse> // DELETE vraća message
}
