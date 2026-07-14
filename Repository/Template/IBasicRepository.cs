namespace PicPay.Repository.Template
{
    public interface IBasicRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        Task<bool> DeleteByIdAsync(Guid id);
        Task SaveChangesAsync();
    }
}
