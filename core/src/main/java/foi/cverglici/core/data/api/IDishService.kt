package foi.cverglici.core.data.api

import foi.cverglici.core.menu.model.DailyMenuItem
import foi.cverglici.core.data.model.DishDetailsResponse
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.Query

interface IDishService {
    @GET("api/DailyMenu/today")
    suspend fun getTodayMenu(): Response<List<DailyMenuItem>>

    @GET("api/DailyMenu/date")
    suspend fun getMenuForDate(@Query("date") date: String): Response<List<DailyMenuItem>>

    @GET("api/Dish/{id}")
    suspend fun getDishDetails(@Path("id") dishId: Int): Response<DishDetailsResponse>
}