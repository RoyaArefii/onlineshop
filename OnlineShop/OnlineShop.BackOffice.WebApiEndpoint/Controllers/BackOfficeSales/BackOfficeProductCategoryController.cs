using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.ProductCategory;
using OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.SaleDtos.ProductCategory;
using PublicTools.Resources;
using ResponseFramework;
using System.Security.Claims;

namespace OnlineShop.BackOffice.WebApiEndpoint.Controllers.BackOfficeSales
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeProductCategoryController : ControllerBase
    {
        #region [- Ctor & Feilds -]
        private readonly IAppProductCategoryService _appProductCategoryService;

        public BackOfficeProductCategoryController(IAppProductCategoryService appProductCategoryService)
        {
            _appProductCategoryService = appProductCategoryService;
        } 
        #endregion

        #region [- Guard -]
        private static JsonResult Guard(PutProductCategoryControllerDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            if (model.IsActive.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            return model.Title.IsNullOrEmpty() ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }

        private static JsonResult Guard(PostProductCategoryControllerDto model)
        {
            return model.Title.IsNullOrEmpty() ? new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory)) : new JsonResult(null);
        }

        #endregion

        #region [-Put-]
        [HttpPut(Name = "PutProductCategory")]
        [Authorize(Roles = "Admin , GodAdmin")]
        public async Task<IActionResult> Put(PutProductCategoryControllerDto model)
        {
            Guard(model);
            var putProductCategoryAppDto = new PutProductCategoryAppDto()
            {
                Id = model.Id,
                Title = model.Title,
                EntityDescription = model.EntityDescription,
                IsActive = model.IsActive,
                ParentId = model.ParentId,
                UserName = GetCurrentUser().Value.ToString()
            };
            var putResult = await _appProductCategoryService.PutAsync(putProductCategoryAppDto);
            return new JsonResult(putResult);
        }
        #endregion
       
        #region [-Post-]
        [HttpPost(Name = "PostProductCategory")]
        [Authorize(Roles = "Admin , GodAdmin")]
        public async Task<IActionResult> Post(PostProductCategoryControllerDto model)
        {
            Guard(model);
            var postProductCategory = new PostProductCategoryAppDto()
            {
                Title = model.Title,
                EntityDescription = model.EntityDescription,
                ParentId = model.ParentId,
                UserName = GetCurrentUser().Value.ToString()
            };
            var postResult = await _appProductCategoryService.PostAsync(postProductCategory);
            return new JsonResult(postResult);
        } 
        #endregion

        #region [-Delete-]
        [HttpDelete(Name = "DeleteProductCategory")]
        [Authorize(Roles = "Admin , GodAdmin")]
        public async Task<IActionResult> Delete(DeleteProductCategoryControllerDto model)
        {
            if (model.Id.Equals(null)) return new JsonResult(new Response<object>(MessageResource.Error_ThisFieldIsMandatory));
            var productCategory = new DeleteProductCategoryAppDto()
            {
                Id = model.Id,
                UserName = GetCurrentUser().Value.ToString()
            };
            var postResult = await _appProductCategoryService.DeleteAsync(productCategory);
            return new JsonResult(postResult);
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
