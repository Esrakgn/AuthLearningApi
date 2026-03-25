using AuthLearningApi.Application.DTOs.Auth;
using AuthLearningApi.Domain.Entities;
using AuthLearningApi.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthLearningApi.Application.DTOs.Auth;
using AuthLearningApi.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BCrypt.Net;
using AuthLearningApi.Application.Common.Models;


namespace AuthLearningApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly JwtTokenGenerator _jwtTokenGenerator;

    private readonly AppDbContext _context;
    public AuthController(AppDbContext context, JwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
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
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "User"
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

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return BadRequest("Email or password is incorrect.");
        }
        var tokenResponse = _jwtTokenGenerator.CreateToken(user);

        return Ok(tokenResponse);
    }


    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMe()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var fullName = User.FindFirst(ClaimTypes.Name)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new
        {
            UserId = userId,
            FullName = fullName,
            Email = email
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnly()
    {
        return Ok("Welcome, Admin!");
    }

    [HttpGet("test-exception")]
    public IActionResult TestException()
    {
        throw new Exception("This is a test exception.");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] PaginationRequest request)
    {
        var totalCount = await _context.Users.CountAsync();
        var users = await _context.Users
       .OrderBy(x => x.Id)
       .Skip((request.PageNumber - 1) * request.PageSize)
       .Take(request.PageSize)
       .Select(x => new
       {
           x.Id,
           x.FullName,
           x.Email,
           x.Role,
           x.CreatedDate
       })
       .ToListAsync();

        var result = new PagedResult<object>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            Items = users.Cast<object>().ToList()
        };

        return Ok(result);
    }


}




    }


