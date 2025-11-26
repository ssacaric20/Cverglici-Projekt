package foi.cverglici.mailauth.ui

import android.app.Dialog
import android.content.Context
import android.os.Bundle
import android.util.Patterns
import android.view.Window
import android.view.WindowManager
import android.widget.Button
import com.google.android.material.textfield.TextInputEditText
import com.google.android.material.textfield.TextInputLayout
import foi.cverglici.mailauth.EmailCredentials
import foi.cverglici.mailauth.R

class EmailAuthDialog(
    context: Context,
    private val mode: Mode,
    private val onSubmit: (EmailCredentials) -> Unit
) : Dialog(context) {

    enum class Mode {
        LOGIN, REGISTER
    }

    private lateinit var nameInputLayout: TextInputLayout
    private lateinit var nameInput: TextInputEditText
    private lateinit var emailInput: TextInputEditText
    private lateinit var passwordInput: TextInputEditText
    private lateinit var submitButton: Button
    private lateinit var cancelButton: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        requestWindowFeature(Window.FEATURE_NO_TITLE)

        val layoutId = when (mode) {
            Mode.LOGIN -> R.layout.dialog_email_login
            Mode.REGISTER -> R.layout.dialog_email_register
        }

        setContentView(layoutId)

        window?.setLayout(
            (context.resources.displayMetrics.widthPixels * 0.90).toInt(), // 90% of screen width
            WindowManager.LayoutParams.WRAP_CONTENT
        )

        initializeViews()
        setupClickListeners()
    }

    private fun initializeViews() {
        emailInput = findViewById(R.id.emailInput)
        passwordInput = findViewById(R.id.passwordInput)
        submitButton = findViewById(R.id.submitButton)
        cancelButton = findViewById(R.id.cancelButton)

        if (mode == Mode.REGISTER) {
            nameInputLayout = findViewById(R.id.nameInputLayout)
            nameInput = findViewById(R.id.nameInput)
        }
    }

    private fun setupClickListeners() {
        submitButton.setOnClickListener {
            handleSubmit()
        }

        cancelButton.setOnClickListener {
            cancel()
        }
    }

    private fun handleSubmit() {
        val email = emailInput.text.toString().trim()
        val password = passwordInput.text.toString()
        val name = if (mode == Mode.REGISTER) {
            nameInput.text.toString().trim()
        } else null

        if (!validateInput(email, password, name)) {
            return
        }

        submitButton.isEnabled = false

        val credentials = EmailCredentials(
            email = email,
            password = password,
            name = name
        )

        onSubmit(credentials)
        dismiss()
    }

    private fun validateInput(email: String, password: String, name: String?): Boolean {
        if (mode == Mode.REGISTER) {
            if (name.isNullOrEmpty()) {
                nameInput.error = "Ime je obavezno"
                return false
            }

            if (name.length < 2) {
                nameInput.error = "Ime mora imati najmanje 2 znaka"
                return false
            }
        }

        if (email.isEmpty()) {
            emailInput.error = "Email je obavezan"
            return false
        }

        if (!Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
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
}