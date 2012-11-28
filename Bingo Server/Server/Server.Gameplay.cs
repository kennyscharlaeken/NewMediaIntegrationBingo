using Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public partial class Server
    {

        public void startGame()
        {
            sendMessageToPlayers(ServerCodes.SERVER_CODE_START);
        }

        public void pauseGame()
        {
            sendMessageToPlayers(ServerCodes.SERVER_CODE_PAUSE);
        }

        public void stopGame()
        {
            sendMessageToPlayers(ServerCodes.SERVER_CODE_STOP);
        }

        public void addPlayer(Player p)
        {
            _players.Add(p);
        }
        public void removePlayer(Player p)
        {
            _players.Remove(p);
        }

        public void close()
        {
            sendMessageToPlayers(ServerCodes.SERVER_CODE_SHUTDOWN);
            foreach (ClientHandler cl in _clients)
            {
                cl.disconnect();
            }
            closeServer();
        }

        public void sendBingoCardsToPlayers()
        {
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO,String.Format(MSG_SENDING_BINGOCARDS,_players.Count()));
            foreach (Player pl in _players)
            {
                if(pl.BingoCards!=null)sendMessageToPlayer(pl,pl.BingoCards);
                Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_CARD_SEND,pl.Ip.ToString()));
            }
        }       

    }

}
