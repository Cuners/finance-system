
using Auth.Application.UseCases;
using Auth.Application.UseCases.GetUsers.Request;
using Auth.Application.UseCases.GetUsers.Response;
using Auth.Application.UseCases.LoginUser.Request;
using Auth.Application.UseCases.LoginUser.Response;
using Auth.Application.UseCases.LogoutUser;
using Auth.Application.UseCases.LogoutUser.Response;
using Auth.Application.UseCases.RegistrateUser.Request;
using Auth.Application.UseCases.RegistrateUser.Response;
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
        private readonly LogoutUseCase _logoutUseCase;
        private readonly IUseCase<UsersRequest, UsersResponse> _getUsersUseCase;
        public AuthController(IUseCase<LoginRequest, LoginRepsonse> loginUseCase,
                              IUseCase<RegistrationRequest, RegistrationResponse> registrateUseCase,
                              LogoutUseCase logoutUseCase,
                              IUseCase<UsersRequest, UsersResponse> getUsersUseCase)
        {
            _loginUseCase = loginUseCase;
            _registrateUseCase = registrateUseCase;
            _logoutUseCase = logoutUseCase;
            _getUsersUseCase = getUsersUseCase;
        }
        [HttpGet]
        public async Task<ActionResult<UsersResponse>> GetUsers(CancellationToken ct)
        {
            
            var response = await _getUsersUseCase.ExecuteAsync(new Application.UseCases.GetUsers.Request.UsersRequest(),ct);
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
        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<LogoutResponse>> Logout()
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
