using OnlineShopDomain.Aggregates.Sale;
using PublicTools.Resources;
using ResponseFramework;
using System.Net;
using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using PublicTools.Tools;
using OnlineShop.EFCore;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderHeaderAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos.OrderDetailAppDtos;
using OnlineShop.Application.Dtos.SaleAppDtos.OrderAppDtos;


namespace OnlineShop.Application.Services.SaleServices
{
    public class OrderHeaderService : IAppOrderHeaderService<DeleteOrderDetailAppDto>
    {
        private readonly IRepository<OrderHeader, Guid> _headerRepository;
        private readonly IRepository<OrderDetail, Guid> _detailRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly OnlineShopDbContext _context;


        #region [-Ctor-]
        public OrderHeaderService(IRepository<OrderHeader, Guid> headerRepository, IRepository<OrderDetail, Guid> detailRepository, IRepository<Product, Guid> productRepository, OnlineShopDbContext context)
        {
            _headerRepository = headerRepository;
            _detailRepository = detailRepository;
            _productRepository = productRepository;
            _context = context;
        }
        #endregion

        #region [-SaveChanges-]
        public async Task SaveChanges()
        {
            _context.SaveChanges();
        }
        #endregion

        #region [- Task<IResponse<object>> PutAsync(PutOrderHeaderAppDto model) -]
        public async Task<IResponse<object>> PutAsync(PutOrderAppDto model)
        {
            //#region [- Validation -]
            //if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            //if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            //if (model.Code.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            //if (model.OrderDate.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            //if (model.Seller.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            //if (model.Buyer.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            //var productResponse = await _productRepository.FindById(dto.ProductId);مهههههههههههههههههم باید باشه 
            //#endregion

            #region [-Task-]          
            var modelHeader = model.orderHeader;
            var orderHeader = await _headerRepository.FindById(modelHeader.Id);
            var putHeader = orderHeader.Result;
            var findDetails = await FindOrderDetailsByHeaderId(modelHeader.Id);
            var orderDetailList = findDetails.Result;
            var modelOrderDetail = model.orderDetails;
            var detailsToDelete = orderDetailList.Where(od => !modelOrderDetail.Any(dto => dto.Id == od.DetailId)).ToList();
            var detailsToAdd = modelOrderDetail.Where(dto => !orderDetailList.Any(od => od.DetailId == dto.Id)).ToList();
            var detailsToUpdate = orderDetailList.Where(od => modelOrderDetail.Any(dto => dto.Id == od.DetailId)).ToList();
            if (!orderHeader.IsSuccessful) return new Response<object>(MessageResource.Error_FailToFindObject);
            try
            {
                #region [-OrfderHeader-]
                putHeader.Id = modelHeader.Id;
                putHeader.EntityDescription = modelHeader.EntityDescription;
                putHeader.IsActive = modelHeader.IsActive;
                putHeader.DateModifiedPersian = Helpers.ConvertToPersianDate(DateTime.Now);
                putHeader.DateModifiedLatin = DateTime.Now;
                var putHeaderResult = await _headerRepository.UpdateAsync(putHeader);
                #endregion

                #region [Update_OrderDetails_Task]
                foreach (var dto in detailsToUpdate)
                {
                    var findDetail = await _detailRepository.FindById(dto.DetailId);
                    var putModel = modelOrderDetail.Where(p => p.Id == dto.DetailId).First();
                    var detail = findDetail.Result;
                    detail.Id = dto.DetailId;
                    detail.OrderHeaderId = orderHeader.Result.Id;
                    detail.Quantity = putModel.Quantity;
                    detail.Code = putModel.Code;
                    detail.Title = putModel.Title;
                    detail.ProductId = putModel.ProductId;
                    detail.UnitPrice = putModel.UnitPrice;
                    detail.EntityDescription = putModel.EntityDescription;
                    detail.IsModified = true;
                    detail.IsActive = putModel.IsActive;
                    detail.DateModifiedLatin = DateTime.Now;
                    detail.DateModifiedPersian = Helpers.ConvertToPersianDate(DateTime.Now);
                    var putUpdateResult = await _detailRepository.UpdateAsync(detail);
                    if (!putUpdateResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
                }
                #endregion

                #region [-Add_OrderDetail_Task-]
                foreach (var dto in detailsToAdd)
                {
                    var addDetail = new OrderDetail();
                    //detail.Id = new Guid();
                    addDetail.OrderHeaderId = orderHeader.Result.Id;
                    addDetail.Quantity = dto.Quantity;
                    addDetail.Code = dto.Code;
                    addDetail.Title = dto.Title;
                    addDetail.ProductId = dto.ProductId;
                    addDetail.UnitPrice = dto.UnitPrice;
                    addDetail.EntityDescription = dto.EntityDescription;
                    addDetail.IsDeleted = false;
                    addDetail.IsModified = false;
                    addDetail.IsActive = dto.IsActive;
                    addDetail.DateCreatedLatin = DateTime.Now;
                    addDetail.DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now);
                    var putAddResult = await _detailRepository.InsertAsync(addDetail);
                    if (!putAddResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
                }
                #endregion

                #region [-Delete_OrderDetail_Task-]
                foreach (var dto in detailsToDelete)
                {
                    var putDeleteResult = await _detailRepository.DeleteByIdAsync(dto.DetailId);
                    if (!putDeleteResult.IsSuccessful) new Response<object>(MessageResource.Error_FailProcess);
                }
                #endregion

                _context.SaveChanges();
            }
            catch (Exception)
            {
                return new Response<object>(MessageResource.Error_FailProcess);
            }


            //var putHeaderFinal = putHeaderResult.Result;
            //var putorderResult = new PutOrderAppDto
            //  {

            //     orderDetails= resultDetailList

            //}
            #endregion

            #region [-Result-]
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, null, HttpStatusCode.OK);
            #endregion        }
        }
        #endregion

