using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;

namespace Gameplay
{
    public class Player
    {
        public IPAddress Ip { get; set; }
        public Image Image { get; set; }

        public List<BingoCard> BingoCards { get; set; }

        public override string ToString()
        {
            return Ip.ToString();
        }
    }
}
