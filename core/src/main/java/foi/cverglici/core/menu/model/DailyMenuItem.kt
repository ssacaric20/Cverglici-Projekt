package foi.cverglic.core.menu.model

data class DailyMenuItem(
    val dailyMenuId: Int,
    val date: String,
    val dishId: Int,
    val dish: Dish
)
