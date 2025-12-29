package foi.cverglici.core.data.api.employee.dailymenu

import foi.cverglici.core.data.model.employee.dailymenu.CreateDailyMenuRequest
import foi.cverglici.core.data.model.employee.dailymenu.DailyMenuDetailsResponse
import foi.cverglici.core.data.model.employee.dailymenu.UpdateDailyMenuRequest
import foi.cverglici.core.data.model.student.dailymenu.DailyMenuItem
import retrofit2.Response
import retrofit2.http.*

interface IEmployeeMenuService {
    @GET("api/DailyMenu/{id}")
    suspend fun getMenuById(@Path("id") menuId: Int): Response<DailyMenuDetailsResponse>

    @GET("api/DailyMenu/today")
    suspend fun getTodayMenu(@Query("category") category: String): Response<List<DailyMenuItem>>

    @POST("api/DailyMenu")
    suspend fun createMenu(@Body request: CreateDailyMenuRequest): Response<DailyMenuDetailsResponse>

    @PUT("api/DailyMenu/{id}")
    suspend fun updateMenu(
        @Path("id") menuId: Int,
        @Body request: UpdateDailyMenuRequest
    ): Response<DailyMenuDetailsResponse>

    @DELETE("api/DailyMenu/{id}")
    suspend fun deleteMenu(@Path("id") menuId: Int): Response<Map<String, String>>
}