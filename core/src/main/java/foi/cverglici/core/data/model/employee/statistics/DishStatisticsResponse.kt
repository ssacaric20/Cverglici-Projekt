package foi.cverglici.core.data.model.employee.statistics

import com.google.gson.annotations.SerializedName

data class DishStatisticsResponse(
    @SerializedName("dishId") val dishId: Int,
    @SerializedName("title") val title: String,
    @SerializedName("imgPath") val imgPath: String?,
    @SerializedName("averageRating") val averageRating: Double?,
    @SerializedName("ratingsCount") val ratingsCount: Int,
    @SerializedName("favoriteCount") val favoriteCount: Int
)