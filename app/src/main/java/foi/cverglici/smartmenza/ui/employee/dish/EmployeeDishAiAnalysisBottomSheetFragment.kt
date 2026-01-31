package foi.cverglici.smartmenza.ui.employee.dish

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.ProgressBar
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

class EmployeeDishAiAnalysisBottomSheetFragment : BottomSheetDialogFragment() {

    private lateinit var dietChipGroup: ChipGroup
    private lateinit var allergensChipGroup: ChipGroup

    private lateinit var aiCalories: TextView
    private lateinit var aiProtein: TextView
    private lateinit var aiCarbs: TextView
    private lateinit var aiFat: TextView
    private lateinit var aiFiber: TextView

    private lateinit var assumptionsText: TextView
    private lateinit var servingText: TextView
    private lateinit var progress: ProgressBar
    private lateinit var acceptButton: Button
    private lateinit var cancelButton: Button

    private var latestNutrition: NutritionEstimateResponse? = null
    private var latestAllergens: FoodAnalysisResult? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View = inflater.inflate(R.layout.employee_ai_analysis_bottom_sheet_fragment, container, false)

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        dietChipGroup = view.findViewById(R.id.dietChipGroup)
        allergensChipGroup = view.findViewById(R.id.allergensChipGroup)

        aiCalories = view.findViewById(R.id.aiCalories)
        aiProtein = view.findViewById(R.id.aiProtein)
        aiCarbs = view.findViewById(R.id.aiCarbs)
        aiFat = view.findViewById(R.id.aiFat)
        aiFiber = view.findViewById(R.id.aiFiber)

        assumptionsText = view.findViewById(R.id.assumptionsText)
        servingText = view.findViewById(R.id.servingText)
        progress = view.findViewById(R.id.aiProgress)

        acceptButton = view.findViewById(R.id.acceptAiButton)
        cancelButton = view.findViewById(R.id.cancelAiButton)

        acceptButton.isEnabled = false

        cancelButton.setOnClickListener { dismiss() }

