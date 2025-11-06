package foi.cverglici.smartmenza.data.api

import foi.cverglici.smartmenza.data.model.RegistrationRequest
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface AuthService {
    @POST("api/Korisnici/register") // path s backenda
    suspend fun registerUser(@Body request: RegistrationRequest): Response<AuthResponse>
}

data class AuthResponse(
    val userId: Int,
    val message: String,
    val jwtToken: String?
)