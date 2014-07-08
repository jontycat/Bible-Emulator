using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Messages.Events;
using Networking.Sessions;

namespace Messages.Events.Incoming
{
    public class InitCryptoMessageEvent : iMessage
    {
        public void invokeMessage(byte[] msg, Session s)
        {
        }
    }
}
