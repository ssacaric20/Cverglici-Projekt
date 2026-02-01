package foi.cverglici.smartmenza.ui.student.menu

import android.app.Dialog
import android.content.Context
import android.os.Bundle
import android.view.View
import android.view.Window
import android.widget.EditText
import android.widget.ImageView
import android.widget.RatingBar
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.FragmentActivity
import androidx.fragment.app.FragmentManager
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.lifecycleScope
import com.google.android.material.button.MaterialButton
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import foi.cverglici.core.data.api.student.dailymenu.IDishService
import foi.cverglici.core.data.api.student.dailymenu.RetrofitDish
import foi.cverglici.core.data.api.student.reviews.RetrofitReview
import foi.cverglici.core.data.model.student.dailymenu.DishDetailsResponse
import foi.cverglici.core.data.model.student.reviews.ReviewResponse
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionTokenProvider
import foi.cverglici.smartmenza.ui.student.favorites.FavoriteManager
import foi.cverglici.smartmenza.ui.student.reviews.ReviewManager
import foi.cverglici.smartmenza.ui.student.reviews.ReviewsAdapter
import kotlinx.coroutines.launch

class DishDetailDialog(
    context: Context,
    private val lifecycleOwner: LifecycleOwner,
    private val fragmentManager: FragmentManager,
    private val dishId: Int
) : Dialog(context) {

    constructor(
        context: Context,
        lifecycleOwner: LifecycleOwner,
        dishId: Int
    ) : this(
        context = context,
        lifecycleOwner = lifecycleOwner,
        fragmentManager = (context as? FragmentActivity)?.supportFragmentManager
            ?: throw IllegalArgumentException("DishDetailDialog requires a FragmentActivity context."),
        dishId = dishId
    )

    private lateinit var closeButton: ImageView
    private lateinit var dishTitle: TextView
    private lateinit var dishImage: ImageView
    private lateinit var favoriteIcon: ImageView
    private lateinit var dishDescription: TextView
    private lateinit var dishPrice: TextView
    private lateinit var caloriesValue: TextView
    private lateinit var carbsValue: TextView
    private lateinit var fiberValue: TextView
    private lateinit var fatValue: TextView
    private lateinit var proteinValue: TextView
    private lateinit var ingredientsChipGroup: ChipGroup
    private lateinit var averageRating: TextView
    private lateinit var ratingCount: TextView
    private lateinit var aiAnalyzeButton: MaterialButton

    private lateinit var favoriteManager: FavoriteManager
    private lateinit var menuService: IDishService
    private var aiTextToAnalyze: String = ""

    private lateinit var reviewsRecyclerView: RecyclerView
    private lateinit var reviewsEmptyState: TextView

    private lateinit var reviewFormContainer: View
    private lateinit var ratingBar: RatingBar
    private lateinit var etReviewText: EditText
    private lateinit var btnWriteReview: View
    private lateinit var btnSubmitReview: View
    private lateinit var btnCancelReview: View

    // reviews
    private lateinit var reviewManager: ReviewManager
    private lateinit var reviewsAdapter: ReviewsAdapter

    // state for edit
    private var editingReviewId: Int? = null

    // TODO: zamijeni sa stvarnim userId iz sessiona
    private val currentUserId: Int = 1

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        requestWindowFeature(Window.FEATURE_NO_TITLE)
        setContentView(R.layout.dish_detail_fragment)

        window?.setLayout(
            (context.resources.displayMetrics.widthPixels * 1),
            (context.resources.displayMetrics.heightPixels * 0.85).toInt()
        )
        window?.setBackgroundDrawableResource(android.R.color.transparent)

        val tokenProvider = SessionTokenProvider(context)
        menuService = RetrofitDish.create(tokenProvider)

        // review service + manager
        val reviewService = RetrofitReview.create(tokenProvider)
        reviewManager = ReviewManager(context, lifecycleOwner, reviewService)

        initializeViews()
        setupClickListeners()

        setupReviewsUi()

        loadDishDetails()
        loadReviews()

        favoriteManager = FavoriteManager(context, lifecycleOwner, favoriteIcon, dishId)
        favoriteManager.initialize()
    }

    private fun initializeViews() {
        closeButton = findViewById(R.id.closeButton)
        dishTitle = findViewById(R.id.dishTitle)
        dishImage = findViewById(R.id.dishImage)
        favoriteIcon = findViewById(R.id.favoriteIcon)
        dishDescription = findViewById(R.id.dishDescription)
        dishPrice = findViewById(R.id.dishPrice)
        caloriesValue = findViewById(R.id.caloriesValue)
        carbsValue = findViewById(R.id.carbsValue)
        fiberValue = findViewById(R.id.fiberValue)
        fatValue = findViewById(R.id.fatValue)
        proteinValue = findViewById(R.id.proteinValue)
        ingredientsChipGroup = findViewById(R.id.ingredientsChipGroup)
        averageRating = findViewById(R.id.averageRating)
        ratingCount = findViewById(R.id.ratingsCount)
        aiAnalyzeButton = findViewById(R.id.aiAnalyzeButton)
        reviewsRecyclerView = findViewById(R.id.reviewsRecyclerView)
        reviewsEmptyState = findViewById(R.id.reviewsEmptyState)
        reviewFormContainer = findViewById(R.id.reviewFormContainer)
        ratingBar = findViewById(R.id.ratingBar)
        etReviewText = findViewById(R.id.etReviewText)
        btnWriteReview = findViewById(R.id.btnWriteReview)
        btnSubmitReview = findViewById(R.id.btnSubmitReview)
        btnCancelReview = findViewById(R.id.btnCancelReview)
    }

    private fun setupClickListeners() {
        closeButton.setOnClickListener { dismiss() }

        aiAnalyzeButton.setOnClickListener {
            if (aiTextToAnalyze.isBlank()) {
                Toast.makeText(context, "Pričekajte da se učitaju podaci o jelu.", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            val sheet = AiAnalysisBottomSheetFragment.newInstance(aiTextToAnalyze)
            sheet.show(fragmentManager, "AiAnalysisBottomSheet")
        }
    }

    private fun loadDishDetails() {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = menuService.getDishDetails(dishId)

                if (response.isSuccessful) {
                    response.body()?.let { dish ->
                        displayDishDetails(dish)
                    }
                } else {
                    val err = response.errorBody()?.string()
                    Toast.makeText(context, "Dish HTTP ${response.code()} $err", Toast.LENGTH_LONG).show()
                    dismiss()
                }

            } catch (e: Exception) {
                Toast.makeText(context, "Dish error: ${e.message}", Toast.LENGTH_LONG).show()
                dismiss()
            }
        }
    }

    private fun displayDishDetails(dish: DishDetailsResponse) {
        dishTitle.text = dish.title
        dishDescription.text = dish.description
        dishPrice.text = context.getString(R.string.price_format, dish.price)

        caloriesValue.text = context.getString(R.string.calories_detail, dish.calories)

        carbsValue.text = context.getString(R.string.grams_format, dish.carbohydrates)
        fiberValue.text = context.getString(R.string.grams_format, dish.fiber)
        fatValue.text = context.getString(R.string.grams_format, dish.fat)
        proteinValue.text = context.getString(R.string.grams_format, dish.protein)

        averageRating.text = context.getString(R.string.average_rating_format, dish.averageRating)
        ratingCount.text = context.getString(R.string.reviews_total_format, dish.ratingsCount)

        dishImage.setImageResource(R.drawable.ic_restaurant)

        ingredientsChipGroup.removeAllViews()
        dish.ingredients.forEach { ingredient ->
            val chip = Chip(context).apply {
                text = ingredient
                isClickable = false
                isCheckable = false
            }
            ingredientsChipGroup.addView(chip)
        }

        val ingredientsText = if (dish.ingredients.isNullOrEmpty()) "" else dish.ingredients.joinToString(", ")
        aiTextToAnalyze = buildString {
            appendLine(dish.title)
            if (!dish.description.isNullOrBlank()) appendLine(dish.description)
            if (ingredientsText.isNotBlank()) appendLine("Ingredients: $ingredientsText")
        }
    }

    // -------------------------
    // Reviews UI + logic
    // -------------------------

    private fun setupReviewsUi() {
        reviewsAdapter = ReviewsAdapter(
            currentUserId = currentUserId,
            onEdit = { review -> openEditReview(review) },
            onDelete = { review -> deleteReview(review) }
        )

        reviewsRecyclerView.layoutManager = LinearLayoutManager(context)
        reviewsRecyclerView.adapter = reviewsAdapter

        hideReviewForm()

        btnWriteReview.setOnClickListener {
            openCreateReview()
        }

        btnCancelReview.setOnClickListener {
            hideReviewForm()
        }

        btnSubmitReview.setOnClickListener {
            submitReview()
        }
    }

    private fun loadReviews() {
        reviewManager.loadReviews(
            dishId = dishId,
            onSuccess = { list ->
                val sorted = list.sortedByDescending { it.createdAt }
                reviewsAdapter.submit(sorted)

                val isEmpty = sorted.isEmpty()
                reviewsEmptyState.visibility = if (isEmpty) View.VISIBLE else View.GONE
                reviewsRecyclerView.visibility = if (isEmpty) View.GONE else View.VISIBLE
            }
        )
    }

    private fun openCreateReview() {
        editingReviewId = null
        ratingBar.rating = 0f
        etReviewText.setText("")
        showReviewForm()
    }

    private fun openEditReview(review: ReviewResponse) {
        editingReviewId = review.dishRatingId
        ratingBar.rating = review.rating.toFloat()
        etReviewText.setText(review.comment)
        showReviewForm()
    }

    private fun deleteReview(review: ReviewResponse) {
        reviewManager.deleteReview(
            dishId = dishId,
            reviewId = review.dishRatingId,
            onSuccess = { loadReviews() }
        )
    }

    private fun submitReview() {
        val rating = ratingBar.rating.toInt()
        val comment = etReviewText.text?.toString()?.trim().orEmpty()

        if (rating <= 0) {
            Toast.makeText(context, "Odaberi ocjenu.", Toast.LENGTH_LONG).show()
            return
        }
        if (comment.isBlank()) {
            Toast.makeText(context, "Upiši komentar.", Toast.LENGTH_LONG).show()
            return
        }

        val editId = editingReviewId
        if (editId == null) {
            reviewManager.createReview(
                dishId,
                rating,
                comment,
                onSuccess = {
                    hideReviewForm()
                    loadReviews()
                }
            )
        } else {
            reviewManager.updateReview(
                dishId = dishId,
                reviewId = editId,
                rating = rating,
                comment = comment,
                onSuccess = {
                    hideReviewForm()
                    loadReviews()
                }
            )
        }
    }

    private fun showReviewForm() {
        reviewFormContainer.visibility = View.VISIBLE
        btnWriteReview.visibility = View.GONE
    }

    private fun hideReviewForm() {
        reviewFormContainer.visibility = View.GONE
        btnWriteReview.visibility = View.VISIBLE
        editingReviewId = null
        ratingBar.rating = 0f
        etReviewText.setText("")
    }
}
