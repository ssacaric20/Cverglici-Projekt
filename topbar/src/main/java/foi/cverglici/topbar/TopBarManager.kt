package foi.cverglici.topbar

import android.app.AlertDialog
import android.view.View
import android.widget.ImageButton
import android.widget.PopupMenu
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import foi.cverglici.topbar.listeners.OnTopBarActionListener

class TopBarManager(
    private val activity: AppCompatActivity,
    private val topBarView: View
) {
    private val titleTextView: TextView = topBarView.findViewById(R.id.appTitle)
    private val userMenuButton: ImageButton = topBarView.findViewById(R.id.btnUserMenu)
    private val container: View = topBarView.findViewById(R.id.topBar)

    private var listener: OnTopBarActionListener? = null

    fun setup(
        config: TopBarConfig,
        listener: OnTopBarActionListener
    ) {
        this.listener = listener

        // Set title
        titleTextView.text = config.title

        // Set background color
        config.backgroundColor?.let { color ->
            container.setBackgroundColor(activity.getColor(color))
        }

        // Set text color
        config.textColor?.let { color ->
            titleTextView.setTextColor(activity.getColor(color))
        }

        // Setup user menu button
        if (config.showUserMenu) {
            userMenuButton.visibility = View.VISIBLE
            userMenuButton.setOnClickListener { view ->
                showUserMenu(view)
            }
        } else {
            userMenuButton.visibility = View.GONE
        }
    }

    private fun showUserMenu(anchorView: View) {
        val popup = PopupMenu(activity, anchorView)
        popup.menuInflater.inflate(R.menu.user_menu, popup.menu)

        popup.setOnMenuItemClickListener { menuItem ->
            when (menuItem.itemId) {
                R.id.menu_logout -> {
                    showLogoutConfirmation()
                    true
                }
                else -> false
            }
        }

        popup.show()
    }

    private fun showLogoutConfirmation() {
        AlertDialog.Builder(activity)
            .setTitle("Odjava")
            .setMessage("Jeste li sigurni da se želite odjaviti?")
            .setPositiveButton("Da") { _, _ ->
                listener?.onLogoutClicked()
            }
            .setNegativeButton("Ne", null)
            .show()
    }

    fun updateTitle(newTitle: String) {
        titleTextView.text = newTitle
    }

    fun updateBackgroundColor(colorResId: Int) {
        container.setBackgroundColor(activity.getColor(colorResId))
    }
}