package foi.cverglici.core.data.api.student.favorite

import foi.cverglici.core.data.model.student.favorite.AddFavoriteRequest
import foi.cverglici.core.data.model.student.favorite.FavoriteDish
import foi.cverglici.core.data.model.student.favorite.FavoriteStatusResponse
import retrofit2.Response
import retrofit2.http.*

interface IFavoriteService {
    @GET("api/Favorite")  // token
    suspend fun getUserFavorites(): Response<List<FavoriteDish>>

    @POST("api/Favorite")
    suspend fun addFavorite(@Body request: AddFavoriteRequest): Response<Map<String, String>>

    @DELETE("api/Favorite/{dishId}")
    suspend fun removeFavorite(@Path("dishId") dishId: Int): Response<Map<String, String>>

    @GET("api/Favorite/check/{dishId}")
    suspend fun isFavorite(@Path("dishId") dishId: Int): Response<FavoriteStatusResponse>
}