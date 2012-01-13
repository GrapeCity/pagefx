using System;
using System.Threading;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    delegate void Operation();

    class Service : IDisposable
    {
        readonly ManualResetEvent m_opSet;
        readonly ManualResetEvent m_opComplete;
        readonly Operation m_quitOperation;
        readonly Thread m_thread;

        Operation m_op;
        bool m_fSyncOp;
        Exception m_opException;

        public Service()
        {
            m_opSet = new ManualResetEvent(false);
            m_opComplete = new ManualResetEvent(true);
            m_quitOperation = delegate { };

            m_thread = new Thread(ThreadFunc);
            m_thread.Start();
        }

        public void RunOperation(Operation op)
        {
            if (op == null)
                throw new ArgumentNullException();

            SetOperationInternal(op, true);
        }

        public void RunOperationAsync(Operation op)
        {
            if (op == null)
                throw new ArgumentNullException();

            SetOperationInternal(op, false);
        }

        public void Close()
        {
            RunOperationAsync(m_quitOperation);
        }

        internal void SetOperationInternal(Operation op, bool fSyncOp)
        {
            while (true)
            {
                m_opComplete.WaitOne();

                if (TrySetOperationInternal(op, fSyncOp))
                {
                    return;
                }
            }
        }

        bool TrySetOperationInternal(Operation op, bool fSyncOp)
        {
            lock (this)
            {
                if (m_op == null)
                {
                    m_op = op;
                    m_fSyncOp = fSyncOp;
                    m_opException = null;
                    m_opComplete.Reset();
                    m_opSet.Set();

                    if (fSyncOp)
                    {
                        m_opComplete.WaitOne();
                        if (m_opException != null)
                        {
                            throw m_opException;
                        }
                    }

                    return true;
                }
                if (m_op == m_quitOperation)
                {
                    if (op == m_quitOperation)
                    {
                        return true; // we are already closed
                    }
                    // Can't try to run something after calling Close
                    throw new InvalidOperationException();
                }
            }

            return false;
        }

        // Thread routine for the poll loop. It handles calls coming in from the debug engine as well as polling for debug events.
        private void ThreadFunc()
        {
            bool fQuit = false;

            while (!fQuit)
            {
                try
                {
                    // If the other thread is dispatching a command, execute it now.
                    bool fReceivedCommand = m_opSet.WaitOne(new TimeSpan(0, 0, 0, 0, 100), false);

                    if (fReceivedCommand)
                    {
                        if (m_fSyncOp)
                        {
                            try
                            {
                                m_op();
                            }
                            catch (ThreadAbortException)
                            {
                                break;
                            }
                            catch (Exception opException)
                            {
                                m_opException = opException;
                            }
                        }
                        else
                        {
                            m_op();
                        }

                        fQuit = (m_op == m_quitOperation);
                        if (!fQuit)
                        {
                            m_op = null;
                        }

                        m_opComplete.Set();
                        m_opSet.Reset();
                    }
                }
                catch (ThreadAbortException)
                {
                    break;
                }
            }
        }

        public void Abort()
        {
            m_thread.Abort();
        }

        void IDisposable.Dispose()
        {
            Close();
        }
    }
}