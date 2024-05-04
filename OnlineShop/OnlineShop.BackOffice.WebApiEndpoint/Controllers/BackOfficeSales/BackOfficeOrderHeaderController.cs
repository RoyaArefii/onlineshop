using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderHeaderAppDtos;
using PublicTools.Resources;
using ResponseFramework;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeSales
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeOrderHeaderController : ControllerBase
    {
        private readonly IAppOrderHeaderService _appOrderHeaderlService;

        public BackOfficeOrderHeaderController(IAppOrderHeaderService appOrderHeaderlService)
        {
            _appOrderHeaderlService = appOrderHeaderlService;
        }

        private static JsonResult Guard(PutOrderHeaderAppDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Code.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Seller.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Buyer.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return model.OrderDate.Equals(null) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }

        private static JsonResult Guard(PostOrderHeaderAppDto model)
        {

            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Code.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Seller.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Buyer.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return model.OrderDate.Equals(null) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }

        [HttpPut(Name = "PutOrderHeader")]
        public async Task<IActionResult> Put(PutOrderHeaderAppDto model)
        {
            Guard(model);
            var putResult = await _appOrderHeaderlService.PutAsync(model);
            return new JsonResult(putResult);
        }

        [HttpPost(Name = "PostOrderHeader")]
        public async Task<IActionResult> Post(PostOrderHeaderAppDto model)
        {
            Guard(model);
            var postResult = await _appOrderHeaderlService.PostAsync(model);
            return new JsonResult(postResult);
        }

        [HttpDelete(Name = "DeleteOrderHeader")]
        public async Task<IActionResult> Delete(DeleteOrderHeaderAppDtos model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var postResult = await _appOrderHeaderlService.DeleteAsync(model);
            return new JsonResult(postResult);
        }

        [HttpGet(Name = "GetOrderHeader")]
        public async Task<IActionResult> GetAll()
        {
            var getresult = await _appOrderHeaderlService.GetAsync();
            return new JsonResult(getresult);
        }
    }
}
