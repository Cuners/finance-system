using Finance.Application.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Finance.Infrastructure.Services
{
    //Используется для получения userId из кук Exchange method
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        public CurrentUserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public int UserId => int.Parse(_httpContext.HttpContext.User.FindFirstValue("user_id"));
    }
}
