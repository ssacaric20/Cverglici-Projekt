package foi.cverglici.core.data.api.student.dailymenu

import foi.cverglici.core.data.model.student.dailymenu.DishDetailsResponse
import foi.cverglici.core.data.model.student.dailymenu.DailyMenuItem
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.Query
import okhttp3.MultipartBody
import retrofit2.http.Multipart
import retrofit2.http.POST
import retrofit2.http.Part

interface IDishService {
    @GET("api/DailyMenu/today")
    suspend fun getTodayMenu(@Query("category") category: String): Response<List<DailyMenuItem>>

    @GET("api/DailyMenu/date")
    suspend fun getMenuForDate(@Query("date") date: String): Response<List<DailyMenuItem>>

    @GET("api/Dish/{id}")
    suspend fun getDishDetails(@Path("id") dishId: Int): Response<DishDetailsResponse>

    @Multipart
    @POST("api/Dish/{id}/upload-image")
    suspend fun uploadDishImage(
        @Path("id") dishId: Int,
        @Part image: MultipartBody.Part
    ): Response<Unit>

}