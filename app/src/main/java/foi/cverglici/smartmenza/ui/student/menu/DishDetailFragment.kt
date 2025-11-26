package foi.cverglici.smartmenza.ui.student.menu

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import foi.cverglici.core.data.api.RetrofitDish
import foi.cverglici.smartmenza.R
import foi.cverglici.core.data.model.DishDetailsResponse
import kotlinx.coroutines.launch

class DishDetailFragment : Fragment() {

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

    private var dishId: Int = -1

    companion object {
        private const val ARG_DISH_ID = "dish_id"

        /**
         * metoda za kreiranje fragmenta sa dish ID
         */
        fun newInstance(dishId: Int): DishDetailFragment {
            val fragment = DishDetailFragment()
            val args = Bundle()
            args.putInt(ARG_DISH_ID, dishId)
            fragment.arguments = args
            return fragment
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        // Dohvati dish ID iz arguments
        dishId = arguments?.getInt(ARG_DISH_ID) ?: -1
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.dish_detail_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        initializeViews(view)
        setupClickListeners()

        // Učitaj podatke o jelu
        if (dishId != -1) {
            loadDishDetails(dishId)
        } else {
            Toast.makeText(requireContext(), "Invalid dish ID", Toast.LENGTH_SHORT).show()
            closeFragment()
        }
    }

    private fun initializeViews(view: View) {
        closeButton = view.findViewById(R.id.closeButton)
        dishTitle = view.findViewById(R.id.dishTitle)
        dishImage = view.findViewById(R.id.dishImage)
        favoriteIcon = view.findViewById(R.id.favoriteIcon)
        dishDescription = view.findViewById(R.id.dishDescription)
        dishPrice = view.findViewById(R.id.dishPrice)
        caloriesValue = view.findViewById(R.id.caloriesValue)
        carbsValue = view.findViewById(R.id.carbsValue)
        fiberValue = view.findViewById(R.id.fiberValue)
        fatValue = view.findViewById(R.id.fatValue)
        proteinValue = view.findViewById(R.id.proteinValue)
        ingredientsChipGroup = view.findViewById(R.id.ingredientsChipGroup)
    }

    private fun setupClickListeners() {
        closeButton.setOnClickListener {
            closeFragment()
        }

        //fav ikona - samo vizualno, nema funkcionalnost
        favoriteIcon.setOnClickListener {
            // TODO: Implementirati dodavanje u favorite
        }
    }

    /**
     * ucitaj detalje jela sa backenda
     */
    private fun loadDishDetails(dishId: Int) {
        lifecycleScope.launch {
            try {
                val response = RetrofitDish.dishService.getDishDetails(dishId)

                if (response.isSuccessful) {
                    response.body()?.let { dish ->
                        displayDishDetails(dish)
                    }
                } else {
                    Toast.makeText(
                        requireContext(),
                        getString(R.string.error_loading_dish),
                        Toast.LENGTH_SHORT
                    ).show()
                    closeFragment()
                }
            } catch (e: Exception) {
                Toast.makeText(
                    requireContext(),
                    "Greška: ${e.message}",
                    Toast.LENGTH_SHORT
                ).show()
                closeFragment()
            }
        }
    }

    /**
     * podaci o jelu na UI
     */
    private fun displayDishDetails(dish: DishDetailsResponse) {
        // Osnovni podaci
        dishTitle.text = dish.title
        dishDescription.text = dish.description
        dishPrice.text = getString(R.string.price_format, dish.price)

        // Kalorije
        caloriesValue.text = getString(R.string.calories_detail, dish.calories)

        // Makronutrijenti
        carbsValue.text = getString(R.string.grams_format, dish.carbohydrates)
        fiberValue.text = "0g" // Backend ne vraća vlakna, stavi 0 ili ukloni
        fatValue.text = getString(R.string.grams_format, dish.fat)
        proteinValue.text = getString(R.string.grams_format, dish.protein)

        // Slika - za sada placeholder
        // TODO: ucitavanje slike
        // Glide.with(this).load(dish.imgPath).into(dishImage)
        dishImage.setImageResource(R.drawable.ic_restaurant)

        // sastojci - dodaj kao chips
        ingredientsChipGroup.removeAllViews()
        dish.ingredients.forEach { ingredient ->
            val chip = Chip(requireContext())
            chip.text = ingredient
            chip.isClickable = false
            chip.isCheckable = false
            ingredientsChipGroup.addView(chip)
        }
    }

    /**
     * vrati se na prethodni ekran
     */
    private fun closeFragment() {
        parentFragmentManager.popBackStack()
    }
}