package foi.cverglici.core.menu.model

import com.google.gson.annotations.SerializedName

data class DailyMenuItem(
    val dailyMenuId: Int,
    val date: String,
    val dishId: Int,
    @SerializedName("jelo")  // backend "jelo", "dish" in code
    val dish: Dish
)
