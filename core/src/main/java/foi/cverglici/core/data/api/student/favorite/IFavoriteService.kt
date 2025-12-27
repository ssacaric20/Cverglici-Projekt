package foi.cverglici.core.data.api.student.favorite

import foi.cverglici.core.data.model.favorite.AddFavoriteRequest
import foi.cverglici.core.data.model.favorite.FavoriteDish
import foi.cverglici.core.data.model.favorite.FavoriteStatusResponse
import retrofit2.Response
import retrofit2.http.*

interface IFavoriteService {
    @GET("api/Favorite/{userId}")
    suspend fun getUserFavorites(@Path("userId") userId: Int): Response<List<FavoriteDish>>

    @POST("api/Favorite")
    suspend fun addFavorite(@Body request: AddFavoriteRequest): Response<Map<String, String>>

    @DELETE("api/Favorite")
    suspend fun removeFavorite(
        @Query("userId") userId: Int,
        @Query("dishId") dishId: Int
    ): Response<Map<String, String>>

    @GET("api/Favorite/check")
    suspend fun isFavorite(
        @Query("userId") userId: Int,
        @Query("dishId") dishId: Int
    ): Response<FavoriteStatusResponse>
}