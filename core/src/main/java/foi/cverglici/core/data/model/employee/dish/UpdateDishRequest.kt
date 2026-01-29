package foi.cverglici.core.data.model.employee.dish

import com.google.gson.annotations.SerializedName

data class UpdateDishRequest(
    @SerializedName("title") val title: String,
    @SerializedName("description") val description: String?,
    @SerializedName("price") val price: Double,
    @SerializedName("calories") val calories: Int?,
    @SerializedName("protein") val protein: Double?,
    @SerializedName("fat") val fat: Double?,
    @SerializedName("carbohydrates") val carbohydrates: Double?,
    @SerializedName("fiber") val fiber: Double?,
    @SerializedName("imgPath") val imgPath: String?
)