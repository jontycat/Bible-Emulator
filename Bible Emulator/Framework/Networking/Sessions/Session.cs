using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Logging;

namespace Networking.Sessions
{
    public class Session
    {
        private Socket mSocket = null;
        private SocketAsyncEventArgs mSocketEventHandle = null;
        private byte[] buffer = null;

        public Session(Socket socket, SocketAsyncEventArgs socketHandle)
        {
            if (socket != null)
            {
                this.mSocket = socket;
                this.mSocketEventHandle = socketHandle;
                this.buffer = new byte[256];
                this.mSocketEventHandle.SetBuffer(buffer, 0, 256);
                this.SendPolicyFileRequest();

            }
        }

        public void BeginReceive()
        {
            mSocket.ReceiveAsync(mSocketEventHandle);
        }

        public void ProcessRequest()
        {
            try
            {
                int recvBytes = mSocketEventHandle.BytesTransferred;

                if (recvBytes > 0)
                {

                }
            }

            finally
            {
                BeginReceive();
            }
        }
        

        public void Send(String x)
        {
            mSocket.Send(Encoding.UTF8.GetBytes(x + (char) 1));
        }


        /// <summary>
        /// if an error is caught, the server will still process the request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="s"></param>
        public void END_RECEIVE(object sender, SocketAsyncEventArgs s)
        {
            ProcessRequest();
        }

        private void SendPolicyFileRequest()
        {
            mSocket.Send(Encoding.UTF8.GetBytes("@@" + (char) 1));
        }

    }
}
