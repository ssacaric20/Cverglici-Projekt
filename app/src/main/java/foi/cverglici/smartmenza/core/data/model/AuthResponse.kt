package foi.cverglici.smartmenza.core.data.model

data class AuthResponse(
    val userId: Int,
    val message: String,
    val token: String
)
