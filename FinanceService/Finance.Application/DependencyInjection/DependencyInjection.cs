using Finance.Application.UseCases;
using Finance.Application.UseCases.Accounts.CreateAccount;
using Finance.Application.UseCases.Accounts.DeleteAccount;
using Finance.Application.UseCases.Accounts.GetAccountById;
using Finance.Application.UseCases.Accounts.GetAccountsByUserId;
using Finance.Application.UseCases.Accounts.GetValueAccounts;
using Finance.Application.UseCases.Accounts.UpdateAccount;
using Finance.Application.UseCases.Budgets.DeleteBudget;
using Finance.Application.UseCases.Budgets.GetBudgetById;
using Finance.Application.UseCases.Budgets.GetBudgetsByUserId;
using Finance.Application.UseCases.Budgets.GetBudgetsStatus;
using Finance.Application.UseCases.Budgets.GetBudgetsSummary;
using Finance.Application.UseCases.Budgets.UpdateBudget;
using Finance.Application.UseCases.Budgets.СreateBudget;
using Finance.Application.UseCases.Categories.GetCategories;
using Finance.Application.UseCases.Transactions.CreateTransaction;
using Finance.Application.UseCases.Transactions.DeleteTransaction;
using Finance.Application.UseCases.Transactions.GetTransactionById;
using Finance.Application.UseCases.Transactions.GetTransactions;
using Finance.Application.UseCases.Transactions.GetTransactionsByAccountId;
using Finance.Application.UseCases.Transactions.GetTransactionsSummary;
using Finance.Application.UseCases.Transactions.UpdateTransaction;
using Finance.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
namespace Finance.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateAccountUseCase, CreateAccountUseCase>();
            services.AddScoped<IGetAccountByIdUseCase, GetAccountByIdUseCase>();
            services.AddScoped<IGetValueAccountsUseCase, GetValueAccountsUseCase>();
            services.AddScoped<IGetAccountsByUserIdUseCase, GetAccountsByUserIdUseCase>();
            services.AddScoped<IDeleteAccountUseCase, DeleteAccountUseCase>();
            services.AddScoped<IUpdateAccountUseCase, UpdateAccountUseCase>();
            services.AddScoped<ICreateBudgetUseCase, CreateBudgetUseCase>();
            services.AddScoped<IDeleteBudgetUseCase, DeleteBudgetUseCase>();
            services.AddScoped<IGetBudgetByIdUseCase, GetBudgetByIdUseCase>();
            services.AddScoped<IGetBudgetsByUserIdUseCase, GetBudgetsByUserIdUseCase>();
            services.AddScoped<IGetBudgetsSummaryUseCase, GetBudgetsSummaryUseCase>();
            services.AddScoped<IGetBudgetsStatusUseCase, GetBudgetsStatusUseCase>();
            services.AddScoped<IUpdateBudgetUseCase, UpdateBudgetUseCase>();
            services.AddScoped<IGetCategoriesUseCase, GetCategoriesUseCase>();
            services.AddScoped<ICreateTransactionUseCase, CreateTransactionUseCase>();
            services.AddScoped<IDeleteTransactionUseCase, DeleteTransactionUseCase>();
            services.AddScoped<IUpdateTransactionUseCase, UpdateTransactionUseCase>();
            services.AddScoped<IGetTransactionsByAccountIdUseCase, GetTransactionsByAccountIdUseCase>();
            services.AddScoped<IGetTransactionByIdUseCase, GetTransactionByIdUseCase>();
            services.AddScoped<IGetTransactionsUseCase, GetTransactionsUseCase>();
            services.AddScoped<IGetTransactionsSummaryUseCase, GetTransactionsSummaryUseCase>();
            services.AddValidatorsFromAssemblyContaining<CreateAccountCommandValidator>();
            services.AddFluentValidationAutoValidation();
            return services;
        }
    }
}
