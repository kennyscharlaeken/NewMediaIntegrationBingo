using Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BingoServer
{
    public partial class GameConsole
    {

        public void start(string what)
        {
            switch (what)
            {
                case "server":
                    _server.open();
                    break;
                case "beacon":
                    if (!_beacon.Active) ThreadPool.QueueUserWorkItem(new WaitCallback(_beacon.listen));
                    break;
                case "game":
                    _server.startGame();
                    break;
                case "up":
                    _server.open();
                    if (!_beacon.Active) ThreadPool.QueueUserWorkItem(new WaitCallback(_beacon.listen));
                    break;
                default:
                    echo(String.Format("Parameter '{0}' not recognized", what));
                    break;
            }
        }

        public void stop(string what)
        {
            switch (what)
            {
                case "server":
                    _server.close();
                    break;
                case "beacon":
                    _beacon.close();
                    break;
                case "game":
                    _server.stopGame();
                    break;
                case "down":
                    _server.close();
                    _beacon.close();
                    break;
                default:
                    echo(String.Format("Parameter '{0}' not recognized", what));
                    break;
            }
        }

        public void flush(string what)
        {
            switch (what)
            {
                case "memcards":
                    _cards.flushMemoryCards();
                    DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, "Flushed memory cards", true);
                    break;
                default:
                    echo(String.Format("Parameter '{0}' not recognized", what));
                    break;
            }
        }
        public void send(string what)
        {
            switch (what)
            {
                case "cards":
                    //_server.sendBingoCardsToPlayers();
                    DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, "Cards has been sent", true);
                    break;
                default:
                    echo(String.Format("Parameter '{0}' not recognized", what));
                    break;
            }
        }

        public void exit()
        {
            _server.close();
            _beacon.close();
            App.Current.Shutdown();
        }

        public void cls()
        {
            DebugConsoleDisplay.clearMessages();
        }

        public void help()
        {
            DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, "Available commands:" + '\n' + "start (beacon/server), stop (beacon/server), flush (memcards), send(cards),maximize,minimize, generatecards, cls, exit, help");
        }

        public void echo(string text)
        {
            DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, text);
        }

        public void generatecards()
        {
            //_server.Players.Add(new Player());
            if (_server.Players.Count() > 0)
            {
                _server.generateBingoCards();
                echo("Generated cards");
            }
        }

        public void maximize()
        {
            BingoServerMain.bingoscreen.WindowStyle = System.Windows.WindowStyle.None;
            BingoServerMain.bingoscreen.WindowState = System.Windows.WindowState.Maximized;
        }
        public void minimize()
        {
            BingoServerMain.bingoscreen.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            BingoServerMain.bingoscreen.WindowState = System.Windows.WindowState.Normal;
        }

        //public void generatecards()
        //{
        //    _server.Players.Add(new Player());
        //    if (_server.Players.Count() > 0)
        //    {
        //        DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, String.Format("Generating cards for {0} player(s)...", _server.Players.Count()), true);
        //        foreach (Player player in _server.Players)
        //        {
        //            player.BingoCards = BingoCards.Singleton.scramblePlayerCards();
        //            //DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, String.Format("- {0} cards have been generated for player '{1}'", player.BingoCards.Count(), player.Ip.ToString()), true);
        //        }
        //    }
        //    else DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, String.Format("No players to generate cards", _server.Players.Count()), true);
        //}

    }
}
