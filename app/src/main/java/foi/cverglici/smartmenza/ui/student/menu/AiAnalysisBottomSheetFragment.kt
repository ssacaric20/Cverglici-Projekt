package foi.cverglici.smartmenza.ui.student.menu

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import com.google.android.material.bottomsheet.BottomSheetDialogFragment
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import foi.cverglici.smartmenza.R

class AiAnalysisBottomSheetFragment : BottomSheetDialogFragment() {

    private lateinit var dietChipGroup: ChipGroup
    private lateinit var allergensChipGroup: ChipGroup

    private lateinit var aiCalories: TextView
    private lateinit var aiProtein: TextView
    private lateinit var aiCarbs: TextView
    private lateinit var aiFat: TextView
    private lateinit var aiFiber: TextView
    private lateinit var aiConfidence: TextView

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View = inflater.inflate(R.layout.ai_analysis_bottom_sheet_fragment, container, false)

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        dietChipGroup = view.findViewById(R.id.dietChipGroup)
        allergensChipGroup = view.findViewById(R.id.allergensChipGroup)

        aiCalories = view.findViewById(R.id.aiCalories)
        aiProtein = view.findViewById(R.id.aiProtein)
        aiCarbs = view.findViewById(R.id.aiCarbs)
        aiFat = view.findViewById(R.id.aiFat)
        aiFiber = view.findViewById(R.id.aiFiber)

        renderFakeData()
    }

    private fun renderFakeData() {
        // Diet
        dietChipGroup.removeAllViews()
        dietChipGroup.addView(makeChip("Vegetarian"))
        dietChipGroup.addView(makeChip("Contains gluten"))

        // Allergens
        allergensChipGroup.removeAllViews()
        allergensChipGroup.addView(makeChip("Milk").apply { tooltipText = "Triggers: parmesan, cream" })
        allergensChipGroup.addView(makeChip("Eggs").apply { tooltipText = "Triggers: mayo" })

        // Nutrition
        aiCalories.text = "620 kcal"
        aiProtein.text = "35 g"
        aiCarbs.text = "70 g"
        aiFat.text = "18 g"
        aiFiber.text = "6 g"
    }

    private fun makeChip(text: String): Chip {
        return Chip(requireContext()).apply {
            this.text = text
            isClickable = true
            isCheckable = false
        }
    }

    companion object {
        fun newInstance(dishDescription: String): AiAnalysisBottomSheetFragment {
            return AiAnalysisBottomSheetFragment().apply {
                arguments = Bundle().apply {
                    putString("dishDescription", dishDescription)
                }
            }
        }
    }
}
