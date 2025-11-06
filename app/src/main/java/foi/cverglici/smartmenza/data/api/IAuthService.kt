package foi.cverglici.smartmenza.data.api

import foi.cverglici.smartmenza.data.model.AuthResponse
import foi.cverglici.smartmenza.data.model.LoginRequest
import foi.cverglici.smartmenza.data.model.RegistrationRequest
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface IAuthService {
    @POST("api/Korisnici/register") // path s backenda
    suspend fun registerUser(@Body request: RegistrationRequest): Response<AuthResponse>

    @POST("api/Korisnici/login") // path s backenda
    suspend fun loginUser(@Body request: LoginRequest): Response<AuthResponse>
}
