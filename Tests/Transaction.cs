using Homework4;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class Transaction
    {
        private Storage storage;

        public Transaction()
        {
            storage = new Storage();
        }

        [TestMethod]
        public void Transaction_Add_Thread1_Success()
        {
            var thread = new Thread(new ThreadStart(() => { storage.Add(1, "1"); }));
            thread.Start();

            thread.Join();

            Assert.AreEqual(storage.Get(1), "1");
        }

        [TestMethod]
        public void Transaction_Add_Thread2_Success()
        {

        }

    }
}
