package foi.cverglici.core.data.api.employee.statistics

import foi.cverglici.core.data.model.employee.statistics.DishStatisticsResponse
import foi.cverglici.core.data.model.employee.statistics.TopDishesResponse
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.Query

interface IStatisticsService {

    @GET("api/Statistics/top-dishes")
    suspend fun getTopDishes(@Query("count") count: Int = 10): Response<TopDishesResponse>

    @GET("api/Statistics/dish/{dishId}")
    suspend fun getDishStatistics(@Path("dishId") dishId: Int): Response<DishStatisticsResponse>
}