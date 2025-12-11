package foi.cverglici.googleauth


import android.app.Activity
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.util.Log
import androidx.activity.result.ActivityResultLauncher
import com.google.android.gms.auth.api.signin.GoogleSignIn
import com.google.android.gms.auth.api.signin.GoogleSignInAccount
import com.google.android.gms.auth.api.signin.GoogleSignInClient
import com.google.android.gms.auth.api.signin.GoogleSignInOptions
import com.google.android.gms.common.api.ApiException
import foi.cverglici.core.auth.IAuthenticationHandler
import foi.cverglici.core.auth.model.AuthResult
import kotlinx.coroutines.suspendCancellableCoroutine
import kotlin.coroutines.resume

class GoogleAuthHandler(
    private val context: Context,
    private val activityResultLauncher: ActivityResultLauncher<Intent>
) : IAuthenticationHandler {

    private val serverClientId: String? by lazy {
        retrieveServerClientId()
    }

    private var pendingContinuation: ((AuthResult) -> Unit)? = null

    override suspend fun login(): AuthResult {
        return performGoogleSignIn()
    }

    override suspend fun register(): AuthResult {
        // Google Sign-In works the same for login and registration
        // The backend determines if it's a new user or existing user
        return performGoogleSignIn()
    }

    override fun isAvailable(): Boolean {
        return serverClientId != null
    }

    /**
     * Google Sign-In flow
     */
    private suspend fun performGoogleSignIn(): AuthResult {
        if (!isAvailable()) {
            return AuthResult.Error("Google Sign-In nije konfiguriran")
        }

        return suspendCancellableCoroutine { continuation ->
            pendingContinuation = { result ->
                continuation.resume(result)
            }

            // Build GoogleSignInOptions with the non-null serverClientId
            val gso = GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                .requestEmail()
                .requestServerAuthCode(serverClientId!!) // Safe to use !! because isAvailable() checked it
                .build()

            val googleSignInClient = GoogleSignIn.getClient(context, gso)
            val signInIntent = googleSignInClient.signInIntent
            activityResultLauncher.launch(signInIntent)

            continuation.invokeOnCancellation {
                pendingContinuation = null
            }
        }
    }

    /**
     * call when activity result is received
     */
    fun handleActivityResult(resultCode: Int, data: Intent?) {
        Log.d("GoogleAuthHandler", "Activity result received: resultCode=$resultCode, data=$data")

        if (resultCode == Activity.RESULT_OK) {
            Log.d("GoogleAuthHandler", "Result OK, processing sign-in")
            val task = GoogleSignIn.getSignedInAccountFromIntent(data)
            try {
                val account = task.getResult(ApiException::class.java)

                Log.d("GoogleAuthHandler", "Account retrieved successfully")
                Log.d("GoogleAuthHandler", "Display Name: ${account?.displayName}")
                Log.d("GoogleAuthHandler", "Email: ${account?.email}")
                Log.d("GoogleAuthHandler", "Id: ${account?.id}")
                Log.d("GoogleAuthHandler", "IdToken: ${account?.idToken}")
                Log.d("GoogleAuthHandler", "Auth code: ${account?.serverAuthCode}")

                handleSignInResult(account)
            } catch (e: ApiException) {
                Log.e("GoogleAuthHandler", "Sign in failed", e)
                Log.e("GoogleAuthHandler", "Failed to get account result: ${e.statusCode}, message: ${e.message}")
                pendingContinuation?.invoke(
                    AuthResult.Error("Google prijava nije uspjela: ${e.message}", e)
                )
                pendingContinuation = null
            }
        } else {
            Log.w("GoogleAuthHandler", "Sign in cancelled or failed")
            Log.e("GoogleAuthHandler", "Google sign-in failed, resultCode=$resultCode, intentData=$data")
            pendingContinuation?.invoke(AuthResult.Cancelled)
            pendingContinuation = null
        }
    }

    /**
     * signed-in Google account
     */
    private fun handleSignInResult(account: GoogleSignInAccount?) {
        if (account == null) {
            pendingContinuation?.invoke(
                AuthResult.Error("Google račun nije pronađen")
            )
            pendingContinuation = null
            return
        }

        val authorizationCode = account.serverAuthCode

        if (authorizationCode != null) {
            Log.d("GoogleAuthHandler", "Authorization code is present, sending to backend")
            // TODO: Send authorization code to your backend
            // For now, we'll create a success result with mock data
            // You should replace this with actual backend call
            sendAuthCodeToBackend(authorizationCode)
        } else {
            Log.e("GoogleAuthHandler", "Authorization code is null")
            pendingContinuation?.invoke(
                AuthResult.Error("Nije moguće dohvatiti autorizacijski kod")
            )
            pendingContinuation = null
        }
    }

    /**
     * authorization code to backend
     * TODO: Implement actual API call to your backend
     */
    private fun sendAuthCodeToBackend(authCode: String) {
        // This is where you'd call your backend API
        // For now, creating a mock success response

        // Example of what the implementation should look like:
        /*
        lifecycleScope.launch {
            try {
                val response = YourApiClient.googleAuth(authCode)
                if (response.isSuccessful) {
                    response.body()?.let { authResponse ->
                        pendingContinuation?.invoke(
                            AuthResult.Success(
                                userId = authResponse.userId,
                                token = authResponse.token,
                                message = authResponse.message
                            )
                        )
                    }
                } else {
                    pendingContinuation?.invoke(
                        AuthResult.Error("Backend authentication failed")
                    )
                }
            } catch (e: Exception) {
                pendingContinuation?.invoke(
                    AuthResult.Error("Network error", e)
                )
            } finally {
                pendingContinuation = null
            }
        }
        */

        // Temporary mock response
        Log.w("GoogleAuthHandler", "Backend integration not implemented yet")
        pendingContinuation?.invoke(
            AuthResult.Error("Backend integracija još nije implementirana")
        )
        pendingContinuation = null
    }

    /**
     * server client ID from AndroidManifest
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
            e.printStackTrace()
            null
        }
    }

    /**
     * sign out from Google
     */
    fun signOut() {
        // sign out if serverClientId available
        serverClientId?.let { clientId ->
            val gso = GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                .requestEmail()
                .requestServerAuthCode(clientId)
                .build()

            val googleSignInClient = GoogleSignIn.getClient(context, gso)
            googleSignInClient.signOut()
        }
    }
}