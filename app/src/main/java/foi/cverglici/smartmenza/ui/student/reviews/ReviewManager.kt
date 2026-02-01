package foi.cverglici.smartmenza.ui.student.reviews

import android.content.Context
import android.util.Log
import android.widget.Toast
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.lifecycleScope
import foi.cverglici.core.data.api.student.reviews.IReviewService
import foi.cverglici.core.data.model.student.reviews.CreateReviewRequest
import foi.cverglici.core.data.model.student.reviews.ReviewResponse
import foi.cverglici.core.data.model.student.reviews.UpdateReviewRequest
import kotlinx.coroutines.launch
import retrofit2.Response

class ReviewManager(
    private val context: Context,
    private val lifecycleOwner: LifecycleOwner,
    private val service: IReviewService
) {

    fun loadReviews(
        dishId: Int,
        onSuccess: (List<ReviewResponse>) -> Unit,
        onError: (String) -> Unit = {}
    ) {
        lifecycleOwner.lifecycleScope.launch {
            val result = safeApiCall(
                call = { service.getDishReviews(dishId) },
                defaultError = "Ne mogu dohvatiti recenzije."
            )

            result.onSuccess(onSuccess).onFailure {
                val msg = it.message ?: "Ne mogu dohvatiti recenzije."
                onError(msg)
                toast(msg)
            }
        }
    }

    fun createReview(
        dishId: Int,
        rating: Int,
        comment: String,
        onSuccess: () -> Unit,
        onError: (String) -> Unit = {}
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                Log.e("REVIEWS", "createReview dishId=$dishId rating=$rating commentLen=${comment.length}")

                val response = service.createReview(dishId, CreateReviewRequest(rating, comment))

                if (response.isSuccessful) {
                    Log.e("REVIEWS", "createReview SUCCESS HTTP ${response.code()} body=${response.body()}")
                    toast("Recenzija spremljena.")
                    onSuccess()
                } else {
                    val err = runCatching { response.errorBody()?.string() }.getOrNull()
                    val msg = "createReview FAILED HTTP ${response.code()} err=${err ?: "null"}"
                    Log.e("REVIEWS", msg)
                    toast(msg)
                    onError(msg)
                }
            } catch (e: Exception) {
                val msg = "createReview EXCEPTION: ${e.message}"
                Log.e("REVIEWS", msg, e)
                toast(msg)
                onError(msg)
            }
        }
    }



    fun updateReview(
        dishId: Int,
        reviewId: Int, // <-- ReviewResponse.dishRatingId
        rating: Int,
        comment: String,
        onSuccess: (ReviewResponse) -> Unit,
        onError: (String) -> Unit = {}
    ) {
        lifecycleOwner.lifecycleScope.launch {
            val body = UpdateReviewRequest(rating = rating, comment = comment)

            val result = safeApiCall(
                call = { service.updateReview(dishId, reviewId, body) },
                defaultError = "Ne mogu ažurirati recenziju."
            )

            result.onSuccess {
                toast("Recenzija ažurirana.")
                onSuccess(it)
            }.onFailure {
                val msg = it.message ?: "Ne mogu ažurirati recenziju."
                onError(msg)
                toast(msg)
            }
        }
    }

    fun deleteReview(
        dishId: Int,
        reviewId: Int,
        onSuccess: () -> Unit,
        onError: (String) -> Unit = {}
    ) {
        lifecycleOwner.lifecycleScope.launch {
            val result = safeApiCall(
                call = { service.deleteReview(dishId, reviewId) },
                defaultError = "Ne mogu obrisati recenziju."
            )

            result.onSuccess {
                toast("Recenzija obrisana.")
                onSuccess()
            }.onFailure {
                val msg = it.message ?: "Ne mogu obrisati recenziju."
                onError(msg)
                toast(msg)
            }
        }
    }

    // -------------------------
    // helpers
    // -------------------------

    private suspend fun <T> safeApiCall(
        call: suspend () -> Response<T>,
        defaultError: String
    ): Result<T> {
        return try {
            val response = call()
            if (response.isSuccessful) {
                val body = response.body()
                if (body != null) Result.success(body)
                else Result.failure(IllegalStateException("Prazan odgovor servera."))
            } else {
                val errText = runCatching { response.errorBody()?.string() }.getOrNull()
                val msg = buildString {
                    append(defaultError)
                    append(" (HTTP ${response.code()})")
                    if (!errText.isNullOrBlank()) append(": ").append(errText)
                }
                Result.failure(IllegalStateException(msg))
            }
        } catch (e: Exception) {
            Result.failure(IllegalStateException("$defaultError (${e.message})", e))
        }
    }

    private fun toast(message: String) {
        Toast.makeText(context, message, Toast.LENGTH_LONG).show()
    }
}
