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
        private TcpClient cl = new TcpClient();
        private bool listen = false;


        public MainWindow()
        {
            InitializeComponent();

            initUdpListener();
            udpStart();
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
                udp.Close();
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
                    setupTCPListener();                    
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void setupTCPListener()
        {
            try
            {
                listen = true;
                while (listen)
                {
                    byte[] buffer = new byte[cl.ReceiveBufferSize];
                    cl.GetStream().Read(buffer, 0, buffer.Length);
                    say(convertToString(new byte[]{buffer[0],buffer[1]}),ConsoleColor.Red);
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void sendLogin()
        {
            // Sending LI
            say(String.Format("Sending LI...", serverip.ToString(), serverport));
            cl.GetStream().Write(new byte[] { 76, 73 }, 0, 2);
            say(String.Format("Sending LI complete", serverip.ToString(), serverport));
        }
        private void sendBingo()
        {
            // Sending BI
            say(String.Format("Sending BI...", serverip.ToString(), serverport));
            cl.GetStream().Write(new byte[] { 66, 73 }, 0, 2);
            say(String.Format("Sending BI complete", serverip.ToString(), serverport));
        }
        private void sendDC()
        {
            //Sending DC
            say(String.Format("Sending DC...", serverip.ToString(), serverport));
            cl.GetStream().Write(new byte[] { 68, 67 }, 0, 2);
            say(String.Format("Sending DC complete", serverip.ToString(), serverport));
            cl.Close();
            listen = false;
        }
        private void sendAdmin()
        {
            //Sending AD
            say(String.Format("Sending AD...", serverip.ToString(), serverport));
            cl.GetStream().Write(new byte[] { 65, 68 }, 0, 2);
            say(String.Format("Sending AD complete", serverip.ToString(), serverport));
        }


        private void udpStart()
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
        internal static string convertToString(byte[] a)
        {
            return new ASCIIEncoding().GetString(a);
        }

        private void btnLI_Click(object sender, RoutedEventArgs e)
        {
            sendLogin();
        }

        private void btnDC_Click(object sender, RoutedEventArgs e)
        {
            sendDC();
        }

        private void btnAD_Click(object sender, RoutedEventArgs e)
        {
            sendAdmin();
        }

        private void btnBI_Click(object sender, RoutedEventArgs e)
        {
            sendBingo();
        }

      
    }
}
