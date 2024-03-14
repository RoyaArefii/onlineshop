using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.ProductAppDtos;
using PublicTools.Resources;
using ResponseFramework;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeProductController : ControllerBase
    {
        private readonly IAppProductService _appProductService;

        public BackOfficeProductController(IAppProductService appProductService)
        {
            _appProductService = appProductService;
        }
        private static JsonResult Guard (PutProductAppDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult (new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Title.IsNullOrEmpty()) return new JsonResult( new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.UnitPrice.Equals(null)) return new JsonResult( new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
             return (model.ProductCategoryId.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)): new JsonResult(null);
        }

        private static JsonResult Guard(PostProductAppDto model)
        {

            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Title.IsNullOrEmpty()) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.UnitPrice.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return (model.ProductCategoryId.Equals(null)) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }
        [HttpPut(Name = "PutProduct")]
        public async Task<IActionResult> Put(PutProductAppDto model)
        {
            Guard(model);
           var putResult = await _appProductService.PutAsync(model);
            return new JsonResult(putResult);   
        }
        [HttpPost(Name = "PostProduct")]
        public async Task<IActionResult> Post(PostProductAppDto model)
        {
            Guard(model);
            var postResult = await _appProductService.PostAsync(model);
            return new JsonResult(postResult);
        }
        [HttpDelete (Name ="DeleteProduct")]
        public async Task<IActionResult> Delete(DeleteProductAppDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult (new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var postResult = await _appProductService.DeleteAsync(model);
            return new JsonResult(postResult);
        }

        [HttpGet (Name = "GetProduct")]
        public async Task<IActionResult> GetAll()
        {
            var getresult = await _appProductService.GetAsync();
            return new JsonResult(getresult);   
        }
    }
}
