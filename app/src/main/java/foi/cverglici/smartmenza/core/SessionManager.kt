package foi.cverglici.smartmenza.core

import android.content.Context
import android.content.SharedPreferences
import androidx.core.content.edit

class SessionManager(context: Context) {
    private val prefs: SharedPreferences = context.getSharedPreferences("AUTH_PREFS", Context.MODE_PRIVATE)

    companion object {
        const val USER_TOKEN = "user_token"
    }

    // pohrana JWT tokena od API
    fun saveAuthToken(token: String) {
        prefs.edit { putString(USER_TOKEN, token) }
    }

    //dohva tokena (za svaki API zahtjev)
    fun fetchAuthToken(): String? {
        return prefs.getString(USER_TOKEN, null)
    }

    // odjava = brisanje tokena
    fun logout() {
        prefs.edit { remove(USER_TOKEN) }
        // Ovdje treba dodati navigaciju na ekran za prijavu/registraciju
    }
}