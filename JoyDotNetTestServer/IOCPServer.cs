
using System.Net;
using System.Net.Sockets;

namespace Joy.Test.Server
{
    /// <summary>
    /// 基于IOCP的服务器
    /// Implements the connection logic for the socket server.
    /// After accepting a connection, all data read from the client
    /// is sent back to the client. The read and echo back to the client pattern
    /// is continued until the client disconnects.
    /// </summary>
    public class IOCPServer
    {
        /// <summary>
        /// 支持的最大连接数
        /// </summary>
        private readonly int m_NumConnections;   // the maximum number of connections the sample is designed to handle simultaneously
        /// <summary>
        /// 消息缓冲区的大小
        /// </summary>
        private readonly int m_BufferSize; // buffer size to use for each socket I/O operation
        /// <summary>
        /// 每个Socket连接有读写两种操作(接受新连接，不需要申请缓冲区)
        /// </summary>
        const int opsToPreAlloc = 2;    // read, write (don't alloc buffer space for accepts)

        /// <summary>
        /// 连接数信号量，连接已满时，新连接等待直到有连接被释放
        /// </summary>
        private readonly Semaphore m_MaxNumberAcceptedClients;

        /// <summary>
        /// 缓冲区管理器
        /// </summary>
        private readonly BufferManager m_BufferManager;  // represents a large reusable set of buffers for all socket operations

        /// <summary>
        /// 用于处理Socket发送、接收消息，处理新连接的SocketAsyncEventArgs对象池
        /// pool of reusable SocketAsyncEventArgs objects for write, read and accept socket operations
        /// </summary>
        private readonly SocketAsyncEventArgsPool m_ReadWritePool;

        /// <summary>
        /// 监听Socket，用于监听并处理客户端的连接
        /// </summary>
        private Socket m_ListenSocket;


        int m_totalBytesRead;           // counter of the total # bytes received by the server
        int m_numConnectedSockets;      // the total number of clients connected to the server


        // Create an uninitialized server instance.
        // To start the server listening for connection requests
        // call the Init method followed by Start method
        //
        // <param name="numConnections">the maximum number of connections the sample is designed to handle simultaneously</param>
        // <param name="receiveBufferSize">buffer size to use for each socket I/O operation</param>
        public IOCPServer(int numConnections, int receiveBufferSize)
        {
            m_totalBytesRead = 0;
            m_numConnectedSockets = 0;
            m_NumConnections = numConnections;
            m_BufferSize = receiveBufferSize;
            // 初始化缓冲区管理器，这里设计为每个连接的每个缓冲区都有独立可用的字节缓冲区
            m_BufferManager = new BufferManager(m_BufferSize * m_NumConnections * opsToPreAlloc, m_BufferSize);
            // 初始化对象池
            m_ReadWritePool = new SocketAsyncEventArgsPool(m_NumConnections);
            m_MaxNumberAcceptedClients = new Semaphore(m_NumConnections, m_NumConnections);
        }

        // Initializes the server by preallocating reusable buffers and
        // context objects.  These objects do not need to be preallocated
        // or reused, but it is done this way to illustrate how the API can
        // easily be used to create reusable objects to increase server performance.
        //
        public void Init()
        {
            // Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds
            // against memory fragmentation
            m_BufferManager.InitBuffer();

            // preallocate pool of SocketAsyncEventArgs objects
            SocketAsyncEventArgs readWriteEventArg;

            for (int i = 0; i < m_NumConnections; i++)
            {
                //Pre-allocate a set of reusable SocketAsyncEventArgs
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);

                // assign a byte buffer from the buffer pool to the SocketAsyncEventArg object
                m_BufferManager.SetBuffer(readWriteEventArg);

                // add SocketAsyncEventArg to the pool
                m_ReadWritePool.Push(readWriteEventArg);
            }
        }

