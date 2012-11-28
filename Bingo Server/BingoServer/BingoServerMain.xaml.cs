using Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BingoServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BingoServerMain : Window
    {

        private GameConsole gs = GameConsole.Singleton;

        public BingoServerMain()
        {
            InitializeComponent();

            gs.DebugConsoleDisplay = ConsoleDebug;
            txtConsole.Focus();
            initDisplay();
        }

        private void initDisplay()
        {
            string w = "====================================================="+'\n'+
                "Welcome to bingo server v1.0" + '\n' + '\n' +
                "Type help for more commands"+'\n'+
                "====================================================="+'\n';
            ConsoleDebug.addMessage(Server.DEBUGLEVELS.INFO, w);
        }

        private void btnConsole_Click(object sender, RoutedEventArgs e)
        {
            interpretAction();
        }

        private void interpretAction()
        {
            Debugger.ConsoleManipulator.InterpretAction(txtConsole.Text);
            txtConsole.Text = "";
            txtConsole.Focus();
        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                interpretAction();
            }
        }

    }
}
