using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Logging;
using System.Collections.Concurrent;

namespace Networking.Sessions
{
   public class SessionManager
    {
       private Dictionary<int, Session> mConnectedClients = null;

       public int ConnectionCount
       {
           get { return mConnectedClients.Count; }
       }

       public SessionManager()
       {
           mConnectedClients = new Dictionary<int, Session>();
           ActionLogger.writeInfo("SessionManager instantiated successfully!");
           
       }

       public void addSession(int ID, Session s)
       {
           mConnectedClients.Add(ID, s);
       }

       public void removeSession(int ID)
       {
           lock (mConnectedClients)
           {
               mConnectedClients.Remove(ID);
           }
       }
    }
}
