using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace BingoServer.Game.Elements
{
	/// <summary>
	/// Interaction logic for BingoNumbersDisplay.xaml
	/// </summary>
	public partial class BingoNumbersDisplay : UserControl
	{
        private List<Image> _numbers = new List<Image>();
        private Storyboard _sb = null;

		public BingoNumbersDisplay()
		{
			this.InitializeComponent();
		}

        public void startMoveAnimation()
        {
            DoubleAnimation doa = new DoubleAnimation();
            doa.From = 0;
            doa.To = -spNumbers.ActualWidth;


            Storyboard sb = new Storyboard();
            sb.Children.Add(doa);

            // Nog eens zoeken hoe animaties in code werken
   
            sb.Begin();

            // _sb = FindResource("CurrentImageMove") as Storyboard;
            // if (_sb != null)
            //{
            //    _sb.Completed += sb_Completed;
            //    _sb.Begin();
            //}            
        }

        void sb_Completed(object sender, EventArgs e)
        {
            Image newimage = new Image(){Stretch=System.Windows.Media.Stretch.None, Source = imgCurrent.Source};
            _numbers.Add(newimage);
            spNumbers.Children.Add(newimage);
            imgCurrent.Source = null;
            _sb.Seek(new TimeSpan(0));
            _sb.Stop();
        }
	}
}