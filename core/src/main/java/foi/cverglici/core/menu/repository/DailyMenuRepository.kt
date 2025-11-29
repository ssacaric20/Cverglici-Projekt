package foi.cverglic.core.menu.repository

import foi.cverglic.core.menu.model.DailyMenuItem
import foi.cverglic.core.menu.model.Dish
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import org.json.JSONArray
import java.net.HttpURLConnection
import java.net.URL

class DailyMenuRepository(
    private val baseUrl: String   // npr. "http://10.0.2.2:7204"
) : IDailyMenuRepository {

    override suspend fun getTodayMenu(): List<DailyMenuItem> =
        fetchMenu("$baseUrl/api/DailyMenu/today")

    override suspend fun getMenuForDate(date: String): List<DailyMenuItem> =
        fetchMenu("$baseUrl/api/DailyMenu/date?date=$date")

    private suspend fun fetchMenu(urlString: String): List<DailyMenuItem> =
        withContext(Dispatchers.IO) {
            val url = URL(urlString)
            val connection = (url.openConnection() as HttpURLConnection).apply {
                requestMethod = "GET"
            }

            try {
                val code = connection.responseCode
                if (code != HttpURLConnection.HTTP_OK) {
                    // za sada: ako nije 200, vrati empty listu
                    return@withContext emptyList()
                }

                val responseText = connection.inputStream
                    .bufferedReader()
                    .use { it.readText() }

                parseDailyMenuJson(responseText)
            } finally {
                connection.disconnect()
            }
        }

    private fun parseDailyMenuJson(json: String): List<DailyMenuItem> {
        val array = JSONArray(json)
        val result = mutableListOf<DailyMenuItem>()

        for (i in 0 until array.length()) {
            val itemObj = array.getJSONObject(i)

            val dishObj = itemObj.getJSONObject("dish")

            val dish = Dish(
                dishId = dishObj.getInt("dishId"),
                title = dishObj.getString("title"),
                description = dishObj.optString("description", null),
                price = dishObj.getDouble("price"),
                calories = dishObj.getInt("calories"),
                protein = dishObj.getDouble("protein"),
                fat = dishObj.getDouble("fat"),
                carbohydrates = dishObj.getDouble("carbohydrates"),
                imgPath = if (dishObj.isNull("imgPath")) null else dishObj.getString("imgPath")
            )

            val menuItem = DailyMenuItem(
                dailyMenuId = itemObj.getInt("dailyMenuId"),
                date = itemObj.getString("date"),
                dishId = itemObj.getInt("dishId"),
                dish = dish
            )

            result += menuItem
        }

        return result
    }
}
