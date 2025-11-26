package foi.cverglici.smartmenza.ui.nav

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.Toast
import androidx.activity.result.ActivityResultLauncher
import androidx.activity.result.contract.ActivityResultContracts
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import foi.cverglici.core.auth.AuthenticationManager
import foi.cverglici.core.auth.model.AuthResult
import foi.cverglici.googleauth.GoogleAuthHandler
import foi.cverglici.mailauth.EmailAuthHandler
import foi.cverglici.smartmenza.MainActivity
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionManager
import kotlinx.coroutines.launch

class AuthFragment : Fragment() {

    private lateinit var sessionManager: SessionManager
    private lateinit var authManager: AuthenticationManager
    private lateinit var googleAuthHandler: GoogleAuthHandler
    private lateinit var googleSignInLauncher: ActivityResultLauncher<Intent>

    private lateinit var loginButton: Button
    private lateinit var registerButton: Button
    private lateinit var googleLoginButton: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        // Initialize session manager
        sessionManager = SessionManager(requireContext())

        // Initialize auth manager
        authManager = AuthenticationManager()

        // Register Google Sign-In launcher
        googleSignInLauncher = registerForActivityResult(
            ActivityResultContracts.StartActivityForResult()
        ) { result ->
            googleAuthHandler.handleActivityResult(result.resultCode, result.data)
        }

        // Initialize and register authentication handlers
        setupAuthHandlers()
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.auth_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        // Initialize UI components
        initializeViews(view)

        // Set up click listeners
        setupClickListeners()
    }

    private fun setupAuthHandlers() {
        // Initialize Email Auth Handler
        val emailAuthHandler = EmailAuthHandler(requireContext())
        authManager.registerHandler(AuthenticationManager.AuthType.EMAIL, emailAuthHandler)

        // Initialize Google Auth Handler
        googleAuthHandler = GoogleAuthHandler(requireContext(), googleSignInLauncher)
        authManager.registerHandler(AuthenticationManager.AuthType.GOOGLE, googleAuthHandler)
    }

    private fun initializeViews(view: View) {
        loginButton = view.findViewById(R.id.loginButton)
        registerButton = view.findViewById(R.id.registerButton)
        googleLoginButton = view.findViewById(R.id.googleLoginButton)
    }

    private fun setupClickListeners() {
        loginButton.setOnClickListener {
            handleEmailLogin()
        }

        registerButton.setOnClickListener {
            handleEmailRegister()
        }

        googleLoginButton.setOnClickListener {
            handleGoogleLogin()
        }
    }

    /**
     * handle email/password login using EmailAuthHandler
     */
    private fun handleEmailLogin() {
        val handler = authManager.getHandler(AuthenticationManager.AuthType.EMAIL)

        if (handler == null || !handler.isAvailable()) {
            showError("Email prijava nije dostupna")
            return
        }

        loginButton.isEnabled = false

        lifecycleScope.launch {
            try {
                when (val result = handler.login()) {
                    is AuthResult.Success -> {
                        // Save token to session
                        sessionManager.saveAuthToken(result.token)

                        showSuccessMessage(result.message)
                        navigateToMainActivity()
                    }
                    is AuthResult.Error -> {
                        showError(result.message)
                    }
                    is AuthResult.Cancelled -> {
                        // User cancelled, do nothing
                        Log.d("AuthFragment", "Login cancelled by user")
                    }
                }
            } catch (e: Exception) {
                showError("Neočekivana greška: ${e.message}")
                Log.e("AuthFragment", "Login error", e)
            } finally {
                loginButton.isEnabled = true
            }
        }
    }

    /**
     * handle email/password registration using EmailAuthHandler
     */
    private fun handleEmailRegister() {
        val handler = authManager.getHandler(AuthenticationManager.AuthType.EMAIL)

        if (handler == null || !handler.isAvailable()) {
            showError("Email registracija nije dostupna")
            return
        }

        registerButton.isEnabled = false

        lifecycleScope.launch {
            try {
                when (val result = handler.register()) {
                    is AuthResult.Success -> {
                        showSuccessMessage(result.message)

                        // Show success and inform user to login
                        showSuccessMessage("Registracija uspješna! Sada se možete prijaviti.")
                    }
                    is AuthResult.Error -> {
                        showError(result.message)
                    }
                    is AuthResult.Cancelled -> {
                        // User cancelled, do nothing
                        Log.d("AuthFragment", "Registration cancelled by user")
                    }
                }
            } catch (e: Exception) {
                showError("Neočekivana greška: ${e.message}")
                Log.e("AuthFragment", "Registration error", e)
            } finally {
                registerButton.isEnabled = true
            }
        }
    }

    /**
     * handle Google login with GoogleAuthHandler
     * login and registration (backend determines which)
     */
    private fun handleGoogleLogin() {
        val handler = authManager.getHandler(AuthenticationManager.AuthType.GOOGLE)

        if (handler == null || !handler.isAvailable()) {
            showError("Google prijava nije dostupna")
            return
        }

        googleLoginButton.isEnabled = false

        lifecycleScope.launch {
            try {
                when (val result = handler.login()) {
                    is AuthResult.Success -> {
                        // Save token to session
                        sessionManager.saveAuthToken(result.token)

                        showSuccessMessage(result.message)
                        navigateToMainActivity()
                    }
                    is AuthResult.Error -> {
                        showError(result.message)
                    }
                    is AuthResult.Cancelled -> {
                        // User cancelled, do nothing
                        Log.d("AuthFragment", "Google login cancelled by user")
                    }
                }
            } catch (e: Exception) {
                showError("Neočekivana greška: ${e.message}")
                Log.e("AuthFragment", "Google login error", e)
            } finally {
                googleLoginButton.isEnabled = true
            }
        }
    }

    private fun showSuccessMessage(message: String) {
        Toast.makeText(requireContext(), message, Toast.LENGTH_LONG).show()
    }

    private fun showError(message: String) {
        Toast.makeText(requireContext(), message, Toast.LENGTH_LONG).show()
    }

    private fun navigateToMainActivity() {
        val intent = Intent(requireContext(), MainActivity::class.java)
        intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
        startActivity(intent)
    }
}