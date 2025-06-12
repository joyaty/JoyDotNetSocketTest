
using System.Net.Sockets;

namespace Joy.Test.Server
{
    /// <summary>
    /// 字节缓冲区管理器，非线程安全
    /// </summary>
    class BufferManager
    {
        /// <summary>
        /// 缓冲区的总大小 (每块缓冲区大小 * 缓冲区总数)
        /// </summary>
        private readonly int m_NumBytes;                 // the total number of bytes controlled by the buffer pool

        /// <summary>
        /// 每块缓冲区的大小
        /// </summary>
        private readonly int m_BufferSize;

        /// <summary>
        /// 字节缓冲区
        /// </summary>
        private byte[] m_Buffer;                // the underlying byte array maintained by the Buffer Manager

        /// <summary>
        /// 空闲的缓冲区起始偏移索引
        /// </summary>
        private readonly Stack<int> m_FreeIndexPool; 

        /// <summary>
        /// 当前可用的缓冲区
        /// </summary>
        private int m_CurrentIndex;

        public BufferManager(int totalBytes, int bufferSize)
        {
            m_NumBytes = totalBytes;
            m_CurrentIndex = 0;
            m_BufferSize = bufferSize;
            m_FreeIndexPool = new Stack<int>();
        }

        /// <summary>
        /// 初始化申请足够大的字节缓冲区
        /// </summary>
        public void InitBuffer()
        {
            // create one big large buffer and divide that out to each SocketAsyncEventArg object
            m_Buffer = new byte[m_NumBytes];
        }

        /// <summary>
        /// 绑定缓冲区到SocketAsyncEventArgs对象上
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (m_FreeIndexPool.Count > 0)
            {
                args.SetBuffer(m_Buffer, m_FreeIndexPool.Pop(), m_BufferSize);
            }
            else
            {
                if ((m_NumBytes - m_BufferSize) < m_CurrentIndex)
                { // 所有的缓冲区都被使用
                    return false;
                }
                args.SetBuffer(m_Buffer, m_CurrentIndex, m_BufferSize);
                m_CurrentIndex += m_BufferSize;
            }
            return true;
        }

        /// <summary>
        /// 解绑SocketAsyncEventArgs
        /// </summary>
        /// <param name="args"></param>
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            m_FreeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}