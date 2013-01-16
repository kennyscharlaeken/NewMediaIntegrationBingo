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
            DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, "Available commands:" + '\n' + "start (beacon/server/game), stop (beacon/server/game), flush (memcards), send(cards),maximize,minimize, generate(cards), cls, exit, help");
        }

        public void echo(string text)
        {
            DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, text);
        }

        public void generate(string what)
        {
            switch (what)
            {
                case "cards":
                    if (_server.Players.Count() > 0)
                    {
                        _server.generateBingoCards();
                        DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.INFO, "Generated cards & send", true);
                    }
                    else DebugConsoleDisplay.addMessage(Server.DEBUGLEVELS.WARNING, "No players to generate cards", false);
                    break;
            };
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

      

    }
}
