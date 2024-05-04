
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.ProductCategory;
using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using OnlineShop.RepositoryDesignPatern.Services.Sale;
using OnlineShopDomain.Aggregates.Sale;
using PublicTools.Resources;
using ResponseFramework;
using System.Diagnostics;
using System.Net;
using System.Security.Policy;

namespace OnlineShop.Application.Services.SaleServices
{
    public class ProductCategoryService :IAppProductCategoryService
    {
        private readonly IRepository<ProductCategory, Guid> _repository;

        #region [-Ctor-]
        public ProductCategoryService(IRepository<ProductCategory, Guid> repository)
        {
            _repository = repository;
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
            var resultDelete = await _repository.DeleteAsync(id);
            if (resultDelete.IsSuccessful)
                return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, deleteProductCategory, HttpStatusCode.OK);
            return new Response<object>(MessageResource.Error_FailProcess);
        }
        #endregion

        #region [- DeleteAsync(DeleteProductCategoryAppDto model) -]
        public async Task<IResponse<object>> DeleteAsync(DeleteProductCategoryAppDto model)
        {
            var deleteProductCategory = new ProductCategory
            {
                Id = model.Id,
                ParentId = model.ParentId,
                IsActive = model.IsActive,
                Title = model.Title ,
                EntityDescription = model.EntityDescription
            };
            if (deleteProductCategory == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var resultDelete = await _repository.DeleteAsync(deleteProductCategory);
            if (!resultDelete.IsSuccessful)
                return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, deleteProductCategory, HttpStatusCode.OK);
        }

        #endregion

        #region [-Task<IResponse<List<GetProductCategoryAppDto>>> GetAsync()-]
        public async Task<IResponse<List<GetProductCategoryAppDto>>> GetAsync()
        {
            var getResult = await _repository.Select();
            if (!getResult.IsSuccessful) return new Response<List<GetProductCategoryAppDto>>(MessageResource.Error_FailProcess);
            var getProductCategoryList = new List<GetProductCategoryAppDto>();

            var getProductCategorys = getResult.Result.Select(item => new GetProductCategoryAppDto()
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

        #region [-PutAsync(PutProductCategoryAppDto model)-]
        public async Task<IResponse<object>> PutAsync(PutProductCategoryAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Title.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            var productCategory = await _repository.FindById(model.Id);
            if (productCategory == null) return new Response<object>(MessageResource.Error_FailToFindObject);
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

            var putProductCategory=  productCategory.Result;
            putProductCategory.ParentId = model.ParentId;
            putProductCategory.IsActive = model.IsActive;
            putProductCategory.Title = model.Title;
            putProductCategory.Id = model.Id;
            putProductCategory.EntityDescription = model.EntityDescription;
            if (putProductCategory == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putResult = await _repository.UpdateAsync(putProductCategory);
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
          
            if (model.Title.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.IsActive.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var postProductCategory = new ProductCategory()
            {
                Id = new Guid(),
                ParentId = model.ParentId,
                IsActive = model.IsActive,
                Title = model.Title,
                EntityDescription = model.EntityDescription,
            };
            var postResult = await _repository.InsertAsync(postProductCategory);
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
            var findProductCategory = new GetProductCategoryAppDto()
            {
                Id = findResult.Result.Id,
                ParentId = findResult.Result.ParentId,
                IsActive = findResult.Result.IsActive,
                Title = findResult.Result.Title,
                EntityDescription=findResult.Result.EntityDescription
            };
            #endregion

            #region [-Result-]
            if (!findResult.IsSuccessful) return new Response<GetProductCategoryAppDto>(MessageResource.Error_FailProcess);
            return new Response<GetProductCategoryAppDto>(true, MessageResource.Info_SuccessfullProcess, string.Empty, findProductCategory, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-SaveChanges()-]
        public async Task SaveChanges()
        {
            await _repository.SaveChanges();
        }
        #endregion
    }
}
