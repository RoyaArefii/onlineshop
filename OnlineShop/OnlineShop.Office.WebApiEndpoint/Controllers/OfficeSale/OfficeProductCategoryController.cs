using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Contracts.SaleContracts;

namespace OnlineShop.Office.WebApiEndpoint.Controllers.OfficeSale
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeProductCategoryController : ControllerBase
    {
        #region [- Ctor & Feilds -]
        private readonly IAppProductCategoryService _appProductCategoryService;

        public OfficeProductCategoryController(IAppProductCategoryService appProductCategoryService)
        {
            _appProductCategoryService = appProductCategoryService;
        }

        #endregion

        #region [- GetAll-]
        [HttpGet(Name = "GetProductCategory")]
        public async Task<IActionResult> GetAll()
        {
            var getresult = await _appProductCategoryService.GetAsync();
            return new JsonResult(getresult);
        }
        #endregion
    }
}
