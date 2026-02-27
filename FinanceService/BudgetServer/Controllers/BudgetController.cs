using Finance.Application.UseCases.Budgets.DeleteBudget.Request;
using Finance.Application.UseCases.Budgets.GetBudgetById.Request;
using Finance.Application.UseCases.Budgets.GetBudgetsByUserId.Request;
using Finance.Application.UseCases.Budgets.UpdateBudget.Request;
using Finance.Application.UseCases.Budgets.DeleteBudget;
using Finance.Application.UseCases.Budgets.GetBudgetById;
using Finance.Application.UseCases.Budgets.GetBudgetsByUserId;
using Finance.Application.UseCases.Budgets.UpdateBudget;
using Finance.Application.UseCases.Budgets.СreateBudget;
using Finance.Application.UseCases.Budgets.СreateBudget.Request;
using Finance.Domain;
using Microsoft.AspNetCore.Mvc;
using Finance.Application.UseCases.Budgets.GetBudgetsSummary;
using Finance.Application.UseCases.Budgets.GetBudgetsSummary.Request;
using Finance.Application.UseCases.Budgets.GetBudgetsStatus.Request;
using Finance.Application.UseCases.Budgets.GetBudgetsStatus;
using Finance.Application.UseCases.Budgets;

namespace BudgetServer.Controllers
{
    [Route("api/budget/[controller]")]
    [ApiController]
    public class BudgetController : Controller
    {
        private readonly CreateBudgetUseCase _createBudget;
        private readonly GetBudgetsByUserIdUseCase _getBudgets;
        private readonly DeleteBudgetUseCase _deleteBudget;
        private readonly GetBudgetByIdUseCase _getBudgetById;
        private readonly UpdateBudgetUseCase _updateBudget;
        private readonly GetBudgetsSummaryUseCase _getBudgetsSummary;
        private readonly GetBudgetsStatusUseCase _getBudgetsStatus;
        public BudgetController(CreateBudgetUseCase createBudget, 
                                GetBudgetsByUserIdUseCase getBudgets, 
                                DeleteBudgetUseCase deleteBudget, 
                                GetBudgetByIdUseCase getBudgetById, 
                                UpdateBudgetUseCase updateBudget,
                                GetBudgetsSummaryUseCase getBudgetsSummary,
                                GetBudgetsStatusUseCase getBudgetsStatus)
        {
            _createBudget = createBudget;
            _getBudgets = getBudgets;
            _getBudgetById = getBudgetById;
            _updateBudget = updateBudget;
            _deleteBudget = deleteBudget;
            _getBudgetsSummary = getBudgetsSummary;
            _getBudgetsStatus= getBudgetsStatus;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Budget>>> GetUserBudgets(CancellationToken ct)
        {
            //if (!Request.Cookies.TryGetValue("UserId", out var userIdString))
            //    return Unauthorized("User is not authenticated.");

            //if (!int.TryParse(userIdString, out var userId))
            //    return Unauthorized("Invalid user ID in cookies.");
            int userId = 1;
            var response = await _getBudgets.ExecuteAsync(
                new GetBudgetsByUserIdRequest { UserId = userId },
                ct
            );

            return Ok(response);
        }
        [HttpGet("summary")]
        public async Task<ActionResult<BudgetSummaryDto>> GetBudgetSummary(CancellationToken ct)
        {
            int userId = 1;
            var response = await _getBudgetsSummary.ExecuteAsync(new GetBudgetsSummaryRequest { UserId = userId }, ct);
            return Ok(response);
        }
        [HttpGet("status")]
        public async Task<ActionResult<IEnumerable<BudgetStatus>>> GetBudgetStatus(CancellationToken ct)
        {
            int userId = 1;
            var response = await _getBudgetsStatus.ExecuteAsync(new GetBudgetsStatusRequest { UserId = userId }, ct);
            return Ok(response);
        }
        [HttpGet("{budgetid}")]
        public async Task<ActionResult<BudgetDto>> GetBudgetById(int budgetid, CancellationToken ct)
        {
            var request = new GetBudgetByIdRequest { BudgetId = budgetid };
            var response = await _getBudgetById.ExecuteAsync(request, ct);

            return Ok(response);
        }
        [HttpPut("{budgetid}")]
        public async Task<ActionResult<BudgetDto>> Update(UpdateBudgetRequest request, CancellationToken ct)
        {
            var response = await _updateBudget.ExecuteAsync(request, ct);

            return Ok(response);
        }
        [HttpDelete("{budgetid}")]
        public async Task<ActionResult> Delete(int budgetid, CancellationToken ct)
        {
            var request = new DeleteBudgetRequest { BudgetId = budgetid };
            var response = await _deleteBudget.ExecuteAsync(request, ct);
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<BudgetDto>> Create(CreateBudgetRequest request, CancellationToken ct)
        {
            var response= await _createBudget.ExecuteAsync(request, ct);
            return Ok(response);
        }
    }
}
