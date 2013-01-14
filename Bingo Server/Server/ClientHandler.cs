using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.IO;
using Gameplay;

namespace Server
{
    public partial class ClientHandler
    {

        //privates
        private bool _active = false;
        private IPEndPoint _ip;
        private TcpClient _client = null;
        private NetworkStream _stream=null;
        private bool _bufferoverflow = false;
        private Server _server = null;
        private string _code = string.Empty;
        private Player _player = new Player();
        private MemoryStream _memstream = new MemoryStream();
        private int _overflowsize = 0;

        //Const
        private const int BUFFER = 200000;

        public bool IsPlayer { get; set; }

        public Player Player { get{return _player;}}
        public bool Admin { get; set; }
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
                if (value != null)
                {
                    _client = value;
                    _stream = _client.GetStream();
                }
            }
        }

        public ClientHandler()
        {
            _server = Server.Singleton;
        }

        // Listen to incoming traffic
        public void listen(object state)
        {
            try
            {
                if (_stream != null)
                {
                    _active = true;
                    while (_active)
                    {
                        byte[] msg = new byte[BUFFER];
                        int bytes = _stream.Read(msg, 0, msg.Length);
                        processMessage(msg, bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_active) throw;
            }
        }

        private void processMessage(byte[] msg, int bytesread)
        {
            //process all messages wether it's a string or bits and bytes , first 2 bytes are code
            string code = Helper.convertToString(new byte[] { msg[0], msg[1] });
            resolveMessage(code, msg);           
        }
        private void captureImage(byte[] image)
        {
            MemoryStream str = new MemoryStream(image);
            _player.Image = Image.FromStream(str);
            str.Flush();
            fireImageUpdated(_player);
        }
      
        private void send(byte[] buffer)
        {
            if (_active)
            {
                if ((_stream != null) && (buffer != null) && (buffer.Length > 0))
                {
                    _stream.Write(buffer, 0, buffer.Length);
                }
            }
        }
        public void sendMessage(object e)
        {
            try
            {
                //byte[] msg = Helper.convertToBytes(Helper.convertToXml(e));
                byte[] msg = Helper.convertToBytes(e);
                send(msg);
            }
            catch (Exception)
            {
                if (_active) throw;
            }            
        }        
        public void sendMessage(string text)
        {
            try
            {
                byte[] msg = Helper.convertToBytes(text);
                send(msg);
            }
            catch (Exception)
            {
                if (_active) throw;
            }
        }

        private void sendAck()
        {
            sendMessage(ServerCodes.SERVER_CODE_ACK);
        }
        private void sendNAck()
        {
            sendMessage(ServerCodes.SERVER_CODE_NACK);
        }

        public void disconnect(bool removeself=false)
        {
            try
            {
                sendMessage(ServerCodes.SERVER_CODE_SHUTDOWN);
                _active = false;
                IsPlayer = false;
                //_player = null;
                _stream.Close();
                if(removeself)Server.Singleton.removeClient(this);                
            }
            catch (Exception ex)
            {                
                throw;
            }
        }

        public override bool Equals(object obj)
        {
            ClientHandler cl = obj as ClientHandler;
            if (cl != null)
            {
                if (IPAddress.Equals(cl.Ip,Ip))return true;
            }
            return false;
        }


    }
}
