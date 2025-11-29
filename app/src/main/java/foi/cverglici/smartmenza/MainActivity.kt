package foi.cverglici.smartmenza

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import foi.cverglici.smartmenza.session.SessionManager
import foi.cverglici.smartmenza.ui.employee.menu.EmployeeMenuListFragment
import foi.cverglici.smartmenza.ui.student.menu.MenuListFragment

class MainActivity : AppCompatActivity() {

    private lateinit var sessionManager: SessionManager

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        sessionManager = SessionManager(applicationContext)

        // Check if user is logged in
        if (!sessionManager.isLoggedIn()) {
            navigateToLogin()
            return
        }

        // Navigate based on role
        if (savedInstanceState == null) {
            navigateBasedOnRole()
        }
    }

    /**
     * navigate based on user role
     */
    private fun navigateBasedOnRole() {
        when {
            sessionManager.isStudent() -> {
                // student sees menu list
                supportFragmentManager.beginTransaction()
                    .replace(R.id.fragmentContainer, MenuListFragment())
                    .commit()
            }
            sessionManager.isEmployee() -> {
                // employee - TODO
                supportFragmentManager.beginTransaction()
                    .replace(R.id.fragmentContainer, EmployeeMenuListFragment())
                    .commit()
            }
            else -> {
                // unknown role - logout and go to login
                Toast.makeText(this, "Nepoznata uloga korisnika", Toast.LENGTH_LONG).show()
                logout()
            }
        }
    }

    /**
     * navigate to login screen
     */
    private fun navigateToLogin() {
        val intent = Intent(this, AuthActivity::class.java)
        intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
        startActivity(intent)
        finish()
    }

    /**
     * logout function - call from toolbar/menu
     */
    fun logout() {
        sessionManager.logout()
        navigateToLogin()
    }
}