using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Threading;

namespace TestClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //private static IPAddress GroupAddress = LocalIPAddress();
        private static IPAddress GroupAddress = IPAddress.Broadcast;
        private const int destinationPort = 3300;
        private const int receivePort = 3301;
        private IPAddress serverip=null;
        private int serverport = 1300;
        TcpClient cl = new TcpClient();


        public MainWindow()
        {
            InitializeComponent();

            initUdpListener();
            start();
        }

        private void initUdpListener()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(udpListener));
        }

        private void udpListener(object state)
        {
            UdpClient udp = new UdpClient(receivePort);
            try
            {                
                IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = udp.Receive(ref point);
                serverip = point.Address;
                say(String.Format("Found a server {0}:{1}", serverip.ToString(), serverport));
                makeTCPConnection();
            }
            catch (Exception ex)
            {
                udp.Close();
            }           

        }

        private void makeTCPConnection()
        {
            if (serverip != null)
            {
                try
                {
                    say(String.Format("Attempting a connection on {0}:{1}", serverip.ToString(), serverport));
                    cl.Connect(serverip, serverport);
                    say(String.Format("Connected", serverip.ToString(), serverport));
                    sendLI(cl);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void sendLI(TcpClient cl)
        {
            // Sending LI
            say(String.Format("Sending LI...", serverip.ToString(), serverport));
            cl.GetStream().Write(new byte[] { 76, 73 }, 0, 2);
            say(String.Format("Sending LI complete", serverip.ToString(), serverport));
        }
        
    
        private void start()
        {
            UdpClient sender = new UdpClient();
            IPEndPoint groupEP = new IPEndPoint(GroupAddress, destinationPort);
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes("BIS");
                //while (true)
                //{
                    say(String.Format("Sending datagram {0} to {1}", "BIS", GroupAddress.Address.ToString()));
                    sender.Send(bytes, bytes.Length, groupEP);
                    Thread.Sleep(100);
                //}
                sender.Close();

            }
            catch (Exception e)
            {
                say(String.Format("Failed: {0}",e.Message), ConsoleColor.Red);
            }
        }


        private void say(string text,ConsoleColor cl = ConsoleColor.Green)
        {
            Console.ForegroundColor = cl;
            Console.WriteLine(text);
        }


        internal static IPAddress LocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily.ToString() == "InterNetwork");
        }
    }
}
