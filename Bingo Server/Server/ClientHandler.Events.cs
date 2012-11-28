using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Gameplay;

namespace Server
{
    public partial class ClientHandler
    {
        public delegate void playerUpdateHandler(ref Player player);
        public static event playerUpdateHandler ImageUpdated;
		public static event playerUpdateHandler PlayerJoined;
        public static event playerUpdateHandler PlayerLeft;

        private void fireImageUpdated(ref Player player)
        {
            if (ImageUpdated != null)
            {
                ImageUpdated.Invoke(ref player);
            }
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_IMAGE, player.ToString()));
        }

        private void firePlayerJoined(ref Player player)
        {
            if (PlayerJoined != null)
            {
                PlayerJoined.Invoke(ref player);
            }
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_JOINED, player.ToString()));
        }

        private void firePlayerLeft(ref Player player)
        {
            if (PlayerLeft != null)
            {
                PlayerLeft.Invoke(ref player);
            }
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_LEFT, player.ToString()));
        }

    }
}
