package foi.cverglici.smartmenza.ui.student.goals

import android.content.Context
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.lifecycleScope
import foi.cverglici.core.data.api.student.nutritiongoal.RetrofitNutritionGoal
import foi.cverglici.core.data.model.student.nutritiongoal.NutritionGoalResponse
import foi.cverglici.core.data.model.student.nutritiongoal.SetNutritionGoalRequest
import foi.cverglici.core.data.model.student.nutritiongoal.TodayNutritionProgressResponse
import foi.cverglici.smartmenza.session.SessionTokenProvider
import kotlinx.coroutines.launch

class GoalsManager(
    private val context: Context,
    private val lifecycleOwner: LifecycleOwner
) {
    private val tokenProvider = SessionTokenProvider(context)
    private val service = RetrofitNutritionGoal.create(tokenProvider)

    fun loadGoalAndTodayProgress(
        onSuccess: (NutritionGoalResponse, TodayNutritionProgressResponse) -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val goal = service.getMyGoal()
                val today = service.getTodayProgress()
                onSuccess(goal, today)
            } catch (e: Exception) {
                onError(e.message ?: "Greška pri dohvaćanju podataka.")
            }
        }
    }

    fun saveGoal(
        request: SetNutritionGoalRequest,
        onSuccess: (NutritionGoalResponse) -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val saved = service.setMyGoal(request)
                onSuccess(saved)
            } catch (e: Exception) {
                onError(e.message ?: "Greška pri spremanju ciljeva.")
            }
        }
    }
}
