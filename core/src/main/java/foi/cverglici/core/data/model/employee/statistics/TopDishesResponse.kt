package foi.cverglici.core.data.model.employee.statistics

import com.google.gson.annotations.SerializedName

data class TopDishesResponse(
    @SerializedName("topRatedDishes") val topRatedDishes: List<DishStatisticsResponse>,
    @SerializedName("mostFavoritedDishes") val mostFavoritedDishes: List<DishStatisticsResponse>
)