        acceptButton.setOnClickListener {
            val n = latestNutrition
            if (n == null) {
                Toast.makeText(requireContext(), "Nema AI rezultata za prihvatiti.", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            val m = n.macros
            val kcal = (m.kcal ?: 0.0)

            val bundle = Bundle().apply {
                putInt(BUNDLE_CALORIES, kcal.toInt())
                putDouble(BUNDLE_PROTEIN, m.protein_g ?: 0.0)
                putDouble(BUNDLE_CARBS, m.carbs_g ?: 0.0)
                putDouble(BUNDLE_FAT, m.fat_g ?: 0.0)
                putDouble(BUNDLE_FIBER, m.fiber_g ?: 0.0)

                // Optional: ako kasnije budeš htio i ovo spremati/koristiti u UI-u
                putBoolean(BUNDLE_IS_VEGAN, latestAllergens?.isVegan ?: false)
                putBoolean(BUNDLE_IS_VEGETARIAN, latestAllergens?.isVegetarian ?: false)
                putBoolean(BUNDLE_IS_GLUTEN_FREE, latestAllergens?.isGlutenFree ?: false)
            }

            parentFragmentManager.setFragmentResult(RESULT_KEY, bundle)
            dismiss()
        }

        val text = arguments?.getString(ARG_TEXT).orEmpty()
        if (text.isBlank()) {
            Toast.makeText(requireContext(), "Nema teksta za AI analizu.", Toast.LENGTH_SHORT).show()
            dismiss()
            return
        }

        runAi(text)
    }

    private fun runAi(text: String) {
        viewLifecycleOwner.lifecycleScope.launch {
            try {
                setLoading(true)

                val token = SessionManager(requireContext()).fetchAuthToken()
                val aiService = RetrofitAi.create(token)

                val allergensCall = async { aiService.analyzeAllergens(AnalyzeFoodRequest(text)) }
                val nutritionCall = async { aiService.analyzeNutrition(NutritionAnalyzeRequest(text, servings = 1)) }

                val allergensResp = allergensCall.await()
                val nutritionResp = nutritionCall.await()

                if (allergensResp.isSuccessful) {
                    latestAllergens = allergensResp.body()
                    latestAllergens?.let { renderAllergens(it) }
                } else {
                    Log.e("EMP_AI", "Allergens failed: ${allergensResp.code()}")
                    Toast.makeText(requireContext(), "AI alergeni fail (${allergensResp.code()})", Toast.LENGTH_SHORT).show()
                }

                if (nutritionResp.isSuccessful) {
                    latestNutrition = nutritionResp.body()
                    latestNutrition?.let { renderNutrition(it) }
                } else {
                    Log.e("EMP_AI", "Nutrition failed: ${nutritionResp.code()}")
                    Toast.makeText(requireContext(), "AI nutritivno fail (${nutritionResp.code()})", Toast.LENGTH_SHORT).show()
                }

                acceptButton.isEnabled = (latestNutrition != null)

            } catch (e: Exception) {
                Log.e("EMP_AI", "AI error", e)
                Toast.makeText(requireContext(), "Greška: ${e.message}", Toast.LENGTH_LONG).show()
            } finally {
                setLoading(false)
            }
        }
    }

    private fun setLoading(loading: Boolean) {
        progress.visibility = if (loading) View.VISIBLE else View.GONE
    }

    private fun renderAllergens(result: FoodAnalysisResult) {
        dietChipGroup.removeAllViews()

        if (result.isVegan) dietChipGroup.addView(makeChip("Vegan"))
        if (result.isVegetarian) dietChipGroup.addView(makeChip("Vegetarian"))
        dietChipGroup.addView(makeChip(if (result.isGlutenFree) "Gluten-free" else "Contains gluten"))

        allergensChipGroup.removeAllViews()
        if (result.allergens.isEmpty()) {
            allergensChipGroup.addView(makeChip("No allergens detected"))
        } else {
            result.allergens.forEach { a ->
                val chip = makeChip(a.allergen).apply {
                    tooltipText = if (a.triggers.isEmpty()) "No triggers"
                    else "Triggers: " + a.triggers.joinToString(", ")
                }
                allergensChipGroup.addView(chip)
            }
        }
    }

    private fun renderNutrition(n: NutritionEstimateResponse) {
        val m = n.macros

        aiCalories.text = "${(m.kcal ?: 0.0).toInt()} kcal"
        aiProtein.text = "${fmt(m.protein_g)} g"
        aiCarbs.text = "${fmt(m.carbs_g)} g"
        aiFat.text = "${fmt(m.fat_g)} g"
        aiFiber.text = "${fmt(m.fiber_g)} g"

        val serving = n.estimatedServingSizeGrams
        servingText.text = if (serving != null) "Serving: ${serving.toInt()} g" else "Serving: -"

        val assumptions = n.assumptions
        assumptionsText.text = if (assumptions.isNullOrEmpty()) "Assumptions: -" else assumptions.joinToString("\n• ", prefix = "• ")
    }

    private fun fmt(v: Double?): String = String.format("%.1f", v ?: 0.0)

    private fun makeChip(text: String): Chip =
        Chip(requireContext()).apply {
            this.text = text
            isClickable = false
            isCheckable = false
        }

    companion object {
        const val RESULT_KEY = "EMP_DISH_AI_RESULT"

        private const val ARG_TEXT = "ai_text"

        private const val BUNDLE_CALORIES = "calories"
        private const val BUNDLE_PROTEIN = "protein"
        private const val BUNDLE_CARBS = "carbs"
        private const val BUNDLE_FAT = "fat"
        private const val BUNDLE_FIBER = "fiber"

        private const val BUNDLE_IS_VEGAN = "isVegan"
        private const val BUNDLE_IS_VEGETARIAN = "isVegetarian"
        private const val BUNDLE_IS_GLUTEN_FREE = "isGlutenFree"

        fun newInstance(text: String): EmployeeDishAiAnalysisBottomSheetFragment =
            EmployeeDishAiAnalysisBottomSheetFragment().apply {
                arguments = Bundle().apply { putString(ARG_TEXT, text) }
            }
    }
}
