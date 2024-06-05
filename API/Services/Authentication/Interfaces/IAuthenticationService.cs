using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.Authentication.Interfaces
{
    public interface IAuthenticationService<T> where T : class 
    {        
        Task<string> GenerateToken(T entity);
    }
}