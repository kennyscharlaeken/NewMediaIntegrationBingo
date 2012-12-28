using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;


using System.Net;
using System.Threading;
using Gameplay;

namespace Server
{

    public partial class Server
    {

        // CONSTANTS
        public const int SERVER_PORT = 1300;

        // Statics
        private static Server _singleton = null;

        // Privates
        private IPAddress _ip = null;
        private TcpListener _serverlistener;
        private List<ClientHandler> _clients = new List<ClientHandler>();
        private bool _listen = false;

        private int _maxpending = 1000;

        private List<Player> _players = new List<Player>();
        public List<Player> Players { get { return _players; } }

        public static Server Singleton
        {
            get
            {
                if (_singleton == null) _singleton = new Server();
                return _singleton;
            }
        }

        public Server()
        {
            _ip = Helper.LocalIPAddress();
            _serverlistener = new TcpListener(_ip,SERVER_PORT);
        }

        // Starts the listen thread
        public void open()
        {
            _listen = true;
            Thread listenthread = new Thread(new ThreadStart(listen));
            listenthread.Start();
        }

        private void listen()
        {
            try
            {
                _serverlistener.Start(_maxpending);
                Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO,String.Format(MSG_SERVER_START,_ip.ToString(),SERVER_PORT));
                while (_listen)
                {
                    TcpClient client = _serverlistener.AcceptTcpClient();
                    ClientHandler scl = retrieveConnectedClient(client);
                    if (!clientExists(scl))
                    {
                        startClient(scl);
                    }
                } 
            }
            catch(Exception)
            {
                if (_listen) throw;
            }
        }

        public void close()
        {
            try
            {
                _listen = false;
                _serverlistener.Stop();
                dcClients();                
                _clients.Clear();                
                Debug.Singleton.sendDebugMessage(DEBUGLEVELS.WARNING, MSG_SERVER_STOP);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void dcClients()
        {
            foreach (ClientHandler item in _clients)
            {
                item.disconnect();
            }
        }

        private bool clientExists(ClientHandler scl)
        {
            return _clients.Contains(scl);
        }

        private ClientHandler retrieveConnectedClient(TcpClient cl)
        {
            IPEndPoint ip = cl.Client.RemoteEndPoint as IPEndPoint;
            ClientHandler scl = new ClientHandler() { Ip = ip};
            cl.NoDelay = true;
            scl.Client = cl;
            return scl;
        }

        private void startClient(ClientHandler cl)
        {            
            if (cl != null)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(cl.listen),null);
                addNewClient(cl);
            }
        }

        private void addNewClient(ClientHandler cl)
        {
            if (!_clients.Contains(cl)) _clients.Add(cl);
        }

        public void removeClient(ClientHandler cl)
        {
            // DC the client first ?!
            _clients.Remove(cl);
        }

        #region "PEOPLE"
        public void sendMessageToAdmins(string msg)
        {
            var res = from n in _clients
                      where n.Admin == true
                      select n;
            foreach (ClientHandler cl in res)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(cl.sendMessage), msg);
            }
        }
        public void sendMessageToPlayers(object msg)
        {
            var res = from n in _clients
                      where n.IsPlayer == true
                      select n;
            foreach (ClientHandler cl in res)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(cl.sendMessage), msg);
            }
        }
        public void sendMessageToPlayer(Player p, string msg)
        {
            Player pl = p;
            var res = from n in _clients
                      where n.Player.Equals(pl)
                      select n;
            ClientHandler cl = null;
            foreach (ClientHandler temp in res)
            {
                cl = temp;
            }
            if (cl != null) cl.sendMessage(msg);
        }
        #endregion

    }

}
