using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Logging;
using System.Threading;

namespace Networking
{
   public class ClientServer
    {
        public int Port
        {
            get { return mPort; }
        }

        public IPEndPoint IP
        {
            get { return mEndPoint; }
        }

        public int MaxConnections
        {
            set { value = mMaxConnections; }
            get { return mMaxConnections; }
        }

        public int ConnectionPoolSize
        {
            set { value = mConnectionPoolSize; }
            get { return mConnectionPoolSize; }
        }

        public SocketAsyncEventArgsPool AcceptEventPool
        {
            get { return mAcceptEvents; }
        }

        public SocketAsyncEventArgsPool ReceiveEventPool
        {
            get { return mReceiveEvents; }
        }

        public int AcceptedConnections
        {
            get { return mAcceptedConnections; }
        }

        private int mPort = 0;
        private IPEndPoint mEndPoint = null;
        private int mMaxConnections = 0;
        private int mConnectionPoolSize = 0;
        private SocketAsyncEventArgsPool mAcceptEvents = null;
        private SocketAsyncEventArgsPool mReceiveEvents = null;
        private Socket mWinSocket = null;
        private int mAcceptedConnections = 0;

        public ClientServer(int port, int maxconn, int connpool)
        {
            if (port > 0)
            {
                this.mPort = port;
                this.mMaxConnections = maxconn;
                this.mConnectionPoolSize = connpool;
                mEndPoint = new IPEndPoint(IPAddress.Any, mPort);
                InstantiatePool();
                ActionLogger.writeInfo("ClientServer instantiated successfully!");
            }
        }


        public void StartListening()
        {
            this.mWinSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mWinSocket.Bind(mEndPoint);
            mWinSocket.Listen(100);
            StartAccept();
            ActionLogger.writeInfo("Listening for connections on " + mEndPoint.ToString());


        }


        private void InstantiatePool()
        {
            mAcceptEvents = new SocketAsyncEventArgsPool(mMaxConnections);
            mReceiveEvents = new SocketAsyncEventArgsPool(mMaxConnections);

            for (int a = 0; a < mConnectionPoolSize; a++)
            {
                SocketAsyncEventArgs obj = new SocketAsyncEventArgs();

                mAcceptEvents.Push(obj);
                obj.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Client);
            }
        }

        private void StartAccept()
        {
            SocketAsyncEventArgs accept;


            if (mAcceptEvents.Count > mConnectionPoolSize)
            {
                accept = mAcceptEvents.Pop();
            }
            else
            {
               accept =  ResizePool();
            }



            bool acceptEvent = mWinSocket.AcceptAsync(accept);

            if (acceptEvent == false)
            {
                //o shit error lolz
            }
        }

        private SocketAsyncEventArgs ResizePool()
        {
            int newCount = mConnectionPoolSize * 2;

            for (int a = mConnectionPoolSize; a < newCount; a++)
            {
                SocketAsyncEventArgs obj = new SocketAsyncEventArgs();

                obj.Completed +=new EventHandler<SocketAsyncEventArgs>(Accept_Client);

                mAcceptEvents.Push(obj);
            }

            mConnectionPoolSize = newCount;

            return mAcceptEvents.Pop();
        }

        private void Accept_Client(object sender, SocketAsyncEventArgs args)
        {
            int mAcceptID = Interlocked.Increment(ref mAcceptedConnections);

            if (args.SocketError != SocketError.Success)
            {
                StartAccept();
                DestroyBadConnection();
            }
            else
            {
                StartAccept();

                //Need to fix this later
                if (mReceiveEvents.Count < mConnectionPoolSize)
                {
                    mReceiveEvents.Push(new SocketAsyncEventArgs());
                }


              SocketAsyncEventArgs receive =  mReceiveEvents.Pop();

                receive.Completed += new EventHandler<SocketAsyncEventArgs>(Process_Receive);

          //    receive.AcceptSocket = args.AcceptSocket;

              ActionLogger.writeInfo("Accepted client[" + mAcceptID + "] " + "on " + args.AcceptSocket.LocalEndPoint.ToString());

              args.AcceptSocket = null;

              mAcceptEvents.Push(args);

              Start_Receive(receive, mAcceptID);
            }
        }

        private void Start_Receive(SocketAsyncEventArgs args, int clientID)
        {
            bool raiseEvent = args.AcceptSocket.ReceiveAsync(args);

            if (raiseEvent == false)
            {
                //O no error fuck
            }
        }

        private void Process_Receive(object sender, SocketAsyncEventArgs args)
        {
            ActionLogger.writeInfo("I got data!");
        }
        private void DestroyBadConnection()
        {
        }

    }
}
