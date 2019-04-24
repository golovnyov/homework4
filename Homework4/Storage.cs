using System.Collections.Generic;
using System.Threading;

namespace Homework4
{
    public class Storage
    {
        private ITransactionManager m_transactionManager;

        private Dictionary<int, object> m_mainDictionary;

        private Dictionary<int, object> m_bufferDictionary;

        public Storage()
        {
            m_transactionManager = new MutexTransaction();

            m_mainDictionary = new Dictionary<int, object>();
            m_bufferDictionary = new Dictionary<int, object>(2);
        }

        public void Add(int key, object value)
        {
            try
            {
                m_transactionManager.BeginTransaction();

                m_bufferDictionary = new Dictionary<int, object>(m_mainDictionary);

                m_bufferDictionary.Add(key, value);

                m_transactionManager.CommitTransaction(Commit);
            }
            catch (System.Exception)
            {
                m_transactionManager.RollbackTransaction(Rollback);

                throw;
            }
        }

        public object Get(int key)
        {
            return m_mainDictionary[key];
        }

        public void Remove(int key)
        {
            try
            {
                m_transactionManager.BeginTransaction();

                m_bufferDictionary = new Dictionary<int, object>(m_mainDictionary);

                m_bufferDictionary.Remove(key);

                m_transactionManager.CommitTransaction(Commit);
            }
            catch (System.Exception)
            {
                m_transactionManager.RollbackTransaction(Rollback);

                throw;
            }
        }

        private void Commit()
        {
            m_mainDictionary = m_bufferDictionary;
        }

        private void Rollback()
        {
            m_bufferDictionary = null;
        }
    }
}
