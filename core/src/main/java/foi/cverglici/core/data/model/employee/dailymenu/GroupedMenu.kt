package foi.cverglici.core.data.model.employee.dailymenu

data class GroupedMenu(
    val dailyMenuId: Int,
    val date: String,
    val category: String,
    val dishIds: List<Int>
)