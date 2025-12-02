package foi.cverglici.smartmenza.ui.student.menu

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ProgressBar
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import foi.cverglici.core.data.api.RetrofitDish
import foi.cverglici.core.menu.model.DailyMenuItem
import foi.cverglici.smartmenza.R
import kotlinx.coroutines.launch

/**
 * Fragment for displaying daily menu list (Student view)
 */
class MenuListFragment : Fragment() {

    private lateinit var recyclerView: RecyclerView
    private lateinit var progressBar: ProgressBar
    private lateinit var emptyStateText: TextView
    private lateinit var adapter: MenuListAdapter

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.menu_list_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        initializeViews(view)
        setupRecyclerView()
        loadTodayMenu()
    }

    private fun initializeViews(view: View) {
        recyclerView = view.findViewById(R.id.menuRecyclerView)
        progressBar = view.findViewById(R.id.progressBar)
        emptyStateText = view.findViewById(R.id.emptyStateText)
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

    /**
     * today menu from API
     */
    private fun loadTodayMenu() {
        showLoading(true)

        lifecycleScope.launch {
            try {
                val response = RetrofitDish.menuService.getTodayMenu()

                if (response.isSuccessful) {
                    val menuItems = response.body() ?: emptyList<DailyMenuItem>()

                    if (menuItems.isEmpty()) {
                        showEmptyState()
                    } else {
                        showMenuItems(menuItems)
                    }
                } else {
                    showError("Greška pri dohvaćanju jelovnika: ${'$'}{response.code()}")
                }
            } catch (e: Exception) {
                Log.e("MenuListFragment", "Error loading menu", e)
                showError("Mrežna greška: ${'$'}{e.message}")
            } finally {
                showLoading(false)
            }
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

        // prikazi DishDetailDialog
        val dialog = DishDetailDialog(
            requireContext(),
            viewLifecycleOwner,
            menuItem.dishId
        )
        dialog.show()
    }
}