package foi.cverglici.googleauth

import foi.cverglici.core.data.model.auth.AuthResponse
import android.app.Activity
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.util.Log
import androidx.activity.result.ActivityResultLauncher
import com.google.android.gms.auth.api.signin.GoogleSignIn
import com.google.android.gms.auth.api.signin.GoogleSignInAccount
import com.google.android.gms.auth.api.signin.GoogleSignInOptions
import com.google.android.gms.common.api.ApiException
import foi.cverglici.core.auth.IAuthenticationHandler
import foi.cverglici.core.auth.model.AuthResult
import foi.cverglici.core.data.api.auth.RetrofitClient
import foi.cverglici.core.data.model.auth.GoogleLoginRequest
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.suspendCancellableCoroutine
import kotlinx.coroutines.withContext
import kotlin.coroutines.resume
import retrofit2.Response


class GoogleAuthHandler(
    private val context: Context,
    private val activityResultLauncher: ActivityResultLauncher<Intent>
) : IAuthenticationHandler {

    private val serverClientId: String? by lazy { retrieveServerClientId() }

    private var pendingContinuation: ((AuthResult) -> Unit)? = null

    override suspend fun login(): AuthResult = performGoogleSignIn()

    override suspend fun register(): AuthResult = performGoogleSignIn()

    override fun isAvailable(): Boolean = serverClientId != null

    /**
     * Google Sign-In flow
     */
    private suspend fun performGoogleSignIn(): AuthResult {
        if (!isAvailable()) {
            return AuthResult.Error("Google Sign-In nije konfiguriran (ServerClientId missing)")
        }

        return suspendCancellableCoroutine { continuation ->
            pendingContinuation = { result ->
                continuation.resume(result)
            }

            // IMPORTANT: Backend očekuje ID token (TokenId), ne serverAuthCode
            val gso = GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                .requestEmail()
                .requestIdToken(serverClientId!!)
                .build()

            val googleSignInClient = GoogleSignIn.getClient(context, gso)
            activityResultLauncher.launch(googleSignInClient.signInIntent)

            continuation.invokeOnCancellation {
                pendingContinuation = null
            }
        }
    }

    /**
     * Call when activity result is received
     */
    fun handleActivityResult(resultCode: Int, data: Intent?) {
        Log.d("GoogleAuthHandler", "Activity result received: resultCode=$resultCode, data=$data")

        if (resultCode == Activity.RESULT_OK) {
            val task = GoogleSignIn.getSignedInAccountFromIntent(data)
            try {
                val account = task.getResult(ApiException::class.java)

                Log.d("GoogleAuthHandler", "Account retrieved successfully")
                Log.d("GoogleAuthHandler", "Display Name: ${account?.displayName}")
                Log.d("GoogleAuthHandler", "Email: ${account?.email}")
                Log.d("GoogleAuthHandler", "Id: ${account?.id}")
                Log.d("GoogleAuthHandler", "IdToken: ${account?.idToken}")

                handleSignInResult(account)
            } catch (e: ApiException) {
                Log.e("GoogleAuthHandler", "Sign in failed", e)
                Log.e(
                    "GoogleAuthHandler",
                    "Failed to get account result: ${e.statusCode}, message: ${e.message}"
                )
                pendingContinuation?.invoke(
                    AuthResult.Error("Google prijava nije uspjela: ${e.message}", e)
                )
                pendingContinuation = null
            }
        } else {
            Log.w("GoogleAuthHandler", "Sign in cancelled or failed")
            pendingContinuation?.invoke(AuthResult.Cancelled)
            pendingContinuation = null
        }
    }

    /**
     * Signed-in Google account
     */
    private fun handleSignInResult(account: GoogleSignInAccount?) {
        if (account == null) {
            pendingContinuation?.invoke(AuthResult.Error("Google račun nije pronađen"))
            pendingContinuation = null
            return
        }

        val idToken = account.idToken
        if (!idToken.isNullOrBlank()) {
            sendIdTokenToBackend(idToken)
        } else {
            // Najčešći razlog: ServerClientId nije WEB client ID
            pendingContinuation?.invoke(
                AuthResult.Error("Nije moguće dohvatiti ID token. Provjeri ServerClientId (mora biti WEB client ID).")
            )
            pendingContinuation = null
        }
    }

    /**
     * Sends ID token to backend: POST api/User/google-login
     */
    private fun sendIdTokenToBackend(idToken: String) {
        CoroutineScope(Dispatchers.IO).launch {
            try {
                val response: Response<AuthResponse> =
                    RetrofitClient.authService.googleLogin(GoogleLoginRequest(tokenId = idToken))

                Log.d("GoogleAuthHandler", "googleLogin HTTP ${response.code()} successful=${response.isSuccessful}")

                if (response.isSuccessful) {
                    val body: AuthResponse? = response.body()
                    if (body != null) {
                        withContext(Dispatchers.Main) {
                            pendingContinuation?.invoke(
                                AuthResult.Success(
                                    userId = body.userId,
                                    token = body.token,
                                    message = body.message,
                                    roleId = body.roleId
                                )
                            )
                            pendingContinuation = null
                        }
                    } else {
                        withContext(Dispatchers.Main) {
                            pendingContinuation?.invoke(AuthResult.Error("Prazan odgovor servera"))
                            pendingContinuation = null
                        }
                    }
                } else {
                    val errText: String? = response.errorBody()?.string()
                    Log.e("GoogleAuthHandler", "googleLogin errorBody=$errText")
                    val msg = "Google login nije uspio. HTTP ${response.code()} ${errText ?: ""}".trim()

                    withContext(Dispatchers.Main) {
                        pendingContinuation?.invoke(AuthResult.Error(msg))
                        pendingContinuation = null
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    pendingContinuation?.invoke(AuthResult.Error("Network error: ${e.message}", e))
                    pendingContinuation = null
                }
            }
        }
    }


    /**
     * Server client ID from AndroidManifest meta-data:
     * com.google.android.gms.auth.api.signin.ServerClientId
     */
    private fun retrieveServerClientId(): String? {
        return try {
            val appInfo = context.packageManager.getApplicationInfo(
                context.packageName,
                PackageManager.GET_META_DATA
            )
            appInfo.metaData?.getString("com.google.android.gms.auth.api.signin.ServerClientId")
        } catch (e: PackageManager.NameNotFoundException) {
            Log.e("GoogleAuthHandler", "Failed to get server client ID", e)
            null
        }
    }

    /**
     * Sign out from Google
     */
    fun signOut() {
        serverClientId?.let { clientId ->
            val gso = GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                .requestEmail()
                .requestIdToken(clientId)
                .build()

            val googleSignInClient = GoogleSignIn.getClient(context, gso)
            googleSignInClient.signOut()
        }
    }
}
