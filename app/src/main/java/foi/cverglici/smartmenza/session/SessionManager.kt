package foi.cverglici.smartmenza.session

import android.content.Context
import android.content.SharedPreferences
import androidx.core.content.edit

class SessionManager(context: Context) {
    private val prefs: SharedPreferences = context.getSharedPreferences("AUTH_PREFS", Context.MODE_PRIVATE)

    companion object {
        const val USER_TOKEN = "user_token"
        const val USER_ID = "user_id"
        const val ROLE_ID = "role_id"

        // Role constants
        const val ROLE_STUDENT = 2
        const val ROLE_EMPLOYEE = 1
    }

    /**
     * save authentication data after login
     */
    fun saveAuthData(userId: Int, token: String, roleId: Int) {
        prefs.edit {
            putInt(USER_ID, userId)
            putString(USER_TOKEN, token)
            putInt(ROLE_ID, roleId)
        }
    }

    /**
     * save JWT token from API
     */
    fun saveAuthToken(token: String) {
        prefs.edit { putString(USER_TOKEN, token) }
    }

    /**
     * get JWT token (for API requests)
     */
    fun fetchAuthToken(): String? {
        return prefs.getString(USER_TOKEN, null)
    }

    /**
     * get user ID
     */
    fun getUserId(): Int {
        return prefs.getInt(USER_ID, -1)
    }

    /**
     * get role ID
     */
    fun getRoleId(): Int {
        return prefs.getInt(ROLE_ID, -1)
    }

    /**
     * check if user is a student
     */
    fun isStudent(): Boolean {
        return getRoleId() == ROLE_STUDENT
    }

    /**
     * check if user is an employee
     */
    fun isEmployee(): Boolean {
        return getRoleId() == ROLE_EMPLOYEE
    }

    /**
     * check if user is logged in
     */
    fun isLoggedIn(): Boolean {
        return fetchAuthToken() != null && getRoleId() != -1
    }

    /**
     * logout
     */
    fun logout() {
        prefs.edit { clear() }
    }
}