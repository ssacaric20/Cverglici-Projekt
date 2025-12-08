using SmartMenza.Business.Models.Auth;
using SmartMenza.Business.Models.Users;

namespace SmartMenza.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserListItemResponse>> GetUsersAsync();
        
        Task<LoginResponse?> LoginUserAsync(LoginRequest request);

        Task<LoginResponse?> RegisterUserAsync(RegistrationRequest request);

        Task<LoginResponse?> LoginGoogleAsync(GoogleLoginRequest request);

        bool IsLoginInputEmpty(LoginRequest request); // Validira input
    }
}
