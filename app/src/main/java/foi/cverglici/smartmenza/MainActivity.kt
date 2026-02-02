package foi.cverglici.smartmenza

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.Fragment
import com.google.android.material.bottomnavigation.BottomNavigationView
import foi.cverglici.navigation.NavigationManager
import foi.cverglici.navigation.Enums.NavigationRole
import foi.cverglici.topbar.TopBarConfig
import foi.cverglici.topbar.TopBarManager
import foi.cverglici.topbar.listeners.OnTopBarActionListener
import foi.cverglici.smartmenza.session.SessionManager
import foi.cverglici.smartmenza.ui.employee.ai.tools.AiToolsFragment
import foi.cverglici.smartmenza.ui.employee.menu.EmployeeMenuListFragment
import foi.cverglici.smartmenza.ui.employee.statistics.StatisticsFragment
import foi.cverglici.smartmenza.ui.student.favorites.FavoritesFragment
import foi.cverglici.smartmenza.ui.student.menu.MenuListFragment
import foi.cverglici.smartmenza.ui.student.goals.GoalsFragment

class MainActivity : AppCompatActivity(), OnTopBarActionListener {

    private lateinit var sessionManager: SessionManager
    private lateinit var navigationManager: NavigationManager
    private lateinit var topBarManager: TopBarManager
    private lateinit var bottomNavigation: BottomNavigationView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        sessionManager = SessionManager(applicationContext)

        if (!sessionManager.isLoggedIn()) {
            navigateToLogin()
            return
        }

        // Initialize views
        val topBarView = findViewById<android.view.View>(R.id.topBar)
        bottomNavigation = findViewById(R.id.bottomNavigation)

        // Setup TopBar
        topBarManager = TopBarManager(this, topBarView)
        setupTopBar()

        // Setup Navigation
        navigationManager = NavigationManager(
            activity = this,
            containerId = R.id.fragmentContainer,
            bottomNavigationView = bottomNavigation
        )

        if (savedInstanceState == null) {
            setupNavigationBasedOnRole()
        }
    }

    private fun setupTopBar() {
        val config = when {
            sessionManager.isStudent() -> TopBarConfig(
                title = "Smart Menza - Student",
                backgroundColor = R.color.orange_primary,
                textColor = R.color.white,
                showUserMenu = true
            )
            sessionManager.isEmployee() -> TopBarConfig(
                title = "Smart Menza - Zaposlenik",
                backgroundColor = R.color.soft_teal,
                textColor = R.color.white,
                showUserMenu = true
            )
            else -> TopBarConfig(
                title = getString(R.string.app_name),
                showUserMenu = true
            )
        }

        topBarManager.setup(config, this)
    }

    override fun onLogoutClicked() {
        logout()
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
            "statistics" -> StatisticsFragment()
            "ai_tools" -> AiToolsFragment()
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