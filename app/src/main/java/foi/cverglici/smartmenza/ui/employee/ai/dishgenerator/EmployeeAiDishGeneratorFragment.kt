package foi.cverglici.smartmenza.ui.employee.ai.dishgenerator

import android.os.Bundle
import android.view.*
import android.widget.*
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import foi.cverglici.core.data.api.ai.RetrofitAi
import foi.cverglici.core.data.model.ai.AnalyzeFoodRequest
import foi.cverglici.core.data.model.ai.FoodAnalysisResult
import foi.cverglici.core.data.model.ai.NutritionAnalyzeRequest
import foi.cverglici.core.data.model.ai.NutritionEstimateResponse
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionManager
import foi.cverglici.smartmenza.ui.employee.ai.common.AiDishPrefillContract
import foi.cverglici.smartmenza.ui.employee.dish.DishMenuFragment
import kotlinx.coroutines.async
import kotlinx.coroutines.launch

class EmployeeAiDishGeneratorFragment : Fragment() {

    private lateinit var nameInput: EditText
    private lateinit var ingredientsInput: EditText
    private lateinit var generateButton: Button
    private lateinit var progress: ProgressBar
    private lateinit var preview: TextView
    private lateinit var acceptButton: Button

    private var latestNutrition: NutritionEstimateResponse? = null
    private var latestAllergens: FoodAnalysisResult? = null

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View {
        return inflater.inflate(R.layout.employee_ai_dish_generator, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        nameInput = view.findViewById(R.id.aiDishNameInput)
        ingredientsInput = view.findViewById(R.id.aiDishIngredientsInput)
        generateButton = view.findViewById(R.id.generateAiButton)
        progress = view.findViewById(R.id.aiProgress)
        preview = view.findViewById(R.id.aiResultPreview)
        acceptButton = view.findViewById(R.id.acceptAiDishButton)

        acceptButton.isEnabled = false

        generateButton.setOnClickListener { runAi() }
        acceptButton.setOnClickListener { acceptAndEdit() }
    }

    private fun runAi() {
        val title = nameInput.text?.toString()?.trim().orEmpty()
        val ingredients = ingredientsInput.text?.toString()?.trim().orEmpty()

        if (title.isBlank() && ingredients.isBlank()) {
            Toast.makeText(requireContext(), "Unesi barem naziv ili sastojke.", Toast.LENGTH_SHORT).show()
            return
        }

        val aiText = buildString {
            if (title.isNotBlank()) appendLine(title)
            if (ingredients.isNotBlank()) appendLine("Ingredients: $ingredients")
        }.trim()

        viewLifecycleOwner.lifecycleScope.launch {
            try {
                setLoading(true)
                acceptButton.isEnabled = false
                preview.text = ""

                val token = SessionManager(requireContext()).fetchAuthToken()
                val aiService = RetrofitAi.create(token)

                val allergensCall = async { aiService.analyzeAllergens(AnalyzeFoodRequest(aiText)) }
                val nutritionCall = async { aiService.analyzeNutrition(NutritionAnalyzeRequest(aiText, servings = 1)) }

                val allergensResp = allergensCall.await()
                val nutritionResp = nutritionCall.await()

                latestAllergens = if (allergensResp.isSuccessful) allergensResp.body() else null
                latestNutrition = if (nutritionResp.isSuccessful) nutritionResp.body() else null

                renderPreview(title, ingredients, latestAllergens, latestNutrition)

                acceptButton.isEnabled = (latestNutrition != null)

                if (!allergensResp.isSuccessful || !nutritionResp.isSuccessful) {
                    Toast.makeText(
                        requireContext(),
                        "AI nije vratio sve podatke (A:${allergensResp.code()} N:${nutritionResp.code()}).",
                        Toast.LENGTH_SHORT
                    ).show()
                }

            } catch (e: Exception) {
                Toast.makeText(requireContext(), "Greška: ${e.message}", Toast.LENGTH_LONG).show()
            } finally {
                setLoading(false)
            }
        }
    }

    private fun renderPreview(
        title: String,
        ingredients: String,
        allergens: FoodAnalysisResult?,
        nutrition: NutritionEstimateResponse?
    ) {
        val sb = StringBuilder()

        if (title.isNotBlank()) sb.appendLine("Naziv: $title")
        if (ingredients.isNotBlank()) sb.appendLine("Sastojci: $ingredients")

        if (nutrition != null) {
            val m = nutrition.macros
            sb.appendLine()
            sb.appendLine("Makrosi:")
            sb.appendLine("  kcal: ${(m.kcal ?: 0.0).toInt()}")
            sb.appendLine("  P: ${fmt(m.protein_g)} g  C: ${fmt(m.carbs_g)} g  F: ${fmt(m.fat_g)} g  Fiber: ${fmt(m.fiber_g)} g")

            val serving = nutrition.estimatedServingSizeGrams
            sb.appendLine("Porcija (procjena): ${serving?.toInt()?.toString() ?: "-"} g")

            if (nutrition.assumptions.isNotEmpty()) {
                sb.appendLine()
                sb.appendLine("Assumptions:")
                nutrition.assumptions.forEach { sb.appendLine(" • $it") }
            }
        }

        if (allergens != null) {
            sb.appendLine()
            sb.appendLine("Diet:")
            sb.appendLine("  Vegan: ${allergens.isVegan}, Vegetarian: ${allergens.isVegetarian}, Gluten-free: ${allergens.isGlutenFree}")

            sb.appendLine("Alergeni:")
            if (allergens.allergens.isEmpty()) sb.appendLine("  - nema detektiranih")
            else allergens.allergens.forEach { sb.appendLine("  - ${it.allergen}") }
        }

        preview.text = sb.toString().trim()
    }

    private fun acceptAndEdit() {
        val nutrition = latestNutrition
        if (nutrition == null) {
            Toast.makeText(requireContext(), "Nema AI rezultata za prihvatiti.", Toast.LENGTH_SHORT).show()
            return
        }

        val title = nameInput.text?.toString()?.trim().orEmpty()
        val ingredients = ingredientsInput.text?.toString()?.trim().orEmpty()
        val allergens = latestAllergens

        val m = nutrition.macros
        val serving = nutrition.estimatedServingSizeGrams?.toInt()

        val description = buildString {
            if (ingredients.isNotBlank()) append("Main ingredients: $ingredients")
            if (serving != null) append("\nServing (AI estimate): ${serving}g")
            if (allergens != null && allergens.allergens.isNotEmpty()) {
                append("\nAllergens: " + allergens.allergens.joinToString(", ") { it.allergen })
            }
        }.ifBlank { null }

        val bundle = Bundle().apply {
            putInt(AiDishPrefillContract.KEY_CALORIES, (m.kcal ?: 0.0).toInt())
            putDouble(AiDishPrefillContract.KEY_PROTEIN, m.protein_g ?: 0.0)
            putDouble(AiDishPrefillContract.KEY_CARBS, m.carbs_g ?: 0.0)
            putDouble(AiDishPrefillContract.KEY_FAT, m.fat_g ?: 0.0)
            putDouble(AiDishPrefillContract.KEY_FIBER, m.fiber_g ?: 0.0)

            putString(AiDishPrefillContract.KEY_TITLE, title)
            putString(AiDishPrefillContract.KEY_DESCRIPTION, description)
            putString(AiDishPrefillContract.KEY_INGREDIENTS, ingredients)
            if (serving != null) putInt(AiDishPrefillContract.KEY_SERVING_GRAMS, serving)
        }

        parentFragmentManager.setFragmentResult(AiDishPrefillContract.RESULT_KEY, bundle)

        parentFragmentManager.beginTransaction()
            .replace(R.id.fragmentContainer, DishMenuFragment.newInstance())
            .addToBackStack("dish_form")
            .commit()
    }

    private fun setLoading(loading: Boolean) {
        progress.visibility = if (loading) View.VISIBLE else View.GONE
        generateButton.isEnabled = !loading
    }

    private fun fmt(v: Double?): String = String.format("%.1f", v ?: 0.0)
}
