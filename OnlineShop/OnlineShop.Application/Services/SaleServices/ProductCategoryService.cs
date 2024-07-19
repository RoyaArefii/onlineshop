using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.ProductCategory;
using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using OnlineShopDomain.Aggregates.Sale;
using OnlineShopDomain.Aggregates.UserManagement;
using PublicTools.Resources;
using PublicTools.Tools;
using ResponseFramework;
using System.Data;
using System.Net;

namespace OnlineShop.Application.Services.SaleServices
{
    public class ProductCategoryService : IAppProductCategoryService
    {

        #region [-Ctor & Fields-]
        private readonly IRepository<ProductCategory, Guid> _repository;
        private readonly ProductService _productService;
        private readonly UserManager<AppUser> _userService;
        public ProductCategoryService(IRepository<ProductCategory, Guid> repository, ProductService productService, UserManager<AppUser> userService)
        {
            _repository = repository;
            _productService = productService;
            _userService = userService;

        }
        #endregion

        #region [- DeleteAsync(string id) -]
        public async Task<IResponse<object>> DeleteAsync(Guid id)
        {
            var deleteProductCategory = await _repository.FindById(id);
            if (deleteProductCategory == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var productList = await _productService.GetAsync();
            if (productList.IsSuccessful && productList.Result.Any(x => x.ProductCategoryId == id && x.IsDeleted != false))
            {
                return new Response<object>(MessageResource.Error_DataWasUsed);

            }
            var resultDelete = await _repository.DeleteByIdAsync(id);
            await _repository.SaveChanges();
            if (resultDelete.IsSuccessful)
                return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, deleteProductCategory, HttpStatusCode.OK);
            return new Response<object>(MessageResource.Error_FailProcess);
        }
        #endregion

        #region [- DeleteAsync(DeleteProductCategoryAppDto model) -]
        public async Task<IResponse<object>> DeleteAsync(DeleteProductCategoryAppDto model)
        {
            #region [-Validation-]
            if (model == null || model.Id == null) return new Response<object>(MessageResource.Error_ModelNull);
            var userLogin = await _userService.FindByNameAsync(model.UserName);
            if (userLogin == null) return new Response<object>(MessageResource.Error_UserNotFound);
            if (!(await _userService.IsInRoleAsync(userLogin, "GodAdmin") || await _userService.IsInRoleAsync(userLogin, "Admin")))
                return new Response<object>(MessageResource.Error_Accessdenied);
            #endregion

            #region [- Task -]
            var productcategory = await _repository.FindById(model.Id);
            if (productcategory == null) return new Response<object>(MessageResource.Error_ModelNull);
            var productList = await _productService.GetAsync();
            if (productList.IsSuccessful && (productList.Result.Any(x => x.ProductCategoryId == productcategory.Result.Id && x.IsDeleted != true)))
            {
                return new Response<object>(MessageResource.Error_DataWasUsed);

            }
            var productcategoryResult = productcategory.Result;
            var resultDelete = await _repository.DeleteAsync(productcategoryResult);
            await _repository.SaveChanges();
            #endregion

            #region [- Result -]
            if (!resultDelete.IsSuccessful)
                return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, productcategoryResult, HttpStatusCode.OK);
            #endregion
        }

        #endregion

