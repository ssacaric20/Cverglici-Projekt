package foi.cverglici.core.data.api.student.dailyfoodintake

import foi.cverglici.core.data.model.student.dailyfoodintake.AddDailyFoodIntakeRequest
import foi.cverglici.core.data.model.student.dailyfoodintake.DailyFoodIntakeResponse
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path

interface IDailyFoodIntakeService {

    @GET("api/DailyFoodIntake/today")
    suspend fun getMyToday(): List<DailyFoodIntakeResponse>

    @POST("api/DailyFoodIntake/today")
    suspend fun addToMyToday(@Body request: AddDailyFoodIntakeRequest): DailyFoodIntakeResponse

    @DELETE("api/DailyFoodIntake/{id}")
    suspend fun delete(@Path("id") id: Int): retrofit2.Response<Unit>

}
