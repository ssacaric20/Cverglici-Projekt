package foi.cverglici.smartmenza.ui.employee.dish

import android.content.Context
import android.util.Log
import android.widget.Toast
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.lifecycleScope
import foi.cverglici.core.data.api.employee.dish.RetrofitEmployeeDish
import foi.cverglici.core.data.model.dish.CreateDishRequest
import foi.cverglici.core.data.model.dish.UpdateDishRequest
import foi.cverglici.core.data.model.menu.DishDetailsResponse
import kotlinx.coroutines.launch

class DishManager(
    private val context: Context,
    private val lifecycleOwner: LifecycleOwner
) {

    fun loadDishDetails(
        dishId: Int,
        onSuccess: (DishDetailsResponse) -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = RetrofitEmployeeDish.dishService.getDishDetails(dishId)

                if (response.isSuccessful) {
                    response.body()?.let { dish ->
                        onSuccess(dish)
                    } ?: run {
                        onError("Jelo nije pronađeno")
                    }
                } else {
                    onError("Greška pri dohvaćanju detalja: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("DishManager", "Error loading dish details", e)
                onError("Mrežna greška: ${e.message}")
            }
        }
    }

    fun createDish(
        request: CreateDishRequest,
        onSuccess: (DishDetailsResponse) -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = RetrofitEmployeeDish.dishService.createDish(request)

                if (response.isSuccessful) {
                    response.body()?.let { createdDish ->
                        Toast.makeText(context, "Jelo uspješno dodano!", Toast.LENGTH_SHORT).show()
                        onSuccess(createdDish)
                    } ?: run {
                        onError("Greška pri kreiranju jela")
                    }
                } else {
                    onError("Greška pri kreiranju: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("DishManager", "Error creating dish", e)
                onError("Mrežna greška: ${e.message}")
            }
        }
    }

    fun updateDish(
        dishId: Int,
        request: UpdateDishRequest,
        onSuccess: (DishDetailsResponse) -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = RetrofitEmployeeDish.dishService.updateDish(dishId, request)

                if (response.isSuccessful) {
                    response.body()?.let { updatedDish ->
                        Toast.makeText(context, "Jelo uspješno ažurirano!", Toast.LENGTH_SHORT).show()
                        onSuccess(updatedDish)
                    } ?: run {
                        onError("Greška pri ažuriranju jela")
                    }
                } else {
                    onError("Greška pri ažuriranju: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("DishManager", "Error updating dish", e)
                onError("Mrežna greška: ${e.message}")
            }
        }
    }

    fun deleteDish(
        dishId: Int,
        onSuccess: () -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = RetrofitEmployeeDish.dishService.deleteDish(dishId)

                if (response.isSuccessful) {
                    Toast.makeText(context, "Jelo uspješno obrisano!", Toast.LENGTH_SHORT).show()
                    onSuccess()
                } else {
                    onError("Greška pri brisanju: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("DishManager", "Error deleting dish", e)
                onError("Mrežna greška: ${e.message}")
            }
        }
    }
}