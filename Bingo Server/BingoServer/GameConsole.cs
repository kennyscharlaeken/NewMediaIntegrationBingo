using Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoServer
{
    public partial class GameConsole
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

       

    }
}
