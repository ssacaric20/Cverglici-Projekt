package foi.cverglici.smartmenza.ui.employee.menu

import android.content.Context
import android.util.Log
import android.widget.Toast
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.lifecycleScope
import foi.cverglici.core.data.api.employee.dailymenu.IEmployeeMenuService
import foi.cverglici.core.data.api.employee.dailymenu.RetrofitEmployeeMenu
import foi.cverglici.core.data.model.employee.dailymenu.CreateDailyMenuRequest
import foi.cverglici.core.data.model.employee.dailymenu.DailyMenuDetailsResponse
import foi.cverglici.core.data.model.employee.dailymenu.UpdateDailyMenuRequest
import foi.cverglici.smartmenza.session.SessionTokenProvider
import kotlinx.coroutines.launch

class MenuManager(
    private val context: Context,
    private val lifecycleOwner: LifecycleOwner
) {
    private val tokenProvider = SessionTokenProvider(context)
    private val menuService: IEmployeeMenuService = RetrofitEmployeeMenu.create(tokenProvider)

    fun loadMenuDetails(
        menuId: Int,
        onSuccess: (DailyMenuDetailsResponse) -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = menuService.getMenuById(menuId)

                if (response.isSuccessful) {
                    response.body()?.let { menu ->
                        onSuccess(menu)
                    } ?: run {
                        onError("Meni nije pronađen")
                    }
                } else {
                    onError("Greška pri dohvaćanju menija: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("MenuManager", "Error loading menu details", e)
                onError("Mrežna greška: ${e.message}")
            }
        }
    }

    fun saveOrUpdateMenu(
        date: String,
        category: Int,
        dishIds: List<Int>,
        onSuccess: (DailyMenuDetailsResponse) -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val categoryString = if (category == 1) "lunch" else "dinner"

                val existingMenuResponse = menuService.getTodayMenu(categoryString)

                if (existingMenuResponse.isSuccessful) {
                    val existingMenuItems = existingMenuResponse.body() ?: emptyList()

                    if (existingMenuItems.isNotEmpty()) {
                        val firstItem = existingMenuItems.first()
                        val dailyMenuId = firstItem.dailyMenuId

                        val existingDishIds = existingMenuItems.map { it.dishId }
                        val mergedDishIds = (existingDishIds + dishIds).distinct()

                        Log.d("MenuManager", "Existing menu found (ID: $dailyMenuId). Merging dishes: $existingDishIds + $dishIds = $mergedDishIds")

                        updateMenu(
                            menuId = dailyMenuId,
                            request = UpdateDailyMenuRequest(
                                date = date,
                                category = category,
                                dishIds = mergedDishIds
                            ),
                            onSuccess = onSuccess,
                            onError = onError
                        )
                    } else {
                        Log.d("MenuManager", "No existing menu found. Creating new.")
                        createMenu(
                            request = CreateDailyMenuRequest(
                                date = date,
                                category = category,
                                dishIds = dishIds
                            ),
                            onSuccess = onSuccess,
                            onError = onError
                        )
                    }
                } else {
                    onError("Greška pri provjeri postojećeg menija: ${existingMenuResponse.code()}")
                }
            } catch (e: Exception) {
                Log.e("MenuManager", "Error in saveOrUpdateMenu", e)
                onError("Mrežna greška: ${e.message}")
            }
        }
    }

    fun createMenu(
        request: CreateDailyMenuRequest,
        onSuccess: (DailyMenuDetailsResponse) -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = menuService.createMenu(request)

                if (response.isSuccessful) {
                    response.body()?.let { createdMenu ->
                        Toast.makeText(context, "Meni uspješno dodan!", Toast.LENGTH_SHORT).show()
                        onSuccess(createdMenu)
                    } ?: run {
                        onError("Greška pri kreiranju menija")
                    }
                } else {
                    onError("Greška pri kreiranju: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("MenuManager", "Error creating menu", e)
                onError("Mrežna greška: ${e.message}")
            }
        }
    }

    fun updateMenu(
        menuId: Int,
        request: UpdateDailyMenuRequest,
        onSuccess: (DailyMenuDetailsResponse) -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = menuService.updateMenu(menuId, request)

                if (response.isSuccessful) {
                    response.body()?.let { updatedMenu ->
                        Toast.makeText(context, "Meni uspješno ažuriran!", Toast.LENGTH_SHORT).show()
                        onSuccess(updatedMenu)
                    } ?: run {
                        onError("Greška pri ažuriranju menija")
                    }
                } else {
                    onError("Greška pri ažuriranju: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("MenuManager", "Error updating menu", e)
                onError("Mrežna greška: ${e.message}")
            }
        }
    }

    fun deleteMenu(
        menuId: Int,
        onSuccess: () -> Unit,
        onError: (String) -> Unit
    ) {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = menuService.deleteMenu(menuId)

                if (response.isSuccessful) {
                    Toast.makeText(context, "Meni uspješno obrisan!", Toast.LENGTH_SHORT).show()
                    onSuccess()
                } else {
                    onError("Greška pri brisanju: ${response.code()}")
                }
            } catch (e: Exception) {
                Log.e("MenuManager", "Error deleting menu", e)
                onError("Mrežna greška: ${e.message}")
            }
        }
    }
}