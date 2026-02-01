package foi.cverglici.smartmenza.ui.student.reviews

data class ReviewUi(
    val id: Int,
    val userDisplayName: String,
    val userId: Int,
    val rating: Int,
    val text: String,
    val dateIso: String,
    val isMine: Boolean
)
