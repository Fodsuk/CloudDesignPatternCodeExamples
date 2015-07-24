using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace CacheAsidePattern
{
    public interface IDataStore
    {
        T GetById<T>(int id) where T : class;
        void Add<T>(int id, T item) where T : class;
    }

    public class DataStore : IDataStore
    {
        private Dictionary<int, object> items = new Dictionary<int, object>();

        public T GetById<T>(int id) where T : class
        {
            var item = items[id];

            if (item == null) return null;

            return item as T;
        }

        public void Add<T>(int id, T item) where T : class
        {
            items[id] = item;
        }
    }
}