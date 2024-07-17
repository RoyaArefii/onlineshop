using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.SaleDtos.Order;
using PublicTools.Resources;
using ResponseFramework;
using System.Security.Claims;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeSales
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeOrderController : ControllerBase
    {
        #region [- Ctor & Fields -]
        private readonly IAppOrderService<GetAllOrderAppDto, GetOrdersAppDto> _appOrderHeaderlService;

        public BackOfficeOrderController(IAppOrderService<GetAllOrderAppDto, GetOrdersAppDto> appOrderHeaderlService)
        {
            _appOrderHeaderlService = appOrderHeaderlService;
        }

        #endregion

        #region [- Guard -]
        private static JsonResult Guard(PutOrderControllerDto model)
        {
            var details = model.orderDetails;
            var header = model.orderHeader;
            foreach (var detail in details)
            {
                //if (detail.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
                if (detail.Code.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
                if (detail.IsActive.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
                if (detail.ProductId.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
                if (detail.Quantity.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
                if (detail.Title.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
                if (detail.UnitPrice.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            }
            if (header.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (header.SellerId.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return header.IsActive.Equals(null) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }
        private static JsonResult Guard(PostOrderControllerDto model)
        {
            var details = model.OrderDetails;
            var header = model.OrderHeader;
            foreach (var detail in details)
            {
                if (detail.Code.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
                if (detail.ProductId.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
                if (detail.Quantity.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
                if (detail.Title.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
                if (detail.UnitPrice.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            }
            if (header.Code.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (header.SellerId.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return header.Title.Equals(null) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        } 
        #endregion

        #region [- Put -]
        [HttpPut(Name = "PutOrderHeader")]
        [Authorize]
        public async Task<IActionResult> Put(PutOrderControllerDto model)
        {
            Guard(model);
            var test = Response.Headers;
            
            var orderDetails = new List<PutOrderDetailAppDto>();
            var headerModel = model.orderHeader;
            foreach (var detail in model.orderDetails)
            {
                var DetailModel = new PutOrderDetailAppDto();
                DetailModel.Id =  detail.Id;
                DetailModel.ProductId = detail.ProductId;
                DetailModel.Code = detail.Code;
                DetailModel.Title = detail.Title;
                DetailModel.UnitPrice = detail.UnitPrice;
                DetailModel.Quantity = detail.Quantity;
                DetailModel.EntityDescription = detail.EntityDescription;
                DetailModel.IsActive = detail.IsActive;
                orderDetails.Add(DetailModel);
            }
            var putHeader = new PutOrderHeaderAppDto()
            {
                Id = headerModel.Id,
                EntityDescription = headerModel.EntityDescription,
                SellerId = headerModel.SellerId,
                IsActive = headerModel.IsActive
            };
            var newModel = new PutOrderAppDto()
            {
                orderDetails = orderDetails,
                orderHeader = putHeader,
                UserName = GetCurrentUser().Value.ToString()
            };
            var putResult = await _appOrderHeaderlService.PutAsync(newModel);
            return new JsonResult(putResult);
        } 
        #endregion

        #region [- Post -]
        [HttpPost(Name = "PostOrderHeader")]
        [Authorize(Roles = "Admin, GodAdmin")]
        public async Task<IActionResult> Post(PostOrderControllerDto model)
        {

            Guard(model);
            var postModel = new PostOrderAppDto
            {
                OrderDetails = model.OrderDetails,
                OrderHeader = model.OrderHeader,
                UserName = GetCurrentUser().Value.ToString()
            };
            var postResult = await _appOrderHeaderlService.PostAsync(postModel);
            if (!postResult.IsSuccessful) { return new JsonResult(new Response<object>(postResult.ErrorMessage)); }
            return new JsonResult(postResult);
        }
        #endregion

        #region [- Delete -]
        [HttpDelete(Name = "DeleteOrder")]
        [Authorize(Roles = "Admin , GodAdmin")]
        public async Task<IActionResult> Delete(DeleteOrderDetailControllerDtos model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var user = GetCurrentUser().Value.ToString();
            var deleteModel = new DeleteOrderAppDto()
            {
                Id = model.Id,
                UserName = GetCurrentUser().Value.ToString()
            };
            var postResult = await _appOrderHeaderlService.DeleteAsync(deleteModel);
            return new JsonResult(postResult);
        }
        #endregion

        #region [- Get -]
        [HttpGet(Name = "GetOrder")]
        [Authorize(Roles = "Admin,GodAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var user = GetCurrentUser().Value.ToString();
            var getresult = await _appOrderHeaderlService.GetAsync();
            return new JsonResult(getresult);
        }
        #endregion
       
        //#region [- DeleteOrderDetailAsync -]
        //[HttpDelete("DeleteOrderDetails", Name = "DeleteOrderDetails")]
        //[Authorize(Roles = "Admin , GodAdmin")]
        //public async Task<IActionResult> DeleteOrderDetail(List<DeleteOrderAppDtos> model)
        //{
        //    if (model.Count.Equals(0)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
        //    var postResult = await _appOrderHeaderlService.DeleteOrderDetailAsync(model);
        //    return new JsonResult(postResult);
        //} 
        //#endregion

        #region [- OtherMethods -]
        private JsonResult GetCurrentUser()
        {
            var identity = User.Claims.ToList<Claim>();
            foreach (var claim in identity)
            {
                if (claim.Type == "Name")
                {
                    string user = claim.Value;
                    return new JsonResult(user);
                }
            }
            return new JsonResult(null);
        }
        #endregion

    }
}
