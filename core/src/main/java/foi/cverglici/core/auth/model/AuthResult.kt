package foi.cverglici.core.auth.model

sealed class AuthResult {
    data class Success(
        val userId: Int,
        val token: String,
        val message: String
    ) : AuthResult()

    data class Error(
        val message: String,
        val exception: Exception? = null
    ) : AuthResult()

    object Cancelled : AuthResult()
}