        // Starts the server such that it is listening for
        // incoming connection requests.
        //
        // <param name="localEndPoint">The endpoint which the server will listening
        // for connection requests on</param>
        public void Start(IPEndPoint localEndPoint)
        {
            // create the socket which listens for incoming connections
            m_ListenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_ListenSocket.Bind(localEndPoint);
            // start the server with a listen backlog of 100 connections
            m_ListenSocket.Listen(100);

            // post accepts on the listening socket
            SocketAsyncEventArgs acceptEventArg = new SocketAsyncEventArgs();
            acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            StartAccept(acceptEventArg);

            //Console.WriteLine("{0} connected sockets with one outstanding receive posted to each....press any key", m_outstandingReadCount);
            Console.WriteLine("Press any key to terminate the server process....");
            Console.Read();
        }

        // Begins an operation to accept a connection request from the client
        //
        // <param name="acceptEventArg">The context object to use when issuing
        // the accept operation on the server's listening socket</param>
        public void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            // loop while the method completes synchronously
            bool willRaiseEvent = false;
            while (!willRaiseEvent)
            {
                m_MaxNumberAcceptedClients.WaitOne();

                // socket must be cleared since the context object is being reused
                acceptEventArg.AcceptSocket = null;
                willRaiseEvent = m_ListenSocket.AcceptAsync(acceptEventArg);
                if (!willRaiseEvent)
                {
                    ProcessAccept(acceptEventArg);
                }
            }
        }

        // This method is the callback method associated with Socket.AcceptAsync
        // operations and is invoked when an accept operation is complete
        //
        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);

            // Accept the next connection request
            StartAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            Interlocked.Increment(ref m_numConnectedSockets);
            Console.WriteLine("Client connection accepted. There are {0} clients connected to the server",
                m_numConnectedSockets);

            // Get the socket for the accepted client connection and put it into the
            //ReadEventArg object user token
            SocketAsyncEventArgs readEventArgs = m_ReadWritePool.Pop();
            readEventArgs.UserToken = e.AcceptSocket;

            // As soon as the client is connected, post a receive to the connection
            bool willRaiseEvent = e.AcceptSocket.ReceiveAsync(readEventArgs);
            if (!willRaiseEvent)
            {
                ProcessReceive(readEventArgs);
            }
        }

        // This method is called whenever a receive or send operation is completed on a socket
        //
        // <param name="e">SocketAsyncEventArg associated with the completed receive operation</param>
        void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        // This method is invoked when an asynchronous receive operation completes.
        // If the remote host closed the connection, then the socket is closed.
        // If data was received then the data is echoed back to the client.
        //
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            // check if the remote host closed the connection
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                //increment the count of the total bytes receive by the server
                Interlocked.Add(ref m_totalBytesRead, e.BytesTransferred);
                Console.WriteLine("The server has read a total of {0} bytes", m_totalBytesRead);

                //echo the data received back to the client
                e.SetBuffer(e.Offset, e.BytesTransferred);
                Socket socket = (Socket)e.UserToken;
                bool willRaiseEvent = socket.SendAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessSend(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        // This method is invoked when an asynchronous send operation completes.
        // The method issues another receive on the socket to read any additional
        // data sent from the client
        //
        // <param name="e"></param>
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                // done echoing data back to the client
                Socket socket = (Socket)e.UserToken;
                // read the next block of data send from the client
                bool willRaiseEvent = socket.ReceiveAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            Socket socket = (Socket)e.UserToken;

            // close the socket associated with the client
            try
            {
                socket.Shutdown(SocketShutdown.Send);
            }
            // throws if client process has already closed
            catch (Exception) { }
            socket.Close();

            // decrement the counter keeping track of the total number of clients connected to the server
            Interlocked.Decrement(ref m_numConnectedSockets);

            // Free the SocketAsyncEventArg so they can be reused by another client
            m_ReadWritePool.Push(e);

            m_MaxNumberAcceptedClients.Release();
            Console.WriteLine("A client has been disconnected from the server. There are {0} clients connected to the server", m_numConnectedSockets);
        }
    }
}