package foi.cverglici.smartmenza.ui.student.menu

import android.app.Dialog
import android.content.Context
import android.os.Bundle
import android.view.Window
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.FragmentActivity
import androidx.fragment.app.FragmentManager
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.lifecycleScope
import com.google.android.material.button.MaterialButton
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import foi.cverglici.core.data.api.student.dailymenu.IDishService
import foi.cverglici.core.data.api.student.dailymenu.RetrofitDish
import foi.cverglici.core.data.model.student.dailymenu.DishDetailsResponse
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionTokenProvider
import foi.cverglici.smartmenza.ui.student.favorites.FavoriteManager
import kotlinx.coroutines.launch
import android.view.View
import android.widget.EditText
import android.widget.RatingBar
import androidx.recyclerview.widget.RecyclerView
import foi.cverglici.smartmenza.ui.student.reviews.ReviewRepository
import foi.cverglici.smartmenza.ui.student.reviews.RetrofitReviewRepository
import foi.cverglici.smartmenza.ui.student.reviews.ReviewsController
import foi.cverglici.core.data.api.student.reviews.RetrofitReview


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

    private lateinit var reviewsController: ReviewsController


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

        initializeViews()
        setupClickListeners()
        loadDishDetails()

        // REVIEWS (API)
        val currentUserId = 1 // privremeno dok se ne dobije userId iz sessiona
        val reviewService = RetrofitReview.create(tokenProvider)

        val repo: ReviewRepository = RetrofitReviewRepository(
            service = reviewService,
            currentUserId = currentUserId
        )

        reviewsController = ReviewsController(
            context = context,
            lifecycleOwner = lifecycleOwner,
            dishId = dishId,
            repo = repo,
            reviewsRecyclerView = reviewsRecyclerView,
            reviewsEmptyState = reviewsEmptyState,
            reviewFormContainer = reviewFormContainer,
            ratingBar = ratingBar,
            etReviewText = etReviewText,
            btnWriteReview = btnWriteReview,
            btnSubmitReview = btnSubmitReview,
            btnCancelReview = btnCancelReview
        )
        reviewsController.init()

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
                e.printStackTrace()
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
}