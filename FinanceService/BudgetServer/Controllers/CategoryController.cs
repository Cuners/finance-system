using Finance.Application.Services;
using Finance.Application.UseCases;
using Finance.Application.UseCases.Categories.GetCategories.Request;
using Finance.Application.UseCases.Categories.GetCategories.Response;
using Microsoft.AspNetCore.Mvc;

namespace BudgetServer.Controllers
{
    [Route("api/budget/[controller]")]
    [ApiController]
    public class CategoryController: Controller
    {
        private readonly IUseCase<GetCategoriesRequest,GetCategoriesResponse> _getCategories;
        private readonly ICurrentUserService _currentUser;
        public CategoryController(IUseCase<GetCategoriesRequest, GetCategoriesResponse> getCategories,
                                  ICurrentUserService currentUser)
        {
            _getCategories = getCategories;
            _currentUser = currentUser;
        }
        [HttpGet]
        public async Task<ActionResult<GetCategoriesResponse>> GetUserCategories(CancellationToken ct)
        {
            int userId = _currentUser.UserId;
            GetCategoriesRequest getCategoriesRequest=new GetCategoriesRequest();
            var response = await _getCategories.ExecuteAsync(getCategoriesRequest,userId,ct);
            return Ok(response);
        }
    }
}
