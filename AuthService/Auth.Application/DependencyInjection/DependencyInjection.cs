using Auth.Application.UseCases;
using Auth.Application.UseCases.GetUsers.Request;
using Auth.Application.UseCases.GetUsers.Response;
using Auth.Application.UseCases.LoginUser.Response;
using Auth.Application.UseCases.LogoutUser;
using Auth.Application.UseCases.RegistrateUser.Request;
using Auth.Application.UseCases.RegistrateUser.Response;
using Auth.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUseCase<LoginRequest,LoginRepsonse>>();
            services.AddScoped<LogoutUseCase>();
            services.AddScoped<IUseCase<RegistrationRequest,RegistrationResponse>>();
            services.AddScoped<IUseCase<UsersRequest,UsersResponse>>();
            services.AddValidatorsFromAssemblyContaining<LoginUserRequestValidator>();
            services.AddFluentValidationAutoValidation();
            return services;
        }
    }
}
