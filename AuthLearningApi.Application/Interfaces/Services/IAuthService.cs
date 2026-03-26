using AuthLearningApi.Application.Common.Models;
using AuthLearningApi.Application.DTOs.Auth;

namespace AuthLearningApi.Application.Interfaces.Services;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<PagedResult<object>> GetUsersAsync(PaginationRequest request);
}


//register ve login için işlemlerini yapacak service 