package foi.cverglici.smartmenza.ui.student.menu

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.google.android.material.bottomsheet.BottomSheetDialogFragment
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import foi.cverglici.core.data.api.ai.RetrofitAi
import foi.cverglici.core.data.api.student.menu.RetrofitDish
import foi.cverglici.core.data.model.ai.AnalyzeFoodRequest
import foi.cverglici.core.data.model.ai.FoodAnalysisResult
import foi.cverglici.core.data.model.ai.NutritionAnalyzeRequest
import foi.cverglici.core.data.model.ai.NutritionEstimateResponse
import foi.cverglici.smartmenza.R
import kotlinx.coroutines.async
import kotlinx.coroutines.launch

class AiAnalysisBottomSheetFragment : BottomSheetDialogFragment() {

    private lateinit var dietChipGroup: ChipGroup
    private lateinit var allergensChipGroup: ChipGroup

    private lateinit var aiCalories: TextView
    private lateinit var aiProtein: TextView
    private lateinit var aiCarbs: TextView
    private lateinit var aiFat: TextView
    private lateinit var aiFiber: TextView
    //private lateinit var aiConfidence: TextView

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? { return inflater.inflate(R.layout.ai_analysis_bottom_sheet_fragment, container, false)}

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        dietChipGroup = view.findViewById(R.id.dietChipGroup)
        allergensChipGroup = view.findViewById(R.id.allergensChipGroup)

        aiCalories = view.findViewById(R.id.aiCalories)
        aiProtein = view.findViewById(R.id.aiProtein)
        aiCarbs = view.findViewById(R.id.aiCarbs)
        aiFat = view.findViewById(R.id.aiFat)
        aiFiber = view.findViewById(R.id.aiFiber)
        //aiConfidence = view.findViewById(R.id.aiConfidence)

        val description = arguments?.getString(ARG_TEXT).orEmpty()
        if (description.isBlank()) {
            Toast.makeText(requireContext(), "No description to analyze.", Toast.LENGTH_SHORT).show()
            return
        }

        loadAnalysis(description)
    }

    private fun loadAnalysis(text: String) {
        viewLifecycleOwner.lifecycleScope.launch {
            try {
                // Run both requests in parallel
                val allergensCall = async {
                    RetrofitAi.aiService.analyzeAllergens(AnalyzeFoodRequest(text))
                }
                val nutritionCall = async {
                    RetrofitAi.aiService.analyzeNutrition(NutritionAnalyzeRequest(text, servings = 1))
                }

                val allergensResp = allergensCall.await()
                val nutritionResp = nutritionCall.await()

                if (!allergensResp.isSuccessful) {
                    Toast.makeText(requireContext(), "Allergen analysis failed (${allergensResp.code()})", Toast.LENGTH_SHORT).show()
                }
                if (!nutritionResp.isSuccessful) {
                    Toast.makeText(requireContext(), "Nutrition analysis failed (${nutritionResp.code()})", Toast.LENGTH_SHORT).show()
                }

                allergensResp.body()?.let { renderAllergens(it) }
                nutritionResp.body()?.let { renderNutrition(it) }

            } catch (e: Exception) {
                Toast.makeText(requireContext(), "Error: ${e.message}", Toast.LENGTH_LONG).show()
            }
        }
    }

    private fun renderAllergens(result: FoodAnalysisResult) {
        // Diet chips
        dietChipGroup.removeAllViews()
        if (result.isVegan) dietChipGroup.addView(makeChip("Vegan"))
        if (result.isVegetarian) dietChipGroup.addView(makeChip("Vegetarian"))
        dietChipGroup.addView(makeChip(if (result.isGlutenFree) "Gluten-free" else "Contains gluten"))

        // Allergen chips
        allergensChipGroup.removeAllViews()
        result.allergens.forEach { a ->
            val chip = makeChip(a.allergen).apply {
                tooltipText = if (a.triggers.isEmpty()) "No triggers"
                else "Triggers: " + a.triggers.joinToString(", ")
            }
            allergensChipGroup.addView(chip)
        }
    }

    private fun renderNutrition(n: NutritionEstimateResponse) {
        aiCalories.text = "${n.calories.toInt()} kcal"
        aiProtein.text = "${fmt(n.protein)} g"
        aiCarbs.text = "${fmt(n.carbohydrates)} g"
        aiFat.text = "${fmt(n.fat)} g"
        aiFiber.text = "${fmt(n.fiber)} g"
        //aiConfidence.text = "Confidence: ${fmt(n.confidence)}"
    }

    private fun fmt(v: Double): String = String.format("%.1f", v)

    private fun makeChip(text: String): Chip =
        Chip(requireContext()).apply {
            this.text = text
            isClickable = true
            isCheckable = false
        }

    companion object {
        private const val ARG_TEXT = "dishDescription"

        fun newInstance(dishDescription: String): AiAnalysisBottomSheetFragment =
            AiAnalysisBottomSheetFragment().apply {
                arguments = Bundle().apply { putString(ARG_TEXT, dishDescription) }
            }
    }
}
