using Microsoft.AspNetCore.Identity;
using OnlineShop.Application.Contracts.SaleContracts;
using OnlineShop.Application.Dtos.SaleAppDtos.ProductAppDtos;
using OnlineShop.Application.Services.UserManagmentServices;
using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using OnlineShopDomain.Aggregates.Sale;
using OnlineShopDomain.Aggregates.UserManagement;

using PublicTools.Resources;
using PublicTools.Tools;
using ResponseFramework;
using System.Net;

namespace OnlineShop.Application.Services.SaleServices
{
    public class ProductService : IAppProductService
    {
        #region [- Ctor & Fields -]
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<OrderDetail, Guid> _detailRepository;
        private readonly UserManager<AppUser> _userManager;


        public ProductService(IRepository<Product, Guid> repository, UserManager<AppUser> userManager, IRepository<OrderDetail, Guid> detailRepository  /*, OrderService orderService */)
        {
            _productRepository = repository;
            _userManager = userManager;
            _detailRepository = detailRepository;  

        }
        #endregion

        #region [-     Ok     -]

        #region [- DeleteAsync(string id) -]
        public async Task<IResponse<object>> DeleteAsync(Guid id)
        {
            if (id.Equals(null))
            {
                return new Response<object>(MessageResource.Error_TheParameterIsNull);
            }
            var findProduct = await _productRepository.FindById(id);
            var deleteProduct = findProduct.Result;
            if (deleteProduct == null)
            {
                return new Response<object>(MessageResource.Error_FailToFindObject);
            }
            var product = new Product();
            product.Id = deleteProduct.Id;
            product.IsDeleted = true;
            product.DateSoftDeletedPersian = Helpers.ConvertToPersianDate(DateTime.Now);
            product.DateSoftDeletedLatin = DateTime.Now;

            var resultDelete = await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChanges();
            if (resultDelete.IsSuccessful)
                return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, deleteProduct, HttpStatusCode.OK);
            return new Response<object>(MessageResource.Error_FailProcess);
        }
        #endregion  
        
        #region [- DeleteAsync(DeleteProductAppDto model) -]
        public async Task<IResponse<object>> DeleteAsync(DeleteProductAppDto model)
        {
            var userLogin = await _userManager.FindByNameAsync(model.UserName);
            if (userLogin == null) return new Response<object>(MessageResource.Error_UserNotFound);
            if (!(await _userManager.IsInRoleAsync(userLogin, "GodAdmin") || await _userManager.IsInRoleAsync(userLogin, "Admin")))
                return new Response<object>(MessageResource.Error_Accessdenied);

            if (model == null) return new Response<object>(MessageResource.Error_ModelNull);
            if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ModelNull);

            var findProduct = await _productRepository.FindById(model.Id);
            if (!findProduct.IsSuccessful) return new Response<object>(MessageResource.Error_FailToFindObject);
            var details = await _detailRepository.Select();
            if( details.Result.Any(x=>x.ProductId==findProduct.Result.Id))
            {
                return new Response<object>(MessageResource.Error_DataWasUsed); 
            }
            var deleteProduct = findProduct.Result;

            deleteProduct.Id = model.Id;
            deleteProduct.IsDeleted = true;
            deleteProduct.DateSoftDeletedPersian = Helpers.ConvertToPersianDate(DateTime.Now);
            deleteProduct.DateSoftDeletedLatin = DateTime.Now;

