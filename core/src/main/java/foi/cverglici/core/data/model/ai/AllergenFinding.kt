package foi.cverglici.core.data.model.ai

data class AllergenFinding(
    val allergen: String,
    val triggers: List<String>
)