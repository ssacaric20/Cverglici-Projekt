package foi.cverglici.topbar

data class TopBarConfig(
    val title: String,
    val backgroundColor: Int? = null,
    val textColor: Int? = null,
    val showUserMenu: Boolean = true
)