        #region [- Task<IResponse<object>> DeleteOrderDetailAsync(List<DeleteOrderDetailsAppDto> model) -]
            public async Task<IResponse<object>> DeleteOrderDetailAsync(List<DeleteOrderDetailAppDto> model)
        {
            var details = model;
            var finalList = new List<DeleteOrderDetailAppDto>();
            if (details.Count == 0) return new Response<object>(MessageResource.Error_TheParameterIsNull);
            foreach (var detail in details)
            {
                if (detail.Id.Equals(null)) return new Response<object>(MessageResource.Error_TheParameterIsNull);
                var findDetail = await _detailRepository.FindById(detail.Id);
                if (!findDetail.IsSuccessful) return new Response<object>(MessageResource.Error_FailToFindObject);


                var orderDetailList = await FindOrderDetailsByHeaderId(findDetail.Result.OrderHeaderId);
                if (orderDetailList.Result.Count == 1 && orderDetailList.Result.First().DetailId == detail.Id)
                    return new Response<object>(MessageResource.Finalobject);

                var result = await _detailRepository.DeleteByIdAsync(detail.Id);
                if (!result.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
                finalList.Add(detail);
                await SaveChanges();

            }
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, finalList, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(Guid id)-]
        public async Task<IResponse<object>> DeleteAsync(Guid id)
        {
            if (id.Equals(null))
            {
                return new Response<object>(MessageResource.Error_TheParameterIsNull);
            }
            using (_context.Database.BeginTransaction())
            {
                try
                {
                    var orderHeader = await FindOrderHeaderById(id);
                    if (!orderHeader.IsSuccessful)
                    {
                        return new Response<object>(MessageResource.Error_FailProcess);
                    }
                    var detailsOrderHeader = await FindOrderDetailsByHeaderId(orderHeader.Result.HeaderId);
                    if (detailsOrderHeader != null)
                    {
                        foreach (var detail in detailsOrderHeader.Result)
                        {
                            if (detailsOrderHeader.Result.Count == 1 && detailsOrderHeader.Result.First().DetailId == detail.DetailId)
                                return new Response<object>(MessageResource.Finalobject);
                            var orderDetailDeleteResult = await _detailRepository.DeleteByIdAsync(detail.DetailId);
                            if (!orderDetailDeleteResult.IsSuccessful)
                            {
                                return new Response<object>(MessageResource.Error_FailProcess);
                            }
                        }
                        var orderHeaderDeleteResult = await _headerRepository.DeleteByIdAsync(orderHeader.Result.HeaderId);
                        if (!orderHeaderDeleteResult.IsSuccessful)
                        {
                            return new Response<object>(MessageResource.Error_FailProcess);
                        }
                    }
                }
                catch (Exception)
                {
                    return new Response<object>(MessageResource.Error_FailProcess);

                }
                return new Response<object>((true, MessageResource.Info_SuccessfullProcess, string.Empty, string.Empty, HttpStatusCode.OK));
            }
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(DeleteOrderDetailAppDtos model)-]
        public async Task<IResponse<object>> DeleteAsync(DeleteOrderDetailAppDtos model)
        {

            if (model.Id.Equals(null))
            {
                return new Response<object>(MessageResource.Error_TheParameterIsNull);
            }
            var orderHeader = await FindOrderHeaderById(model.Id);
            if (!orderHeader.IsSuccessful) return new Response<object>(MessageResource.Error_FailToFindObject);

            using (_context.Database.BeginTransaction())
            {
                try
                {
                    var detailsOrderHeader = await FindOrderDetailsByHeaderId(orderHeader.Result.HeaderId);
                    if (detailsOrderHeader != null)
                    {
                        foreach (var detail in detailsOrderHeader.Result)
                        {
                            var orderDetailDeleteResult = await _detailRepository.DeleteByIdAsync(detail.DetailId);
                            if (!orderDetailDeleteResult.IsSuccessful)
                            {
                                return new Response<object>(MessageResource.Error_FailProcess);
                            }
                        }
                        var orderHeaderDeleteResult = await _headerRepository.DeleteByIdAsync(orderHeader.Result.HeaderId);
                        if (!orderHeaderDeleteResult.IsSuccessful)
                        {
                            return new Response<object>(MessageResource.Error_FailProcess);
                        }
                    }
                    _context.SaveChanges();
                    _context.Database.CommitTransaction();
                }
                catch (Exception)
                {
                    _context.Database.RollbackTransaction();
                    return new Response<object>(MessageResource.Error_FailProcess);
                }
                return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, string.Empty, HttpStatusCode.OK);
            }
        }
        #endregion

