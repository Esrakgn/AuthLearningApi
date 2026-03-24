using System;
using System.Collections.Generic;
using System.Text;

namespace AuthLearningApi.Domain.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; //veritabanındı tuttuğumuz güvenli şifre sürümü 
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;


    } 
}
//kullanıcı kavramımın ana hali