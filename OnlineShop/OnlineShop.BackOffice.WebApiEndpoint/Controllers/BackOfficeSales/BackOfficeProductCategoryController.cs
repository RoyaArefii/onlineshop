using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.ProductCategory;
using PublicTools.Resources;
using ResponseFramework;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeSales
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeProductCategoryController : ControllerBase
    {
        private readonly IAppProductCategoryService _appProductCategoryService;

        public BackOfficeProductCategoryController(IAppProductCategoryService appProductCategoryService)
        {
            _appProductCategoryService = appProductCategoryService;
        }
        private static JsonResult Guard(PutProductCategoryAppDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.IsActive.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return model.Title.IsNullOrEmpty() ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }

        private static JsonResult Guard(PostProductCategoryAppDto model)
        {

            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.IsActive.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return model.Title.IsNullOrEmpty() ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }

        [HttpPut(Name = "PutProductCategory")]
        public async Task<IActionResult> Put(PutProductCategoryAppDto model)
        {
            Guard(model);
            var putResult = await _appProductCategoryService.PutAsync(model);
            return new JsonResult(putResult);
        }

        [HttpPost(Name = "PostProductCategory")]
        public async Task<IActionResult> Post(PostProductCategoryAppDto model)
        {
            Guard(model);
            var postResult = await _appProductCategoryService.PostAsync(model);
            return new JsonResult(postResult);
        }

        [HttpDelete(Name = "DeleteProductCategory")]
        public async Task<IActionResult> Delete(DeleteProductCategoryAppDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var postResult = await _appProductCategoryService.DeleteAsync(model);
            return new JsonResult(postResult);
        }

        [HttpGet(Name = "GetProductCategory")]
        public async Task<IActionResult> GetAll()
        {
            var getresult = await _appProductCategoryService.GetAsync();
            return new JsonResult(getresult);
        }
    }
}
