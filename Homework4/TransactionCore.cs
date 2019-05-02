using System;

namespace Homework4
{
    internal sealed class TransactionCore : IDisposable
    {
        private readonly System.Timers.Timer m_timer;

        private bool m_isDisposed;

        public TransactionCore(int timeout = 2000)
        {
            m_timer = new System.Timers.Timer(timeout);

            m_timer.Start();

            m_timer.Elapsed += (x, e) =>
            {
                throw new Homework4.TimeoutException();
            };

            m_timer.Enabled = true;
            m_timer.AutoReset = false;
        }

        public void Dispose()
        {
            if (!m_isDisposed)
            {
                m_timer.Close();

                m_isDisposed = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}
