using Finance.Application.UseCases;
using Finance.Application.UseCases.Budgets.DeleteBudget.Request;
using Finance.Application.UseCases.Budgets.DeleteBudget.Response;
using Finance.Application.UseCases.Budgets.GetBudgetById.Request;
using Finance.Application.UseCases.Budgets.GetBudgetById.Response;
using Finance.Application.UseCases.Budgets.GetBudgetsByUserId.Request;
using Finance.Application.UseCases.Budgets.GetBudgetsByUserId.Response;
using Finance.Application.UseCases.Budgets.GetBudgetsStatus.Request;
using Finance.Application.UseCases.Budgets.GetBudgetsStatus.Response;
using Finance.Application.UseCases.Budgets.GetBudgetsSummary.Request;
using Finance.Application.UseCases.Budgets.GetBudgetsSummary.Response;
using Finance.Application.UseCases.Budgets.UpdateBudget.Request;
using Finance.Application.UseCases.Budgets.UpdateBudget.Response;
using Finance.Application.UseCases.Budgets.СreateBudget.Request;
using Finance.Application.UseCases.Budgets.СreateBudget.Response;
using Microsoft.AspNetCore.Mvc;

namespace BudgetServer.Controllers
{
    [Route("api/budget/[controller]")]
    [ApiController]
    public class BudgetController : Controller
    {
        private readonly IUseCase<CreateBudgetRequest, CreateBudgetResponse> _createBudget;
        private readonly IUseCase<GetBudgetsByUserIdRequest, GetBudgetsByUserIdResponse> _getBudgets;
        private readonly IUseCase<DeleteBudgetRequest, DeleteBudgetResponse> _deleteBudget;
        private readonly IUseCase<GetBudgetByIdRequest, GetBudgetByIdResponse> _getBudgetById;
        private readonly IUseCase<UpdateBudgetRequest, UpdateBudgetResponse> _updateBudget;
        private readonly IUseCase<GetBudgetsSummaryRequest, GetBudgetsSummaryResponse> _getBudgetsSummary;
        private readonly IUseCase<GetBudgetsStatusRequest, GetBudgetsStatusResponse> _getBudgetsStatus;

        public BudgetController(IUseCase<CreateBudgetRequest, CreateBudgetResponse> createBudget,
                                IUseCase<GetBudgetsByUserIdRequest, GetBudgetsByUserIdResponse> getBudgets,
                                IUseCase<DeleteBudgetRequest, DeleteBudgetResponse> deleteBudget,
                                IUseCase<GetBudgetByIdRequest, GetBudgetByIdResponse> getBudgetById,
                                IUseCase<UpdateBudgetRequest, UpdateBudgetResponse> updateBudget,
                                IUseCase<GetBudgetsSummaryRequest, GetBudgetsSummaryResponse> getBudgetsSummary,
                                IUseCase<GetBudgetsStatusRequest, GetBudgetsStatusResponse> getBudgetsStatus)
        {
            _createBudget = createBudget;
            _getBudgets = getBudgets;
            _getBudgetById = getBudgetById;
            _updateBudget = updateBudget;
            _deleteBudget = deleteBudget;
            _getBudgetsSummary = getBudgetsSummary;
            _getBudgetsStatus = getBudgetsStatus;
        }
        [HttpGet]
        public async Task<ActionResult<GetBudgetsByUserIdResponse>> GetUserBudgets(CancellationToken ct)
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
        public async Task<ActionResult<GetBudgetsSummaryResponse>> GetBudgetSummary(CancellationToken ct)
        {
            int userId = 1;
            var response = await _getBudgetsSummary.ExecuteAsync(new GetBudgetsSummaryRequest { UserId = userId }, ct);
            return Ok(response);
        }
        [HttpGet("status")]
        public async Task<ActionResult<GetBudgetsStatusResponse>> GetBudgetStatus(CancellationToken ct)
        {
            int userId = 1;
            var response = await _getBudgetsStatus.ExecuteAsync(new GetBudgetsStatusRequest { UserId = userId }, ct);
            return Ok(response);
        }
        [HttpGet("{budgetid}")]
        public async Task<ActionResult<GetBudgetByIdResponse>> GetBudgetById(int budgetid, CancellationToken ct)
        {
            var request = new GetBudgetByIdRequest { BudgetId = budgetid };
            var response = await _getBudgetById.ExecuteAsync(request, ct);

            return Ok(response);
        }
        [HttpPut("{budgetid}")]
        public async Task<ActionResult<UpdateBudgetResponse>> Update(UpdateBudgetRequest request, CancellationToken ct)
        {
            var response = await _updateBudget.ExecuteAsync(request, ct);

            return Ok(response);
        }
        [HttpDelete("{budgetid}")]
        public async Task<ActionResult<DeleteBudgetResponse>> Delete(int budgetid, CancellationToken ct)
        {
            var request = new DeleteBudgetRequest { BudgetId = budgetid };
            var response = await _deleteBudget.ExecuteAsync(request, ct);
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<CreateBudgetResponse>> Create(CreateBudgetRequest request, CancellationToken ct)
        {
            request.UserId = 1;
            var response= await _createBudget.ExecuteAsync(request, ct);
            return Ok(response);
        }
    }
}
