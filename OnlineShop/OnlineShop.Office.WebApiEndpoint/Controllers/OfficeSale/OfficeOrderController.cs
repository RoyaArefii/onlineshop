using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos;

namespace OnlineShop.Office.WebApiEndpoint.Controllers.OfficeSale
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeOrderController : ControllerBase
    {
        //[HttpGet("GetOrderUser", Name = "GetOrderUser")]
        //public async Task<IActionResult> GetAllUser()
        //{
        //    var user = GetCurrentUser().Value.ToString();
        //    var getModel = new GetAllOrderAppDto()
        //    {
        //        UserName = user
        //    };
        //    var getresult = await _appOrderHeaderlService.GetUsersOrder(getModel);
        //    return new JsonResult(getresult);
        //}
    }
}
