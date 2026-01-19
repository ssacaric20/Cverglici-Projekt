using SmartMenza.Business.Security.Services.Interfaces;
using SmartMenza.Data.Entities;

namespace SmartMenza.UnitTests.Helpers
{
    public class FakeTokenService : ITokenService
    {
        public string GenerateToken(User user)
            => "FAKE_JWT_TOKEN";
    }
}
