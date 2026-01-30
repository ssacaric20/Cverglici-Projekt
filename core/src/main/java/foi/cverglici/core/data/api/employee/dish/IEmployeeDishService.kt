package foi.cverglici.core.data.api.employee.dish

import foi.cverglici.core.data.model.employee.dish.CreateDishRequest
import foi.cverglici.core.data.model.employee.dish.DishListItem
import foi.cverglici.core.data.model.employee.dish.UpdateDishRequest
import foi.cverglici.core.data.model.student.dailymenu.DishDetailsResponse
import retrofit2.Response
import retrofit2.http.*

interface IEmployeeDishService {
    @GET("api/Dish")
    suspend fun getAllDishes(): Response<List<DishListItem>>

    @GET("api/Dish/{id}")
    suspend fun getDishDetails(@Path("id") dishId: Int): Response<DishDetailsResponse>

    @POST("api/Dish")
    suspend fun createDish(@Body request: CreateDishRequest): Response<DishDetailsResponse>

    @PUT("api/Dish/{id}")
    suspend fun updateDish(
        @Path("id") dishId: Int,
        @Body request: UpdateDishRequest
    ): Response<DishDetailsResponse>

    @DELETE("api/Dish/{id}")
    suspend fun deleteDish(@Path("id") dishId: Int): Response<Map<String, String>>
}