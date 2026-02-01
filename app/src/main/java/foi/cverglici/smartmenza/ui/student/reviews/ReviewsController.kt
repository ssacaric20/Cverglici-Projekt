package foi.cverglici.smartmenza.ui.student.reviews

import android.app.AlertDialog
import android.content.Context
import android.view.View
import android.widget.EditText
import android.widget.RatingBar
import android.widget.TextView
import android.widget.Toast
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import foi.cverglici.smartmenza.R
import kotlinx.coroutines.launch

class ReviewsController(
    private val context: Context,
    private val lifecycleOwner: LifecycleOwner,
    private val dishId: Int,
    private val repo: ReviewRepository,

    private val reviewsRecyclerView: RecyclerView,
    private val reviewsEmptyState: TextView,

    private val reviewFormContainer: View,
    private val ratingBar: RatingBar,
    private val etReviewText: EditText,
    private val btnWriteReview: View,
    private val btnSubmitReview: View,
    private val btnCancelReview: View
) {

    private val adapter = ReviewsAdapter(
        onEdit = { startEdit(it) },
        onDelete = { confirmDelete(it) }
    )

    private var editingReviewId: Int? = null

    fun init() {
        reviewsRecyclerView.layoutManager = LinearLayoutManager(context)
        reviewsRecyclerView.adapter = adapter

        btnWriteReview.setOnClickListener { showCreateForm() }
        btnCancelReview.setOnClickListener { hideForm() }
        btnSubmitReview.setOnClickListener { submit() }

        load()
    }

    private fun load() {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val list = repo.getForDish(dishId)
                adapter.submit(list)
                reviewsEmptyState.visibility = if (list.isEmpty()) View.VISIBLE else View.GONE
            } catch (e: Exception) {
                e.printStackTrace()
                Toast.makeText(context, "Reviews error: ${e.message}", Toast.LENGTH_LONG).show()
            }

        }
    }

    private fun showCreateForm() {
        editingReviewId = null
        ratingBar.rating = 0f
        etReviewText.setText("")
        reviewFormContainer.visibility = View.VISIBLE
    }

    private fun startEdit(review: ReviewUi) {
        editingReviewId = review.id
        ratingBar.rating = review.rating.toFloat()
        etReviewText.setText(review.text)
        reviewFormContainer.visibility = View.VISIBLE
    }

    private fun hideForm() {
        editingReviewId = null
        ratingBar.rating = 0f
        etReviewText.setText("")
        reviewFormContainer.visibility = View.GONE
    }

    private fun submit() {
        val rating = ratingBar.rating.toInt()
        val text = etReviewText.text?.toString()?.trim().orEmpty()

        if (rating <= 0) {
            Toast.makeText(context, context.getString(R.string.rating_required), Toast.LENGTH_SHORT).show()
            return
        }
        if (text.length < 3) {
            Toast.makeText(context, context.getString(R.string.review_too_short), Toast.LENGTH_SHORT).show()
            return
        }

        lifecycleOwner.lifecycleScope.launch {
            try {
                val id = editingReviewId
                if (id == null) {
                    repo.create(dishId, rating, text)
                } else {
                    repo.update(dishId, id, rating, text)
                }
                hideForm()
                load()
            } catch (e: Exception) {
                Toast.makeText(context, context.getString(R.string.error_saving_review), Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun confirmDelete(review: ReviewUi) {
        AlertDialog.Builder(context)
            .setTitle(R.string.delete_review_title)
            .setMessage(R.string.delete_review_message)
            .setPositiveButton(R.string.delete) { _, _ -> delete(review.id) }
            .setNegativeButton(android.R.string.cancel, null)
            .show()
    }

    private fun delete(reviewId: Int) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                repo.delete(dishId, reviewId)
                load()
            } catch (e: Exception) {
                Toast.makeText(context, context.getString(R.string.error_deleting_review), Toast.LENGTH_SHORT).show()
            }
        }
    }
}
