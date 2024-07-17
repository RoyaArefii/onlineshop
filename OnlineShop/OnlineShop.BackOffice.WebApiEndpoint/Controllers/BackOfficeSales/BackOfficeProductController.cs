using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.ProductAppDtos;
using OnlineShop.Application.Services.SaleServices;
using OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.SaleDtos.Product;
using PublicTools.Resources;
using ResponseFramework;
using System.Security.Claims;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeSales
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeProductController : ControllerBase
    {
        #region [- Ctor & Fields -]
        private readonly IAppProductService _appProductService;

        public BackOfficeProductController(IAppProductService appProductService)
        {
            _appProductService = appProductService;
        } 
        #endregion

        #region [- Guard -]
        private static JsonResult Guard(PutProductControllerDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Title.IsNullOrEmpty()) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.UnitPrice.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Code.IsNullOrEmpty()) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return model.ProductCategoryId.Equals(null) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }

        private static JsonResult Guard(PostProductControllerDto model)
        {
            if (model.Title.IsNullOrEmpty()) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.UnitPrice.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.Code.IsNullOrEmpty()) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return model.ProductCategoryId.Equals(null) ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }
        #endregion
        
        #region [- CRUD -]

        #region [- Put -]
        [HttpPut(Name = "PutProduct")]
        [Authorize(Roles = "Admin , GodAdmin")]
        public async Task<IActionResult> Put(PutProductControllerDto model)
        {
            Guard(model);
            var product = new PutProductAppDto()
            {
                Id = model.Id,
                Title = model.Title,
                UnitPrice = model.UnitPrice,
                Code = model.Code,
                EntityDescription = model.EntityDescription,
                IsActive = model.IsActive,
                ProductCategoryId = model.ProductCategoryId,
                UserName = GetCurrentUser().Value.ToString()
            };
            var putResult = await _appProductService.PutAsync(product);
            return new JsonResult(putResult);
        }
        #endregion

        #region [-Post-]
        [HttpPost(Name = "PostProduct")]
        [Authorize(Roles = "Admin , GodAdmin")]
        public async Task<IActionResult> Post(PostProductControllerDto model)
        {
            Guard(model);
            var postProduct = new PostProductAppDto()
            {
                Code = model.Code,
                Title = model.Title,
                UnitPrice = model.UnitPrice,
                EntityDescription = model.EntityDescription,
                ProductCategoryId = model.ProductCategoryId,
                UserName = GetCurrentUser().Value.ToString()
            };
            var postResult = await _appProductService.PostAsync(postProduct);
            return new JsonResult(postResult);
        }

        #endregion

        #region [-Delete-]
        [HttpDelete(Name = "DeleteProduct")]
        [Authorize(Roles = "Admin , GodAdmin")]
        public async Task<IActionResult> Delete(DeleteProductControllerDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var DeleteProduct = new DeleteProductAppDto()
            {
                Id = model.Id,
                UserName = GetCurrentUser().Value.ToString()
            };
            var postResult = await _appProductService.DeleteAsync(DeleteProduct);
            return new JsonResult(postResult);
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
        [HttpPost("GetProduct", Name = "GetProduct")]
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

        #endregion

        #region [- JsonResult GetCurrentUser() -]
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
