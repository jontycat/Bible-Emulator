using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Networking
{
   public class SocketAsyncEventArgsPool
    {
        Stack<SocketAsyncEventArgs> mPool = null; 

        public SocketAsyncEventArgsPool(int size)
        {
            mPool = new Stack<SocketAsyncEventArgs>(size);
        }

       /// <summary>
       /// places a SocketAsyncEventArgs object at the top of the stack
       /// </summary>
       /// <param name="obj"></param>
        public void Push(SocketAsyncEventArgs obj)
        {
            if (obj == null)
                throw new NullReferenceException("SocketAsyncEventArgs Object not instantiated!");
            else
                mPool.Push(obj);
        }

       /// <summary>
       /// 'Pops' or returns a SocketAsyncEventArgs obj from the top of the stack.
       /// </summary>
       /// <returns></returns>
        public SocketAsyncEventArgs Pop()
        {
            lock (mPool)
            {
                return mPool.Pop();
            }
        }

       /// <summary>
       /// Returns a count of the current objects in the stack
       /// </summary>
        public int Count
        {
            get { return mPool.Count; }
        }
    }
}
