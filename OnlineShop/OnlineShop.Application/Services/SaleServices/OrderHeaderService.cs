using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderHeaderAppDtos;
using OnlineShop.RepositoryDesignPatern.Services.Sale;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;
using OnlineShopDomain.Aggregates.Sale;
using OnlineShop.Application.Dtos.SaleAppDtos.ProductAppDtos;
using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;


namespace OnlineShop.Application.Services.SaleServices
{
    public class OrderHeaderService : IAppOrderHeaderService
    {
        private readonly IRepository<OrderHeader, Guid> _repository;

        #region [-Ctor-]
        public OrderHeaderService(IRepository<OrderHeader, Guid> repository)
        {
            _repository = repository;
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(Guid id)-]

        public async Task<IResponse<object>> DeleteAsync(Guid id)
        {
            if (id.Equals(null))
            {
                return new Response<object>(MessageResource.Error_TheParameterIsNull);
            }
            var deleteOrderHeader = _repository.FindById(id);
            if (deleteOrderHeader == null)
            {
                return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            }
            var resultDelete = await _repository.DeleteAsync(id);
            if (!resultDelete.IsSuccessful)
            {
                return new Response<object>(MessageResource.Error_FailProcess);
            }
            return new Response<object>((true, MessageResource.Info_SuccessfullProcess, string.Empty, resultDelete, HttpStatusCode.OK));
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(DeleteOrderHeaderAppDtos model)-]
        public async Task<IResponse<object>> DeleteAsync(DeleteOrderHeaderAppDtos model)
        {
            if (model == null) return new Response<object>(MessageResource.Error_ModelNull);
            var deleteOrderHeader = new OrderHeader
            {
                Id = model.Id,

            };
            if (deleteOrderHeader == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var resultDelete = await _repository.DeleteAsync(deleteOrderHeader);
            if (!resultDelete.IsSuccessful)
                return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, deleteOrderHeader, HttpStatusCode.OK);
        }

        public Task<IResponse<object>> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region [-Task<IResponse<GetOrderHeaderAppDto>> FindById(Guid id)-]
        public async Task<IResponse<GetOrderHeaderAppDto>> FindById(Guid id)
        {
            #region [-Validation-]
            if (id.Equals(null)) return new Response<GetOrderHeaderAppDto>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var findResult = await _repository.FindById(id);
            var findOrderHeader = new GetOrderHeaderAppDto()
            {
                Id = findResult.Result.Id,
                Code = findResult.Result.Code,
                OrderDate = findResult.Result.OrderDate,
                Seller = findResult.Result.Seller,
                Buyer = findResult.Result.Buyer,
            };
            #endregion

            #region [-Result-]
            if (!findResult.IsSuccessful) return new Response<GetOrderHeaderAppDto>(MessageResource.Error_FailProcess);
            return new Response<GetOrderHeaderAppDto>(true, MessageResource.Info_SuccessfullProcess, string.Empty, findOrderHeader, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [- Task<IResponse<List<GetOrderHeaderAppDto>>> GetAsync() -]
        public async Task<IResponse<List<GetOrderHeaderAppDto>>> GetAsync()
        {
            var getResult = await _repository.Select();
            if (!getResult.IsSuccessful) return new Response<List<GetOrderHeaderAppDto>>(MessageResource.Error_FailProcess);
            var getOrderHeaderList = new List<GetOrderHeaderAppDto>();
            var getrderHeaders = getResult.Result.Select(item => new GetOrderHeaderAppDto()
            {
                Id = item.Id,
                Code = item.Code,
                OrderDate = item.OrderDate,
                Seller = item.Seller,
                Buyer = item.Buyer,
            }).ToList();

            return new Response<List<GetOrderHeaderAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, getrderHeaders, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> PostAsync(PostOrderHeaderAppDto model) -]
        public async Task<IResponse<object>> PostAsync(PostOrderHeaderAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Code.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.OrderDate.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Seller.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Buyer.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var postOrderHeader = new OrderHeader
            {
                Id = model.Id,
                Code = model.Code,
                OrderDate = model.OrderDate,
                Seller = model.Seller,
                Buyer = model.Buyer,
            };
            if (postOrderHeader == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var postResult = await _repository.InsertAsync(postOrderHeader);
            #endregion

            #region [-Result-] 
            if (!postResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, postResult, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [- Task<IResponse<object>> PutAsync(PutOrderHeaderAppDto model) -]
        public async Task<IResponse<object>> PutAsync(PutOrderHeaderAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Code.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.OrderDate.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Seller.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Buyer.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var putOrderHeader = new OrderHeader
            {
                Id = model.Id,
                Code = model.Code,
                OrderDate = model.OrderDate,
                Seller = model.Seller,
                Buyer = model.Buyer,
            };
            if (putOrderHeader == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putResult = await _repository.UpdateAsync(putOrderHeader);
            #endregion

            #region [-Result-] 
            if (!putResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, putResult, HttpStatusCode.OK);
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
