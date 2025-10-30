package foi.cverglici.smartmenza

import android.os.Bundle
import android.widget.Button
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import com.google.android.material.textfield.TextInputEditText

class MainActivity : AppCompatActivity() {
    private lateinit var tabLogin: Button
    private lateinit var tabRegister: Button
    private lateinit var emailInput: TextInputEditText
    private lateinit var passwordInput: TextInputEditText
    private lateinit var loginButton: Button
    private lateinit var googleLoginButton: Button


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        initializeViews()
        setupClickListeners()

        //enableEdgeToEdge()
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

        // Login button
        loginButton.setOnClickListener {
            handleLogin()
        }

        // Google login button
        googleLoginButton.setOnClickListener {
            handleGoogleLogin()
        }
    }

    private fun selectLoginTab() {
        tabLogin.setBackgroundResource(R.drawable.selected_bg)
        tabLogin.setTextColor(ContextCompat.getColor(this, R.color.text_primary))

        tabRegister.setBackgroundColor(ContextCompat.getColor(this, android.R.color.transparent))
        tabRegister.setTextColor(ContextCompat.getColor(this, R.color.text_secondary))

        Toast.makeText(this, "Prijava odabrana", Toast.LENGTH_SHORT).show()
    }

    private fun selectRegisterTab() {
        tabRegister.setBackgroundResource(R.drawable.selected_bg)
        tabRegister.setTextColor(ContextCompat.getColor(this, R.color.text_primary))

        tabLogin.setBackgroundColor(ContextCompat.getColor(this, android.R.color.transparent))
        tabLogin.setTextColor(ContextCompat.getColor(this, R.color.text_secondary))

        Toast.makeText(this, "Registracija nije još implementirana", Toast.LENGTH_SHORT).show()
    }

    private fun handleLogin() {
        val email = emailInput.text.toString().trim()
        val password = passwordInput.text.toString()

        if (!validateLoginInput(email, password)) {
            return
        }

        // API call na backend

        Toast.makeText(
            this,
            "Prijava za $email",
            Toast.LENGTH_LONG
        ).show()
    }

    private fun validateLoginInput(email: String, password: String): Boolean {
        if (email.isEmpty()) {
            emailInput.error = "Email je obavezan"
            return false
        }

        if (!android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
            emailInput.error = "Unesite ispravan email"
            return false
        }

        if (password.isEmpty()) {
            passwordInput.error = "Zaporka je obavezna"
            return false
        }

        if (password.length < 6) {
            passwordInput.error = "Zaporka mora imati najmanje 6 znakova"
            return false
        }

        return true
    }

    private fun handleGoogleLogin() {
        // google sign in
        // google dependency
        // autentifikacija

        Toast.makeText(
            this,
            "Google prijava nije još implementirana",
            Toast.LENGTH_SHORT
        ).show()
    }
}
