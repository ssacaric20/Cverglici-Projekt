package foi.cverglici.core.data.api.auth

import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.concurrent.TimeUnit

object RetrofitClient{
    private const val BASE_URL = "http://10.0.2.2:5166/"
    //private const val BASE_URL = "https://smartmenza-h5csfahadafnajaq.germanywestcentral-01.azurewebsites.net"
    private val loggingInterceptor = HttpLoggingInterceptor().apply {
        level = HttpLoggingInterceptor.Level.BODY  // Logs full request/response
    }

    // OkHttp client with logging and timeouts
    private val client = OkHttpClient.Builder()
        .addInterceptor(loggingInterceptor)
        .connectTimeout(30, TimeUnit.SECONDS)    // Connection timeout
        .readTimeout(30, TimeUnit.SECONDS)       // Read timeout
        .writeTimeout(30, TimeUnit.SECONDS)      // Write timeout
        .build()

    val authService: IAuthService by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .client(client)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
            .create(IAuthService::class.java)
    }
}