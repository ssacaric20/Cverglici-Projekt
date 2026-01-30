package foi.cverglici.core.auth

interface ITokenProvider {
    fun getToken(): String?
}