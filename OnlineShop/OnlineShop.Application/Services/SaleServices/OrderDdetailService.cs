using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderDetailAppDtos;
using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using OnlineShop.RepositoryDesignPatern.Services.Sale;
using OnlineShopDomain.Aggregates.Sale;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;

namespace OnlineShop.Application.Services.SaleServices
{
    public class OrderDdetailService : IAppOrderDetailService
    {
        private readonly IRepository<OrderDetail , Guid> _repository;

        #region [-Ctor-]
        public OrderDdetailService(IRepository<OrderDetail, Guid> repository)
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
            var deleteOrderDetail = _repository.FindById(id);
            if (deleteOrderDetail == null)
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

        #region [-Task<IResponse<object>> DeleteAsync(DeleteOrderDetailAppDto model)-]

        public async Task<IResponse<object>> DeleteAsync(DeleteOrderDetailAppDto model)
        {
            if (model == null) return new Response<object>(MessageResource.Error_ModelNull);
            var deleteOrderDetail = new OrderDetail
            {
                Id = model.Id
            };
            if (deleteOrderDetail == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var resultDelete = await _repository.DeleteAsync(deleteOrderDetail);
            if (!resultDelete.IsSuccessful)
                return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, deleteOrderDetail, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<GetOrderDetailAppDto>> FindById(Guid id)-]
        public async Task<IResponse<GetOrderDetailAppDto>> FindById(Guid id)
        {
            #region [-Validation-]
            if (id.Equals(null)) return new Response<GetOrderDetailAppDto>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var findResult = await _repository.FindById(id);
            var findOrderDetail = new GetOrderDetailAppDto()
            {
                ProductId = findResult.Result.ProductId,
                OrderHeaderid = findResult.Result.OrderHeaderid,
                UnitPrice = findResult.Result.UnitPrice,
                Quantity = findResult.Result.Quantity,
            };
            #endregion

            #region [-Result-]
            if (!findResult.IsSuccessful) return new Response<GetOrderDetailAppDto>(MessageResource.Error_FailProcess);
            return new Response<GetOrderDetailAppDto>(true, MessageResource.Info_SuccessfullProcess, string.Empty, findOrderDetail, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<List<GetOrderDetailAppDto>>> GetAsync()-]
        public async Task<IResponse<List<GetOrderDetailAppDto>>> GetAsync()
        {
            var getResult = await _repository.Select();
            if (!getResult.IsSuccessful) return new Response<List<GetOrderDetailAppDto>>(MessageResource.Error_FailProcess);
            var getOrderDetailList = new List<GetOrderDetailAppDto>();
            var getOrderDetails = getResult.Result.Select(item => new GetOrderDetailAppDto()
            {
                ProductId = item.ProductId,
                OrderHeaderid = item.OrderHeaderid,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,

            }).ToList();
            return new Response<List<GetOrderDetailAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, getOrderDetails, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> PostAsync(PostOrderDetailAppDto model)-]
        public async Task<IResponse<object>> PostAsync(PostOrderDetailAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.ProductId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.OrderHeaderid.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.UnitPrice.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Quantity.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var postOrderDetail = new OrderDetail
            {
                ProductId = model.ProductId,
                OrderHeaderid = model.OrderHeaderid,
                UnitPrice = model.UnitPrice,
                Quantity = model.Quantity,

            };
            if (postOrderDetail == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var postResult = await _repository.InsertAsync(postOrderDetail);
            #endregion

            #region [-Result-] 
            if (!postResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, postResult, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<object>> PutAsync(PutOrderDetailAppDto model)-]
        public async Task<IResponse<object>> PutAsync(PutOrderDetailAppDto model)
        {
            #region [- Validation -]
            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.ProductId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.OrderHeaderid.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.UnitPrice.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Quantity.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var putOrderDetail = new OrderDetail
            {
                ProductId = model.ProductId,
                OrderHeaderid = model.OrderHeaderid,
                UnitPrice = model.UnitPrice,
                Quantity = model.Quantity,
            };
            if (putOrderDetail == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putResult = await _repository.UpdateAsync(putOrderDetail);
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
