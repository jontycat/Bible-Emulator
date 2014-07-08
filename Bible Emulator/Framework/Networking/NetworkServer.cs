using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.ServiceModel.Channels;
using Networking.Sessions;

namespace Networking
{
    public class NetworkServer
    {
        public int Port
        {
            get { return mPort; }
        }

        public int MaxConnections
        {
            get { return mMaxConnections; }
        }

        public IPEndPoint EndPoint
        {
            get { return mEndPoint; }
        }

        public SocketAsyncEventArgsPool AcceptEvents
        {
            get { return mAcceptEvents; }
        }

        public SocketAsyncEventArgsPool ReceiveEvents
        {
            get { return mReceiveEvents; }
        }

        public Socket Socket
        {
            get { return mSocket; }
        }

        public int AcceptedConnections
        {
            get { return mAcceptedConnections; }
        }

        private int mPort = 0;
        private int mMaxConnections = 0;
        private int mMaxSimultaneousAccepts = 0;
        private int mMaxSimultaneousRecv = 0;
        private IPEndPoint mEndPoint;
        private int mAcceptedConnections = 0;

        private Socket mSocket = null;
        private SocketAsyncEventArgsPool mReceiveEvents = null;
        private SocketAsyncEventArgsPool mAcceptEvents = null;

        public bool IsListening = false;

        public NetworkServer(int port, int maxconn, int accepts, int recv)
        {
            if (port > 0 && maxconn > 0 && accepts > 0 && recv > 0)
            {
                this.mEndPoint = new IPEndPoint(IPAddress.Any, port);
                this.mPort = port;
                this.mMaxConnections = maxconn;
                this.mMaxSimultaneousAccepts = accepts;
                this.mMaxSimultaneousRecv = recv;
                InstantiateEvents();
            }
            else
            {
                ActionLogger.writeInfo("Please specfic valid values for your sockets in the configuration file!");
            }
        }

        private void InstantiateEvents()
        {
            mAcceptEvents = new SocketAsyncEventArgsPool(mMaxConnections);
            mReceiveEvents = new SocketAsyncEventArgsPool(mMaxConnections);

            for (int a = 0; a < mMaxSimultaneousAccepts; a++)
            {

                SocketAsyncEventArgs obj = new SocketAsyncEventArgs();
                obj.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptClient);
                mAcceptEvents.Push(obj);
            }

            for (int a = 0; a < mMaxSimultaneousRecv; a++)
            {
                SocketAsyncEventArgs obj = new SocketAsyncEventArgs();
                mReceiveEvents.Push(obj);
            }
        }

        public void StartListening()
        {
            try
            {
                this.mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.mSocket.Bind(mEndPoint);
                this.mSocket.Listen(100);
                BeginAccept();
                IsListening = true;

                return;
            }
            catch(Exception e)
            {
                ActionLogger.writeInfo(e.Message);
                IsListening = false;

                return;
            }
        }

          private void BeginAccept()
          {
              try
              {
                  SocketAsyncEventArgs obj = null;

                  if (mAcceptEvents.Count > 1)
                  {
                      obj = mAcceptEvents.Pop();
                  }
                  else
                  {
                      obj = CreateNewAcceptObj();
                  }

                  mSocket.AcceptAsync(obj);
              }

              catch (Exception e)
              {
                  ActionLogger.writeInfo(e.Message);
              }
       }

          private SocketAsyncEventArgs CreateNewAcceptObj()
          {
              SocketAsyncEventArgs obj = new SocketAsyncEventArgs();
              obj.Completed +=new EventHandler<SocketAsyncEventArgs>(AcceptClient);

              return obj;
          }

       private  void AcceptClient(object sender, SocketAsyncEventArgs args)
       {
           int clientID = Interlocked.Increment(ref mAcceptedConnections);

           try
           {
               if (args.SocketError != SocketError.Success)
               {
                   // if error fix later lol w.e
               }

               ActionLogger.writeInfo("Client[" + clientID + "] " + "has connected on " + args.AcceptSocket.LocalEndPoint);

               SocketAsyncEventArgs mReceive = mReceiveEvents.Pop();

               Session sess = new Session(args.AcceptSocket, mReceive);

               EntryWay.returnEnvironment.SessionManager.addSession(clientID, sess);

               mReceive.Completed +=new EventHandler<SocketAsyncEventArgs>(sess.END_RECEIVE);

               mReceive.AcceptSocket = args.AcceptSocket;

               args.AcceptSocket = null;

               mAcceptEvents.Push(args);

               sess.BeginReceive();

           }
           finally
           {
               BeginAccept();
           }
       }

    }
}
