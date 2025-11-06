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
import foi.cverglici.smartmenza.data.model.RegistrationRequest
import kotlinx.coroutines.launch


class RegistrationFragment : Fragment() {

    // UI components - these will be initialized in onViewCreated
    private lateinit var nameInputRegister: TextInputEditText
    private lateinit var emailInputRegister: TextInputEditText
    private lateinit var passwordInputRegister: TextInputEditText
    private lateinit var registerButton: Button
    private lateinit var googleRegisterButton: Button

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the registration fragment layout
        return inflater.inflate(R.layout.registration_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        // Initialize UI components
        initializeViews(view)

        // Set up click listeners
        setupClickListeners()
    }

    private fun initializeViews(view: View) {
        nameInputRegister = view.findViewById(R.id.nameInputRegister)
        emailInputRegister = view.findViewById(R.id.emailInputRegister)
        passwordInputRegister = view.findViewById(R.id.passwordInputRegister)
        registerButton = view.findViewById(R.id.registerButton)
        googleRegisterButton = view.findViewById(R.id.googleRegisterButton)
    }

    private fun setupClickListeners() {
        registerButton.setOnClickListener {
            handleRegister()
        }

        googleRegisterButton.setOnClickListener {
            handleGoogleRegister()
        }
    }

    private fun handleRegister() {
        val name = nameInputRegister.text.toString().trim()
        val email = emailInputRegister.text.toString().trim()
        val password = passwordInputRegister.text.toString()

        if (!validateRegisterInput(name, email, password)) {
            return
        }

        val requestBody = RegistrationRequest(name, email, password)

        //da se onemoguci register button tijekom API calla
        registerButton.isEnabled = false

        lifecycleScope.launch {
            try {
                val response = RetrofitClient.authService.registerUser(requestBody)

                if(response.isSuccessful) {
                    response.body()?.let { authResponse ->
                        //spremanje
                        //sessionManager.saveUserId(authResponse.userId)
                        showSuccessMessage(authResponse.message)

                        activity?.supportFragmentManager?.beginTransaction()
                            ?.replace(R.id.fragmentContainer, LoginFragment())
                            ?.commit()
                    }
                } else {
                    val errorMsg = response.errorBody()?.string() ?: "Registracija nije uspjela."
                    showError(errorMsg)
                }
            } catch (e: Exception) {
                showError("Mrežna greška: ${e.message}")
            } finally {
                registerButton.isEnabled = true
            }
        }
    }

    private fun validateRegisterInput(name: String, email: String, password: String): Boolean {

        if (name.isEmpty()) {
            nameInputRegister.error = "Ime je obavezno"
            return false
        }

        if (name.length < 2) {
            nameInputRegister.error = "Ime mora imati najmanje 2 znaka"
            return false
        }

        if (email.isEmpty()) {
            emailInputRegister.error = "Email je obavezan"
            return false
        }

        if (!android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
            emailInputRegister.error = "Unesite ispravan email"
            return false
        }

        if (password.isEmpty()) {
            passwordInputRegister.error = "Zaporka je obavezna"
            return false
        }

        if (password.length < 6) {
            passwordInputRegister.error = "Zaporka mora imati najmanje 6 znakova"
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

    private fun handleGoogleRegister() {
        // TODO: Implement Google Sign-In flow for registration
        // The process is the same as login, but you might want to
        // collect additional user information after successful authentication

        Toast.makeText(
            requireContext(),
            "Google registracija nije još implementirana",
            Toast.LENGTH_SHORT
        ).show()
    }
}