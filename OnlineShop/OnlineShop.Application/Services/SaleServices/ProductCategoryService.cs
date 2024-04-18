
using OnlineShop.Application.Dtos.SaleAppDtos.ProductCategory;
using OnlineShop.RepositoryDesignPatern.Services.Sale;
using OnlineShopDomain.Aggregates.Sale;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;

namespace OnlineShop.Application.Services.SaleServices
{
    public class ProductCategoryService
    {
        private readonly ProductCategoryRepository _repository;

        #region [-Ctor-]
        public ProductCategoryService(ProductCategoryRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region [- DeleteAsync(string id) -]
        public async Task<IResponse<object>> DeleteAsync(string id)
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
                Title = model.Title,
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
                Title = item.Title,
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
            #endregion

            #region [-Task-]
            var putProductCategory = new ProductCategory
            {
                Id = model.Id,
                Title = model.Title,
            };
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
            if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var postProductCategory = new ProductCategory()
            {
                Id = new Guid(),
                Title = model.Title,
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
        public async Task<IResponse<GetProductCategoryAppDto>> FindById(string id)
        {
            #region [-Validation-]
            if (string.IsNullOrWhiteSpace(id)) return new Response<GetProductCategoryAppDto>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]

            var findResult = await _repository.FindById(id);
            var findProductCategory = new GetProductCategoryAppDto()
            {
                Id = findResult.Result.Id,
                Title = findResult.Result.Title,

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
