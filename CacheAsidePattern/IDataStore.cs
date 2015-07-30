namespace CacheAsidePattern
{
    public interface IDataStore
    {
        T GetById<T>(int id) where T : class;
        void Add<T>(int id, T item) where T : class;
    }
}