package foi.cverglici.smartmenza.ui.employee.dish

import android.app.AlertDialog
import android.net.Uri
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
import androidx.activity.result.contract.ActivityResultContracts
import androidx.fragment.app.Fragment
import com.google.android.material.textfield.TextInputEditText
import foi.cverglici.core.data.api.employee.dish.IEmployeeDishService
import foi.cverglici.core.data.api.employee.dish.RetrofitEmployeeDish
import foi.cverglici.core.data.model.employee.dish.CreateDishRequest
import foi.cverglici.core.data.model.employee.dish.UpdateDishRequest
import foi.cverglici.core.data.model.student.dailymenu.DishDetailsResponse
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionTokenProvider
import foi.cverglici.smartmenza.ui.employee.ai.common.AiDishPrefillContract

class DishMenuFragment : Fragment() {

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
    private lateinit var aiAnalyzeButton: Button
    private lateinit var saveDishButton: Button
    private lateinit var cancelButton: Button
    private lateinit var deleteButton: Button
    private lateinit var uploadDishImageButton: Button
    private lateinit var dishImagePlaceholderText: TextView

    private lateinit var dishService: IEmployeeDishService

    private var dishId: Int? = null
    private var isEditMode: Boolean = false
    private lateinit var dishManager: DishManager
    private var selectedImageUri: Uri? = null

    companion object {
        private const val ARG_DISH_ID = "dish_id"

        fun newInstance(): DishMenuFragment = DishMenuFragment()

        fun newInstance(dishId: Int): DishMenuFragment {
            return DishMenuFragment().apply {
                arguments = Bundle().apply { putInt(ARG_DISH_ID, dishId) }
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

        val tokenProvider = SessionTokenProvider(requireContext())
        dishService = RetrofitEmployeeDish.create(tokenProvider)

        dishManager = DishManager(requireContext(), viewLifecycleOwner)

        initializeViews(view)
        setupCategorySpinner()
        setupClickListeners()
        setupAiResultListener()

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
        aiAnalyzeButton = view.findViewById(R.id.aiAnalyzeButton)
        saveDishButton = view.findViewById(R.id.saveDishButton)
        cancelButton = view.findViewById(R.id.cancelButton)
        deleteButton = view.findViewById(R.id.deleteButton)
        uploadDishImageButton = view.findViewById(R.id.uploadDishImageButton)
        dishImagePlaceholderText = view.findViewById(R.id.dishImagePlaceholderText)
    }

    private fun setupCategorySpinner() {
        val categories = arrayOf(getString(R.string.lunch), getString(R.string.dinner))

        val adapter = ArrayAdapter(requireContext(), android.R.layout.simple_spinner_item, categories)
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        categorySpinner.adapter = adapter
    }

    private fun setupClickListeners() {
        saveDishButton.setOnClickListener { handleSaveDish() }
        cancelButton.setOnClickListener { handleCancel() }
        deleteButton.setOnClickListener { handleDeleteDish() }

        uploadDishImageButton.setOnClickListener { pickImage.launch("image/*") }

        aiAnalyzeButton.setOnClickListener {
            val text = buildAiInputText()
            if (text.isBlank()) {
                Toast.makeText(requireContext(), "Unesite barem naziv jela za AI analizu.", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }
            AiAnalysisBottomSheetFragment
                .newInstance(text)
                .show(parentFragmentManager, "EmployeeDishAiAnalysis")
        }
    }

    private fun setupAiResultListener() {
        parentFragmentManager.setFragmentResultListener(
            AiDishPrefillContract.RESULT_KEY,
            viewLifecycleOwner
        ) { _, bundle ->
            val calories = bundle.getInt("calories", 0)
            val protein = bundle.getDouble("protein", 0.0)
            val carbs = bundle.getDouble("carbs", 0.0)
            val fat = bundle.getDouble("fat", 0.0)
            val fiber = bundle.getDouble("fiber", 0.0)

            val title = bundle.getString(AiDishPrefillContract.KEY_TITLE)
            val desc  = bundle.getString(AiDishPrefillContract.KEY_DESCRIPTION)
            val ing   = bundle.getString(AiDishPrefillContract.KEY_INGREDIENTS)

            if (!title.isNullOrBlank()) titleInput.setText(title)
            if (!desc.isNullOrBlank()) descriptionInput.setText(desc)
            if (!ing.isNullOrBlank()) ingredientsInput.setText(ing)

            caloriesInput.setText(calories.toString())
            proteinInput.setText(protein.toString())
            carbsInput.setText(carbs.toString())
            fatInput.setText(fat.toString())
            fiberInput.setText(fiber.toString())

            Toast.makeText(requireContext(), "AI vrijednosti primijenjene u formu.", Toast.LENGTH_SHORT).show()
        }
    }

    private fun buildAiInputText(): String {
        val title = titleInput.text?.toString()?.trim().orEmpty()
        val desc = descriptionInput.text?.toString()?.trim().orEmpty()
        val ing = ingredientsInput.text?.toString()?.trim().orEmpty()

        return buildString {
            if (title.isNotBlank()) appendLine(title)
            if (desc.isNotBlank()) appendLine(desc)
            if (ing.isNotBlank()) appendLine("Ingredients: $ing")
        }.trim()
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

        uploadDishImageButton.setOnClickListener { pickImage.launch("image/*") }
    }

    private fun handleSaveDish() {
        if (!validateInputs()) return

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
        aiAnalyzeButton.isEnabled = !isLoading
    }
}
