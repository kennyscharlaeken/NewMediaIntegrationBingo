
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class ServerBeacon
    {

        private const string MSG_ACTIVE = "Beacon is now active on {0}:{1}. Waiting on transmitters.";
        private const string MSG_BIS = "Transmition received from {0} and replied";
        private const string MSG_OFF = "Beacon has been turned off.";

        private const int BEACON_PORT = 3300;
        private static IPAddress BEACON_lISTEN_IP = IPAddress.Any;

        // BIS <- start byte for broadcast server retrieval (0x424953)
        private const string BIS = "BIS";

        private static ServerBeacon _beacon;
        public static ServerBeacon Singleton
        {
            get
            {
                if (_beacon == null) _beacon = new ServerBeacon();
                return _beacon;
            }
        }
        private ServerBeacon(){}


        public bool Active { get; set; }

        private bool _listen = false;


        private UdpClient _udp = null;
        private IPEndPoint _groupep = new IPEndPoint(BEACON_lISTEN_IP, BEACON_PORT);

        public void listen(object e)
        {
            try
            {
                _listen = true;
                Active = true;
                _udp = new UdpClient(BEACON_PORT);
                Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_ACTIVE,BEACON_lISTEN_IP,BEACON_PORT));
                while (_listen)
                {
                    byte[] msg = _udp.Receive(ref _groupep);
                    object[] para = new object[2] { _groupep.Address.ToString(), msg };
                    ThreadPool.QueueUserWorkItem(new WaitCallback(processMessage), para);
                }
                Active = false;
            }
            catch (Exception ex)
            {
                if(_listen)throw;
            }

        }

        public void close()
        {
            try
            {
                _listen = false;
                Active = false;
                if(_udp!=null)_udp.Close();
                Debug.Singleton.sendDebugMessage(DEBUGLEVELS.WARNING, MSG_OFF);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void processMessage(object e)
        {
            object[] para = e as object[];
            if (para != null)
            {
                string msg = Helper.convertToString(para[1] as byte[]);
                if (msg == BIS)
                {                   
                    sendResponse((string)para[0]);
                }                
            }
        }

        private void sendResponse(string adress)
        {
            IPAddress ip;
            IPEndPoint end;
            UdpClient cl = new UdpClient();
            if(IPAddress.TryParse(adress,out ip))
            {
                end = new IPEndPoint(ip, 3301);
                //end = new IPEndPoint(ip, BEACON_PORT);
                byte[] msg = Helper.convertToBytes(Server.SERVER_PORT);
                //_udp.Send(msg,msg.Count(),end);
                if (_listen)
                {
                    cl.Send(msg, msg.Count(), end);
                    Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_BIS, adress));
                }
            }
        }

        


    }
}
