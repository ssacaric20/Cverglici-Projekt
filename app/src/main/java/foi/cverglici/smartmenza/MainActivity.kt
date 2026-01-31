package foi.cverglici.smartmenza

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.Fragment
import com.google.android.material.bottomnavigation.BottomNavigationView
import foi.cverglici.navigation.NavigationManager
import foi.cverglici.navigation.Enums.NavigationRole
import foi.cverglici.smartmenza.session.SessionManager
import foi.cverglici.smartmenza.ui.employee.menu.EmployeeMenuListFragment
import foi.cverglici.smartmenza.ui.student.favorites.FavoritesFragment
import foi.cverglici.smartmenza.ui.student.menu.MenuListFragment
import foi.cverglici.smartmenza.ui.student.goals.GoalsFragment


class MainActivity : AppCompatActivity() {

    private lateinit var sessionManager: SessionManager
    private lateinit var navigationManager: NavigationManager
    private lateinit var bottomNavigation: BottomNavigationView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        sessionManager = SessionManager(applicationContext)

        if (!sessionManager.isLoggedIn()) {
            navigateToLogin()
            return
        }

        bottomNavigation = findViewById(R.id.bottomNavigation)
        navigationManager = NavigationManager(
            activity = this,
            containerId = R.id.fragmentContainer,
            bottomNavigationView = bottomNavigation
        )

        if (savedInstanceState == null) {
            setupNavigationBasedOnRole()
        }
    }

    private fun setupNavigationBasedOnRole() {
        when {
            sessionManager.isStudent() -> {
                navigationManager.setupNavigation(NavigationRole.STUDENT, ::getFragmentForTag)
            }
            sessionManager.isEmployee() -> {
                navigationManager.setupNavigation(NavigationRole.EMPLOYEE, ::getFragmentForTag)
            }
            else -> {
                Toast.makeText(this, "Nepoznata uloga korisnika", Toast.LENGTH_LONG).show()
                logout()
            }
        }
    }

    private fun getFragmentForTag(tag: String): Fragment {
        return when (tag) {
            "menu" -> MenuListFragment()
            "favorites" -> FavoritesFragment()
            "goals" -> GoalsFragment()
            "employee_menu" -> EmployeeMenuListFragment()
            "statistics" -> TODO("Statistics - will be implemented later")
            "ai_tools" -> TODO("AI Tools - will be implemented later")
            else -> MenuListFragment()
        }
    }

    private fun navigateToLogin() {
        val intent = Intent(this, AuthActivity::class.java)
        intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
        startActivity(intent)
        finish()
    }

    fun logout() {
        sessionManager.logout()
        navigateToLogin()
    }
}