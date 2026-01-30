package foi.cverglici.smartmenza

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import foi.cverglici.smartmenza.ui.auth.AuthFragment

class AuthActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.auth_activity)

        if (savedInstanceState == null) {
            supportFragmentManager.beginTransaction()
                .replace(R.id.fragmentContainer, AuthFragment())
                .commit()
        }
    }
}