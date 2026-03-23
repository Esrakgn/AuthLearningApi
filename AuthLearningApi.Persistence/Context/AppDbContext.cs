using AuthLearningApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthLearningApi.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; } // kullanıcılar tablosu gibi çalışır 
}

//veritabanında users tablosu olarak tutulmasını sağlamak