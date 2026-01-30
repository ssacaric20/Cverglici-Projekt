package foi.cverglici.smartmenza.ui.student.favorites


import android.content.Context
import android.util.Log
import android.widget.ImageView
import android.widget.Toast
import androidx.core.content.ContextCompat
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.lifecycleScope
import foi.cverglici.core.data.api.student.favorite.RetrofitFavorite
import foi.cverglici.core.data.model.student.favorite.AddFavoriteRequest
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionManager
import foi.cverglici.smartmenza.session.SessionTokenProvider
import kotlinx.coroutines.launch

class FavoriteManager(
    private val context: Context,
    private val lifecycleOwner: LifecycleOwner,
    private val favoriteIcon: ImageView,
    private val dishId: Int
) {
    private val tokenProvider = SessionTokenProvider(context)
    private val favoriteService = RetrofitFavorite.create(tokenProvider)
    private var isFavorite: Boolean = false

    fun initialize() {
        checkFavoriteStatus()
        setupClickListener()
    }

    private fun setupClickListener() {
        favoriteIcon.setOnClickListener {
            toggleFavorite()
        }
    }

    private fun checkFavoriteStatus() {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = favoriteService.isFavorite(dishId)

                if (response.isSuccessful) {
                    isFavorite = response.body()?.isFavorite ?: false
                    updateFavoriteIcon()
                }
            } catch (e: Exception) {
                Log.e("FavoriteManager", "Error checking favorite status", e)
            }
        }
    }

    private fun toggleFavorite() {
        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = if (isFavorite) {
                    favoriteService.removeFavorite(dishId)
                } else {
                    val userId = SessionManager(context).getUserId()
                    favoriteService.addFavorite(AddFavoriteRequest(userId, dishId))
                }

                if (response.isSuccessful) {
                    isFavorite = !isFavorite
                    updateFavoriteIcon()

                    val message = if (isFavorite) "Dodano u favorite!" else "Uklonjeno iz favorita"
                    Toast.makeText(context, message, Toast.LENGTH_SHORT).show()
                } else {
                    Toast.makeText(context, "Greška pri ažuriranju favorita", Toast.LENGTH_SHORT).show()
                }
            } catch (e: Exception) {
                Log.e("FavoriteManager", "Error toggling favorite", e)
                Toast.makeText(context, "Mrežna greška", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun updateFavoriteIcon() {
        val iconRes = if (isFavorite) {
            R.drawable.ic_favorite_filled
        } else {
            R.drawable.fav_circle_bg
        }

        favoriteIcon.setImageResource(iconRes)

        val tintColor = if (isFavorite) {
            ContextCompat.getColor(context, R.color.orange_primary)
        } else {
            ContextCompat.getColor(context, R.color.text_secondary)
        }
        favoriteIcon.setColorFilter(tintColor)
    }
}