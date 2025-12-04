package foi.cverglici.core.data.model.auth

data class LoginRequest(
    val email: String,
    val passwordHash: String
)
