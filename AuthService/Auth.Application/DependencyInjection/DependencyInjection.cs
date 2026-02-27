using Auth.Application.UseCases.GetUsers;
using Auth.Application.UseCases.LoginUser;
using Auth.Application.UseCases.LogoutUser;
using Auth.Application.UseCases.RegistrateUser;
using Auth.Application.Validators;
using FluentValidation;
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
            services.AddScoped<LoginUseCase>();
            services.AddScoped<LogoutUseCase>();
            services.AddScoped<RegistrateUseCase>();
            services.AddScoped<GetUsersUseCase>();
            services.AddValidatorsFromAssemblyContaining<LoginUserRequestValidator>();
            services.AddFluentValidationAutoValidation();
            return services;
        }
    }
}
