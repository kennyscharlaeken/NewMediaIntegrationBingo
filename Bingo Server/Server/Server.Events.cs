﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class Server
    {

        public event Action PlayersAreReady;
        public event Action GameStarted;
        public event Action GamePaused;
        public event Action GameResumed;
        public event Action GameStopped;
        public event Action BingoCardsSend;

        public delegate void numberSendHandler(string number);
        public event numberSendHandler BingoNumberSend;

        private void firePlayersAreReady()
        {
            if (PlayersAreReady != null)
            {
                PlayersAreReady.Invoke();
            }
        }

        private void fireGameStarted()
        {
            if (GameStarted != null)
            {
                GameStarted.Invoke();
            }
        }

        private void fireGamePaused()
        {
            if (GamePaused != null)
            {
                GamePaused.Invoke();
            }
        }

        private void fireGameResumed()
        {
            if (GameResumed != null)
            {
                GameResumed.Invoke();
            }
        }

        private void fireGameStopped()
        {
            if (GameStopped != null)
            {
                GameStopped.Invoke();
            }
        }

        private void fireBingoCardsSend()
        {
            if (BingoCardsSend!=null)
            {
                BingoCardsSend.Invoke();
            }
        }

        private void fireBingoNumberSend(string number)
        {
            if (BingoNumberSend !=null)
            {
                BingoNumberSend.Invoke(number);
            }
            Debug.Singleton.sendDebugMessage(DEBUGLEVELS.INFO, String.Format(MSG_NUMBER_SEND, number, _players.Count()));
        }

    }
}