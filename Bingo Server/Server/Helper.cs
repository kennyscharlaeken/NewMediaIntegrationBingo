using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server
{
    internal static class Helper
    {

        internal static IPAddress LocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily.ToString() == "InterNetwork");
        }

        internal static byte[] convertToBytes(string text)
        {
            return new ASCIIEncoding().GetBytes(text);
        }
        internal static byte[] convertToBytes(int i)
        {
            return BitConverter.GetBytes(i);
        }
        internal static byte[] convertToBytes(object e)
        {
            if (e == null) return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, e);
            return ms.ToArray();
        }

        internal static string convertToString(byte[] a)
        {
            return new ASCIIEncoding().GetString(a);
        }

        internal static string convertImgToString(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return Helper.convertToString(byteArray);
        }

        internal static string convertNumberList(IEnumerable<object> list)
        {
            string result = "";
            foreach (object item in list)
            {
                string nr = item.ToString();
                result += nr.Length+nr;
            }
            return result;
        }

        internal static string convertToXml(object e)
        {
            string xml = string.Empty;
            if (e != null)
            {
                XmlSerializer s = new XmlSerializer(e.GetType());
                MemoryStream ms = new MemoryStream();
                s.Serialize(ms, e);
                xml = new ASCIIEncoding().GetString(ms.ToArray());
                ms.Close();
            }
            return xml;
        }

    }
}
