using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using Messages.Events;
using Messages.Events.Incoming;

namespace Messages
{
    public class MessageHandler
    {
        private Dictionary<int, iMessage> mMessageDictionary = null;

        public MessageHandler()
        {
            mMessageDictionary = new Dictionary<int, iMessage>();
            ActionLogger.writeInfo("MessageHandler instantiated successfully!");
            addValues();
        }

        private void addValues()
        {
            mMessageDictionary.Add(206, new InitCryptoMessageEvent());

            foreach (iMessage msg in mMessageDictionary.Values)
            {
                ActionLogger.writeInfo("Registered >> " + msg.GetType().Name);
            }

            ActionLogger.writeInfo("Loaded MessageDictionary with [" + mMessageDictionary.Count + "] value(s)!");
        }

       public iMessage invoke(int ID)
        {
            return mMessageDictionary[ID];
        }
    }
}
