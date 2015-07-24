namespace CacheAsidePattern
{
    public interface IRepository
    {
        T GetById<T>(int id) where T : class;
    }

    public class Repository : IRepository
    {
        private readonly IDataStore _dataStore;
        private readonly ICache _cache;

        public Repository(IDataStore dataStore, ICache cache)
        {
            _dataStore = dataStore;
            _cache = cache;
        }

        public T GetById<T>(int id) where T : class
        {
            T item;

            if (!_cache.TryGet(id, out item))
            {
                item = _dataStore.GetById<T>(id);

                _cache.Put(id, item);
            }
         
            return item;
        }
    }
}