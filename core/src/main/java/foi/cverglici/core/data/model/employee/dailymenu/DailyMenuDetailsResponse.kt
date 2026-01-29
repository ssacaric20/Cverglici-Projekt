package foi.cverglici.core.data.model.employee.dailymenu

import com.google.gson.annotations.SerializedName

data class DailyMenuDetailsResponse(
    @SerializedName("dailyMenuId") val dailyMenuId: Int,
    @SerializedName("date") val date: String,
    @SerializedName("category") val category: String,
    @SerializedName("dishes") val dishes: List<DailyMenuDish>
)

data class DailyMenuDish(
    @SerializedName("dishId") val dishId: Int,
    @SerializedName("title") val title: String,
    @SerializedName("price") val price: Double,
    @SerializedName("description") val description: String?,
    @SerializedName("calories") val calories: Int?,
    @SerializedName("protein") val protein: Double?,
    @SerializedName("fat") val fat: Double?,
    @SerializedName("carbohydrates") val carbohydrates: Double?,
    @SerializedName("fiber") val fiber: Double?,
    @SerializedName("imgPath") val imgPath: String?
)