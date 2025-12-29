package foi.cverglici.smartmenza.ui.employee.dish

import android.app.AlertDialog
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.ImageView
import android.widget.Spinner
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.google.android.material.textfield.TextInputEditText
import foi.cverglici.core.data.model.employee.dish.CreateDishRequest
import foi.cverglici.core.data.model.employee.dish.UpdateDishRequest
import foi.cverglici.core.data.model.student.dailymenu.DishDetailsResponse
import foi.cverglici.smartmenza.R

class DishFormFragment : Fragment() {

    private lateinit var formTitle: TextView
    private lateinit var dishImagePreview: ImageView
    private lateinit var categorySpinner: Spinner
    private lateinit var titleInput: TextInputEditText
    private lateinit var descriptionInput: TextInputEditText
    private lateinit var priceInput: TextInputEditText
    private lateinit var caloriesInput: TextInputEditText
    private lateinit var carbsInput: TextInputEditText
    private lateinit var fiberInput: TextInputEditText
    private lateinit var fatInput: TextInputEditText
    private lateinit var proteinInput: TextInputEditText
    private lateinit var ingredientsInput: TextInputEditText
    private lateinit var saveDishButton: Button
    private lateinit var cancelButton: Button
    private lateinit var deleteButton: Button

    private var dishId: Int? = null
    private var isEditMode: Boolean = false
    private lateinit var dishManager: DishManager

