namespace AspNet_RssReader_Domain.Abstract
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        void Save();
    }
}
