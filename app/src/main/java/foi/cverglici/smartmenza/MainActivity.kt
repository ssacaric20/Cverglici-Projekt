package foi.cverglici.smartmenza

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import foi.cverglici.smartmenza.core.SessionManager

class MainActivity : AppCompatActivity() {

    private lateinit var sessionManager: SessionManager

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        sessionManager = SessionManager(applicationContext)

        // jel prijavljen
        if (sessionManager.fetchAuthToken() == null) {
            // ako nema tokena, nije prijavljen - navigacija na LoginActivity
            navigateToLogin()
            return
        }

        // ako je prijavljen, nastavi
        setContentView(R.layout.activity_main)

        // tu ide logika za pocetni ekran
    }

    private fun navigateToLogin() {
        val intent = Intent(this, LoginActivity::class.java)
        // u slucaju da proba stisnut back
        intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
        startActivity(intent)
        finish()
    }

    // 💡 odjava
    fun onLogoutButtonClicked() {
        sessionManager.logout() // brise token
        navigateToLogin()      // navigacija na LoginActivity
    }
}