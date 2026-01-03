package foi.cverglici.smartmenza.ui.student.menu

import android.app.Dialog
import android.content.Context
import android.os.Bundle
import android.view.Window
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.FragmentManager
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.lifecycleScope
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import foi.cverglici.core.data.api.student.menu.RetrofitDish
import foi.cverglici.core.data.model.menu.DishDetailsResponse
import foi.cverglici.smartmenza.R
import kotlinx.coroutines.launch

class DishDetailDialog(
    context: Context,
    private val lifecycleOwner: LifecycleOwner,
    private val fragmentManager: FragmentManager,
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
    private lateinit var aiAnalyzeButton: com.google.android.material.button.MaterialButton

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
    }

    private fun setupClickListeners() {
        closeButton.setOnClickListener {
            dismiss()
        }

        // Favorite icon - just visual for now, no functionality
        favoriteIcon.setOnClickListener {
            // TODO: Implement add to favorites
            Toast.makeText(context, "Dodavanje u favorite uskoro!", Toast.LENGTH_SHORT).show()
        }

        aiAnalyzeButton.setOnClickListener {
            val text = dishDescription.text?.toString().orEmpty()
            val sheet = AiAnalysisBottomSheetFragment.newInstance(text)
            sheet.show(fragmentManager, "AiAnalysis")
        }
    }

    /**
     * load dish details
     */
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
                    "Greška: ${e.message}",
                    Toast.LENGTH_SHORT
                ).show()
                dismiss()
            }
        }
    }

    /**
     * dish data on UI
     */
    private fun displayDishDetails(dish: DishDetailsResponse) {
        // basic information
        dishTitle.text = dish.title
        dishDescription.text = dish.description
        dishPrice.text = context.getString(R.string.price_format, dish.price)

        // calories
        caloriesValue.text = context.getString(R.string.calories_detail, dish.calories)

        // macronutrients
        carbsValue.text = context.getString(R.string.grams_format, dish.carbohydrates)
        fiberValue.text = context.getString(R.string.grams_format, dish.fiber)
        fatValue.text = context.getString(R.string.grams_format, dish.fat)
        proteinValue.text = context.getString(R.string.grams_format, dish.protein)
        averageRating.text = context.getString(R.string.average_rating_format, dish.averageRating)
        ratingCount.text = context.getString(R.string.reviews_total_format, dish.ratingsCount)


        // image - placeholder
        // TODO: Load image with Glide
        // Glide.with(context).load(dish.imgPath).into(dishImage)
        dishImage.setImageResource(R.drawable.ic_restaurant)

        // ingredients - chips
        ingredientsChipGroup.removeAllViews()
        dish.ingredients.forEach { ingredient ->
            val chip = Chip(context)
            chip.text = ingredient
            chip.isClickable = false
            chip.isCheckable = false
            ingredientsChipGroup.addView(chip)
        }
    }
}