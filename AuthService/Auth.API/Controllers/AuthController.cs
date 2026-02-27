
using Auth.Application.UseCases.GetUsers;
using Auth.Application.UseCases.LoginUser;
using Auth.Application.UseCases.LoginUser.Request;
using Auth.Application.UseCases.LoginUser.Response;
using Auth.Application.UseCases.LogoutUser;
using Auth.Application.UseCases.LogoutUser.Response;
using Auth.Application.UseCases.RegistrateUser;
using Auth.Application.UseCases.RegistrateUser.Request;
using Auth.Application.UseCases.RegistrateUser.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly LoginUseCase _loginUseCase;
        private readonly RegistrateUseCase _registrateUseCase;
        private readonly LogoutUseCase _logoutUseCase;
        private readonly GetUsersUseCase _getUsersUseCase;
        public AuthController(LoginUseCase loginUseCase, 
                              RegistrateUseCase registrateUseCase,
                              LogoutUseCase logoutUseCase,
                              GetUsersUseCase getUsersUseCase)
        {
            _loginUseCase = loginUseCase;
            _registrateUseCase = registrateUseCase;
            _logoutUseCase = logoutUseCase;
            _getUsersUseCase = getUsersUseCase;
        }
        [HttpGet]
        public async Task<ActionResult> GetUsers(CancellationToken ct)
        {
            var response = await _getUsersUseCase.ExecuteAsync(ct);
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest loginRequest, CancellationToken ct)
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
        public async Task<ActionResult> Registration(RegistrationRequest registrationRequest, CancellationToken ct)
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
        public async Task<ActionResult> Logout()
        {
            var response = _logoutUseCase.Execute();
            if (response is LogoutSuccessRepsonse)
            {
                if (!Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
                {
                    return Ok("No active session");
                }
                Response.Cookies.Delete("access_token");
                Response.Cookies.Delete("refresh_token");
                return Ok("Logged out");
            }
            return BadRequest("Logout failed");
        }
        private void SetCookies(string access, string refresh)
        {
            Response.Cookies.Append("access_token", access, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SameSite = SameSiteMode.Strict,
                Secure = true
            });

            Response.Cookies.Append("refresh_token", refresh, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.Strict,
                Secure = true
            });
        }
    }
}
