package foi.cverglici.smartmenza.ui.employee.menu

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ProgressBar
import android.widget.TextView
import android.widget.Toast
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.google.android.material.floatingactionbutton.FloatingActionButton
import foi.cverglici.core.data.api.student.dailymenu.RetrofitDish
import foi.cverglici.core.data.model.student.dailymenu.DailyMenuItem
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.ui.employee.dish.DishMenuFragment
import kotlinx.coroutines.launch
import java.text.SimpleDateFormat
import foi.cverglici.core.data.api.employee.dailymenu.RetrofitEmployeeMenu
import foi.cverglici.core.data.api.employee.dailymenu.IEmployeeMenuService
import foi.cverglici.core.data.api.student.dailymenu.IDishService
import foi.cverglici.smartmenza.session.SessionTokenProvider
import java.util.*

class EmployeeMenuListFragment : Fragment() {

    private lateinit var recyclerView: RecyclerView
    private lateinit var progressBar: ProgressBar
    private lateinit var emptyStateText: TextView
    private lateinit var adapter: EmployeeMenuAdapter
    private lateinit var fabAddDish: FloatingActionButton
    private lateinit var fabAddMenu: FloatingActionButton

    private lateinit var tabLunch: TextView
    private lateinit var tabDinner: TextView
    private lateinit var menuService: IDishService
    private lateinit var employeeMenuService: IEmployeeMenuService

    private var currentCategory: String = "lunch"
    private var currentMenuId: Int? = null

    companion object {
        private const val KEY_CURRENT_CATEGORY = "current_category"
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.employee_menu_list_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        val tokenProvider = SessionTokenProvider(requireContext())
        menuService = RetrofitDish.create(tokenProvider)
        employeeMenuService = RetrofitEmployeeMenu.create(tokenProvider)

        savedInstanceState?.let {
            currentCategory = it.getString(KEY_CURRENT_CATEGORY, "lunch")
        }

        initializeViews(view)
        setupRecyclerView()
        setupTabClickListeners()
        setupFabClickListeners()

        updateTabSelection(isLunchActive = currentCategory == "lunch")
        loadTodayMenu()
    }

    override fun onResume() {
        super.onResume()
        updateTabSelection(isLunchActive = currentCategory == "lunch")
        loadTodayMenu()
    }

    override fun onSaveInstanceState(outState: Bundle) {
        super.onSaveInstanceState(outState)
        outState.putString(KEY_CURRENT_CATEGORY, currentCategory)
    }

    private fun initializeViews(view: View) {
        recyclerView = view.findViewById(R.id.menuRecyclerView)
        progressBar = view.findViewById(R.id.dishesLoadingIndicator)
        emptyStateText = view.findViewById(R.id.emptyStateText)
        tabLunch = view.findViewById(R.id.tabLunch)
        tabDinner = view.findViewById(R.id.tabDinner)
        fabAddDish = view.findViewById(R.id.fabAddDish)
        fabAddMenu = view.findViewById(R.id.fabAddMenu)
    }

    private fun setupRecyclerView() {
        adapter = EmployeeMenuAdapter(
            onMenuClicked = { menuItem ->
                onMenuClicked(menuItem)
            }
        )

        recyclerView.apply {
            layoutManager = LinearLayoutManager(requireContext())
            adapter = this@EmployeeMenuListFragment.adapter
        }
    }

    private fun setupTabClickListeners() {
        tabLunch.setOnClickListener {
            if (currentCategory != "lunch") {
                currentCategory = "lunch"
                currentMenuId = null
                updateTabSelection(isLunchActive = true)
                loadTodayMenu()
            }
        }

        tabDinner.setOnClickListener {
            if (currentCategory != "dinner") {
                currentCategory = "dinner"
                currentMenuId = null
                updateTabSelection(isLunchActive = false)
                loadTodayMenu()
            }
        }
    }

    private fun setupFabClickListeners() {
        fabAddDish.setOnClickListener {
            navigateToAddDish()
        }

        fabAddMenu.setOnClickListener {
            navigateToAddMenu()
        }
    }

