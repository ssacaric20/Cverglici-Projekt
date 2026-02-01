package foi.cverglici.smartmenza.ui.employee.statistics

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.EditText
import android.widget.ImageView
import android.widget.ProgressBar
import android.widget.TextView
import android.widget.Toast
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import foi.cverglici.core.data.api.employee.statistics.IStatisticsService
import foi.cverglici.core.data.api.employee.statistics.RetrofitStatistics
import foi.cverglici.core.data.model.employee.statistics.DishStatisticsResponse
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionTokenProvider
import kotlinx.coroutines.launch

class StatisticsFragment : Fragment() {

    private lateinit var recyclerView: RecyclerView
    private lateinit var progressBar: ProgressBar
    private lateinit var emptyStateText: TextView
    private lateinit var adapter: StatisticsAdapter

    private lateinit var tabTopRated: TextView
    private lateinit var tabMostFavorited: TextView
    private lateinit var searchDishInput: EditText
    private lateinit var searchButton: ImageView
    private lateinit var countChipGroup: ChipGroup
    private lateinit var chipTop3: Chip
    private lateinit var chipTop5: Chip
    private lateinit var chipTop10: Chip

    private lateinit var statisticsService: IStatisticsService

    private var currentTab: TabType = TabType.TOP_RATED
    private var currentCount: Int = 10

    private var topRatedDishes: List<DishStatisticsResponse> = emptyList()
    private var mostFavoritedDishes: List<DishStatisticsResponse> = emptyList()

    enum class TabType {
        TOP_RATED,
        MOST_FAVORITED
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_statistics, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        val tokenProvider = SessionTokenProvider(requireContext())
        statisticsService = RetrofitStatistics.create(tokenProvider)

        initializeViews(view)
        setupRecyclerView()
        setupTabClickListeners()
        setupSearchFunctionality()
        setupCountChips()

        loadStatistics()
    }

    private fun initializeViews(view: View) {
        recyclerView = view.findViewById(R.id.statisticsRecyclerView)
        progressBar = view.findViewById(R.id.progressBar)
        emptyStateText = view.findViewById(R.id.emptyStateText)

        tabTopRated = view.findViewById(R.id.tabTopRated)
        tabMostFavorited = view.findViewById(R.id.tabMostFavorited)
        searchDishInput = view.findViewById(R.id.searchDishInput)
        searchButton = view.findViewById(R.id.searchButton)
        countChipGroup = view.findViewById(R.id.countChipGroup)
        chipTop3 = view.findViewById(R.id.chipTop3)
        chipTop5 = view.findViewById(R.id.chipTop5)
        chipTop10 = view.findViewById(R.id.chipTop10)
    }

    private fun setupRecyclerView() {
        adapter = StatisticsAdapter(
            onDishClicked = { dish ->
                navigateToDishStatistics(dish.dishId)
            }
        )

        recyclerView.apply {
            layoutManager = LinearLayoutManager(requireContext())
            adapter = this@StatisticsFragment.adapter
        }
    }

    private fun setupTabClickListeners() {
        tabTopRated.setOnClickListener {
            if (currentTab != TabType.TOP_RATED) {
                currentTab = TabType.TOP_RATED
                updateTabSelection(isTopRatedActive = true)
                displayCurrentTab()
            }
        }

        tabMostFavorited.setOnClickListener {
            if (currentTab != TabType.MOST_FAVORITED) {
                currentTab = TabType.MOST_FAVORITED
                updateTabSelection(isTopRatedActive = false)
                displayCurrentTab()
            }
        }
    }

    private fun setupSearchFunctionality() {
        searchButton.setOnClickListener {
            val dishName = searchDishInput.text.toString().trim()
            if (dishName.isNotEmpty()) {
                searchDishByName(dishName)
            } else {
                Toast.makeText(requireContext(), "Unesite naziv jela", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun setupCountChips() {
        countChipGroup.setOnCheckedStateChangeListener { _, checkedIds ->
            if (checkedIds.isNotEmpty()) {
                when (checkedIds[0]) {
                    R.id.chipTop3 -> currentCount = 3
                    R.id.chipTop5 -> currentCount = 5
                    R.id.chipTop10 -> currentCount = 10
                }
                displayCurrentTab()
            }
        }
    }

    private fun loadStatistics() {
        showLoading(true)

        lifecycleScope.launch {
            try {
                val response = statisticsService.getTopDishes(count = 10)

                if (response.isSuccessful) {
                    val data = response.body()
                    if (data != null) {
                        topRatedDishes = data.topRatedDishes
                        mostFavoritedDishes = data.mostFavoritedDishes
                        displayCurrentTab()
                    } else {
                        showError("Nema dostupnih podataka")
                    }
                } else {
                    showError("Greška pri dohvaćanju statistike: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("StatisticsFragment", "Error loading statistics", e)
                showError("Mrežna greška: ${e.message}")
            } finally {
                showLoading(false)
            }
        }
    }

    private fun displayCurrentTab() {
        val dishes = when (currentTab) {
            TabType.TOP_RATED -> topRatedDishes.take(currentCount)
            TabType.MOST_FAVORITED -> mostFavoritedDishes.take(currentCount)
        }

        if (dishes.isEmpty()) {
            showEmptyState()
        } else {
            showDishes(dishes)
        }
    }

    private fun searchDishByName(dishName: String) {
        val allDishes = topRatedDishes + mostFavoritedDishes
        val matchedDish = allDishes.find {
            it.title.contains(dishName, ignoreCase = true)
        }

        if (matchedDish != null) {
            navigateToDishStatistics(matchedDish.dishId)
        } else {
            Toast.makeText(
                requireContext(),
                "Jelo '$dishName' nije pronađeno u statistici",
                Toast.LENGTH_SHORT
            ).show()
        }
    }

    private fun navigateToDishStatistics(dishId: Int) {
        val fragment = DishStatisticsFragment.newInstance(dishId)

        parentFragmentManager.beginTransaction()
            .replace(R.id.fragmentContainer, fragment)
            .addToBackStack(null)
            .commit()
    }

    private fun updateTabSelection(isTopRatedActive: Boolean) {
        if (isTopRatedActive) {
            tabTopRated.setBackgroundResource(R.drawable.bg_segment_active)
            tabTopRated.setTextColor(ContextCompat.getColor(requireContext(), R.color.text_primary))

            tabMostFavorited.background = null
            tabMostFavorited.setTextColor(ContextCompat.getColor(requireContext(), R.color.text_secondary))
        } else {
            tabMostFavorited.setBackgroundResource(R.drawable.bg_segment_active)
            tabMostFavorited.setTextColor(ContextCompat.getColor(requireContext(), R.color.text_primary))

            tabTopRated.background = null
            tabTopRated.setTextColor(ContextCompat.getColor(requireContext(), R.color.text_secondary))
        }
    }

    private fun showDishes(dishes: List<DishStatisticsResponse>) {
        progressBar.visibility = View.GONE
        emptyStateText.visibility = View.GONE
        recyclerView.visibility = View.VISIBLE

        adapter.submitList(null)
        adapter.submitList(dishes)
    }

    private fun showEmptyState() {
        progressBar.visibility = View.GONE
        adapter.submitList(emptyList())
        recyclerView.visibility = View.GONE
        emptyStateText.visibility = View.VISIBLE
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
}