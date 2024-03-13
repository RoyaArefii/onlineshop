using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.ProductAppDtos;
using OnlineShop.RepositoryDesignPatern.Services.Sale;
using OnlineShopDomain.Aggregates.Sale;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;

namespace OnlineShop.Application.Services.SaleServices
{
    public class ProductService : IAppProductService
    {
        private readonly ProductRepository _repository;

        #region [-Ctor-]
        public ProductService(ProductRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region [- DeleteAsync(string id) -]
        public async Task<IResponse<object>> DeleteAsync(string id)
        {
            var deleteProduct = await _repository.FindById(id);
            if (deleteProduct == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var resultDelete = await _repository.DeleteAsync(id);
            if (resultDelete.IsSuccessful)
                return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, deleteProduct, HttpStatusCode.OK);
            return new Response<object>(MessageResource.Error_FailProcess);
        }
        #endregion

        #region [- DeleteAsync(DeleteProductAppDto model) -]
        public async Task<IResponse<object>> DeleteAsync(DeleteProductAppDto model)
        {
            var deleteProduct = new Product
            {
                Id = model.Id,
                Title = model.Title,
                ProductCategoryId = model.ProductCategoryId,
                Code = model.Code,
                UnitPrice = model.UnitPrice,
            };
            if (deleteProduct == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var resultDelete = await _repository.DeleteAsync(deleteProduct);
            if (!resultDelete.IsSuccessful)
                return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, deleteProduct, HttpStatusCode.OK);
        }

        #endregion

        #region [-Task<IResponse<List<GetProductAppDto>>> GetAsync()-????????????????????????????]
        public async Task<IResponse<List<GetProductAppDto>>> GetAsync()
        {
            var getResult = await _repository.Select();
            if (!getResult.IsSuccessful) return new Response<List<GetProductAppDto>>(MessageResource.Error_FailProcess);
            var getproductList = new List<GetProductAppDto>();
            //foreach (var product in getResult.Result)
            //{
            //    var getProducts = new GetProductAppDto
            //    {
            //        Id = product.Id,
            //        Title = product.Title,
            //        ProductCategoryId = product.ProductCategoryId,
            //        Code = product.Code,
            //        UnitPrice = product.UnitPrice,
            //    };
            //    getproductList.Add(getProducts);
            //}
            var getProducts = getResult.Result.Select(item => new GetProductAppDto()
            {
                Id = item.Id,
                Title = item.Title,
                ProductCategoryId = item.ProductCategoryId,
                Code = item.Code,
                UnitPrice = item.UnitPrice,
            }).ToList();

            return new Response<List<GetProductAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, getProducts, HttpStatusCode.OK);
        }
        #endregion

        #region [-PutAsync(PutProductAppDto model)-]
        public async Task<IResponse<object>> PutAsync(PutProductAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Code.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.ProductCategoryId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.UnitPrice.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Title.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var putProduct = new Product
            {
                UnitPrice = model.UnitPrice,
                ProductCategoryId = model.ProductCategoryId,
                Code = model.Code,
                Id = model.Id,
                Title = model.Title,
            };
            if (putProduct == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putResult = await _repository.UpdateAsync(putProduct);
            #endregion

            #region [-Result-] 
            if (!putResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, putResult, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-PostAsync(PutProductAppDto model)-]
        public async Task<IResponse<object>> PostAsync(PostProductAppDto model)
        {
            #region [-Validation-]
            if (model.ProductCategoryId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Title.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.UnitPrice.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Code.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var putProduct = new Product()
            {
                Id = new Guid(),
                Title = model.Title,
                Code = model.Code,
                UnitPrice = model.UnitPrice,
            };
            var putResult = await _repository.InsertAsync(putProduct);
            #endregion

            #region [-Result-]
            if (!putResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, putProduct, HttpStatusCode.OK);
            #endregion

        }
        #endregion

        #region [-FindById(string id)-]
        public async Task<IResponse<GetProductAppDto>> FindById(string id)
        {
            #region [-Validation-]
            if (string.IsNullOrWhiteSpace(id)) return new Response<GetProductAppDto>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]

            var findResult = await _repository.FindById(id);
            var findProduct = new GetProductAppDto()
            {
                Id = findResult.Result.Id,
                Code = findResult.Result.Title,
                Title = findResult.Result.Code,
                UnitPrice = findResult.Result.UnitPrice,
                ProductCategoryId = findResult.Result.ProductCategoryId,
            };
            #endregion

            #region [-Result-]
            if (!findResult.IsSuccessful) return new Response<GetProductAppDto>(MessageResource.Error_FailProcess);
            return new Response<GetProductAppDto>(true, MessageResource.Info_SuccessfullProcess, string.Empty, findProduct, HttpStatusCode.OK);
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
