package foi.cverglici.core.data.model.favorite

import com.google.gson.annotations.SerializedName

data class FavoriteStatusResponse(
    @SerializedName("isFavorite") val isFavorite: Boolean
)