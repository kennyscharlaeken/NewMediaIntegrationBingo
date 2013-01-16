using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for WinScreen.xaml
    /// </summary>
    public partial class WinScreen : UserControl
    {
        public WinScreen()
        {
            InitializeComponent();
        }

        public System.Drawing.Image PlayerWinImage 
        {
            set
            {
                try
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(value);
                    IntPtr hBitmap = bmp.GetHbitmap();
                    System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    imgPlayer.Source = WpfBitmap;
                    imgPlayer.Stretch = Stretch.Uniform;

                }
                catch (Exception)
                {
                   //something failed
                }
            }
        }

    }
}
