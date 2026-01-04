package foi.cverglici.core.data.api.ai

import foi.cverglici.core.data.model.ai.AnalyzeFoodRequest
import foi.cverglici.core.data.model.ai.FoodAnalysisResult
import foi.cverglici.core.data.model.ai.NutritionAnalyzeRequest
import foi.cverglici.core.data.model.ai.NutritionEstimateResponse
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST
interface IAiAnalysisService {

    @POST("api/food/analyze")
    suspend fun analyzeAllergens(@Body req: AnalyzeFoodRequest): Response<FoodAnalysisResult>

    @POST("api/nutrition/analyze")
    suspend fun analyzeNutrition(@Body req: NutritionAnalyzeRequest): Response<NutritionEstimateResponse>
}