package foi.cverglici.core.data.api.auth

import foi.cverglici.core.data.model.auth.AuthResponse
import foi.cverglici.core.data.model.auth.GoogleLoginRequest
import foi.cverglici.core.data.model.auth.LoginRequest
import foi.cverglici.core.data.model.auth.RegistrationRequest
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface IAuthService {
    @POST("api/User/login")
    suspend fun loginUser(@Body request: LoginRequest): Response<AuthResponse>

    @POST("api/User/register")
    suspend fun registerUser(@Body request: RegistrationRequest): Response<AuthResponse>

    @POST("api/User/google-login")
    suspend fun googleLogin(@Body request: GoogleLoginRequest): Response<AuthResponse>
}
