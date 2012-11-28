using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public sealed class Debug
    {

        private static Debug _singelton;
        public static Debug Singleton
        {
            get
            {
                if (_singelton == null) _singelton = new Debug();
                return _singelton;
            }
        }
        private Debug() { }

        public delegate void DebugMessageHandler(DEBUGLEVELS c,string m);
        public event DebugMessageHandler DebugMessage;

        private void fireDebugMessage(DEBUGLEVELS c, string m)
        {
            if (DebugMessage != null) DebugMessage.Invoke(c,m);
        }


        public void sendDebugMessage(DEBUGLEVELS c, string m)
        {
            fireDebugMessage(c,m);
        }

    }

    public enum DEBUGLEVELS
    {
        WARNING,
        ERROR,
        INFO
    }

}
