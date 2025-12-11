package foi.cverglici.core.data.model.auth

data class AuthResponse(
    val userId: Int,
    val message: String,
    val token: String,
    val roleId: Int
)
