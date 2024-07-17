using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Services.SaleServices;
using OnlineShop.Office.WebApiEndpoint.ControllerDtos.SaleDtos;
using PublicTools.Resources;
using ResponseFramework;


namespace OnlineShop.Office.WebApiEndpoint.Controllers.OfficeSale
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeProductController : ControllerBase
    {
        #region [- Ctor & Fields -]
        private readonly IAppProductService _appProductService;

        public OfficeProductController(IAppProductService appProductService)
        {
            _appProductService = appProductService;
        }

        #endregion

        #region [- GetAll -]
        [HttpGet(Name = "GetAllProduct")]
        public async Task<IActionResult> GetAll()
        {
            var getresult = await _appProductService.GetAsync();
            return new JsonResult(getresult);
        }
        #endregion

        #region [GetProduct]
        /// <summary>
        /// خطا داد که 	TypeError: Failed to execute 'fetch' on 'Window': Request with GET/HEAD method cannot have body.
        /// </summary>
        [HttpGet("GetProduct", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(GetProductByIdControllerDto model)
        {
            if (model == null) return new JsonResult(new Response<object>(MessageResource.Error_FailToFindObject));
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var getProduct = new GetProductByIdAppDto()
            {
                Id = model.Id
            };
            var result = await _appProductService.FindById(getProduct.Id);
            if (!result.IsSuccessful) return new JsonResult(new Response<object>(result.ErrorMessage));
            return new JsonResult(result);
        }
        #endregion
    }
}
