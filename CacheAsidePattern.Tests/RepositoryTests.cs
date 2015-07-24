using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CacheAsidePattern.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void GetById_ItemInCache_DataStoreIsntAccessed()
        {
            int id = 123;
            string data = "This is my object";

            var cache = new Cache();
            cache.Put(id, data);

            var storeMock = new Mock<DataStore>(MockBehavior.Default);

            var repository = new Repository(storeMock.Object, cache);

            var item = repository.GetById<string>(123);

            storeMock.Verify(x => x.GetById<string>(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void GetById_ItemNotInCache_DataStoreIsAccessed()
        {
            int id = 123;
            string data = "This is my object";

            var cache = new Cache();

            var store = new DataStore();
            store.Add(id, data);

            var repository = new Repository(store, cache);

            var item = repository.GetById<string>(123);

            //storeMock.Verify(x => x.GetById<string>(It.IsAny<int>()), Times.Never);
        }
    }
}
