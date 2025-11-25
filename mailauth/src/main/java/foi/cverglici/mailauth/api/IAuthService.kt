package foi.cverglici.mailauth.api

import foi.cverglici.mailauth.model.AuthResponse
import foi.cverglici.mailauth.model.LoginRequest
import foi.cverglici.mailauth.model.RegistrationRequest
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface IAuthService {
    @POST("api/User/register") // path s backenda
    suspend fun registerUser(@Body request: RegistrationRequest): Response<AuthResponse>

    @POST("api/User/login") // path s backenda
    suspend fun loginUser(@Body request: LoginRequest): Response<AuthResponse>
}
