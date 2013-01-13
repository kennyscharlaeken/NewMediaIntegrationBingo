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

namespace BingoServer.Game
{
    /// <summary>
    /// Interaction logic for GameScreen.xaml
    /// </summary>
    public partial class GameScreen : UserControl
    {

        private List<Image> _numbersimages = new List<Image>();

        public GameScreen()
        {
            InitializeComponent();
        }
         
        public void setNewestBall(string p)
        {
            BitmapImage bmpimgc = null;
            BitmapImage bmpimgnew = new BitmapImage(new Uri(@"pack://application:,,,/BingoServer;component/Resources/" + p + ".png"));
            if (imgLastBall.Source != null)bmpimgc = imgLastBall.Source as BitmapImage;
            imgLastBall.Source = bmpimgnew;
            if (bmpimgc != null)
            {
                Image cimage = new Image() { Source = bmpimgc };
                cimage.Margin = new Thickness(0, 5, 5, 0);
                cimage.Stretch = Stretch.None;
                wpLastBalls.Children.Add(cimage);
                _numbersimages.Add(cimage);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Server.Server.Singleton.startGame();
        }

    }
}
