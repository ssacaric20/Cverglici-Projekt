package foi.cverglici.smartmenza.ui.employee.statistics

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.ProgressBar
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import foi.cverglici.core.data.api.employee.statistics.IStatisticsService
import foi.cverglici.core.data.api.employee.statistics.RetrofitStatistics
import foi.cverglici.core.data.api.student.reviews.IReviewService
import foi.cverglici.core.data.api.student.reviews.RetrofitReview
import foi.cverglici.core.data.model.employee.statistics.DishStatisticsResponse
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionTokenProvider
import foi.cverglici.smartmenza.ui.student.reviews.ReviewsAdapter
import kotlinx.coroutines.launch

class DishStatisticsFragment : Fragment() {

    private lateinit var progressBar: ProgressBar
    private lateinit var statisticsContainer: LinearLayout
    private lateinit var closeButton: ImageView

    private lateinit var dishTitle: TextView
    private lateinit var averageRatingValue: TextView
    private lateinit var ratingsCountValue: TextView
    private lateinit var favoriteCountValue: TextView

    private lateinit var reviewsRecyclerView: RecyclerView
    private lateinit var emptyReviewsText: TextView
    private lateinit var reviewsAdapter: ReviewsAdapter

    private lateinit var statisticsService: IStatisticsService
    private lateinit var reviewService: IReviewService

    private var dishId: Int = -1

    companion object {
        private const val ARG_DISH_ID = "dish_id"

        fun newInstance(dishId: Int): DishStatisticsFragment {
            return DishStatisticsFragment().apply {
                arguments = Bundle().apply {
                    putInt(ARG_DISH_ID, dishId)
                }
            }
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {
            dishId = it.getInt(ARG_DISH_ID, -1)
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_dish_statistics, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        val tokenProvider = SessionTokenProvider(requireContext())
        statisticsService = RetrofitStatistics.create(tokenProvider)
        reviewService = RetrofitReview.create(tokenProvider)

        initializeViews(view)
        setupRecyclerView()
        setupCloseButton()

        if (dishId != -1) {
            loadDishStatistics()
            loadDishReviews()
        } else {
            Toast.makeText(requireContext(), "Greška: Nepoznato jelo", Toast.LENGTH_SHORT).show()
            navigateBack()
        }
    }

    private fun initializeViews(view: View) {
        progressBar = view.findViewById(R.id.progressBar)
        statisticsContainer = view.findViewById(R.id.statisticsContainer)
        closeButton = view.findViewById(R.id.closeButton)

        dishTitle = view.findViewById(R.id.dishTitle)
        averageRatingValue = view.findViewById(R.id.averageRatingValue)
        ratingsCountValue = view.findViewById(R.id.ratingsCountValue)
        favoriteCountValue = view.findViewById(R.id.favoriteCountValue)

        reviewsRecyclerView = view.findViewById(R.id.reviewsRecyclerView)
        emptyReviewsText = view.findViewById(R.id.emptyReviewsText)
    }

    private fun setupRecyclerView() {
        reviewsAdapter = ReviewsAdapter(
            currentUserId = -1,
            onEdit = {},
            onDelete = {}
        )

        reviewsRecyclerView.apply {
            layoutManager = LinearLayoutManager(requireContext())
            adapter = reviewsAdapter
        }
    }

    private fun setupCloseButton() {
        closeButton.setOnClickListener {
            navigateBack()
        }
    }

    private fun loadDishStatistics() {
        showLoading(true)

        lifecycleScope.launch {
            try {
                val response = statisticsService.getDishStatistics(dishId)

                if (response.isSuccessful) {
                    val statistics = response.body()
                    if (statistics != null) {
                        displayStatistics(statistics)
                    } else {
                        showError("Jelo nije pronađeno u statistici")
                    }
                } else {
                    showError("Greška pri dohvaćanju statistike: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("DishStatisticsFragment", "Error loading statistics", e)
                showError("Mrežna greška: ${e.message}")
            } finally {
                showLoading(false)
            }
        }
    }

    private fun loadDishReviews() {
        lifecycleScope.launch {
            try {
                val response = reviewService.getDishReviews(dishId)

                if (response.isSuccessful) {
                    val reviews = response.body() ?: emptyList()

                    if (reviews.isEmpty()) {
                        emptyReviewsText.visibility = View.VISIBLE
                        reviewsRecyclerView.visibility = View.GONE
                    } else {
                        emptyReviewsText.visibility = View.GONE
                        reviewsRecyclerView.visibility = View.VISIBLE
                        reviewsAdapter.submit(reviews)
                    }
                } else {
                    Log.e("DishStatisticsFragment", "Failed to load reviews: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("DishStatisticsFragment", "Error loading reviews", e)
            }
        }
    }

    private fun displayStatistics(statistics: DishStatisticsResponse) {
        dishTitle.text = statistics.title

        if (statistics.averageRating != null) {
            averageRatingValue.text = String.format("%.1f", statistics.averageRating)
        } else {
            averageRatingValue.text = "-"
        }

        ratingsCountValue.text = statistics.ratingsCount.toString()
        favoriteCountValue.text = statistics.favoriteCount.toString()

        statisticsContainer.visibility = View.VISIBLE
    }

    private fun showLoading(isLoading: Boolean) {
        if (isLoading) {
            progressBar.visibility = View.VISIBLE
            statisticsContainer.visibility = View.GONE
        } else {
            progressBar.visibility = View.GONE
        }
    }

    private fun showError(message: String) {
        Toast.makeText(requireContext(), message, Toast.LENGTH_LONG).show()
        navigateBack()
    }

    private fun navigateBack() {
        parentFragmentManager.popBackStack()
    }
}