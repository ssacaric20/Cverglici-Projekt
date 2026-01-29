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
import kotlinx.coroutines.launch

class FavoriteManager(
    private val context: Context,
    private val lifecycleOwner: LifecycleOwner,
    private val favoriteIcon: ImageView,
    private val dishId: Int
) {
    private val sessionManager = SessionManager(context)
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
        val userId = sessionManager.getUserId()

        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = RetrofitFavorite.favoriteService.isFavorite(userId, dishId)

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
        val userId = sessionManager.getUserId()

        lifecycleOwner.lifecycleScope.launch {
            try {
                val response = if (isFavorite) {
                    RetrofitFavorite.favoriteService.removeFavorite(userId, dishId)
                } else {
                    RetrofitFavorite.favoriteService.addFavorite(
                        AddFavoriteRequest(userId, dishId)
                    )
                }

                if (response.isSuccessful) {
                    isFavorite = !isFavorite
                    updateFavoriteIcon()

                    val message = if (isFavorite) {
                        "Dodano u favorite!"
                    } else {
                        "Uklonjeno iz favorita"
                    }
                    Toast.makeText(context, message, Toast.LENGTH_SHORT).show()
                } else {
                    Toast.makeText(
                        context,
                        "Greška pri ažuriranju favorita",
                        Toast.LENGTH_SHORT
                    ).show()
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