package foi.cverglici.core.data.api.ai

import okhttp3.Interceptor
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.concurrent.TimeUnit

object RetrofitAi {

    // Choose ONE (must end with /)
    private const val BASE_URL = "http://10.0.2.2:5166/"
    //private const val BASE_URL = "https://smartmenza-h5csfahadafnajaq.germanywestcentral-01.azurewebsites.net/"

    private val loggingInterceptor = HttpLoggingInterceptor().apply {
        level = HttpLoggingInterceptor.Level.BODY
    }

    fun create(token: String?): IAiAnalysisService {
        val authInterceptor = Interceptor { chain ->
            val reqBuilder = chain.request().newBuilder()

            if (!token.isNullOrBlank()) {
                reqBuilder.addHeader("Authorization", "Bearer $token")
            }

            chain.proceed(reqBuilder.build())
        }

        val client = OkHttpClient.Builder()
            .addInterceptor(authInterceptor)
            .addInterceptor(loggingInterceptor)
            .connectTimeout(30, TimeUnit.SECONDS)
            .readTimeout(30, TimeUnit.SECONDS)
            .writeTimeout(30, TimeUnit.SECONDS)
            .build()

        return Retrofit.Builder()
            .baseUrl(BASE_URL)
            .client(client)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
            .create(IAiAnalysisService::class.java)
    }
}
