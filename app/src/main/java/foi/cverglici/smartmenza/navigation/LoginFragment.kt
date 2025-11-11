package foi.cverglici.smartmenza.navigation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.google.android.material.textfield.TextInputEditText
import foi.cverglici.smartmenza.R

class LoginFragment : Fragment() {

    private lateinit var emailInput: TextInputEditText
    private lateinit var passwordInput: TextInputEditText
    private lateinit var loginButton: Button
    private lateinit var googleLoginButton: Button

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.login_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        // Initialize UI components
        initializeViews(view)

        // Set up click listeners
        setupClickListeners()
    }

    private fun initializeViews(view: View) {
        emailInput = view.findViewById(R.id.emailInput)
        passwordInput = view.findViewById(R.id.passwordInput)
        loginButton = view.findViewById(R.id.loginButton)
        googleLoginButton = view.findViewById(R.id.googleLoginButton)
    }

    private fun setupClickListeners() {
        loginButton.setOnClickListener {
            handleLogin()
        }

        googleLoginButton.setOnClickListener {
            handleGoogleLogin()
        }
    }

    private fun handleLogin() {
        val email = emailInput.text.toString().trim()
        val password = passwordInput.text.toString()

        // Validate input
        if (!validateLoginInput(email, password)) {
            return
        }

        // TODO: Make API call to backend
        // Example:
        // authService.login(email, password) { success, error ->
        //     if (success) {
        //         navigateToHomeScreen()
        //     } else {
        //         showError(error)
        //     }
        // }

        Toast.makeText(
            requireContext(),
            "Prijava za: $email",
            Toast.LENGTH_LONG
        ).show()
    }

    private fun validateLoginInput(email: String, password: String): Boolean {
        // Check if email is empty
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
        // TODO: Implement Google Sign-In flow
        // You'll need to:
        // 1. Add Google Sign-In dependency
        // 2. Configure Google Sign-In in Firebase Console
        // 3. Implement the authentication flow

        Toast.makeText(
            requireContext(),
            "Google prijava nije još implementirana",
            Toast.LENGTH_SHORT
        ).show()
    }
}