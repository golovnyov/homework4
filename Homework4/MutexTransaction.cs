using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Homework4
{
    public class MutexTransaction : ITransactionManager
    {
        private readonly Mutex m_mutex = new Mutex();
        
        public bool BeginTransaction()
        {
            return m_mutex.WaitOne();
        }

        public bool CommitTransaction(Action action)
        {
            if (action != null)
            {
                action.Invoke();
            }

            m_mutex.ReleaseMutex();

            return true;
        }

        public bool RollbackTransaction(Action action)
        {
            if (action != null)
            {
                action.Invoke();
            }

            m_mutex.ReleaseMutex();

            return true;
        }
    }
}
