using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BingoServer
{
    /// <summary>
    /// Interaction logic for BingoScreen.xaml
    /// </summary>
    public partial class BingoScreen : Window
    {

        private Game.IntroScreen _intro = null;
        private Game.GameScreen _game = null;

        private Server.Server _se = Server.Server.Singleton;

        private int _players = 0;

        private delegate void playerUpdateHandler(Gameplay.Player p, string msg);
        private delegate void bingoNumberHandler(string number);

        public BingoScreen()
        {
            InitializeComponent();

            //initIntroScreen();
            initGameScreen();

            initEvents();
            txtbVersion.Text = Helper.getVersion();
        }

        private void initEvents()
        {
            Server.ClientHandler.PlayerJoined += ClientHandler_PlayerJoined;
            Server.ClientHandler.PlayerLeft += ClientHandler_PlayerLeft;
            Server.Server.BingoNewNumber += Server_BingoNewNumber;
            _se.GameStarted += _se_GameStarted;            
        }

        private void Server_BingoNewNumber(int number)
        {
            _se.sendBingoNumber(number.ToString());
            Dispatcher.Invoke(new bingoNumberHandler(newBingoNumber),number.ToString());
        }

        private void initIntroScreen()
        {
            _intro = new Game.IntroScreen();
            _intro.Players = _players;
            // Animate first ?
            ccBingo.Content = _intro;            
        }

        private object getCurrentControl()
        {
            return ccBingo.Content;
        }

        private void initGameScreen()
        {
            _game = new Game.GameScreen();
            // Animate first ?
            ccBingo.Content = _game;
        }

        #region EVENTS

        // Redirect to dispatcher thread
        protected void ClientHandler_PlayerJoined(Gameplay.Player player, string msg = "")
        {
            Dispatcher.Invoke(new playerUpdateHandler(updatePlayerJoined),player,msg);
        }
        protected void ClientHandler_PlayerLeft(Gameplay.Player player, string msg = "")
        {
            Dispatcher.Invoke(new playerUpdateHandler(updatePlayerLeft), player, msg);
        }
        protected void _se_GameStarted()
        {
            Dispatcher.Invoke(new Action(gameStarted));
        }

        // Do the actual updating
        private void updatePlayerJoined(Gameplay.Player p,string msg)
        {
            if (getCurrentControl() == _intro)
            {
                _players++;
                _intro.Players = _players;
            }            
        }
        private void updatePlayerLeft(Gameplay.Player player, string msg = "")
        {
            if (getCurrentControl() == _intro)
            {
                _players--;
                _intro.Players = _players;
            }
        }
        private void gameStarted()
        {
            initGameScreen();
        }
        private void newBingoNumber(string number)
        {
            _game.setNewestBall(number);
        }
        #endregion

        

    }
}