    private fun loadTodayMenu() {
        showLoading(true)

        lifecycleScope.launch {
            try {
                val response = menuService.getTodayMenu(currentCategory)  // Koristi menuService

                if (response.isSuccessful) {
                    val menuItems = response.body() ?: emptyList()

                    if (menuItems.isNotEmpty()) {
                        currentMenuId = findMenuIdFromItems(menuItems)
                        showMenuItems(menuItems)
                    } else {
                        currentMenuId = null
                        showEmptyState()
                    }
                } else {
                    showError("Greška pri dohvaćanju jelovnika: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("EmployeeMenuListFragment", "Error loading menu", e)
                showError("Mrežna greška: ${e.message}")
            } finally {
                showLoading(false)
            }
        }
    }

    private suspend fun findMenuIdFromItems(items: List<DailyMenuItem>): Int? {
        if (items.isEmpty()) return null

        val todayDate = SimpleDateFormat("yyyy-MM-dd", Locale.getDefault()).format(Date())
        val categoryString = currentCategory

        for (i in 1..200) {
            try {
                val response = employeeMenuService.getMenuById(i)  // Koristi employeeMenuService

                if (response.isSuccessful) {
                    val menu = response.body()
                    if (menu != null &&
                        menu.date == todayDate &&
                        menu.category.lowercase() == categoryString) {
                        Log.d("EmployeeMenuListFragment", "Found menuId: ${menu.dailyMenuId}")
                        return menu.dailyMenuId
                    }
                }
            } catch (e: Exception) {
                break
            }
        }

        return null
    }

    private fun updateTabSelection(isLunchActive: Boolean) {
        if (isLunchActive) {
            tabLunch.setBackgroundResource(R.drawable.bg_segment_active)
            tabLunch.setTextColor(ContextCompat.getColor(requireContext(), R.color.text_primary))

            tabDinner.background = null
            tabDinner.setTextColor(ContextCompat.getColor(requireContext(), R.color.text_secondary))
        } else {
            tabDinner.setBackgroundResource(R.drawable.bg_segment_active)
            tabDinner.setTextColor(ContextCompat.getColor(requireContext(), R.color.text_primary))

            tabLunch.background = null
            tabLunch.setTextColor(ContextCompat.getColor(requireContext(), R.color.text_secondary))
        }
    }

    private fun showMenuItems(items: List<DailyMenuItem>) {
        progressBar.visibility = View.GONE
        emptyStateText.visibility = View.GONE
        recyclerView.visibility = View.VISIBLE

        adapter.submitList(null)
        adapter.submitList(items)
    }

    private fun showEmptyState() {
        progressBar.visibility = View.GONE
        adapter.submitList(emptyList())
        recyclerView.visibility = View.GONE
        emptyStateText.visibility = View.VISIBLE
        emptyStateText.text = "Nema dostupnih jela za danas.\nDodajte nova jela klikom na + gumb."
    }

    private fun showLoading(isLoading: Boolean) {
        if (isLoading) {
            progressBar.visibility = View.VISIBLE
            recyclerView.visibility = View.GONE
            emptyStateText.visibility = View.GONE
        } else {
            progressBar.visibility = View.GONE
        }
    }

    private fun showError(message: String) {
        progressBar.visibility = View.GONE
        Toast.makeText(requireContext(), message, Toast.LENGTH_LONG).show()
        showEmptyState()
    }

    private fun navigateToAddDish() {
        val fragment = DishMenuFragment.newInstance()

        parentFragmentManager.beginTransaction()
            .replace(R.id.fragmentContainer, fragment)
            .addToBackStack(null)
            .commit()
    }

    private fun navigateToAddMenu() {
        val fragment = if (currentMenuId != null) {
            MenuFormFragment.newInstance(currentMenuId!!)
        } else {
            MenuFormFragment.newInstance()
        }

        parentFragmentManager.beginTransaction()
            .replace(R.id.fragmentContainer, fragment)
            .addToBackStack(null)
            .commit()
    }

    private fun onMenuClicked(menuItem: DailyMenuItem) {
        Log.d("EmployeeMenuListFragment", "Menu clicked: ${menuItem.dish.title}")

        val fragment = if (currentMenuId != null) {
            MenuFormFragment.newInstance(currentMenuId!!)
        } else {
            MenuFormFragment.newInstance()
        }

        parentFragmentManager.beginTransaction()
            .replace(R.id.fragmentContainer, fragment)
            .addToBackStack(null)
            .commit()
    }
}