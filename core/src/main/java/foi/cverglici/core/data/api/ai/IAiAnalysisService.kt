package foi.cverglici.core.data.api.ai

import foi.cverglici.core.data.model.ai.AnalyzeFoodRequest
import foi.cverglici.core.data.model.ai.FoodAnalysisResult
import foi.cverglici.core.data.model.ai.NutritionAnalyzeRequest
import foi.cverglici.core.data.model.ai.NutritionEstimateResponse
import okhttp3.ResponseBody
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface IAiAnalysisService {

    // Typed (your current usage)
    @POST("api/Food/analyze")
    suspend fun analyzeAllergens(@Body req: AnalyzeFoodRequest): Response<FoodAnalysisResult>

    @POST("api/food/nutrition")
    suspend fun analyzeNutrition(@Body req: NutritionAnalyzeRequest): Response<NutritionEstimateResponse>

    // Raw (debug) – lets you see exact JSON coming from backend/OpenAI
    @POST("api/Food/analyze")
    suspend fun analyzeAllergensRaw(@Body req: AnalyzeFoodRequest): Response<ResponseBody>

    @POST("api/food/nutrition")
    suspend fun analyzeNutritionRaw(@Body req: NutritionAnalyzeRequest): Response<ResponseBody>
}
