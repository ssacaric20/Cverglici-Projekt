package foi.cverglici.core.data.api.student.nutritiongoal

import foi.cverglici.core.data.model.student.nutritiongoal.NutritionGoalResponse
import foi.cverglici.core.data.model.student.nutritiongoal.SetNutritionGoalRequest
import foi.cverglici.core.data.model.student.nutritiongoal.TodayNutritionProgressResponse
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.PUT

interface INutritionGoalService {

    @GET("api/NutritionGoal/me")
    suspend fun getMyGoal(): NutritionGoalResponse

    @PUT("api/NutritionGoal/me")
    suspend fun setMyGoal(@Body request: SetNutritionGoalRequest): NutritionGoalResponse

    @GET("api/nutrition-goal-statistics/today")
    suspend fun getTodayProgress(): TodayNutritionProgressResponse
}
