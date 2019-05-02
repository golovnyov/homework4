using Homework4;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Tests
{
    [TestClass]
    public class TransactionTests
    {
        private readonly Storage storage;

        public TransactionTests()
        {
            storage = new Storage();
        }

        [TestMethod]
        public void Transaction_Consistency_Add_Success()
        {
            using (storage.BeginTransaction())
            {
                try
                {
                    storage.Add(1, "1");

                    storage.Add(2, "2");

                    storage.CommitTransaction();
                }
                catch (Exception)
                {
                    storage.RollbackTransaction();
                }
            }

            Assert.AreEqual(storage.Get(1), "1");
            Assert.AreEqual(storage.Get(2), "2");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Transaction_Consistency_Add_Failure()
        {
            using (storage.BeginTransaction())
            {
                try
                {
                    storage.Add(1, "1");

                    ThrowException();

                    storage.CommitTransaction();
                }
                catch (Exception)
                {
                    storage.RollbackTransaction();
                }
            }

            Assert.AreEqual(storage.Get(1), "1");
        }

        [TestMethod]
        public void Transaction_Concurency_Add_Success()
        {
            using (storage.BeginTransaction())
            {
                try
                {
                    storage.Add(1, "1");

                    // emulates an attempt to read dirty data by another user
                    Assert.AreEqual(storage.VolatileGet(1), null);

                    // reads data within transaction by the user who initiates the transaction
                    Assert.AreEqual(storage.Get(1), "1");

                    storage.CommitTransaction();
                }
                catch (Exception)
                {
                    storage.RollbackTransaction();
                }
            }
            //reads committed data
            Assert.AreEqual(storage.Get(1), "1");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Transaction_Concurency_RemoveJustAddedValue_Success()
        {
            using (storage.BeginTransaction())
            {
                try
                {
                    storage.Add(1, "1");

                    // emulates an attempt to modify data by another user
                    storage.VolatileRemove(1);

                    if (storage.Get(1) != null)
                    {
                        storage.Remove(1);
                    }

                    storage.CommitTransaction();
                }
                catch (Exception)
                {
                    storage.RollbackTransaction();
                }
            }

            //reads committed data
            Assert.AreEqual(storage.Get(1), "1");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Transaction_Timeout()
        {
            using (storage.BeginTransaction())
            {
                try
                {
                    storage.Add(1, "1");

                    Thread.Sleep(6000);

                    storage.CommitTransaction();
                }
                catch (Exception ex)
                {
                    Assert.AreEqual(ex.GetType(), typeof(Homework4.TimeoutException));

                    storage.RollbackTransaction();
                }
            }

            // must produce exception
            storage.Get(1);
        }

        private void ThrowException()
        {
            throw new Exception();
        }
    }
}
