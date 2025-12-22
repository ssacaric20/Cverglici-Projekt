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
import foi.cverglici.core.data.api.student.menu.RetrofitDish
import foi.cverglici.core.data.model.menu.DailyMenuItem
import foi.cverglici.smartmenza.R
import kotlinx.coroutines.launch

/**
 * fragment za view zaposlenika, manage daily menu
 */
class EmployeeMenuListFragment : Fragment() {

    private lateinit var recyclerView: RecyclerView
    private lateinit var progressBar: ProgressBar
    private lateinit var emptyStateText: TextView
    private lateinit var adapter: EmployeeMenuListAdapter
    private lateinit var fabAddDish: FloatingActionButton

    // tab views
    private lateinit var tabLunch: TextView
    private lateinit var tabDinner: TextView

    // current selected category
    private var currentCategory: String = "lunch"

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.employee_menu_list_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        initializeViews(view)
        setupRecyclerView()
        setupTabClickListeners()
        setupFabClickListener()
        loadTodayMenu()
    }

    private fun initializeViews(view: View) {
        recyclerView = view.findViewById(R.id.menuRecyclerView)
        progressBar = view.findViewById(R.id.progressBar)
        emptyStateText = view.findViewById(R.id.emptyStateText)
        tabLunch = view.findViewById(R.id.tabLunch)
        tabDinner = view.findViewById(R.id.tabDinner)
        fabAddDish = view.findViewById(R.id.fabAddDish)
    }

    private fun setupRecyclerView() {
        adapter = EmployeeMenuListAdapter { menuItem ->
            onDishClicked(menuItem)
        }

        recyclerView.apply {
            layoutManager = LinearLayoutManager(requireContext())
            adapter = this@EmployeeMenuListFragment.adapter
        }
    }

    private fun setupTabClickListeners() {
        tabLunch.setOnClickListener {
            if (currentCategory != "lunch") {
                currentCategory = "lunch"
                updateTabSelection(isLunchActive = true)
                loadTodayMenu()
            }
        }

        tabDinner.setOnClickListener {
            if (currentCategory != "dinner") {
                currentCategory = "dinner"
                updateTabSelection(isLunchActive = false)
                loadTodayMenu()
            }
        }
    }

    private fun setupFabClickListener() {
        fabAddDish.setOnClickListener {
            navigateToAddDish()
        }
    }

    /**
     * load today menu prema trenutnoj kategoriji
     */
    private fun loadTodayMenu() {
        showLoading(true)

        lifecycleScope.launch {
            try {
                val response = RetrofitDish.menuService.getTodayMenu(currentCategory)

                if (response.isSuccessful) {
                    val menuItems = response.body() ?: emptyList<DailyMenuItem>()

                    if (menuItems.isEmpty()) {
                        showEmptyState()
                    } else {
                        showMenuItems(menuItems)
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

    /**
     * update tab visual selection
     */
    private fun updateTabSelection(isLunchActive: Boolean) {
        if (isLunchActive) {
            // highlight lunch
            tabLunch.setBackgroundResource(R.drawable.bg_segment_active)
            tabLunch.setTextColor(ContextCompat.getColor(requireContext(), R.color.text_primary))

            // deactivate dinner
            tabDinner.background = null
            tabDinner.setTextColor(ContextCompat.getColor(requireContext(), R.color.text_secondary))
        } else {
            // highlight dinner
            tabDinner.setBackgroundResource(R.drawable.bg_segment_active)
            tabDinner.setTextColor(ContextCompat.getColor(requireContext(), R.color.text_primary))

            // deactivate lunch
            tabLunch.background = null
            tabLunch.setTextColor(ContextCompat.getColor(requireContext(), R.color.text_secondary))
        }
    }

    private fun showMenuItems(items: List<DailyMenuItem>) {
        emptyStateText.visibility = View.GONE
        recyclerView.visibility = View.VISIBLE
        adapter.submitList(items)
    }

    private fun showEmptyState() {
        recyclerView.visibility = View.GONE
        emptyStateText.visibility = View.VISIBLE
        emptyStateText.text = "Nema dostupnih jela za danas.\nDodajte nova jela klikom na + gumb."
    }

    private fun showLoading(isLoading: Boolean) {
        progressBar.visibility = if (isLoading) View.VISIBLE else View.GONE
        recyclerView.visibility = if (isLoading) View.GONE else View.VISIBLE
    }

    private fun showError(message: String) {
        Toast.makeText(requireContext(), message, Toast.LENGTH_LONG).show()
        showEmptyState()
    }

    /**
     * navigate to add new dish
     */
    private fun navigateToAddDish() {
        val fragment = EmployeeDishFormFragment.newInstance()

        parentFragmentManager.beginTransaction()
            .replace(R.id.fragmentContainer, fragment)
            .addToBackStack(null)
            .commit()
    }

    /**
     * handle dish click - navigate to edit
     */
    private fun onDishClicked(menuItem: DailyMenuItem) {
        Log.d("EmployeeMenuListFragment", "Dish clicked: ${menuItem.dish.title} (ID: ${menuItem.dishId})")

        val fragment = EmployeeDishFormFragment.newInstance(menuItem.dishId)

        parentFragmentManager.beginTransaction()
            .replace(R.id.fragmentContainer, fragment)
            .addToBackStack(null)
            .commit()
    }
}