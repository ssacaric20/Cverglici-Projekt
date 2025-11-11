package foi.cverglici.smartmenza.data.model

data class AuthResponse(
    val userId: Int,
    val message: String,
    val token: String
)
