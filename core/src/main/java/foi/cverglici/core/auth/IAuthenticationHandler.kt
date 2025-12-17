package foi.cverglici.core.auth

import foi.cverglici.core.auth.model.AuthResult

interface IAuthenticationHandler {
    suspend fun login(): AuthResult
    suspend fun register(): AuthResult
    fun isAvailable(): Boolean
}