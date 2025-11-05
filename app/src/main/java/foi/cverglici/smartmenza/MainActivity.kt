package foi.cverglici.smartmenza

import android.os.Bundle
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import foi.cverglici.smartmenza.navigation.LoginFragment
import foi.cverglici.smartmenza.navigation.RegistrationFragment

class MainActivity : AppCompatActivity() {

    private lateinit var tabLogin: Button
    private lateinit var tabRegister: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        initializeViews()
        setupClickListeners()

        if (savedInstanceState == null) {
            showLoginFragment()
        }

        updateTabSelection(isLoginActive = true)

        tabLogin.setOnClickListener {
            supportFragmentManager.beginTransaction()
                .replace(R.id.fragmentContainer, LoginFragment())
                .commit()

            updateTabSelection(isLoginActive = true)
        }

        tabRegister.setOnClickListener {
            supportFragmentManager.beginTransaction()
                .replace(R.id.fragmentContainer, RegistrationFragment())
                .commit()

            updateTabSelection(isLoginActive = false)
        }
    }

    private fun initializeViews() {
        tabLogin = findViewById(R.id.tabLogin)
        tabRegister = findViewById(R.id.tabRegister)
    }

    private fun setupClickListeners() {
        tabLogin.setOnClickListener {
            selectLoginTab()
        }

        tabRegister.setOnClickListener {
            selectRegisterTab()
        }
    }

    private fun selectLoginTab() {
        updateTabAppearance(isLoginSelected = true)
        showLoginFragment()
    }

    private fun selectRegisterTab() {
        // Update tab appearance
        updateTabAppearance(isLoginSelected = false)

        // Show registration fragment
        showRegistrationFragment()
    }

    private fun updateTabAppearance(isLoginSelected: Boolean) {
        if (isLoginSelected) {
            // Login tab selected
            tabLogin.setBackgroundResource(R.drawable.selected_bg)
            tabLogin.setTextColor(ContextCompat.getColor(this, R.color.text_primary))

            tabRegister.setBackgroundColor(ContextCompat.getColor(this, android.R.color.transparent))
            tabRegister.setTextColor(ContextCompat.getColor(this, R.color.text_secondary))
        } else {
            tabRegister.setBackgroundResource(R.drawable.selected_bg)
            tabRegister.setTextColor(ContextCompat.getColor(this, R.color.text_primary))

            tabLogin.setBackgroundColor(ContextCompat.getColor(this, android.R.color.transparent))
            tabLogin.setTextColor(ContextCompat.getColor(this, R.color.text_secondary))
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
            loginButton.setBackgroundTintList(ContextCompat.getColorStateList(this, R.color.white))
            loginButton.setTextColor(ContextCompat.getColor(this, R.color.text_primary))

            registerButton.setBackgroundTintList(ContextCompat.getColorStateList(this, R.color.transparent)) // Assuming default/inactive background is transparent
            registerButton.setTextColor(ContextCompat.getColor(this, R.color.text_secondary))
        } else {
            registerButton.setBackgroundTintList(ContextCompat.getColorStateList(this, R.color.white))
            registerButton.setTextColor(ContextCompat.getColor(this, R.color.text_primary))

            loginButton.setBackgroundTintList(ContextCompat.getColorStateList(this, R.color.transparent)) // Assuming default/inactive background is transparent
            loginButton.setTextColor(ContextCompat.getColor(this, R.color.text_secondary))
        }
    }
}