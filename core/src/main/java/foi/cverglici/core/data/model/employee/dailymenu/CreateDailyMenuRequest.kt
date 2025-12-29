package foi.cverglici.core.data.model.employee.dailymenu

import com.google.gson.annotations.SerializedName

data class CreateDailyMenuRequest(
    @SerializedName("date") val date: String,
    @SerializedName("category") val category: Int,
    @SerializedName("dishIds") val dishIds: List<Int>
)