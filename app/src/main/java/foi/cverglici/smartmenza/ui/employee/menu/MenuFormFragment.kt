package foi.cverglici.smartmenza.ui.employee.menu

import android.app.AlertDialog
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import foi.cverglici.core.data.api.employee.dish.IEmployeeDishService
import foi.cverglici.core.data.api.employee.dish.RetrofitEmployeeDish
import foi.cverglici.core.data.model.employee.dailymenu.UpdateDailyMenuRequest
import foi.cverglici.core.data.model.employee.dish.DishListItem
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionTokenProvider
import kotlinx.coroutines.launch
import java.text.SimpleDateFormat
import java.util.*

class MenuFormFragment : Fragment() {

    private lateinit var formTitle: TextView
    private lateinit var categorySpinner: Spinner
    private lateinit var dishesMultiSelect: Button
    private lateinit var selectedDishesText: TextView
    private lateinit var saveMenuButton: Button
    private lateinit var cancelButton: Button
    private lateinit var deleteButton: Button

    private var menuId: Int? = null
    private var isEditMode: Boolean = false
    private lateinit var menuManager: MenuManager

    private var allDishes: List<DishListItem> = emptyList()
    private var selectedDishIds: MutableList<Int> = mutableListOf()
    private lateinit var dishService: IEmployeeDishService

    companion object {
        private const val ARG_MENU_ID = "menu_id"

        fun newInstance(): MenuFormFragment {
            return MenuFormFragment()
        }

        fun newInstance(menuId: Int): MenuFormFragment {
            return MenuFormFragment().apply {
                arguments = Bundle().apply {
                    putInt(ARG_MENU_ID, menuId)
                }
            }
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        arguments?.let {
            if (it.containsKey(ARG_MENU_ID)) {
                menuId = it.getInt(ARG_MENU_ID)
                isEditMode = true
            }
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_menu_form, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        val tokenProvider = SessionTokenProvider(requireContext())
        dishService = RetrofitEmployeeDish.create(tokenProvider)

        menuManager = MenuManager(requireContext(), viewLifecycleOwner)

        initializeViews(view)
        setupCategorySpinner()
        setupClickListeners()
        loadAllDishes()

        if (isEditMode) {
            formTitle.text = "Uredi meni"
            deleteButton.visibility = View.VISIBLE
            menuId?.let { loadMenuData(it) }
        } else {
            formTitle.text = "Dodaj meni"
            deleteButton.visibility = View.GONE
        }
    }

    private fun initializeViews(view: View) {
        formTitle = view.findViewById(R.id.formTitle)
        categorySpinner = view.findViewById(R.id.categorySpinner)
        dishesMultiSelect = view.findViewById(R.id.dishesMultiSelect)
        selectedDishesText = view.findViewById(R.id.selectedDishesText)
        saveMenuButton = view.findViewById(R.id.saveMenuButton)
        cancelButton = view.findViewById(R.id.cancelButton)
        deleteButton = view.findViewById(R.id.deleteButton)
    }

    private fun setupCategorySpinner() {
        val categories = arrayOf("Ručak", "Večera")

        val adapter = ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_item,
            categories
        )
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        categorySpinner.adapter = adapter
    }

    private fun setupClickListeners() {
        dishesMultiSelect.setOnClickListener {
            showDishSelectionDialog()
        }

        saveMenuButton.setOnClickListener {
            handleSaveMenu()
        }

        cancelButton.setOnClickListener {
            navigateBack()
        }

        deleteButton.setOnClickListener {
            handleDeleteMenu()
        }
    }

    private fun loadAllDishes() {
        lifecycleScope.launch {
            try {
                val response = dishService.getAllDishes()

                if (response.isSuccessful) {
                    allDishes = response.body() ?: emptyList()
                } else {
                    Toast.makeText(requireContext(), "Greška pri dohvaćanju jela", Toast.LENGTH_SHORT).show()
                }
            } catch (e: Exception) {
                Toast.makeText(requireContext(), "Greška: ${e.message}", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun showDishSelectionDialog() {
        val dishNames = allDishes.map { it.title }.toTypedArray()
        val checkedItems = BooleanArray(allDishes.size) { index ->
            selectedDishIds.contains(allDishes[index].dishId)
        }

        AlertDialog.Builder(requireContext())
            .setTitle("Odaberi jela")
            .setMultiChoiceItems(dishNames, checkedItems) { _, which, isChecked ->
                val dishId = allDishes[which].dishId
                if (isChecked) {
                    if (!selectedDishIds.contains(dishId)) {
                        selectedDishIds.add(dishId)
                    }
                } else {
                    selectedDishIds.remove(dishId)
                }
            }
            .setPositiveButton("Potvrdi") { _, _ ->
                updateSelectedDishesText()
            }
            .setNegativeButton("Odustani", null)
            .show()
    }

    private fun updateSelectedDishesText() {
        if (selectedDishIds.isEmpty()) {
            selectedDishesText.text = "Nijedno jelo nije odabrano"
        } else {
            val selectedNames = allDishes
                .filter { selectedDishIds.contains(it.dishId) }
                .joinToString(", ") { it.title }
            selectedDishesText.text = "Odabrana jela: $selectedNames"
        }
    }

    private fun loadMenuData(menuId: Int) {
        menuManager.loadMenuDetails(
            menuId = menuId,
            onSuccess = { menu ->
                categorySpinner.setSelection(if (menu.category == "Lunch") 0 else 1)
                selectedDishIds = menu.dishes.map { it.dishId }.toMutableList()
                updateSelectedDishesText()
            },
            onError = { error ->
                Toast.makeText(requireContext(), error, Toast.LENGTH_SHORT).show()
                navigateBack()
            }
        )
    }

    private fun handleSaveMenu() {
        if (selectedDishIds.isEmpty()) {
            Toast.makeText(requireContext(), "Morate odabrati barem jedno jelo", Toast.LENGTH_SHORT).show()
            return
        }

        val category = if (categorySpinner.selectedItemPosition == 0) 1 else 2
        val todayDate = SimpleDateFormat("yyyy-MM-dd", Locale.getDefault()).format(Date())

        if (isEditMode && menuId != null) {
            val updateRequest = UpdateDailyMenuRequest(
                date = todayDate,
                category = category,
                dishIds = selectedDishIds
            )

            menuManager.updateMenu(
                menuId = menuId!!,
                request = updateRequest,
                onSuccess = {
                    navigateBack()
                },
                onError = { error ->
                    Toast.makeText(requireContext(), error, Toast.LENGTH_SHORT).show()
                }
            )
        } else {
            menuManager.saveOrUpdateMenu(
                date = todayDate,
                category = category,
                dishIds = selectedDishIds,
                onSuccess = {
                    navigateBack()
                },
                onError = { error ->
                    Toast.makeText(requireContext(), error, Toast.LENGTH_SHORT).show()
                }
            )
        }
    }

    private fun handleDeleteMenu() {
        AlertDialog.Builder(requireContext())
            .setTitle("Brisanje menija")
            .setMessage("Jeste li sigurni da želite obrisati ovaj meni?")
            .setPositiveButton("Da") { _, _ ->
                menuId?.let { id ->
                    menuManager.deleteMenu(
                        menuId = id,
                        onSuccess = {
                            navigateBack()
                        },
                        onError = { error ->
                            Toast.makeText(requireContext(), error, Toast.LENGTH_SHORT).show()
                        }
                    )
                }
            }
            .setNegativeButton("Ne", null)
            .show()
    }

    private fun navigateBack() {
        parentFragmentManager.popBackStack()
    }
}