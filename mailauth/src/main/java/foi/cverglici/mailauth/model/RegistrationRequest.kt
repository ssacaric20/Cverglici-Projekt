package foi.cverglici.mailauth.model

data class RegistrationRequest(
    val name: String,
    val email: String,
    val password: String
)
