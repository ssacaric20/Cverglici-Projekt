package foi.cverglici.navigation

import android.view.MenuItem
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentActivity
import com.google.android.material.bottomnavigation.BottomNavigationView
import foi.cverglici.navigation.Enums.NavigationRole

class NavigationManager(
    private val activity: FragmentActivity,
    private val containerId: Int,
    private val bottomNavigationView: BottomNavigationView
) {

    private var fragmentProvider: ((String) -> Fragment)? = null

    fun setupNavigation(
        role: NavigationRole,
        onFragmentProvider: (String) -> Fragment
    ) {
        this.fragmentProvider = onFragmentProvider

        when (role) {
            NavigationRole.STUDENT -> setupStudentNavigation()
            NavigationRole.EMPLOYEE -> setupEmployeeNavigation()
        }

        bottomNavigationView.setOnItemSelectedListener { item ->
            handleNavigationItemSelected(item)
        }
    }

    private fun setupStudentNavigation() {
        bottomNavigationView.menu.clear()
        bottomNavigationView.inflateMenu(R.menu.student_bottom_menu)

        navigateToTag("menu")
    }

    private fun setupEmployeeNavigation() {
        bottomNavigationView.menu.clear()
        bottomNavigationView.inflateMenu(R.menu.employee_bottom_menu)

        navigateToTag("employee_menu")
    }

    private fun handleNavigationItemSelected(item: MenuItem): Boolean {
        val tag = when (item.itemId) {
            R.id.nav_menu -> "menu"
            R.id.nav_favorites -> "favorites"
            R.id.nav_goals -> "goals"
            R.id.nav_employee_menu -> "employee_menu"
            R.id.nav_statistics -> "statistics"
            R.id.nav_ai_tools -> "ai_tools"
            else -> return false
        }

        navigateToTag(tag)
        return true
    }

    private fun navigateToTag(tag: String) {
        val fragment = fragmentProvider?.invoke(tag) ?: return

        activity.supportFragmentManager.beginTransaction()
            .replace(containerId, fragment, tag)
            .commit()
    }

    fun setSelectedItem(tag: String) {
        val itemId = when (tag) {
            "menu" -> R.id.nav_menu
            "favorites" -> R.id.nav_favorites
            "goals" -> R.id.nav_goals
            "employee_menu" -> R.id.nav_employee_menu
            "statistics" -> R.id.nav_statistics
            "ai_tools" -> R.id.nav_ai_tools
            else -> return
        }

        bottomNavigationView.selectedItemId = itemId
    }
}