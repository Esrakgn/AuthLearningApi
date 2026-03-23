using AuthLearningApi.Application.DTOs.Auth;
using AuthLearningApi.Domain.Entities;
using AuthLearningApi.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthLearningApi.Application.DTOs.Auth;


namespace AuthLearningApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{ 
    private readonly AppDbContext _context;
    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var isEmailExists = await _context.Users.AnyAsync(x => x.Email == request.Email);// burada da email eşleşmesini kontrol ediyor 
        //bu maile sahip bir kullanıcı var mı sorguluyoruz 
        //anyasync = en az bir kayıt var mı

        if (isEmailExists) // eğer aynı mail varsa içeri girer ve 400 döner
        {
            return BadRequest("This email is already in use.");
        }

        var user = new AppUser // kullanıcıdan gelen verileri veritabanına uygun nesneye dönüştürür
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = request.Password
        };

        await _context.Users.AddAsync(user); 
        //tabloya eklemek için eklenecek nesne olarak işaretler
        await _context.SaveChangesAsync();
        //Bu satır çalışınca EF Core SQL üretir ve veriyi gerçekten tabloya insert eder.

        return Ok("User registered successfully.");

        }

         [HttpPost("login")]
         public async Task<IActionResult> Login(LoginRequest request)
         {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user is null)
        {
            return BadRequest("User not found.");
        }

        if (user.PasswordHash != request.Password)
        {
            return BadRequest("Email or password is incorrect.");
        }

        return Ok("Login successful.");
    }

}
