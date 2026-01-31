package foi.cverglici.smartmenza.ui.student.menu

import android.os.Bundle
import android.util.Log
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
import foi.cverglici.core.data.model.ai.AnalyzeFoodRequest
import foi.cverglici.core.data.model.ai.FoodAnalysisResult
import foi.cverglici.core.data.model.ai.NutritionAnalyzeRequest
import foi.cverglici.core.data.model.ai.NutritionEstimateResponse
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionManager
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

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? = inflater.inflate(R.layout.ai_analysis_bottom_sheet_fragment, container, false)

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        dietChipGroup = view.findViewById(R.id.dietChipGroup)
        allergensChipGroup = view.findViewById(R.id.allergensChipGroup)

        aiCalories = view.findViewById(R.id.aiCalories)
        aiProtein = view.findViewById(R.id.aiProtein)
        aiCarbs = view.findViewById(R.id.aiCarbs)
        aiFat = view.findViewById(R.id.aiFat)
        aiFiber = view.findViewById(R.id.aiFiber)

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
                val token = SessionManager(requireContext()).fetchAuthToken()
                val aiService = RetrofitAi.create(token)

                // RAW debug calls (to see exact JSON)
                val rawAllergensCall = async { aiService.analyzeAllergensRaw(AnalyzeFoodRequest(text)) }
                val rawNutritionCall = async { aiService.analyzeNutritionRaw(NutritionAnalyzeRequest(text, servings = 1)) }

                val rawAllergensResp = rawAllergensCall.await()
                val rawNutritionResp = rawNutritionCall.await()

                val rawAllergensJson =
                    rawAllergensResp.body()?.string() ?: rawAllergensResp.errorBody()?.string()
                val rawNutritionJson =
                    rawNutritionResp.body()?.string() ?: rawNutritionResp.errorBody()?.string()

                Log.d("AI_RAW", "ALLERGENS HTTP ${rawAllergensResp.code()} JSON: $rawAllergensJson")
                Log.d("AI_RAW", "NUTRITION HTTP ${rawNutritionResp.code()} JSON: $rawNutritionJson")

                // Typed calls for UI
                val allergensTypedCall = async { aiService.analyzeAllergens(AnalyzeFoodRequest(text)) }
                val nutritionTypedCall = async { aiService.analyzeNutrition(NutritionAnalyzeRequest(text, servings = 1)) }

                val allergensResp = allergensTypedCall.await()
                val nutritionResp = nutritionTypedCall.await()

                if (!allergensResp.isSuccessful) {
                    Toast.makeText(requireContext(), "Allergen analysis failed (${allergensResp.code()})", Toast.LENGTH_SHORT).show()
                } else {
                    allergensResp.body()?.let { renderAllergens(it) }
                }

                if (!nutritionResp.isSuccessful) {
                    Toast.makeText(requireContext(), "Nutrition analysis failed (${nutritionResp.code()})", Toast.LENGTH_SHORT).show()
                } else {
                    nutritionResp.body()?.let { renderNutrition(it) }
                }

            } catch (e: Exception) {
                Toast.makeText(requireContext(), "Error: ${e.message}", Toast.LENGTH_LONG).show()
                Log.e("AI_RAW", "Exception", e)
            }
        }
    }

    private fun renderAllergens(result: FoodAnalysisResult) {
        dietChipGroup.removeAllViews()

        if (result.isVegan) dietChipGroup.addView(makeChip("Vegan"))
        if (result.isVegetarian) dietChipGroup.addView(makeChip("Vegetarian"))
        dietChipGroup.addView(makeChip(if (result.isGlutenFree) "Gluten-free" else "Contains gluten"))

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
        val m = n.macros

        // kcal may be null, so fallback to 0
        val kcal = (m.kcal ?: 0.0).toInt()
        aiCalories.text = "$kcal kcal"

        aiProtein.text = "${fmt(m.protein_g)} g"
        aiCarbs.text = "${fmt(m.carbs_g)} g"
        aiFat.text = "${fmt(m.fat_g)} g"
        aiFiber.text = "${fmt(m.fiber_g)} g"
    }

    private fun fmt(v: Double?): String = String.format("%.1f", v ?: 0.0)

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
