using Finance.Application.Common;
using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Finance.Application.UseCases.Budgets.СreateBudget
{
    public class CreateBudgetUseCase : ICreateBudgetUseCase
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateBudgetUseCase> _logger;
        private readonly IBudgetCacheInvalidator _cache;

        public CreateBudgetUseCase(
            IBudgetRepository budgetRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateBudgetUseCase> logger,
            IBudgetCacheInvalidator cache)
        {
            _budgetRepository = budgetRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<CreateBudgetResult>> ExecuteAsync(
            CreateBudgetCommand command,
            int userId,
            CancellationToken ct)
        {
            try
            {
                var budget = new Domain.Budget
                {
                    UserId = userId,
                    Name = command.Name,
                    LimitAmount = command.LimitAmount,
                    Date = DateOnly.FromDateTime(DateTime.UtcNow),
                    CategoryId = command.CategoryId
                };

                await _budgetRepository.CreateBudget(budget);
                await _unitOfWork.SaveChangesAsync(ct);
                await _cache.InvalidateAsync(userId, budget.BudgetId, ct);

                return Result<CreateBudgetResult>.Success(new CreateBudgetResult(budget.BudgetId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create budget {BudgetName} for user {UserId}", command.Name, userId);
                return Result<CreateBudgetResult>.Failure(
                    "BUDGET_CREATE_FAILED",
                    "Unable to create budget at this time");
            }
        }
    }
}
