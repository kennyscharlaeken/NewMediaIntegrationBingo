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
            Server.Singleton.sendBingoNumber(number);
            fireNewBingoNumber(number);
        }
       
        public void startGame()
        {
            sendMessageToPlayers(ServerCodes.SERVER_CODE_START);
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
                sendBingoCardsToPlayers();
            }
        }

        public void readyPlayers()
        {
            foreach (ClientHandler cl in _clients)cl.PlayerIsReady += cl_PlayerIsReady;
            //sendMessageToPlayers(ServerCodes.SERVER_CODE_READYP);
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
                _plready = 0;
            }
        }

        public bool playerWin(Player p)
        {
            if (IsGameStarted)
            {
                if (GameWinner == null)
                {
                    GameWinner = p;
                    //Serialize Player !!
                    sendMessageToPlayers(ServerCodes.SERVER_CODE_WIN+Helper.convertImgToString(p.Image));
                    firePlayerWon(p);
                    Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_PLAYER_WON, p));
                    return true;
                }
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
            _drawtimer =  new Timer(new TimerCallback(drawANumber), null, 1000, 10000);
        }
        private void stopDrawinTimer()
        {
            _drawtimer.Dispose();
        }

        public void sendBingoNumber(int number)
        {
            string msg = ServerCodes.SERVER_CODE_NUMBER + number.ToString();
            sendMessageToPlayers(msg);
            fireBingoNumberSend(number);
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_NUMBER_SEND, number, _players.Count()));
        }

    }

}
