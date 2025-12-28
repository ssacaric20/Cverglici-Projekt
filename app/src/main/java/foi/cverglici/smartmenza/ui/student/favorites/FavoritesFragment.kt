package foi.cverglici.smartmenza.ui.student.favorites

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
import foi.cverglici.core.data.api.student.favorite.RetrofitFavorite
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionManager
import foi.cverglici.smartmenza.ui.student.menu.DishDetailDialog
import kotlinx.coroutines.launch

class FavoritesFragment : Fragment() {

    private lateinit var recyclerView: RecyclerView
    private lateinit var progressBar: ProgressBar
    private lateinit var emptyStateText: TextView
    private lateinit var adapter: FavoritesAdapter
    private lateinit var sessionManager: SessionManager

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_favorites, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        sessionManager = SessionManager(requireContext())
        initializeViews(view)
        setupRecyclerView()
        loadFavorites()
    }

    override fun onResume() {
        super.onResume()
        loadFavorites()
    }

    private fun initializeViews(view: View) {
        recyclerView = view.findViewById(R.id.favoritesRecyclerView)
        progressBar = view.findViewById(R.id.progressBar)
        emptyStateText = view.findViewById(R.id.emptyStateText)
    }

    private fun setupRecyclerView() {
        adapter = FavoritesAdapter(
            onDishClicked = { favoriteDish ->
                showDishDetails(favoriteDish.dishId)
            },
            onRemoveFavorite = { favoriteDish ->
                removeFavorite(favoriteDish.dishId)
            }
        )

        recyclerView.apply {
            layoutManager = LinearLayoutManager(requireContext())
            adapter = this@FavoritesFragment.adapter
        }
    }

    private fun loadFavorites() {
        showLoading(true)

        val userId = sessionManager.getUserId()

        lifecycleScope.launch {
            try {
                val response = RetrofitFavorite.favoriteService.getUserFavorites(userId)

                if (response.isSuccessful) {
                    val favorites = response.body() ?: emptyList()

                    if (favorites.isEmpty()) {
                        showEmptyState()
                    } else {
                        showFavorites(favorites)
                    }
                } else {
                    showError("Greška pri dohvaćanju favorita: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("FavoritesFragment", "Error loading favorites", e)
                showError("Mrežna greška: ${e.message}")
            } finally {
                showLoading(false)
            }
        }
    }

    private fun removeFavorite(dishId: Int) {
        val userId = sessionManager.getUserId()

        lifecycleScope.launch {
            try {
                val response = RetrofitFavorite.favoriteService.removeFavorite(userId, dishId)

                if (response.isSuccessful) {
                    Toast.makeText(requireContext(), "Uklonjeno iz favorita", Toast.LENGTH_SHORT).show()
                    loadFavorites()
                } else {
                    Toast.makeText(requireContext(), "Greška pri uklanjanju", Toast.LENGTH_SHORT).show()
                }
            } catch (e: Exception) {
                Log.e("FavoritesFragment", "Error removing favorite", e)
                Toast.makeText(requireContext(), "Mrežna greška", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun showDishDetails(dishId: Int) {
        val dialog = DishDetailDialog(
            requireContext(),
            viewLifecycleOwner,
            dishId
        )
        dialog.show()
    }

    private fun showFavorites(favorites: List<foi.cverglici.core.data.model.favorite.FavoriteDish>) {
        emptyStateText.visibility = View.GONE
        recyclerView.visibility = View.VISIBLE
        adapter.submitList(favorites)
    }

    private fun showEmptyState() {
        recyclerView.visibility = View.GONE
        emptyStateText.visibility = View.VISIBLE
        emptyStateText.text = "Nemate omiljenih jela.\nDodajte ih klikom na ❤️ ikonu!"
    }

    private fun showLoading(isLoading: Boolean) {
        progressBar.visibility = if (isLoading) View.VISIBLE else View.GONE
        recyclerView.visibility = if (isLoading) View.GONE else View.VISIBLE
    }

    private fun showError(message: String) {
        Toast.makeText(requireContext(), message, Toast.LENGTH_LONG).show()
        showEmptyState()
    }
}