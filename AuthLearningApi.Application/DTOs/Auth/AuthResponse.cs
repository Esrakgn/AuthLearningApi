using System;
using System.Collections.Generic;
using System.Text;

namespace AuthLearningApi.Application.DTOs.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
//kullanıcıya token döner 
