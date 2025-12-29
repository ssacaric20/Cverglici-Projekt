package foi.cverglici.smartmenza.ui.employee.menu

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
import foi.cverglici.smartmenza.R
import androidx.activity.result.contract.ActivityResultContracts
import android.net.Uri

/**
 * ua adding ili editing
 */
class EmployeeDishFormFragment : Fragment() {

    // view components
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
    private lateinit var uploadDishImageButton: Button
    private lateinit var dishImagePlaceholderText: TextView

    private var dishId: Int? = null
    private var isEditMode: Boolean = false
    private var selectedImageUri: Uri? = null

    companion object {
        private const val ARG_DISH_ID = "dish_id"

        /**
         * create new instance for adding
         */
        fun newInstance(): EmployeeDishFormFragment {
            return EmployeeDishFormFragment()
        }

        /**
         * create new instance for editing
         */
        fun newInstance(dishId: Int): EmployeeDishFormFragment {
            return EmployeeDishFormFragment().apply {
                arguments = Bundle().apply {
                    putInt(ARG_DISH_ID, dishId)
                }
            }
        }
    }

    private val pickImage =
        registerForActivityResult(ActivityResultContracts.GetContent()) { uri ->
            if (uri != null) {
                dishImagePreview.setImageURI(uri)
                dishImagePlaceholderText.visibility = View.GONE
                selectedImageUri = uri
            }
        }


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        // get dish ID from arguments if editing
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

        initializeViews(view)
        setupCategorySpinner()
        setupClickListeners()

        // update title based on mode
        if (isEditMode) {
            formTitle.text = getString(R.string.edit_dish)
            // TODO: Load dish data from API (Task 2)
        } else {
            formTitle.text = getString(R.string.add_new_dish)
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
        uploadDishImageButton = view.findViewById(R.id.uploadDishImageButton)
        dishImagePlaceholderText = view.findViewById(R.id.dishImagePlaceholderText)
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

        uploadDishImageButton.setOnClickListener {
            pickImage.launch("image/*")
        }
    }

    /**
     * handle save dish action
     * TODO: This will be implemented in Task 2 (CRUD logic)
     */
    private fun handleSaveDish() {
        // Basic validation (detailed validation in Task 3)
        if (titleInput.text.isNullOrBlank()) {
            titleInput.error = getString(R.string.error_title_required)
            return
        }

        if (priceInput.text.isNullOrBlank()) {
            priceInput.error = getString(R.string.error_price_required)
            return
        }

        // get form data
        val category = when (categorySpinner.selectedItemPosition) {
            0 -> "lunch"
            1 -> "dinner"
            else -> "lunch"
        }

        val title = titleInput.text.toString().trim()
        val description = descriptionInput.text.toString().trim()
        val price = priceInput.text.toString().toDoubleOrNull() ?: 0.0
        val calories = caloriesInput.text.toString().toIntOrNull() ?: 0
        val carbs = carbsInput.text.toString().toDoubleOrNull() ?: 0.0
        val fiber = fiberInput.text.toString().toDoubleOrNull() ?: 0.0
        val fat = fatInput.text.toString().toDoubleOrNull() ?: 0.0
        val protein = proteinInput.text.toString().toDoubleOrNull() ?: 0.0

        // Parse ingredients (comma-separated)
        val ingredientsText = ingredientsInput.text.toString().trim()
        val ingredients = if (ingredientsText.isNotBlank()) {
            ingredientsText.split(",").map { it.trim() }.filter { it.isNotEmpty() }
        } else {
            emptyList()
        }

        // TODO: Task 2 - API call to save/update dish
        Toast.makeText(
            requireContext(),
            "Spremanje jela - TODO: API integracija",
            Toast.LENGTH_SHORT
        ).show()

        // For now, just show placeholder message
        if (isEditMode) {
            Toast.makeText(requireContext(), "Izmjena jela ID: $dishId", Toast.LENGTH_SHORT).show()
        } else {
            Toast.makeText(requireContext(), "Dodavanje novog jela", Toast.LENGTH_SHORT).show()

            // Clear form after adding new dish (so user can add more)
            clearForm()
        }
    }

    /**
     * clear all input fields
     */
    private fun clearForm() {
        titleInput.text?.clear()
        descriptionInput.text?.clear()
        priceInput.text?.clear()
        caloriesInput.text?.clear()
        carbsInput.text?.clear()
        fiberInput.text?.clear()
        fatInput.text?.clear()
        proteinInput.text?.clear()
        ingredientsInput.text?.clear()
        categorySpinner.setSelection(0)
        selectedImageUri = null
        dishImagePreview.setImageResource(R.drawable.ic_restaurant)
        dishImagePlaceholderText.visibility = View.VISIBLE
    }

    /**
     * handle cancel action
     */
    private fun handleCancel() {
        // Go back to previous screen
        parentFragmentManager.popBackStack()
    }
}