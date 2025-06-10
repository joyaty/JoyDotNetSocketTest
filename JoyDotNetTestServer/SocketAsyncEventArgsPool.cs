
using System.Net.Sockets;

namespace Joy.Test.Server
{
    /// <summary>
    /// 简单实现的SocketAsyncEventArgs对象池类，用于优化Socket异步操作性能
    /// </summary>
    class SocketAsyncEventArgsPool
    {
        /// <summary>
        /// 空闲可复用的SocketAsyncEventArgs对象栈
        /// </summary>
        private readonly Stack<SocketAsyncEventArgs> m_Pool;

        /// <summary>
        /// 统计对象池中有多少个可用的
        /// </summary>
        public int Count => m_Pool.Count;

        /// <summary>
        /// 初始化对象池
        /// </summary>
        /// <param name="capacity"></param>
        public SocketAsyncEventArgsPool(int capacity)
        {
            m_Pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        /// <summary>
        /// 归还一个空闲对象到对象池中
        /// </summary>
        /// <param name="item"></param>
        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                Console.WriteLine("返回一个空对象到SocketAsyncEventArgsPool对象池！！");
                return;
            }
            lock (m_Pool)
            {
                m_Pool.Push(item);
            }
        }

        /// <summary>
        /// 从对象池中获取一个空闲的SocketAsyncEventArgs对象
        /// </summary>
        /// <returns></returns>
        public SocketAsyncEventArgs Pop()
        {
            if (Count > 0)
            { // 栈中有空闲对象，直接使用
                lock (m_Pool)
                {
                    return m_Pool.Pop();
                }
            }
            // 无空闲，返回一个新的对象
            return new SocketAsyncEventArgs();
        }
    }
}