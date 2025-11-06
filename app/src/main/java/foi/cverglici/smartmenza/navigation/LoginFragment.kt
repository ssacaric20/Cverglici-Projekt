package foi.cverglici.smartmenza.navigation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.google.android.material.textfield.TextInputEditText
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.data.api.RetrofitClient
import foi.cverglici.smartmenza.data.model.LoginRequest
import kotlinx.coroutines.launch

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

        val requestBody = LoginRequest (email, password)

        //da se onemoguci prijava button tijekom API calla
        loginButton.isEnabled = false

        lifecycleScope.launch {
            try {
                val response = RetrofitClient.authService.loginUser(requestBody)

                if (response.isSuccessful) {
                    response.body()?.let { authResponse ->
                        // spremanje
                        // sessionManager.saveUserId(authResponse.userId)
                        showSuccessMessage(authResponse.message)

                        // navigacija na login nakon registracije
                        activity?.supportFragmentManager?.beginTransaction()
                            ?.replace(R.id.fragmentContainer, LoginFragment())
                            ?.commit()
                    }
                } else {
                    val errorMsg = response.errorBody()?.string() ?: "Greška kod prijave."
                    showError(errorMsg)
                }
            } catch (e: Exception) {
                showError(e.message ?: "Mrežna greška: ${e.message}")
            } finally {
                loginButton.isEnabled = true
            }
        }
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

    private fun showSuccessMessage(message: String) {
        Toast.makeText(requireContext(), message, Toast.LENGTH_LONG).show()
    }

    private fun showError(message: String) {
        Toast.makeText(requireContext(), message, Toast.LENGTH_LONG).show()
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