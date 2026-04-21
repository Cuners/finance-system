
using Auth.Application.UseCases;
using Auth.Application.UseCases.GetUsers.Request;
using Auth.Application.UseCases.GetUsers.Response;
using Auth.Application.UseCases.LoginUser.Request;
using Auth.Application.UseCases.LoginUser.Response;
using Auth.Application.UseCases.LogoutUser;
using Auth.Application.UseCases.LogoutUser.Request;
using Auth.Application.UseCases.LogoutUser.Response;
using Auth.Application.UseCases.RefreshToken.Request;
using Auth.Application.UseCases.RefreshToken.Response;
using Auth.Application.UseCases.RegistrateUser.Request;
using Auth.Application.UseCases.RegistrateUser.Response;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUseCase<LoginRequest, LoginRepsonse> _loginUseCase;
        private readonly IUseCase<RegistrationRequest, RegistrationResponse> _registrateUseCase;
        private readonly IUseCase<UsersRequest, UsersResponse> _getUsersUseCase;
        private readonly IUseCase<RefreshTokenRequest, RefreshTokenResponse> _refreshTokenUseCase;
        private readonly IUseCase<LogoutRequest, LogoutResponse> _logoutUseCase;
        public AuthController(IUseCase<LoginRequest, LoginRepsonse> loginUseCase,
                              IUseCase<RegistrationRequest, RegistrationResponse> registrateUseCase,
                              IUseCase<LogoutRequest, LogoutResponse> logoutUseCase,
                              IUseCase<UsersRequest, UsersResponse> getUsersUseCase,
                              IUseCase<RefreshTokenRequest, RefreshTokenResponse> refreshTokenUseCase)
        {
            _loginUseCase = loginUseCase;
            _registrateUseCase = registrateUseCase;
            _logoutUseCase = logoutUseCase;
            _getUsersUseCase = getUsersUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
        }
        [HttpGet]
        public async Task<ActionResult<UsersResponse>> GetUsers(CancellationToken ct)
        {
            
            var response = await _getUsersUseCase.ExecuteAsync(new UsersRequest(),ct);
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginRepsonse>> Login(LoginRequest loginRequest, CancellationToken ct)
        {
            var response = await _loginUseCase.ExecuteAsync(loginRequest,ct);
            var responses= response as LoginSuccessRepsonse;
            if(responses!=null)
            {
                SetCookies(responses.AccessToken, responses.RefreshToken);
                return Ok(responses.AccessToken);
            }
            return Ok(response);
        }
        [HttpPost("registration")]
        public async Task<ActionResult<RegistrationResponse>> Registration(RegistrationRequest registrationRequest, CancellationToken ct)
        {
            var response = await _registrateUseCase.ExecuteAsync(registrationRequest,ct);
            var responses = response as RegistrationSuccessResponse;
            if (responses != null)
            {
                SetCookies(responses.AccessToken, responses.RefreshToken);
                return Ok(responses.AccessToken);
            }
            return Ok(response);
        }
        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<ActionResult<LogoutResponse>> Logout(CancellationToken ct)
        {

            if (!Request.Cookies.TryGetValue("refresh_token", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
            {
                Response.Cookies.Delete("access_token");
                Response.Cookies.Delete("refresh_token");
                return Ok("No active session");
            }
            var response = await _logoutUseCase.ExecuteAsync(new LogoutRequest(refreshToken), ct);
            if (response is LogoutSuccessRepsonse)
            {

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(-1), 
                    HttpOnly = true,
                    Secure = false, 
                    SameSite = SameSiteMode.Lax
                };
                Response.Cookies.Delete("access_token");
                Response.Cookies.Delete("refresh_token");

                return Ok(new { message = "Tokens deketed" });
            }
            return BadRequest(new { message = "Tokens deleted" });
        }
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshToken(CancellationToken ct)
        {
            if (!Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
            {
                return Unauthorized(new { message = "Refresh token missing" });
            }

            var response = await _refreshTokenUseCase.ExecuteAsync(new RefreshTokenRequest(refreshToken), ct);

            if (response is RefreshTokenSuccessResponse)
            {
                var responses = response as RefreshTokenSuccessResponse;
                SetCookies(responses.AccessToken, responses.RefreshToken);
                return Ok(new { message = "Tokens refreshed" });
            }

            return Unauthorized(new { message = "Invalid refresh token" });
        }
        private void SetCookies(string access, string refresh)
        {
            Response.Cookies.Append("access_token", access, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SameSite = SameSiteMode.Lax,
                Secure = false
            });

            Response.Cookies.Append("refresh_token", refresh, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.Lax,
                Secure = false
            });
        }
    }
}
