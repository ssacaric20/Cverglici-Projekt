using SmartMenza.Data.Entities;

namespace SmartMenza.Business.Security.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}

