using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using PublicTools.Resources;
using ResponseFramework;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeSales
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeOrderController : ControllerBase
    {
        private readonly IAppOrderHeaderService<DeleteOrderDetailAppDto> _appOrderHeaderlService;

        public BackOfficeOrderController(IAppOrderHeaderService<DeleteOrderDetailAppDto> appOrderHeaderlService)
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

        private static JsonResult Guard(PostOrder model)
        {

            //if (model.ProductId.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.OrderHeader.SellerId.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.OrderHeader.BuyerId.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.OrderHeader.Quantity.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return model.OrderHeader.Code.Equals(null) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }

        [HttpPut(Name = "PutOrderHeader")]
        public async Task<IActionResult> Put(PutOrderHeaderAppDto model)
        {
            Guard(model);
            var putResult = await _appOrderHeaderlService.PutAsync(model);
            return new JsonResult(putResult);
        }

        [HttpPost(Name = "PostOrderHeader")]
        public async Task<IActionResult> Post(PostOrder model)
        {
            Guard(model);
            var postResult = await _appOrderHeaderlService.PostAsync(model);
            return new JsonResult(postResult);
        }

        [HttpDelete(Name = "DeleteOrder")]
        public async Task<IActionResult> Delete(DeleteOrderDetailAppDtos model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var postResult = await _appOrderHeaderlService.DeleteAsync(model);
            return new JsonResult(postResult);
        }
        [HttpDelete("DeleteOrderDetailAsync", Name = "DeleteOrderDetails")]
        public async Task<IActionResult> DeleteOrderDetail(List<DeleteOrderDetailAppDto> model)
        {
            if (model.Count.Equals(0)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var postResult = await _appOrderHeaderlService.DeleteOrderDetailAsync(model);
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
