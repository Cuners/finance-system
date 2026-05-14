using Finance.Application.Services;
using Finance.Application.UseCases.Budgets.DeleteBudget;
using Finance.Application.UseCases.Budgets.GetBudgetById;
using Finance.Application.UseCases.Budgets.GetBudgetsByUserId;
using Finance.Application.UseCases.Budgets.GetBudgetsStatus;
using Finance.Application.UseCases.Budgets.GetBudgetsSummary;
using Finance.Application.UseCases.Budgets.UpdateBudget;
using Finance.Application.UseCases.Budgets.СreateBudget;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetServer.Controllers
{
    [Route("api/budget/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly ICreateBudgetUseCase _createBudget;
        private readonly IGetBudgetsByUserIdUseCase _getBudgets;
        private readonly IDeleteBudgetUseCase _deleteBudget;
        private readonly IGetBudgetByIdUseCase _getBudgetById;
        private readonly IUpdateBudgetUseCase _updateBudget;
        private readonly IGetBudgetsSummaryUseCase _getBudgetsSummary;
        private readonly IGetBudgetsStatusUseCase _getBudgetsStatus;
        private readonly ICurrentUserService _currentUser;

        public BudgetController(
            ICreateBudgetUseCase createBudget,
            IGetBudgetsByUserIdUseCase getBudgets,
            IDeleteBudgetUseCase deleteBudget,
            IGetBudgetByIdUseCase getBudgetById,
            IUpdateBudgetUseCase updateBudget,
            IGetBudgetsSummaryUseCase getBudgetsSummary,
            IGetBudgetsStatusUseCase getBudgetsStatus,
            ICurrentUserService currentUser)
        {
            _createBudget = createBudget;
            _getBudgets = getBudgets;
            _getBudgetById = getBudgetById;
            _updateBudget = updateBudget;
            _deleteBudget = deleteBudget;
            _getBudgetsSummary = getBudgetsSummary;
            _getBudgetsStatus = getBudgetsStatus;
            _currentUser = currentUser;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GetBudgetsByUserIdResult>> GetUserBudgets(CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _getBudgets.ExecuteAsync(
                new GetBudgetsByUserIdQuery(), 
                userId, 
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpGet("summary")]
        [Authorize]
        public async Task<ActionResult<GetBudgetsSummaryResult>> GetBudgetSummary(CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _getBudgetsSummary.ExecuteAsync(
                new GetBudgetsSummaryQuery(), 
                userId, 
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpGet("status")]
        [Authorize]
        public async Task<ActionResult<GetBudgetsStatusResult>> GetBudgetStatus(CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _getBudgetsStatus.ExecuteAsync(
                new GetBudgetsStatusQuery(), 
                userId, 
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpGet("{budgetid}")]
        [Authorize]
        public async Task<ActionResult<GetBudgetByIdResult>> GetBudgetById(
            int budgetid, 
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _getBudgetById.ExecuteAsync(
                new GetBudgetByIdQuery(budgetid), 
                userId, 
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpPut("{budgetid}")]
        [Authorize]
        public async Task<ActionResult<UpdateBudgetResult>> Update(
            UpdateBudgetCommand command,
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _updateBudget.ExecuteAsync(command, userId, ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpDelete("{budgetid}")]
        [Authorize]
        public async Task<ActionResult<DeleteBudgetResult>> Delete(
            int budgetid, 
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _deleteBudget.ExecuteAsync(
                new DeleteBudgetCommand(budgetid), 
                userId, 
                ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateBudgetResult>> Create(
            CreateBudgetCommand command,
            CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _createBudget.ExecuteAsync(command, userId, ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
    }
}
