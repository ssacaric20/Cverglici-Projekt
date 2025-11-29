package foi.cverglici.mailauth

import android.content.Context
import foi.cverglici.mailauth.api.RetrofitClient
import foi.cverglici.mailauth.model.LoginRequest
import foi.cverglici.mailauth.model.RegistrationRequest
import foi.cverglici.mailauth.ui.EmailAuthDialog
import foi.cverglici.core.auth.IAuthenticationHandler
import foi.cverglici.core.auth.model.AuthResult
import kotlinx.coroutines.launch
import kotlinx.coroutines.suspendCancellableCoroutine
import kotlin.coroutines.resume

class EmailAuthHandler(
    private val context: Context
) : IAuthenticationHandler {

    override suspend fun login(): AuthResult {
        return suspendCancellableCoroutine { continuation ->
            val dialog = EmailAuthDialog(context, EmailAuthDialog.Mode.LOGIN) { credentials ->
                // This callback is invoked when user submits the form
                kotlinx.coroutines.GlobalScope.launch {
                    val result = performLogin(credentials.email, credentials.password)
                    continuation.resume(result)
                }
            }

            dialog.setOnCancelListener {
                continuation.resume(AuthResult.Cancelled)
            }

            dialog.show()

            continuation.invokeOnCancellation {
                dialog.dismiss()
            }
        }
    }

    override suspend fun register(): AuthResult {
        return suspendCancellableCoroutine { continuation ->
            val dialog = EmailAuthDialog(context, EmailAuthDialog.Mode.REGISTER) { credentials ->
                kotlinx.coroutines.GlobalScope.launch {
                    val result = performRegistration(
                        credentials.name ?: "",
                        credentials.email,
                        credentials.password
                    )
                    continuation.resume(result)
                }
            }

            dialog.setOnCancelListener {
                continuation.resume(AuthResult.Cancelled)
            }

            dialog.show()

            continuation.invokeOnCancellation {
                dialog.dismiss()
            }
        }
    }

    override fun isAvailable(): Boolean {
        return true // Email auth is always available
    }

    private suspend fun performLogin(email: String, password: String): AuthResult {
        return try {
            val request = LoginRequest(email, password)
            val response = RetrofitClient.authService.loginUser(request)

            if (response.isSuccessful) {
                response.body()?.let { authResponse ->
                    AuthResult.Success(
                        userId = authResponse.userId,
                        token = authResponse.token,
                        message = authResponse.message,
                        roleId = authResponse.roleId
                    )
                } ?: AuthResult.Error("Neispravna odgovor od servera")
            } else {
                val errorMsg = response.errorBody()?.string() ?: "Prijava nije uspjela"
                AuthResult.Error(errorMsg)
            }
        } catch (e: Exception) {
            AuthResult.Error("Mrežna greška: ${e.message}", e)
        }
    }

    private suspend fun performRegistration(
        name: String,
        email: String,
        password: String
    ): AuthResult {
        return try {
            val request = RegistrationRequest(name, email, password)
            val response = RetrofitClient.authService.registerUser(request)

            if (response.isSuccessful) {
                response.body()?.let { authResponse ->
                    AuthResult.Success(
                        userId = authResponse.userId,
                        token = authResponse.token,
                        message = authResponse.message,
                        roleId = authResponse.roleId
                    )
                } ?: AuthResult.Error("Neispravna odgovor od servera")
            } else {
                val errorMsg = response.errorBody()?.string() ?: "Registracija nije uspjela"
                AuthResult.Error(errorMsg)
            }
        } catch (e: Exception) {
            AuthResult.Error("Mrežna greška: ${e.message}", e)
        }
    }
}

data class EmailCredentials(
    val email: String,
    val password: String,
    val name: String? = null
)