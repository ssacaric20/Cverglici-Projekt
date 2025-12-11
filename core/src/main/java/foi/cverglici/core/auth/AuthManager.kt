package foi.cverglici.core.auth

/**
 * manages multiple authentication handlers
 */

class AuthenticationManager {
    private val handlers = mutableMapOf<AuthType, IAuthenticationHandler>()

    enum class AuthType {
        EMAIL,
        GOOGLE
    }

    /**
     * register an authentication handler
     */
    fun registerHandler(type: AuthType, handler: IAuthenticationHandler) {
        if (handler.isAvailable()) {
            handlers[type] = handler
        }
    }

    /**
     * get a specific authentication handler
     */
    fun getHandler(type: AuthType): IAuthenticationHandler? {
        return handlers[type]
    }

    /**
     * is a specific auth type is available
     */
    fun isAuthTypeAvailable(type: AuthType): Boolean {
        return handlers[type]?.isAvailable() == true
    }
}