using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public sealed class ServerClient
    {

        //privates
        private bool _listen = true;
        private IPEndPoint _endpoint;
        private UdpClient _udpcl = new UdpClient();

        public IPEndPoint Endpoint
        {
            get { return _endpoint; }
            set { _endpoint = value; }
        }

        public ServerClient()
        {
            Endpoint = new IPEndPoint(IPAddress.Any, Server.SERVER_PORT);
        }


        // Listen to incoming traffic on specific Endpoint
        public void listen(object state)
        {
            if (Endpoint != null)
            {
                _udpcl.Client.Connect(Endpoint);
                while (_listen)
                {
                    byte[] msg = _udpcl.Receive(ref _endpoint);
                    // Have a message do something with it
                }
            }
            else throw new Exception("No Enpoint was set for client");
        }


        public override bool Equals(object obj)
        {
            ServerClient cl = obj as ServerClient;
            if (cl != null)
            {
                if (cl.Endpoint.Address.Equals(Endpoint.Address))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
