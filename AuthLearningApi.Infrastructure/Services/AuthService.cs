using AuthLearningApi.Application.Common.Models;
using AuthLearningApi.Application.DTOs.Auth;
using AuthLearningApi.Application.Interfaces.Services;
using AuthLearningApi.Domain.Entities;
using AuthLearningApi.Infrastructure.Authentication;
using AuthLearningApi.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthLearningApi.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthService(AppDbContext context, JwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        var isEmailExists = await _context.Users.AnyAsync(x => x.Email == request.Email);

        if (isEmailExists)
        {
            throw new InvalidOperationException("This email is already in use.");
        }

        var user = new AppUser
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "User"
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return "User registered successfully.";
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user is null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new InvalidOperationException("Email or password is incorrect.");
        }

        return _jwtTokenGenerator.CreateToken(user);
    }

    public async Task<PagedResult<object>> GetUsersAsync(PaginationRequest request)
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

        return new PagedResult<object>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            Items = users.Cast<object>().ToList()
        };
    }
}
