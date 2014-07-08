using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Networking.Sessions;

namespace Messages.Events
{
    public interface iMessage
    {
         void invokeMessage(byte[] msg, Session s);
    }
}
