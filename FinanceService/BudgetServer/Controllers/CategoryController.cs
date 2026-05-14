using Finance.Application.Services;
using Finance.Application.UseCases.Categories.GetCategories;
using Microsoft.AspNetCore.Mvc;

namespace BudgetServer.Controllers
{
    [Route("api/budget/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGetCategoriesUseCase _getCategories;
        private readonly ICurrentUserService _currentUser;

        public CategoryController(
            IGetCategoriesUseCase getCategories,
            ICurrentUserService currentUser)
        {
            _getCategories = getCategories;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<ActionResult<GetCategoriesResult>> GetUserCategories(CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var result = await _getCategories.ExecuteAsync(new GetCategoriesQuery(), userId, ct);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
    }
}
