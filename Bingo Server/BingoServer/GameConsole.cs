using Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BingoServer
{
    public class GameConsole
    {
        private static GameConsole _singleton = null;
        public static GameConsole Singleton
        {
            get
            {
                if (_singleton == null) _singleton = new GameConsole();
                return _singleton;
            }
        }

        private Server.Server _server = Server.Server.Singleton;
        private Server.ServerBeacon _beacon = Server.ServerBeacon.Singleton;
        private Gameplay.BingoCards _cards = Gameplay.BingoCards.Singleton;

        public Debugger.DebugConsole DebugConsoleDisplay { get; set; }

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
                default:
                    echo(String.Format("Parameter '{0}' not recognized",what));
                    break;
            }
        }

        public void flush(string what)
        {
            switch (what)
            {
                case "memcards":
                    _cards.flushMemoryCards();
                    DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO,"Flushed memory cards",true);
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
                    _server.sendBingoCardsToPlayers();
                    DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, "Cards has been sent",true);
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
            DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, "Available commands:"+'\n'+"start (beacon/server), stop (beacon/server), flush (memcards), generatecards, cls, exit, help");
        }

        public void echo(string text)
        {
            DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, text);
        }

        

        public void generatecards()
        {
            DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, String.Format("Generating cards for {0} player(s)...", _server.Players.Count()),true);
            foreach (Player player in _server.Players)
            {
                player.BingoCards = BingoCards.Singleton.scramblePlayerCards();
                DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, String.Format("- {0} cards have been generated for player '{1}'", player.BingoCards.Count(), player.Ip.ToString()),true);
            }
        }

    }
}
