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
        public delegate void playerUpdateHandler(Player player,string msg="");
        public static event playerUpdateHandler ImageUpdated;
		public static event playerUpdateHandler PlayerJoined;
        public static event playerUpdateHandler PlayerLeft;
        public static event playerUpdateHandler PlayerRejected;
        public static event playerUpdateHandler PlayerOped;
        public static event playerUpdateHandler PlayerCodeRejected;
        public static event playerUpdateHandler PlayerCalledBingo;
        public static event playerUpdateHandler PlayerDropped;

        public event Action PlayerIsReady;

        private void fireImageUpdated(Player player)
        {
            if (ImageUpdated != null)
            {
                ImageUpdated.Invoke(player);
            }
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_IMAGE, player.ToString()));
        }

        private void firePlayerJoined(Player player)
        {
            if (PlayerJoined != null)
            {
                PlayerJoined.Invoke(player);
            }
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_JOINED, player.ToString()));
        }

        private void firePlayerRejected(Player player)
        {
            if (PlayerRejected != null)
            {
                PlayerRejected(player);
            }
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.WARNING, String.Format(MSG_REJECTED, player.ToString()));
        }

        private void firePlayerLeft(Player player)
        {
            if (PlayerLeft != null)
            {
                PlayerLeft.Invoke(player);
            }
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_LEFT, player.ToString()));
        }

        private void firePlayerOped(Player player)
        {
            if (PlayerOped != null)
            {
                PlayerOped.Invoke(player);
            }
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_OP, player.ToString()));
        }

        private void firePlayerCodeRejected(Player player,string code)
        {
            if (PlayerCodeRejected != null)
            {
                PlayerCodeRejected.Invoke(player);
            }
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_CODE_REJECTED, code, player.ToString()));
        }

        private void firePlayerCalledBingo(Player player)
        {
            if (PlayerCalledBingo != null)
            {
                PlayerCalledBingo.Invoke(player);
            }
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_BINGO, player.ToString()));
        }

        private void firePlayerDropped(Player player)
        {
            if(PlayerDropped !=null)
            {
                PlayerDropped.Invoke(player,"player dropped");
            }
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.ERROR, String.Format(MSG_CONDC, _player.Ip));
        }

        private void firePlayerReady()
        {
            if (PlayerIsReady != null)
            {
                PlayerIsReady.Invoke();
            }
        }

    }
}
