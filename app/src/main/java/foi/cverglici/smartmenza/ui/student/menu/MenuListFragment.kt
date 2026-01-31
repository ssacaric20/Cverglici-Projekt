package foi.cverglici.smartmenza.ui.student.menu

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
import foi.cverglici.core.data.api.student.dailymenu.IDishService
import foi.cverglici.core.data.api.student.dailymenu.RetrofitDish
import foi.cverglici.core.data.model.student.dailymenu.DailyMenuItem
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionTokenProvider
import kotlinx.coroutines.launch

class MenuListFragment : Fragment() {

    private lateinit var recyclerView: RecyclerView
    private lateinit var progressBar: ProgressBar
    private lateinit var emptyStateText: TextView
    private lateinit var adapter: MenuListAdapter
    private lateinit var menuService: IDishService

    private lateinit var tabLunch: TextView
    private lateinit var tabDinner: TextView

    private var currentCategory: String = "lunch"

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.menu_list_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        val tokenProvider = SessionTokenProvider(requireContext())
        menuService = RetrofitDish.create(tokenProvider)

        initializeViews(view)
        setupRecyclerView()
        setupTabClickListeners()
        loadTodayMenu()
    }

    private fun initializeViews(view: View) {
        recyclerView = view.findViewById(R.id.menuRecyclerView)
        progressBar = view.findViewById(R.id.progressBar)
        emptyStateText = view.findViewById(R.id.emptyStateText)
        tabLunch = view.findViewById(R.id.tabLunch)
        tabDinner = view.findViewById(R.id.tabDinner)
    }

    private fun setupRecyclerView() {
        adapter = MenuListAdapter { menuItem ->
            onDishClicked(menuItem)
        }

        recyclerView.apply {
            layoutManager = LinearLayoutManager(requireContext())
            adapter = this@MenuListFragment.adapter
        }
    }

    private fun setupTabClickListeners() {
        tabLunch.setOnClickListener {
            if (currentCategory != "lunch") {
                currentCategory = "lunch"
                updateTabSelection(isLunchActive = true)
                adapter.submitList(null)
                loadTodayMenu()
            }
        }

        tabDinner.setOnClickListener {
            if (currentCategory != "dinner") {
                currentCategory = "dinner"
                updateTabSelection(isLunchActive = false)
                adapter.submitList(null)
                loadTodayMenu()
            }
        }
    }

    private fun loadTodayMenu() {
        showLoading(true)

        lifecycleScope.launch {
            try {
                val response = menuService.getTodayMenu(currentCategory)

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
                Log.e("MenuListFragment", "Error loading menu", e)
                showError("Mrežna greška: ${e.message}")
            } finally {
                showLoading(false)
            }
        }
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
        emptyStateText.visibility = View.GONE
        recyclerView.visibility = View.VISIBLE
        adapter.submitList(items)
    }

    private fun showEmptyState() {
        recyclerView.visibility = View.GONE
        emptyStateText.visibility = View.VISIBLE
        emptyStateText.text = "Nema dostupnih jela za danas.\nProvjerite kasnije!"
    }

    private fun showLoading(isLoading: Boolean) {
        progressBar.visibility = if (isLoading) View.VISIBLE else View.GONE
        recyclerView.visibility = if (isLoading) View.GONE else View.VISIBLE
    }

    private fun showError(message: String) {
        Toast.makeText(requireContext(), message, Toast.LENGTH_LONG).show()
        showEmptyState()
    }

    private fun onDishClicked(menuItem: DailyMenuItem) {
        Log.d("MenuListFragment", "Dish clicked: ${menuItem.dish.title} (ID: ${menuItem.dishId})")

        val dialog = DishDetailDialog(
            requireContext(),
            viewLifecycleOwner,
            parentFragmentManager,
            menuItem.dishId
        )
        dialog.show()
    }
}