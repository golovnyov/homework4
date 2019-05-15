using System;
using System.Collections.Generic;

namespace Homework4
{
    public class Storage
    {
        private Dictionary<int, string> m_mainDictionary;

        private Dictionary<int, string> m_bufferDictionary;

        public Storage()
        {
            m_mainDictionary = new Dictionary<int, string>();
        }

        #region Transaction

        public void BeginTransaction()
        {
            m_bufferDictionary = new Dictionary<int, string>(m_mainDictionary);

        }

        public void CommitTransaction()
        {
            if (m_bufferDictionary != null)
            {
                m_mainDictionary = new Dictionary<int, string>(m_bufferDictionary);

                m_bufferDictionary = null;
            }
        }

        public void RollbackTransaction()
        {
            m_bufferDictionary = null;
        }

        #endregion

        public void Add(int key, string value)
        {
            if (m_bufferDictionary != null)
            {
                m_bufferDictionary.Add(key, value);
            }
            else
            {
                m_mainDictionary.Add(key, value);
            }
        }

        public object Get(int key)
        {
            if (m_bufferDictionary != null)
            {
                return m_bufferDictionary[key];
            }

            return m_mainDictionary[key];
        }

        public void Remove(int key)
        {
            if (m_bufferDictionary != null)
            {
                m_bufferDictionary.Remove(key);
            }
            else
            {
                m_mainDictionary.Remove(key);
            }
        }

        public object VolatileGet(int key)
        {
            return m_mainDictionary.ContainsKey(key) ? m_mainDictionary[key] : null;
        }

        public void VolatileRemove(int key)
        {
            if (m_mainDictionary.ContainsKey(key) && m_bufferDictionary == null)
            {
                m_mainDictionary.Remove(key);
            }
        }
    }
}
