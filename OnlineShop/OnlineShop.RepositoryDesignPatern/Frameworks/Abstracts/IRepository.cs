




using ResponseFramework;

namespace OnlineShop.RepositoryDesignPatern.Frameworks.Abstracts
{
    public interface IRepository<TEntity, in TPrimaryKey> where TEntity : class
    {
        Task<IResponse<object>> InsertAsync(TEntity entity);  
        Task<IResponse<object>> UpdateAsync(TEntity entity);
        Task<IResponse<object>> DeleteAsync(TPrimaryKey id);
        Task<IResponse<object>> DeleteAsync(TEntity entity);
        Task<IResponse<List<TEntity>>> Select();
        Task<IResponse<TEntity>> FindById(TPrimaryKey id);
        Task SaveChanges();

    }

}
