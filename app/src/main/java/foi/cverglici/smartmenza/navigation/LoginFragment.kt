package foi.cverglici.smartmenza.navigation

import android.app.Activity
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
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
import com.google.android.gms.auth.api.signin.GoogleSignIn
import com.google.android.gms.auth.api.signin.GoogleSignInOptions
import com.google.android.gms.common.api.ApiException
import com.google.android.material.textfield.TextInputEditText
import foi.cverglici.smartmenza.R

class LoginFragment : Fragment() {

    private lateinit var googleSignInLauncher: ActivityResultLauncher<Intent>
    private lateinit var serverClientId: String
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

    override fun onCreate(savedInstanceState: Bundle?) {

        super.onCreate(savedInstanceState)

        serverClientId = getServerClientId(requireContext()) ?: ""

        Log.d("serverClientId", "Value: resultCode=${serverClientId}")

        // Registracija launcher-a u Fragment-u
        googleSignInLauncher = registerForActivityResult(
            ActivityResultContracts.StartActivityForResult()
        ) { result ->
            Log.d(
                "GoogleSignIn",
                "Activity result received: resultCode=${result.resultCode}, data=${result.data}"
            )

            if (result.resultCode == Activity.RESULT_OK) {
                Log.d("GoogleSignIn", "Result OK, processing sign-in")
                val task = GoogleSignIn.getSignedInAccountFromIntent(result.data)
                try {
                    val account = task.getResult(ApiException::class.java)

                    Log.d("GoogleSignIn", "Account retrieved successfully")
                    Log.d("GoogleSignIn", "Display Name: ${account?.displayName}")
                    Log.d("GoogleSignIn", "Email: ${account?.email}")
                    Log.d("GoogleSignIn", "Id: ${account?.id}")
                    Log.d("GoogleSignIn", "IdToken: ${account?.idToken}")
                    Log.d("GoogleSignIn", "Auth code: ${account?.serverAuthCode}")

                    val authorizationCode = account?.serverAuthCode
                    if (authorizationCode != null) {
                        Log.d("GoogleSignIn", "Authorization code is present, sending to backend")
                        //sendCodeToBackend(authorizationCode)
                    } else {
                        Log.e("GoogleSignIn", "Authorization code is null")
                    }
                } catch (e: ApiException) {
                    Log.e(
                        "GoogleSignIn",
                        "Failed to get account result: ${e.statusCode}, message: ${e.message}"
                    )
                }
            } else {
                Log.e(
                    "GoogleSignIn",
                    "Google sign-in failed, resultCode=${result.resultCode}, intentData=${result.data}"
                )
            }
        }
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

        Toast.makeText(
            requireContext(),
            "Prijava za: $email",
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

        startGoogleSignIn()
    }

    private fun startGoogleSignIn() {
        val gso = GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
            .requestEmail()
            .requestServerAuthCode(serverClientId)
            .build()

        val googleSignInClient = GoogleSignIn.getClient(requireActivity(), gso)
        val signInIntent = googleSignInClient.signInIntent
        googleSignInLauncher.launch(signInIntent)
    }


    fun getServerClientId(context: Context): String? {
        return try {
            val appInfo = context.packageManager.getApplicationInfo(
                context.packageName,
                PackageManager.GET_META_DATA
            )
            appInfo.metaData?.getString("com.google.android.gms.auth.api.signin.ServerClientId")
        } catch (e: PackageManager.NameNotFoundException) {
            e.printStackTrace()
            null
        }
    }
}
