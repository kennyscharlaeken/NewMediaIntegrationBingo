using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        private Timer _wintimer = new Timer(10000);

        private delegate void playerUpdateHandler(Gameplay.Player p, string msg);
        private delegate void bingoNumberHandler(int number);

        public BingoScreen()
        {
            InitializeComponent();

            initIntroScreen();
            //initGameScreen();
            //initWinScreen(null);

            initEvents();
            txtbVersion.Text = Helper.getVersion();
        }

        

        private void initEvents()
        {
            Server.ClientHandler.PlayerJoined += ClientHandler_PlayerJoined;
            Server.ClientHandler.PlayerLeft += ClientHandler_PlayerLeft;
            Server.Server.BingoNewNumber += Server_BingoNewNumber;
            _se.PlayerWon += _se_PlayerWon;
            _se.GameStarted += _se_GameStarted;
            _wintimer.Elapsed += _wintimer_Elapsed;
        }

        private void initIntroScreen()
        {
            _intro = new Game.IntroScreen();
            _intro.Players = _players;
            // Animate first ?
            ccBingo.Content = _intro;            
        }
        private void initGameScreen()
        {
            _game = new Game.GameScreen();
            // Animate first ?
            ccBingo.Content = _game;
        }
        private void initWinScreen(System.Drawing.Image img)
        {
            if (img == null) ccBingo.Content = new Game.WinScreen();
            else ccBingo.Content = new Game.WinScreen() { PlayerWinImage = img };
        }

        private object getCurrentControl()
        {
            return ccBingo.Content;
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
        protected void Server_BingoNewNumber(int number)
        {
            Dispatcher.Invoke(new bingoNumberHandler(updateBingoNumber), number);
        }
        protected void _se_PlayerWon(Gameplay.Player p)
        {
            Dispatcher.Invoke(new playerUpdateHandler(updatePlayerWon), p, string.Empty);
        }
        protected void _wintimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _wintimer.Stop();
            _game.resset();
            ccBingo.Content = _game;
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
        private void updatePlayerWon(Gameplay.Player player, string msg = "")
        {
            if (player != null)
            {
                initWinScreen(player.Image);
                _wintimer.Start();
            }
        }
        private void gameStarted()
        {
            initGameScreen();
        }
        private void updateBingoNumber(int number)
        {
            _game.setNewestBall(number);
        }
        #endregion

    }
}
