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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace BingoServer.Debugger
{
    /// <summary>
    /// Interaction logic for DebugConsole.xaml
    /// </summary>
    public partial class DebugConsole : UserControl
    {

        private ObservableCollection<string> _message = new ObservableCollection<string>();
        public ObservableCollection<string> DebugMessages { get{return _message;} }

        public DebugConsole()
        {
            InitializeComponent();
            lbConsole.ItemsSource = DebugMessages;
            initEvents();
        }

        private void initEvents()
        {
            Server.Debug.Singleton.DebugMessage += Singleton_DebugMessage;
        }

        void Singleton_DebugMessage(Server.DEBUGLEVELS c, string m)
        {
            Dispatcher.Invoke(new messageHandler(addMessage), c,m,true);
        }

        private delegate void messageHandler(Server.DEBUGLEVELS c,string m,bool t=false);
        public void addMessage(Server.DEBUGLEVELS c,string m,bool timedisplay=false)
        {
            string time = (timedisplay ? Helper.timeStamp() + " " : string.Empty); 
            _message.Add(time+m);
        }

        public void clearMessages()
        {
            _message.Clear();
        }


    }
}