        #region [-Task<IResponse<List<GetProductCategoryAppDto>>> GetAsync()-]
        public async Task<IResponse<List<GetProductCategoryAppDto>>> GetAsync()
        {
            var getResult = await _repository.Select();
            if (!getResult.IsSuccessful) return new Response<List<GetProductCategoryAppDto>>(MessageResource.Error_FailProcess);
            var getProductCategoryList = new List<GetProductCategoryAppDto>();

            var getProductCategorys = getResult.Result.Where(p => p.IsActive == true).Select(item => new GetProductCategoryAppDto()
            {
                Id = item.Id,
                ParentId = item.ParentId,
                IsActive = item.IsActive,
                Title = item.Title,
                EntityDescription = item.EntityDescription
            }).ToList();

            return new Response<List<GetProductCategoryAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, getProductCategorys, HttpStatusCode.OK);
        }
        #endregion

        #region [-SaveChanges()-]
        public async Task SaveChanges()
        {
            await _repository.SaveChanges();
        }
        #endregion

        #region [-PutAsync(PutProductCategoryAppDto model)-]
        public async Task<IResponse<object>> PutAsync(PutProductCategoryAppDto model)
        {
            #region [- Validation -]
            var userLogin = await _userService.FindByNameAsync(model.UserName);
            if (Helpers.IsDeleted(userLogin) || userLogin == null) return new Response<object>(true, string.Empty, MessageResource.Error_UserNotFound, null, HttpStatusCode.OK);

            if (!(await _userService.IsInRoleAsync(userLogin, "GodAdmin") || await _userService.IsInRoleAsync(userLogin, "Admin")))
                return new Response<object>(MessageResource.Error_Accessdenied);

            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Title.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            var productCategory = await _repository.FindById(model.Id);
            if (productCategory.IsSuccessful == null || productCategory.Result.IsActive == false) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (!model.ParentId.Equals(null))
            {
                Guid? nullParent = model.ParentId;
                Guid parent = nullParent ?? Guid.Empty;
                if (parent != null)
                {
                    var finalParent = await _repository.FindById(parent);
                    if (finalParent.Result == null) return new Response<object>(MessageResource.Error_InValidProductCateegoryParentId);
                    else if (finalParent.Result != null && finalParent.Result.IsActive == false || finalParent.Result.Id == model.Id) return new Response<object>(MessageResource.Error_InValidProductCateegoryParentId);
                }
            }
                #endregion

                #region [-Task-]

                //var putProductCategory = new ProductCategory
                //{
                //    Id = productCategory.Result.Id,
                //    ParentId = productCategory.Result.ParentId,
                //    IsActive = productCategory.Result.IsActive,
                //    Title = productCategory.Result.Title,
                //    EntityDescription = productCategory.Result.EntityDescription,
                //};

                //به خاطر خطای زیر کدهای بالا کامنت شد -
                //پاسخ chatgpt:
                //System.InvalidOperationException: The instance of entity type 'ProductCategory' cannot be tracked because another instance with the same key value for { 'Id'} is already being tracked.When attaching existing entities, ensure that only one entity instance with a given key value is attached.Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.
                //at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.IdentityMap`1.ThrowIdentityConflict(InternalEntityEntry entry)
                //at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.IdentityMap`1.Add(TKey key, InternalEntityEntry entry, Boolean updateDuplicate)
                //at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.IdentityMap`1.Add(TKey key, InternalEntityEntry entry)
                //at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.IdentityMap`1.Add(InternalEntityEntry entry)
                //at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.

                var putProductCategory = productCategory.Result;
            putProductCategory.ParentId = model.ParentId;
            putProductCategory.IsActive = model.IsActive;
            putProductCategory.Title = model.Title;
            putProductCategory.Id = model.Id;
            putProductCategory.EntityDescription = model.EntityDescription;
            if (putProductCategory == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putResult = await _repository.UpdateAsync(putProductCategory);
            await SaveChanges();
            #endregion

            #region [-Result-] 
            if (!putResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, putResult, HttpStatusCode.OK);
            #endregion
        }
        #endregion       

        #region [-PostAsync(PutProductCategoryAppDto model)-]
        public async Task<IResponse<object>> PostAsync(PostProductCategoryAppDto model)
        {            
            #region [-Validation-]
            var userLogin = await _userService.FindByNameAsync(model.UserName);
            if (Helpers.IsDeleted(userLogin) || userLogin == null) return new Response<object>(MessageResource.Error_UserNotFound);
            if (!(await _userService.IsInRoleAsync(userLogin, "GodAdmin") || await _userService.IsInRoleAsync(userLogin, "Admin")))
                return new Response<object>(MessageResource.Error_Accessdenied);

            if (model.Title.IsNullOrEmpty()) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (!model.ParentId.Equals(null))
            {
                Guid? nullParent = model.ParentId;
                Guid parent = nullParent ?? Guid.Empty;
                if (parent != null)
                {
                    var finalParent = await _repository.FindById(parent);
                    if (finalParent.Result == null ) return new Response<object>(MessageResource.Error_InValidProductCateegoryParentId);
                    else if (finalParent.Result!=null && finalParent.Result.IsActive == false) return new Response<object>(MessageResource.Error_InValidProductCateegoryParentId);
                }
            }
            #endregion
          
            #region [-Task-]
            var postProductCategory = new ProductCategory()
            {
                ParentId = model.ParentId,
                IsActive = true,
                Title = model.Title,
                EntityDescription = model.EntityDescription
            };
            var postResult = await _repository.InsertAsync(postProductCategory);
            await SaveChanges();
            #endregion
           
            #region [-Result-]
            if (!postResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, postProductCategory, HttpStatusCode.OK);
            #endregion

        }
        #endregion

        #region [-FindById(string id)-]
        public async Task<IResponse<GetProductCategoryAppDto>> FindById(Guid id)
        {
            #region [-Validation-]
            if (id.Equals(null)) return new Response<GetProductCategoryAppDto>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]

            var findResult = await _repository.FindById(id);
            if (findResult.Result.IsActive == false || findResult == null) return new Response<GetProductCategoryAppDto>(true, string.Empty, MessageResource.Error_RoleNotFound, null, HttpStatusCode.OK);

            var findProductCategory = new GetProductCategoryAppDto()
            {
                Id = findResult.Result.Id,
                ParentId = findResult.Result.ParentId,
                IsActive = findResult.Result.IsActive,
                Title = findResult.Result.Title,
                EntityDescription = findResult.Result.EntityDescription
            };
            #endregion

            #region [-Result-]
            if (!findResult.IsSuccessful) return new Response<GetProductCategoryAppDto>(MessageResource.Error_FailProcess);
            return new Response<GetProductCategoryAppDto>(true, MessageResource.Info_SuccessfullProcess, string.Empty, findProductCategory, HttpStatusCode.OK);
            #endregion
        }
        #endregion


    }
}
