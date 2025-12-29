package foi.cverglici.core.data.model.student.favorite

import com.google.gson.annotations.SerializedName

data class AddFavoriteRequest(
    @SerializedName("userId") val userId: Int,
    @SerializedName("dishId") val dishId: Int
)