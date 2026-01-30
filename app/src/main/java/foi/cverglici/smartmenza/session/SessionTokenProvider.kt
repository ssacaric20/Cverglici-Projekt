package foi.cverglici.smartmenza.session

import android.content.Context
import foi.cverglici.core.auth.ITokenProvider

class SessionTokenProvider(context: Context) : ITokenProvider {
    private val sessionManager = SessionManager(context)

    override fun getToken(): String? {
        return sessionManager.fetchAuthToken()
    }
}