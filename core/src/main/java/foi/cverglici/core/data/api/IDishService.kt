package foi.cverglici.core.data.api

import foi.cverglici.core.data.model.DishDetailsResponse
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Path

interface IDishService {
    @GET("api/Dish/{id}")
    suspend fun getDishDetails(@Path("id") dishId: Int): Response<DishDetailsResponse>
}