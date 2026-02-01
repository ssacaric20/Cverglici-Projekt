package foi.cverglici.smartmenza.ui.student.goals

import android.content.Context
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.lifecycleScope
import foi.cverglici.core.data.api.student.dailyfoodintake.IDailyFoodIntakeService
import foi.cverglici.core.data.api.student.dailyfoodintake.RetrofitDailyFoodIntake
import foi.cverglici.core.data.model.student.dailyfoodintake.AddDailyFoodIntakeRequest
import foi.cverglici.core.data.model.student.dailyfoodintake.DailyFoodIntakeResponse
import foi.cverglici.smartmenza.session.SessionTokenProvider
import kotlinx.coroutines.launch

class DailyFoodIntakeManager(
    context: Context,
    private val lifecycleOwner: LifecycleOwner
) {

    private val api: IDailyFoodIntakeService =
        RetrofitDailyFoodIntake.create(SessionTokenProvider(context))

    fun loadToday(
        onSuccess: (List<DailyFoodIntakeResponse>) -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val result = api.getMyToday()
                onSuccess(result)
            } catch (e: Exception) {
                onError(e.message ?: "Greška pri dohvaćanju dnevnog unosa.")
            }
        }
    }

    fun addDish(
        dishId: Int,
        onSuccess: () -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                api.addToMyToday(AddDailyFoodIntakeRequest(dishId))
                onSuccess()
            } catch (e: Exception) {
                onError(e.message ?: "Greška pri dodavanju jela.")
            }
        }
    }

    fun deleteItem(
        id: Int,
        onSuccess: () -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                api.delete(id)
                onSuccess()
            } catch (e: Exception) {
                onError(e.message ?: "Greška pri brisanju.")
            }
        }
    }





}
