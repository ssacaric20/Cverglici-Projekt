package foi.cverglici.core.data.model.favorite

import com.google.gson.annotations.SerializedName

data class FavoriteDish(
    @SerializedName("dishId") val dishId: Int,
    @SerializedName("title") val title: String,
    @SerializedName("description") val description: String?,
    @SerializedName("price") val price: Double,
    @SerializedName("calories") val calories: Int?,
    @SerializedName("imgPath") val imgPath: String?
)