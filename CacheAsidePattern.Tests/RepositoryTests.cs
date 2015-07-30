using System;
using System.Diagnostics;
using System.Threading;
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
            //Arrange
            var cache = new Cache();
            cache.Put(123, "This is my data is from the cache");

            var storeMock = new Mock<DataStore>() { CallBase = true};
            storeMock.Object.Add(123, "This is my data is from the data store");

            var repository = new Repository(storeMock.Object, cache);

            //Act
            var item = repository.GetById<string>(123);

            //Assert
            Assert.AreEqual("This is my data is from the cache", item);
            storeMock.Verify(x => x.GetById<string>(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void GetById_ItemNotInCache_DataStoreIsAccessed()
        {
            //Arrange
            var cache = new Cache();
            
            var storeMock = new Mock<DataStore>(){CallBase = true};
            storeMock.Object.Add(123, "This is my data is from the data store");

            var repository = new Repository(storeMock.Object, cache);

            //Act
            var item = repository.GetById<string>(123);

            //Assert
            Assert.AreEqual("This is my data is from the data store", item);
            storeMock.Verify(x => x.GetById<string>(It.IsAny<int>()), Times.Once);
        }


        [TestMethod]
        public void GetById_ItemExpiredFromCache_DataStoreIsAccessed()
        {
            //Arrange
            var cache = new Cache();
            cache.Put(123, "This is my data is from the cache");
            cache.ItemExperationInSeconds = 10;

            var store = new DataStore();
            store.Add(123, "This is my data is from the data store");

            var repository = new Repository(store, cache);

            //Act
            Thread.Sleep(10); //wait 10 seconds for cache to expire
            var item = repository.GetById<string>(123);

            //Assert
            Assert.AreEqual("This is my data is from the cache", item);
        }
    }
}
