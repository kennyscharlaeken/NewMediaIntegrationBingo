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
        public Player GameWinner { get; set; }
        public bool IsGameStarted { get; set; }
        public bool IsGamePaused { get; set; }

        private int _plready = 0;

        private Timer _drawtimer = null;

        private static void drawANumber(object state)
        {
            int number = Gameplay.BingoDrum.Singleton.pickRandomNumber();
            fireNewBingoNumber(number);
        }
       
        public void startGame()
        {
            //sendMessageToPlayers(ServerCodes.SERVER_CODE_START);
            IsGameStarted = true;
            fireGameStarted();
            startDrawinTimer();
        }

        public void pauseGame()
        {
            sendMessageToPlayers(ServerCodes.SERVER_CODE_PAUSE);
            IsGamePaused = true;
            fireGamePaused();
            stopDrawinTimer();
        }

        public void resumeGame()
        {
            sendMessageToPlayers(ServerCodes.SERVER_CODE_RESUME);
            IsGamePaused = false;
            fireGameResumed();
            startDrawinTimer();
        }

        public void stopGame()
        {
            stopDrawinTimer();
            sendMessageToPlayers(ServerCodes.SERVER_CODE_STOP);
            IsGamePaused = false;
            IsGameStarted = false;
            fireGameStopped();            
        }

        public void generateBingoCards()
        {           
            if (_players.Count() > 0)
            {
                // Generate
                foreach (Player player in _players)
                {
                    player.BingoCards = BingoCards.Singleton.scramblePlayerCards();
                }
                // Send
                //sendBingoCardsToPlayers();
            }
        }

        public void readyPlayers()
        {
            foreach (ClientHandler cl in _clients)cl.PlayerIsReady += cl_PlayerIsReady;
            sendMessageToPlayers(ServerCodes.SERVER_CODE_READYP);
        }

        private void cl_PlayerIsReady()
        {
            _plready++;
            if (_plready >= _players.Count())
            {
                if (PlayersAreReady != null)
                {
                    foreach (ClientHandler cl in _clients)cl.PlayerIsReady -= cl_PlayerIsReady;
                    firePlayersAreReady();
                }
            }
        }

        public bool playerWin(Player p)
        {
            if (GameWinner == null)
            {
                GameWinner = p;
                List<byte> msg = new List<byte>();
                msg.AddRange(Helper.convertToBytes(ServerCodes.SERVER_CODE_WIN));
                // Serialize the player first;
                //msg.AddRange(Helper.convertToBytes(p));
                sendMessageToPlayers(msg.ToArray());
                return true;
            }
            return false;
        }

        public bool addPlayer(Player p)
        {
            if (!IsGameStarted)
            {
                _players.Add(p);
                return true;
            }
            return false;
        }

        public void removePlayer(Player p)
        {
            _players.Remove(p);
        }

        private void sendBingoCardsToPlayers()
        {
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO,String.Format(MSG_SENDING_BINGOCARDS,_players.Count()));
            foreach (Player pl in _players)
            {
                if (pl.BingoCards != null)
                {
                    // Serialize the bingocards first (done) just convert to a string
                    string lists = ServerCodes.SERVER_CODE_CLIENT_CARD + serializeBingoCardsList(pl.BingoCards);
                    sendMessageToPlayer(pl, lists);
                }// else throw an error or fill in the cards ?
                Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_CARD_SEND,pl.Ip.ToString()));
            }
            fireBingoCardsSend();
        }

        private string serializeBingoCardsList(List<BingoCard> plcards)
        {
            string lists = string.Empty;
            foreach (BingoCard card in plcards)
            {
                lists += Helper.convertNumberList(card);
                lists += ";";
            }
            return lists;
        }

        private void startDrawinTimer()
        {
            _drawtimer =  new Timer(new TimerCallback(drawANumber), null, 1000, 1000);
        }
        private void stopDrawinTimer()
        {
            _drawtimer.Dispose();
        }

        public void sendBingoNumber(string number)
        {
            List<byte> msg = new List<byte>();
            msg.AddRange(Helper.convertToBytes(ServerCodes.SERVER_CODE_NUMBER));
            msg.AddRange(Helper.convertToBytes(number));
            sendMessageToPlayers(msg);
            fireBingoNumberSend(number);
        }

    }

}
