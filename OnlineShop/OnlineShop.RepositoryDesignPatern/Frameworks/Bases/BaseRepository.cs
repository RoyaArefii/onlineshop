using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts;
using OnlineShopDomain.Aggregates.UserManagement;
using ResponseFramework;
using PublicTools.Resources;

namespace onlineshop.repositorydesignpatern.frameworks.bases
{
    public class BaseRepository<TDbContext, TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
                                                                    where TEntity : class
                                                                    where TDbContext :  IdentityDbContext<AppUser, AppRole, string,
                                                                                        IdentityUserClaim<string>,
                                                                                        AppUserRole,
                                                                                        IdentityUserLogin<string>,
                                                                                        IdentityRoleClaim<string>,
                                                                                        IdentityUserToken<string>>
    {
        protected readonly TDbContext _dbContext;
        protected readonly DbSet<TEntity> dbSet;

        #region [-Ctor-]
        public BaseRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
            dbSet = dbContext.Set<TEntity>();
        }
        #endregion

        #region [-InsertAsync(TEntity entity)-]
        public async Task<IResponse<object>> InsertAsync(TEntity entity)
        {
            await using (_dbContext)
            {
                await dbSet.AddAsync(entity);
                await SaveChanges();
                return new Response<object>(entity);

            }
        }
        #endregion

        #region [-UpdateAsync(TEntity entity)-]
        public async Task<IResponse<object>> UpdateAsync(TEntity entity)
        {
            dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await SaveChanges();
            return new Response<object>(entity);
        }
        #endregion
       
        #region [-DeleteAsync(TPrimaryKey id)-]
        public async Task<IResponse<object>> DeleteAsync(TPrimaryKey id)
        {
            var deleteEntity = await dbSet.FindAsync(id);
            if (deleteEntity != null) return new Response<object>(MessageResource.Error_FailToFindObject);
            dbSet.Remove(deleteEntity);
            await SaveChanges();
            return new Response<object>(deleteEntity);//چرا اینجا entity برگردوندیم؟ به نظرم باید message  برگردونیم
        }
        #endregion

        #region [-Task<IResponse<object>> DeleteAsync(TEntity entity)-]
        public async Task<IResponse<object>> DeleteAsync(TEntity entity)
        {
            if (_dbContext.Entry(entity).State==EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
            await SaveChanges();
            return new Response<object>(entity);    
        }
        #endregion

        #region [-Task<IResponse<List<TEntity>>> Select()-]
        public async Task<IResponse<List<TEntity>>> Select()
        {
           var q = await dbSet.AsNoTracking().ToListAsync();
            //var response = new ResponseFramework.Response<List<TEntity>>(new List<TEntity>()) { Result = q };
            //return response;
            return new Response<List<TEntity>>(q);  

        }
        #endregion

        #region [-Task<IResponse<TEntity>> FindById(TPrimaryKey id)-]
        public async Task<IResponse<TEntity>> FindById(TPrimaryKey id)
        {
            var q = await dbSet.FindAsync(id);
            return q == null ? new Response<TEntity>(MessageResource.Error_FailToFindObject) : new Response<TEntity>(q); 
        }
        #endregion

        #region [-Task SaveChanges()-]
        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
