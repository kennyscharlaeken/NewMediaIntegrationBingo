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
        private IPAddress _ip = null;
        private TcpListener _server;
        private List<ServerClient> _clients = new List<ServerClient>();
        private bool _listen = false;

        private int _maxpending = 32;

        public Server()
        {
            _ip = Helper.LocalIPAddress();
            _server = new TcpListener(_ip,SERVER_PORT);
        }

        // Starts the listen thread
        public void startToListen()
        {
            _listen = true;
            Thread listenthread = new Thread(new ThreadStart(listen));
            listenthread.Start();
        }

        private void listen()
        {
            try
            {
                _server.Start(_maxpending);
                while (_listen)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    ServerClient scl = retrieveConnectedClient(client);

                    if (!clientExists(scl))
                    {
                        startClient(scl);
                    }

                }
                _server.Stop();              
            }
            catch
            {
                if (_listen) throw;
            }
        }

        private bool clientExists(ServerClient scl)
        {
            return _clients.Contains(scl);
        }

        private ServerClient retrieveConnectedClient(TcpClient cl)
        {
            IPEndPoint ip = cl.Client.RemoteEndPoint as IPEndPoint;
            ServerClient scl = new ServerClient() { Ip = ip};
            return scl;
        }

        protected void sendMessageToAll(byte[] msg)
        {
            foreach(ServerClient client in _clients)
            {
                client.sendMessage(msg);
            }
        }

        private void startClient(ServerClient cl)
        {            
            if (cl != null)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(cl.listen),null);
                _clients.Add(cl);
            }
        }

    }

}
