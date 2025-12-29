package foi.cverglici.core.data.model.employee.dish

import com.google.gson.annotations.SerializedName

data class DishListItem(
    @SerializedName("dishId") val dishId: Int,
    @SerializedName("title") val title: String,
    @SerializedName("price") val price: Double,
    @SerializedName("description") val description: String?,
    @SerializedName("calories") val calories: Int?,
    @SerializedName("imgPath") val imgPath: String?
)