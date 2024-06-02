using OnlineShopDomain.Aggregates.Sale;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;
using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using PublicTools.Tools;
using OnlineShop.EFCore;
using OnlineShop.Application.Dtos.SaleAppDtos;

using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos;


namespace OnlineShop.Application.Services.SaleServices
{
    public class OrderHeaderService : IAppOrderHeaderService
    {
        private readonly IRepository<OrderHeader, Guid> _headerRepository;
        private readonly IRepository<OrderDetail, Guid> _detailRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly OnlineShopDbContext _context;

        #region [-Ctor-]
        public OrderHeaderService(IRepository<OrderHeader, Guid> headerRepository, OnlineShopDbContext context, IRepository<OrderDetail, Guid> detailRepository, IRepository<Product, Guid> productRepository)
        {
            _headerRepository = headerRepository;
            _detailRepository = detailRepository;
            _context = context;
            _productRepository = productRepository;
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(Guid id)-]

        public async Task<IResponse<object>> DeleteAsync(Guid id)
        {
            if (id.Equals(null))
            {
                return new Response<object>(MessageResource.Error_TheParameterIsNull);
            }
            var deleteOrderHeader = _headerRepository.FindById(id);
            if (deleteOrderHeader == null)
            {
                return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            }
            var resultDelete = await _headerRepository.DeleteAsync(id);
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
            var resultDelete = await _headerRepository.DeleteAsync(deleteOrderHeader);
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
            var findResult = await _headerRepository.FindById(id);
            var findOrderHeader = new GetOrderHeaderAppDto()
            {
                Id = findResult.Result.Id,
                Code = findResult.Result.Code,
                //OrderDate = findResult.Result.OrderDate,
                SellerId = findResult.Result.SellerId,
                BuyerId = findResult.Result.BuyerId,
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
            var getResult = await _headerRepository.Select();
            if (!getResult.IsSuccessful) return new Response<List<GetOrderHeaderAppDto>>(MessageResource.Error_FailProcess);
            var getOrderHeaderList = new List<GetOrderHeaderAppDto>();
            var getrderHeaders = getResult.Result.Select(item => new GetOrderHeaderAppDto()
            {
                Id = item.Id,
                Code = item.Code,
                //OrderDate = item.OrderDate,
                SellerId = item.SellerId,
                BuyerId = item.BuyerId,
            }).ToList();

            return new Response<List<GetOrderHeaderAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, getrderHeaders, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> PostAsync(PostOrderHeaderAppDto model) -]
        public async Task<IResponse<object>> PostAsync(PostOrder model)
        {
            // header validation
            //if (model.PostOrders.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            //var product = await _productRepository.FindById(model.PostOrders.ProductId)
            //if (product == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            //if (model.Code.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            //if (model.SellerId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            //if (model.BuyerId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            //if (model.Quantity.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);

            // foreach for Details validation
            //if (product == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            //if (detail.Quantity.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            //if (detail.Code.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);


            OrderHeader header = default;
            using (_context.Database.BeginTransaction())
            {
                try
                {
                    var orderHeaderDto = model.OrderHeader;
                    header = new OrderHeader()
                    {
                        Id = new Guid(),
                        Code = orderHeaderDto.Code, //generate shavad ,
                        DateCreatedLatin = DateTime.Now,
                        DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now),
                        IsActive = true,
                        IsDeleted = false,
                        IsModified = false,
                        SellerId = orderHeaderDto.SellerId,//in bayad ba login generatte shavad 
                        BuyerId = orderHeaderDto.BuyerId
                    };
                    var postResult = await _headerRepository.InsertAsync(header);
                    _context.SaveChanges();
                    if (!postResult.IsSuccessful)
                    {
                        _context.Database.RollbackTransaction();
                        // return error Response 
                    }

                    List<PostOrderDetailAppDto> detailDtos = model.OrderDetails;
                    foreach (var dto in detailDtos)
                    {
                        var productResponse = await _productRepository.FindById(dto.ProductId);
                        var product = productResponse.Result;

                        var detail = new OrderDetail();
                        detail.Id = new Guid();
                        detail.OrderHeaderId = postResult.Result.Id;
                        detail.Quantity = dto.Quantity;
                        detail.ProductId = product.Id;
                        detail.UnitPrice = product.UnitPrice;
                        detail.EntityDescription = dto.EntityDescription;
                        detail.IsDeleted = false;
                        detail.IsModified = false;
                        detail.IsActive = true;
                        detail.DateCreatedLatin = DateTime.Now;
                        detail.DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now);
                        var orderDetailResult = await _detailRepository.InsertAsync(detail);
                        _context.SaveChanges();
                        _context.Database.CommitTransaction();
                    }
                }
                catch (Exception)
                {
                    _context.Database.RollbackTransaction();
                    return new Response<object>(MessageResource.Error_FailProcess);

                }
                

                    //if (orderDetailResult.IsSuccessful)
                    //{
                    //    return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, postResult, HttpStatusCode.OK);
                        
                    //}
                    //else
                    //{
                    //    _context.Database.RollbackTransaction();
                    //    return new Response<object>(MessageResource.Error_FailProcess);
                    //    // return error Response 
                    //}
            }

            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, header, HttpStatusCode.OK);
        
        }


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
                //OrderDate = model.OrderDate,
                SellerId = model.Seller,
                BuyerId = model.Buyer,
            };
            if (putOrderHeader == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            var putResult = await _headerRepository.UpdateAsync(putOrderHeader);
            #endregion

            #region [-Result-] 
            if (!putResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, putResult, HttpStatusCode.OK);
            #endregion
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
#endregion