        #region [-Task<IResponse<GetOrderHeaderAppDto>> FindById(Guid id)-]
        public async Task<IResponse<GetOrdersAppDto>> FindById(Guid id)
        {
            #region [-Validation-]
            if (id.Equals(null)) return new Response<GetOrdersAppDto>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var find = await _headerRepository.FindById(id);
            if (!find.IsSuccessful)
                return new Response<GetOrdersAppDto>(MessageResource.Error_FailToFindObject);
            var findResult = find.Result;
            var getOrderHeader = new GetOrderHeaderAppDto()
            {
                HeaderId = findResult.Id,
                Code = findResult.Code,
                SellerId = findResult.SellerId,
                BuyerId = findResult.BuyerId,
                Title = findResult.Title,
                DateCreatedLatin = findResult.DateCreatedLatin,
                DateCreatedPersian = findResult.DateCreatedPersian,
                DateModifiedLatin = findResult.DateModifiedLatin,
                DateModifiedPersian = findResult.DateModifiedPersian,
                DateSoftDeletedLatin = findResult.DateSoftDeletedLatin,
                DateSoftDeletedPersian = findResult.DateSoftDeletedPersian,
                EntityDescription = findResult.EntityDescription,
                IsActive = findResult.IsActive,
                IsDeleted = findResult.IsDeleted,
                IsModified = findResult.IsModified
            };

            var details = await _detailRepository.Select();
            if (!details.IsSuccessful)
                return new Response<GetOrdersAppDto>(MessageResource.Error_FailProcess);
            var detailResult = details.Result;
            var getOrderDetailList = detailResult.Where(item => item.OrderHeaderId == id).Select(item => new GetOrderDetailAppDto()
            {
                Code = item.Code,
                DateCreatedLatin = item.DateCreatedLatin,
                DateCreatedPersian = item.DateCreatedPersian,
                DateModifiedLatin = item.DateModifiedLatin,
                DateModifiedPersian = item.DateModifiedPersian,
                DateSoftDeletedLatin = item.DateSoftDeletedLatin,
                DateSoftDeletedPersian = item.DateSoftDeletedPersian,
                DetailId = item.Id,
                EntityDescription = item.EntityDescription,
                IsActive = item.IsActive,
                IsDeleted = item.IsDeleted,
                IsModified = item.IsModified,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Title = item.Title,
                UnitPrice = item.UnitPrice
            }).ToList();

            var finalOrder = new GetOrdersAppDto()
            {
                OrderDetails = getOrderDetailList,
                OrderHeader = getOrderHeader
            };
            #endregion

            #region [-Result-]
            return new Response<GetOrdersAppDto>(true, MessageResource.Info_SuccessfullProcess, string.Empty, finalOrder, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [- Task<IResponse<List<GetOrdersAppDto>>> GetAsync() -]
        public async Task<IResponse<List<GetOrdersAppDto>>> GetAsync()
        {
            var headerList = await _headerRepository.Select();
            var orderFinalList = new List<GetOrdersAppDto>();
            var detailsDto = new GetOrdersAppDto().OrderDetails;
            if (headerList != null)
                foreach (var header in headerList.Result)
                {

                    var detailList = await FindOrderDetailsByHeaderId(header.Id);
                    if (detailList != null)
                    {
                        var detailDtoList = detailList.Result.Select(detail => new GetOrderDetailAppDto
                        {
                            HeaderId = header.Id,
                            DetailId = detail.DetailId,
                            Code = detail.Code,
                            Title = detail.Title,
                            ProductId = detail.ProductId,
                            IsActive = detail.IsActive,
                            IsDeleted = detail.IsDeleted,
                            IsModified = detail.IsModified,
                            DateCreatedLatin = detail.DateCreatedLatin,
                            DateCreatedPersian = detail.DateCreatedPersian,
                            DateModifiedLatin = detail.DateModifiedLatin,
                            DateModifiedPersian = detail.DateModifiedPersian,
                            DateSoftDeletedLatin = detail.DateSoftDeletedLatin,
                            DateSoftDeletedPersian = detail.DateSoftDeletedPersian,
                            EntityDescription = detail.EntityDescription,
                            Price = detail.UnitPrice * detail.Quantity,
                            Quantity = detail.Quantity,
                            UnitPrice = detail.UnitPrice
                        }).ToList();
                        var totalPriceOrderHeader = detailDtoList.Sum(p => p.Price);
                        var headerDto = new GetOrderHeaderAppDto()
                        {
                            HeaderId = header.Id,
                            BuyerId = header.BuyerId,
                            SellerId = header.SellerId,
                            Code = header.Code,
                            Title = header.Title,
                            DateCreatedLatin = header.DateCreatedLatin,
                            DateCreatedPersian = header.DateCreatedPersian,
                            DateModifiedLatin = header.DateModifiedLatin,
                            DateModifiedPersian = header.DateModifiedPersian,
                            DateSoftDeletedPersian = header.DateSoftDeletedPersian,
                            DateSoftDeletedLatin = header.DateSoftDeletedLatin,
                            IsActive = header.IsActive,
                            IsDeleted = header.IsDeleted,
                            IsModified = header.IsModified,
                            EntityDescription = header.EntityDescription,
                            TotalPrice = totalPriceOrderHeader
                        };
                        var orderFinal = new GetOrdersAppDto()
                        {
                            OrderDetails = detailDtoList,
                            OrderHeader = headerDto
                        };
                        orderFinalList.Add(orderFinal);
                    }
                }

            return new Response<List<GetOrdersAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, orderFinalList, HttpStatusCode.OK);
        }
        #endregion

        #region [-Task<IResponse<GetOrderHeaderAppDto>> FindOrderHeaderById(Guid id)-]
        public async Task<IResponse<GetOrderHeaderAppDto>> FindOrderHeaderById(Guid id)
        {
            #region [-Validation-]
            if (id.Equals(null)) return new Response<GetOrderHeaderAppDto>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var find = await _headerRepository.FindById(id);
            if (!find.IsSuccessful)
                return new Response<GetOrderHeaderAppDto>(MessageResource.Error_FailToFindObject);
            var findResult = find.Result;
            var orderHeader = new GetOrderHeaderAppDto()
            {
                HeaderId = findResult.Id,
                Code = findResult.Code,
                SellerId = findResult.SellerId,
                BuyerId = findResult.BuyerId,
                Title = findResult.Title,
                DateCreatedLatin = findResult.DateCreatedLatin,
                DateCreatedPersian = findResult.DateCreatedPersian,
                DateModifiedLatin = findResult.DateModifiedLatin,
                DateModifiedPersian = findResult.DateModifiedPersian,
                DateSoftDeletedLatin = findResult.DateSoftDeletedLatin,
                DateSoftDeletedPersian = findResult.DateSoftDeletedPersian,
                EntityDescription = findResult.EntityDescription,
                IsActive = findResult.IsActive,
                IsDeleted = findResult.IsDeleted,
                IsModified = findResult.IsModified
            };
            #endregion

            #region [-Result-]
            return new Response<GetOrderHeaderAppDto>(true, MessageResource.Info_SuccessfullProcess, string.Empty, orderHeader, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<List<GetOrderDetailAppDto>>> FindOrderDetailsByHeaderId(Guid id)-]
        public async Task<IResponse<List<GetOrderDetailAppDto>>> FindOrderDetailsByHeaderId(Guid id)
        {
            #region [-Validation-]
            if (id.Equals(null)) return new Response<List<GetOrderDetailAppDto>>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion
            #region [-Task-]
            var orderDeatilFinal = new List<GetOrderDetailAppDto>();
            var findHeaderResult = await _headerRepository.FindById(id);
            var getOrderDetailList = new List<GetOrderDetailAppDto>();
            if (findHeaderResult.Result == null || findHeaderResult == null)
            {
                return new Response<List<GetOrderDetailAppDto>>(MessageResource.Error_FailProcess);
            }
            var details = await _detailRepository.Select();
            if (!details.IsSuccessful)
                return new Response<List<GetOrderDetailAppDto>>(MessageResource.Error_FailProcess);
            var detailResult = details.Result;
            getOrderDetailList = detailResult.Where(item => item.OrderHeaderId == id).Select(item => new GetOrderDetailAppDto()
            {
                Code = item.Code,
                DateCreatedLatin = item.DateCreatedLatin,
                DateCreatedPersian = item.DateCreatedPersian,
                DateModifiedLatin = item.DateModifiedLatin,
                DateModifiedPersian = item.DateModifiedPersian,
                DateSoftDeletedLatin = item.DateSoftDeletedLatin,
                DateSoftDeletedPersian = item.DateSoftDeletedPersian,
                DetailId = item.Id,
                EntityDescription = item.EntityDescription,
                IsActive = item.IsActive,
                IsDeleted = item.IsDeleted,
                IsModified = item.IsModified,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Title = item.Title,
                UnitPrice = item.UnitPrice
            }).ToList();
            #endregion
            #region [ - Result - ]
            return new Response<List<GetOrderDetailAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, getOrderDetailList, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #region [-Task<IResponse<object>> PostAsync(PostOrder model) -]
        public async Task<IResponse<object>> PostAsync(PostOrderAppDto model)
        {

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
                        //Id = new Guid(),
                        Code = orderHeaderDto.Code, //generate shavad ,
                        Title = orderHeaderDto.Title,
                        DateCreatedLatin = DateTime.Now,
                        DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now),
                        IsActive = true,
                        IsDeleted = false,
                        IsModified = false,
                        SellerId = orderHeaderDto.SellerId,//in bayad ba login generatte shavad 
                        BuyerId = model.UserName,//orderHeaderDto.BuyerId,
                        EntityDescription = orderHeaderDto.EntityDescription
                    };
                    var postResult = await _headerRepository.InsertAsync(header);
                    //_context.SaveChanges();
                    //if (!postResult.IsSuccessful)
                    //{
                    //    _context.Database.RollbackTransaction();
                    //    // return error Response 
                    //}

                    List<PostOrderDetailAppDto> detailDtos = model.OrderDetails;
                    foreach (var dto in detailDtos)
                    {
                        var productResponse = await _productRepository.FindById(dto.ProductId);
                        var product = productResponse.Result;

                        var detail = new OrderDetail();
                        detail.Id = new Guid();
                        detail.OrderHeaderId = postResult.Result.Id;
                        detail.Quantity = dto.Quantity;
                        detail.Code = dto.Code;
                        detail.Title = dto.Title;
                        detail.ProductId = product.Id;
                        detail.UnitPrice = dto.UnitPrice;
                        detail.EntityDescription = dto.EntityDescription;
                        detail.IsDeleted = false;
                        detail.IsModified = false;
                        detail.IsActive = true;
                        detail.DateCreatedLatin = DateTime.Now;
                        detail.DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now);
                        var orderDetailResult = await _detailRepository.InsertAsync(detail);
                        //_context.SaveChanges();
                        //context.Database.CommitTransaction();
                    }
                    _context.SaveChanges();
                    _context.Database.CommitTransaction();
                }
                catch (Exception ex)
                {
                    _context.Database.RollbackTransaction();
                    return new Response<object>(MessageResource.Error_FailProcess);

                }
            }
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, header, HttpStatusCode.OK);
        }
        #endregion

    }
}
