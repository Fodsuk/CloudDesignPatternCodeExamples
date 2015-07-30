namespace CacheAsidePattern
{
    public interface ICache
    {
        int ItemExperationInSeconds { get; set; }

        bool TryGet<T>(int key, out T item) where T : class;
        void Put(int key, object item);
    }
}