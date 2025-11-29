package foi.cverglic.core.menu.repository

import foi.cverglic.core.menu.model.DailyMenuItem

interface IDailyMenuRepository {

    suspend fun getTodayMenu(): List<DailyMenuItem>

    suspend fun getMenuForDate(date: String): List<DailyMenuItem>
}


