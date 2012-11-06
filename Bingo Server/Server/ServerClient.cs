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
        private bool _listen = false;
        private IPEndPoint _ip;
        private TcpClient _client = null;
        private NetworkStream _stream=null;

        public IPEndPoint Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        public TcpClient Client
        {
            get { return _client; }
            set 
            {
                if (_client != null)
                {
                    _stream = _client.GetStream();
                }
            }
        }

        public ServerClient()
        {}

        // Listen to incoming traffic
        public void listen(object state)
        {
            try
            {
                if (_stream != null)
                {
                    _listen = true;
                    while (_listen)
                    {
                        byte[] msg = new byte[256];
                        int bytes = _stream.Read(msg, 0, msg.Length);
                        processMessage(msg,bytes);
                    }
                }
            }
            catch (Exception)
            {
                // do something
                throw;
            }
        }

        private void processMessage(byte[] msg, int bytes)
        {
            // process all messages wether it's a string or bits and bytes
        }

        public void sendMessage(byte[] msg)
        {
            try
            {
                if ( (_stream != null) && (msg!=null) && (msg.Length>0))
                {
                    _stream.Write(msg, 0, msg.Length);
                }
            }
            catch (Exception)
            {
                // do something
                throw;
            }            
        }


        public override bool Equals(object obj)
        {
            ServerClient cl = obj as ServerClient;
            if (cl != null)
            {
                if (IPAddress.Equals(cl.Ip,Ip))return true;
            }
            return false;
        }


    }
}