            var resultDelete = await _productRepository.UpdateAsync(deleteProduct);
            await _productRepository.SaveChanges();
            if (!resultDelete.IsSuccessful)
                return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, deleteProduct, HttpStatusCode.OK);
        }
        #endregion
        
        #region [-Task<IResponse<List<GetProductAppDto>>> GetAsync()-]
        public async Task<IResponse<List<GetProductAppDto>>> GetAsync()
        {
            var getResult = await _productRepository.Select();
            if (!getResult.IsSuccessful) return new Response<List<GetProductAppDto>>(MessageResource.Error_FailProcess);
            var getproductList = new List<GetProductAppDto>();
            var getProducts = getResult.Result.Select(item => new GetProductAppDto()
            {
                Id = item.Id,
                Title = item.Title,
                ProductCategoryId = item.ProductCategoryId,
                Code = item.Code,
                UnitPrice = item.UnitPrice,
                IsActive = item.IsActive,
                DateCreatedLatin = item.DateCreatedLatin,
                DateCreatedPersian = item.DateCreatedPersian,
                EntityDescription = item.EntityDescription,
                IsModified = item.IsModified,
                DateModifiedLatin = item.DateModifiedLatin,
                DateModifiedPersian = item.DateModifiedPersian,
                IsDeleted = item.IsDeleted,
                DateSoftDeletedLatin = item.DateSoftDeletedLatin,
                DateSoftDeletedPersian = item.DateSoftDeletedPersian
            }).ToList();

            return new Response<List<GetProductAppDto>>(true, MessageResource.Info_SuccessfullProcess, string.Empty, getProducts, HttpStatusCode.OK);
        }
        #endregion
        
        #region [-PutAsync(PutProductAppDto model)-]
        public async Task<IResponse<object>> PutAsync(PutProductAppDto model)
        {
            #region [- Validation -]
            var userLogin = await _userManager.FindByNameAsync(model.UserName);
            if (userLogin == null) return new Response<object>(MessageResource.Error_UserNotFound);
            if (!(await _userManager.IsInRoleAsync(userLogin, "GodAdmin") || await _userManager.IsInRoleAsync(userLogin, "Admin")))
                return new Response<object>(MessageResource.Error_Accessdenied);

            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.Id.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.ProductCategoryId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Title.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Code.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.UnitPrice.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.IsActive.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var product = await _productRepository.FindById(model.Id);
            if (product == null) return new Response<object>(MessageResource.Error_FailToFindObject);

            var putProduct = product.Result;

            putProduct.UnitPrice = model.UnitPrice;
            putProduct.ProductCategoryId = model.ProductCategoryId;
            putProduct.Code = model.Code;
            putProduct.Id = model.Id;
            putProduct.Title = model.Title;
            putProduct.IsActive = model.IsActive;
            putProduct.IsDeleted = false;
            putProduct.IsModified = true;
            putProduct.DateModifiedLatin = DateTime.Now;
            putProduct.DateModifiedPersian = Helpers.ConvertToPersianDate(DateTime.Now);
            putProduct.EntityDescription = model.EntityDescription;

            var putResult = await _productRepository.UpdateAsync(putProduct);
            await SaveChanges();
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
            await _productRepository.SaveChanges();
        }
        #endregion

        #region [-PostAsync(PutProductAppDto model)-]
        public async Task<IResponse<object>> PostAsync(PostProductAppDto model)
        {
            #region [- Validation -]
            var userLogin = await _userManager.FindByNameAsync(model.UserName);
            if (userLogin == null) return new Response<object>(MessageResource.Error_UserNotFound);
            if (!(await _userManager.IsInRoleAsync(userLogin, "GodAdmin") || await _userManager.IsInRoleAsync(userLogin, "Admin")))
                return new Response<object>(MessageResource.Error_Accessdenied);

            if (model == null) return new Response<object>(MessageResource.Error_FailToFindObject);
            if (model.ProductCategoryId.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Title.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.Code.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            if (model.UnitPrice.Equals(null)) return new Response<object>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]
            var postProduct = new Product()
            {
               // Id = new Guid(),
                Title = model.Title,
                Code = model.Code,
                UnitPrice = model.UnitPrice,
                ProductCategoryId = model.ProductCategoryId,
                EntityDescription = model.EntityDescription,
                IsActive = true,
                DateCreatedLatin = DateTime.UtcNow,
                DateCreatedPersian = Helpers.ConvertToPersianDate(DateTime.Now),
                IsModified = false,
                IsDeleted = false,
            };
            var postResult = await _productRepository.InsertAsync(postProduct);
            await SaveChanges();
            #endregion

            #region [-Result-]
            if (!postResult.IsSuccessful) return new Response<object>(MessageResource.Error_FailProcess);
            return new Response<object>(true, MessageResource.Info_SuccessfullProcess, string.Empty, postResult, HttpStatusCode.OK);
            #endregion

        }
        #endregion
        
        #region [-FindById(string id)-]
        public async Task<IResponse<GetProductAppDto>> FindById(Guid id)
        {
            #region [-Validation-]
            if (id.Equals(null)) return new Response<GetProductAppDto>(MessageResource.Error_ThisFieldIsMandatory);
            #endregion

            #region [-Task-]

            var findResult = await _productRepository.FindById(id);
            if (!findResult.IsSuccessful) return new Response<GetProductAppDto>(MessageResource.Error_FailToFindObject);
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
            if (findProduct==null ) return new Response<GetProductAppDto>(MessageResource.Error_FailProcess);
            return new Response<GetProductAppDto>(true, MessageResource.Info_SuccessfullProcess, string.Empty, findProduct, HttpStatusCode.OK);
            #endregion
        }
        #endregion

        #endregion
        
    }
}
