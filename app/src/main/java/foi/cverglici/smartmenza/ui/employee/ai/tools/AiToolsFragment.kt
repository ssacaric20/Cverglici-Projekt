package foi.cverglici.smartmenza.ui.employee.ai.tools

import android.os.Bundle
import android.view.*
import android.widget.Button
import androidx.fragment.app.Fragment
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.ui.employee.ai.dishgenerator.EmployeeAiDishGeneratorFragment

class AiToolsFragment : Fragment() {

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View {
        return inflater.inflate(R.layout.ai_tools_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        view.findViewById<Button>(R.id.openDishGeneratorButton).setOnClickListener {
            parentFragmentManager.beginTransaction()
                .replace(R.id.fragmentContainer, EmployeeAiDishGeneratorFragment())
                .addToBackStack("ai_tools")
                .commit()
        }
    }
}
