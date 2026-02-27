using Finance.Application.UseCases.Accounts.CreateAccount;
using Finance.Application.UseCases.Accounts.GetAccountsByUserId;
using Finance.Application.UseCases.Accounts.DeleteAccount;
using Finance.Application.UseCases.Accounts.GetAccountById;
using Finance.Application.UseCases.Accounts.UpdateAccount;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Finance.Application.UseCases.Budgets.СreateBudget;
using Finance.Application.UseCases.Budgets.DeleteBudget;
using Finance.Application.UseCases.Budgets.GetBudgetById;
using Finance.Application.UseCases.Budgets.GetBudgetsByUserId;
using Finance.Application.UseCases.Budgets.UpdateBudget;
using Finance.Application.UseCases.Categories.GetCategories;
using Finance.Application.UseCases.Transactions.CreateTransaction;
using Finance.Application.UseCases.Transactions.DeleteTransaction;
using Finance.Application.UseCases.Transactions.GetTransactionById;
using Finance.Application.UseCases.Transactions.GetTransactionsByAccountId;
using FluentValidation;
using Finance.Application.Validators;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Finance.Application.UseCases.Transactions.GetTransactions;
using Finance.Application.UseCases.Transactions.GetTransactionsSummary;
using Finance.Application.UseCases.Accounts.GetValueAccounts;
using Finance.Application.UseCases.Budgets.GetBudgetsSummary;
using Finance.Application.UseCases.Budgets.GetBudgetsStatus;
namespace Finance.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateAccountUseCase>();
            services.AddScoped<GetAccountByIdUseCase>();
            services.AddScoped<GetValueAccountsUseCase>();
            services.AddScoped<GetAccountsByUserIdUseCase>();
            services.AddScoped<DeleteAccountUseCase>();
            services.AddScoped<UpdateAccountUseCase>();
            services.AddScoped<CreateBudgetUseCase>();
            services.AddScoped<DeleteBudgetUseCase>();
            services.AddScoped<GetBudgetByIdUseCase>();
            services.AddScoped<GetBudgetsByUserIdUseCase>();
            services.AddScoped<GetBudgetsSummaryUseCase>();
            services.AddScoped<GetBudgetsStatusUseCase>();
            services.AddScoped<UpdateBudgetUseCase>();
            services.AddScoped<GetCategoriesUseCase>();
            services.AddScoped<CreateTransactionUseCase>();
            services.AddScoped<DeleteTransactionUseCase>();
            services.AddScoped<GetTransactionsByAccountIdUseCase>();
            services.AddScoped<GetTransactionByIdUseCase>();
            services.AddScoped<GetTransactionsUseCase>();
            services.AddScoped<GetTransactionsSummaryUseCase>();
            services.AddValidatorsFromAssemblyContaining<CreateAccountRequestValidator>();
            services.AddFluentValidationAutoValidation();
            return services;
        }
    }
}
