package foi.cverglici.mailauth.model

data class LoginRequest(
    val email: String,
    val passwordHash: String
)
