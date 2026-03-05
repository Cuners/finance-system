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
        public CategoryController(IUseCase<GetCategoriesRequest, GetCategoriesResponse> getCategories)
        {
            _getCategories = getCategories;
        }
        [HttpGet]
        public async Task<ActionResult<GetCategoriesResponse>> GetUserCategories(CancellationToken ct)
        {
            //if (!Request.Cookies.TryGetValue("UserId", out var userIdString))
            //    return Unauthorized("User is not authenticated.");

            //if (!int.TryParse(userIdString, out var userId))
            //    return Unauthorized("Invalid user ID in cookies.");
            GetCategoriesRequest getCategoriesRequest=new GetCategoriesRequest();
            var response = await _getCategories.ExecuteAsync(getCategoriesRequest,ct);
            return Ok(response);
        }
    }
}