    companion object {
        private const val ARG_DISH_ID = "dish_id"

        fun newInstance(): DishFormFragment {
            return DishFormFragment()
        }

        fun newInstance(dishId: Int): DishFormFragment {
            return DishFormFragment().apply {
                arguments = Bundle().apply {
                    putInt(ARG_DISH_ID, dishId)
                }
            }
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        arguments?.let {
            if (it.containsKey(ARG_DISH_ID)) {
                dishId = it.getInt(ARG_DISH_ID)
                isEditMode = true
            }
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.employee_dish_form_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        dishManager = DishManager(requireContext(), viewLifecycleOwner)

        initializeViews(view)
        setupCategorySpinner()
        setupClickListeners()

        if (isEditMode) {
            formTitle.text = getString(R.string.edit_dish)
            deleteButton.visibility = View.VISIBLE
            dishId?.let { loadDishData(it) }
        } else {
            formTitle.text = getString(R.string.add_new_dish)
            deleteButton.visibility = View.GONE
        }
    }

    private fun initializeViews(view: View) {
        formTitle = view.findViewById(R.id.formTitle)
        dishImagePreview = view.findViewById(R.id.dishImagePreview)
        categorySpinner = view.findViewById(R.id.categorySpinner)
        titleInput = view.findViewById(R.id.titleInput)
        descriptionInput = view.findViewById(R.id.descriptionInput)
        priceInput = view.findViewById(R.id.priceInput)
        caloriesInput = view.findViewById(R.id.caloriesInput)
        carbsInput = view.findViewById(R.id.carbsInput)
        fiberInput = view.findViewById(R.id.fiberInput)
        fatInput = view.findViewById(R.id.fatInput)
        proteinInput = view.findViewById(R.id.proteinInput)
        ingredientsInput = view.findViewById(R.id.ingredientsInput)
        saveDishButton = view.findViewById(R.id.saveDishButton)
        cancelButton = view.findViewById(R.id.cancelButton)
        deleteButton = view.findViewById(R.id.deleteButton)
    }

    private fun setupCategorySpinner() {
        val categories = arrayOf(
            getString(R.string.lunch),
            getString(R.string.dinner)
        )

        val adapter = ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_item,
            categories
        )
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        categorySpinner.adapter = adapter
    }

    private fun setupClickListeners() {
        saveDishButton.setOnClickListener {
            handleSaveDish()
        }

        cancelButton.setOnClickListener {
            handleCancel()
        }

        deleteButton.setOnClickListener {
            handleDeleteDish()
        }
    }

    private fun loadDishData(dishId: Int) {
        showLoading(true)

        dishManager.loadDishDetails(
            dishId = dishId,
            onSuccess = { dish ->
                displayDishData(dish)
                showLoading(false)
            },
            onError = { error ->
                Toast.makeText(requireContext(), error, Toast.LENGTH_SHORT).show()
                showLoading(false)
                navigateBack()
            }
        )
    }

    private fun displayDishData(dish: DishDetailsResponse) {
        titleInput.setText(dish.title)
        descriptionInput.setText(dish.description)
        priceInput.setText(dish.price.toString())
        caloriesInput.setText(dish.calories?.toString() ?: "")
        carbsInput.setText(dish.carbohydrates?.toString() ?: "")
        fiberInput.setText(dish.fiber?.toString() ?: "")
        fatInput.setText(dish.fat?.toString() ?: "")
        proteinInput.setText(dish.protein?.toString() ?: "")
    }

    private fun handleSaveDish() {
        if (!validateInputs()) {
            return
        }

        val title = titleInput.text.toString().trim()
        val description = descriptionInput.text.toString().trim()
        val price = priceInput.text.toString().toDoubleOrNull() ?: 0.0
        val calories = caloriesInput.text.toString().toIntOrNull()
        val carbs = carbsInput.text.toString().toDoubleOrNull()
        val fiber = fiberInput.text.toString().toDoubleOrNull()
        val fat = fatInput.text.toString().toDoubleOrNull()
        val protein = proteinInput.text.toString().toDoubleOrNull()

        showLoading(true)

        if (isEditMode && dishId != null) {
            val updateRequest = UpdateDishRequest(
                title = title,
                description = description,
                price = price,
                calories = calories,
                protein = protein,
                fat = fat,
                carbohydrates = carbs,
                fiber = fiber,
                imgPath = null
            )

            dishManager.updateDish(
                dishId = dishId!!,
                request = updateRequest,
                onSuccess = {
                    showLoading(false)
                    navigateBack()
                },
                onError = { error ->
                    Toast.makeText(requireContext(), error, Toast.LENGTH_SHORT).show()
                    showLoading(false)
                }
            )
        } else {
            val createRequest = CreateDishRequest(
                title = title,
                description = description,
                price = price,
                calories = calories,
                protein = protein,
                fat = fat,
                carbohydrates = carbs,
                fiber = fiber,
                imgPath = null
            )

            dishManager.createDish(
                request = createRequest,
                onSuccess = {
                    showLoading(false)
                    navigateBack()
                },
                onError = { error ->
                    Toast.makeText(requireContext(), error, Toast.LENGTH_SHORT).show()
                    showLoading(false)
                }
            )
        }
    }

    private fun handleDeleteDish() {
        AlertDialog.Builder(requireContext())
            .setTitle("Brisanje jela")
            .setMessage("Jeste li sigurni da želite obrisati ovo jelo?")
            .setPositiveButton("Da") { _, _ ->
                dishId?.let { id ->
                    showLoading(true)

                    dishManager.deleteDish(
                        dishId = id,
                        onSuccess = {
                            showLoading(false)
                            navigateBack()
                        },
                        onError = { error ->
                            Toast.makeText(requireContext(), error, Toast.LENGTH_SHORT).show()
                            showLoading(false)
                        }
                    )
                }
            }
            .setNegativeButton("Ne", null)
            .show()
    }

    private fun validateInputs(): Boolean {
        if (titleInput.text.isNullOrBlank()) {
            titleInput.error = getString(R.string.error_title_required)
            return false
        }

        if (priceInput.text.isNullOrBlank()) {
            priceInput.error = getString(R.string.error_price_required)
            return false
        }

        val price = priceInput.text.toString().toDoubleOrNull()
        if (price == null || price <= 0) {
            priceInput.error = "Cijena mora biti veća od 0"
            return false
        }

        return true
    }

    private fun handleCancel() {
        navigateBack()
    }

    private fun navigateBack() {
        parentFragmentManager.popBackStack()
    }

    private fun showLoading(isLoading: Boolean) {
        saveDishButton.isEnabled = !isLoading
        deleteButton.isEnabled = !isLoading
        cancelButton.isEnabled = !isLoading
    }
}