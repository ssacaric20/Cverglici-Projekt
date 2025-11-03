package foi.cverglici.smartmenza

import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import foi.cverglici.smartmenza.navigation.LoginFragment
import foi.cverglici.smartmenza.navigation.RegistrationFragment

class LoginActivity : AppCompatActivity() {

    private lateinit var tabLogin: Button
    private lateinit var tabRegister: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.login_activity)

        tabLogin = findViewById(R.id.tabLogin)
        tabRegister = findViewById(R.id.tabRegister)

        if (savedInstanceState == null) {
            showLoginFragment()
            updateTabSelection(isLoginActive = true)
        }

        setupClickListeners()
    }

    private fun setupClickListeners() {
        tabLogin.setOnClickListener {
            updateTabSelection(isLoginActive = true)
            showLoginFragment()
        }

        tabRegister.setOnClickListener {
            updateTabSelection(isLoginActive = false)
            showRegistrationFragment()
        }
    }


    private fun showLoginFragment() {
        replaceFragment(LoginFragment())
    }

    private fun showRegistrationFragment() {
        replaceFragment(fragment = (RegistrationFragment()))
    }

    private fun replaceFragment(fragment: Fragment) {
        supportFragmentManager.beginTransaction()
            .replace(R.id.fragmentContainer, fragment)
            .commit()
    }

    private fun updateTabSelection(isLoginActive: Boolean) {
        val loginButton = findViewById<Button>(R.id.tabLogin)
        val registerButton = findViewById<Button>(R.id.tabRegister)

        if (isLoginActive) {
            // highlight login tab
            loginButton.setBackgroundTintList(ContextCompat.getColorStateList(this, R.color.white))
            loginButton.setTextColor(ContextCompat.getColor(this, R.color.text_primary))

            // zgasi register tab
            registerButton.setBackgroundTintList(ContextCompat.getColorStateList(this, R.color.transparent)) // Assuming default/inactive background is transparent
            registerButton.setTextColor(ContextCompat.getColor(this, R.color.text_secondary))
        } else {
            // highlight register tab
            registerButton.setBackgroundTintList(ContextCompat.getColorStateList(this, R.color.white))
            registerButton.setTextColor(ContextCompat.getColor(this, R.color.text_primary))

            // zgasi login tab
            loginButton.setBackgroundTintList(ContextCompat.getColorStateList(this, R.color.transparent)) // Assuming default/inactive background is transparent
            loginButton.setTextColor(ContextCompat.getColor(this, R.color.text_secondary))
        }
    }
}