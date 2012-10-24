using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;


using System.Net;
using System.Threading;

namespace Server
{

   // 
   //   For games we use UDP
   //

    public sealed class Server 
    {

        // CONSTANTS
        public const int SERVER_PORT = 1300;

        // Privates
        private UdpClient _serverlistener;
        private List<ServerClient> _clients = new List<ServerClient>();
        private bool _listen = false;

        public Server()
        {
            _serverlistener = new UdpClient(SERVER_PORT);
        }

        // Starts the listen thread
        public void startToListen()
        {
            _listen = true;
            Thread listenthread = new Thread(new ThreadStart(listen));
            listenthread.Start();
        }

        // The thread that is listening into incomming transmissions. (Only new clients will be initialized)
        private void listen()
        {
            try
            {
                while (_listen)
                {

                    IPEndPoint pt = new IPEndPoint(Helper.LocalIPAddress(),SERVER_PORT);
                    byte[] receivebytes = _serverlistener.Receive(ref pt);

                    ServerClient cl = getClientConnected(pt);
                    if (cl!=null)
                    {
                        startClient(cl);
                    }
                }                
            }
            catch (Exception)
            {
                throw;
            }
        }

        // If Adress does not already exists in the list we add it.
        private ServerClient getClientConnected(IPEndPoint pt)
        {
            ServerClient cl = new ServerClient();
            cl.Endpoint = pt;
            if (!_clients.Contains(cl)) _clients.Add(cl); else cl = null;
            return cl;
        }

        // starts a client listener for incomming traffic on that specific ip
        private void startClient(ServerClient cl)
        {            
            if (cl != null)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(cl.listen),null);
            }
        }

    }

}
