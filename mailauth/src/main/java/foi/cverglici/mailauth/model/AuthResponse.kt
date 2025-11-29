package foi.cverglici.mailauth.model

data class AuthResponse(
    val userId: Int,
    val message: String,
    val token: String,
    val roleId: Int
)
