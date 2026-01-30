package foi.cverglici.smartmenza.ui.student.menu

import android.app.Dialog
import android.content.Context
import android.os.Bundle
import android.view.View
import android.view.Window
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.lifecycleScope
import com.google.android.material.button.MaterialButton
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import foi.cverglici.core.data.api.student.dailymenu.RetrofitDish
import foi.cverglici.core.data.model.student.dailymenu.DishDetailsResponse
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.ui.student.favorites.FavoriteManager
import kotlinx.coroutines.launch

class DishDetailDialog(
    context: Context,
    private val lifecycleOwner: LifecycleOwner,
    private val dishId: Int
) : Dialog(context) {

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

    private lateinit var writeReviewButton: MaterialButton
    private lateinit var reviewFormContainer: View
    private lateinit var cancelReviewButton: MaterialButton
    private lateinit var submitReviewButton: MaterialButton

    private lateinit var favoriteManager: FavoriteManager

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        requestWindowFeature(Window.FEATURE_NO_TITLE)
        setContentView(R.layout.dish_detail_fragment)

        window?.setLayout(
            (context.resources.displayMetrics.widthPixels * 1),
            (context.resources.displayMetrics.heightPixels * 0.85).toInt()
        )

        window?.setBackgroundDrawableResource(android.R.color.transparent)

        initializeViews()
        setupClickListeners()
        loadDishDetails()

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

        writeReviewButton = findViewById(R.id.btnWriteReview)
        reviewFormContainer = findViewById(R.id.reviewFormContainer)
        cancelReviewButton = findViewById(R.id.btnCancelReview)
        submitReviewButton = findViewById(R.id.btnSubmitReview)
    }

    private fun setupClickListeners() {
        closeButton.setOnClickListener {
            dismiss()
        }

        writeReviewButton.setOnClickListener {
            reviewFormContainer.visibility =
                if (reviewFormContainer.visibility == View.VISIBLE) View.GONE else View.VISIBLE
        }

        cancelReviewButton.setOnClickListener {
            reviewFormContainer.visibility = View.GONE
        }

        submitReviewButton.setOnClickListener {
            Toast.makeText(context, "Recenzija poslana", Toast.LENGTH_SHORT).show()
            reviewFormContainer.visibility = View.GONE
        }
    }

    private fun loadDishDetails() {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = RetrofitDish.menuService.getDishDetails(dishId)

                if (response.isSuccessful) {
                    response.body()?.let { dish ->
                        displayDishDetails(dish)
                    }
                } else {
                    Toast.makeText(
                        context,
                        context.getString(R.string.error_loading_dish),
                        Toast.LENGTH_SHORT
                    ).show()
                    dismiss()
                }
            } catch (e: Exception) {
                Toast.makeText(
                    context,
                    e.message ?: "Greška",
                    Toast.LENGTH_SHORT
                ).show()
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
        dish.ingredients.forEach {
            val chip = Chip(context)
            chip.text = it
            chip.isClickable = false
            chip.isCheckable = false
            ingredientsChipGroup.addView(chip)
        }
    }
}
