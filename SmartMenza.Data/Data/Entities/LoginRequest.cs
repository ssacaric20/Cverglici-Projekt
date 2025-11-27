using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMenza.Data.Data.Entities
{
    public class LoginRequest()
    {
        public string email { get; set; }
        public string passwordHash { get; set; }
    }

    public class GoogleLoginRequest()
    {
        public string tokenId { get; set; }
    }
    public class LoginResponse
    {
        public int userId { get; set; }
        public string message { get; set; }
        public string token { get; set; }
    }
}
