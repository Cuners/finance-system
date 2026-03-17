using Auth.Application.UseCases;
using Auth.Application.UseCases.GetUsers.Request;
using Auth.Application.UseCases.GetUsers.Response;
using Auth.Application.UseCases.LoginUser.Response;
using Auth.Application.UseCases.LoginUser.Request;
using Auth.Application.UseCases.LogoutUser;
using Auth.Application.UseCases.LogoutUser.Request;
using Auth.Application.UseCases.LogoutUser.Response;
using Auth.Application.UseCases.RefreshToken.Request;
using Auth.Application.UseCases.RefreshToken.Response;
using Auth.Application.UseCases.RegistrateUser.Request;
using Auth.Application.UseCases.RegistrateUser.Response;
using Auth.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Auth.Application.UseCases.LoginUser;
using Auth.Application.UseCases.RegistrateUser;
using Auth.Application.UseCases.GetUsers;
using Auth.Application.UseCases.RefreshToken;

namespace Auth.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUseCase<LoginRequest, LoginRepsonse>, LoginUseCase>();
            services.AddScoped<IUseCase<LogoutRequest,LogoutResponse>, LogoutUseCase>();
            services.AddScoped<IUseCase<RegistrationRequest,RegistrationResponse>, RegistrateUseCase>();
            services.AddScoped<IUseCase<UsersRequest,UsersResponse>, GetUsersUseCase>();
            services.AddScoped<IUseCase<RefreshTokenRequest, RefreshTokenResponse>, RefreshTokenUseCase>();
            services.AddValidatorsFromAssemblyContaining<LoginUserRequestValidator>();
            services.AddFluentValidationAutoValidation();
            return services;
        }
    }
}
