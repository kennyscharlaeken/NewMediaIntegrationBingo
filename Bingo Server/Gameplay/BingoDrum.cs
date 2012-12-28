using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Gameplay
{
    public class BingoDrum
    {

        private static BingoDrum _singleton = null;
        public static BingoDrum Singleton
        {
            get
            {
                if (_singleton == null) _singleton = new BingoDrum();
                return _singleton;
            }
        }
        private BingoDrum() { }

        private Random _rand = new Random();
        private string _lastnumber = "";

        public List<string> Numbers { get; set; }
     
        private string pickRandomNumber()
        {
            setNumbers();
            int rindex = _rand.Next(Numbers.Count()-1);
            string number = Numbers[rindex];
            if (number != _lastnumber)
            {
                _lastnumber = number;
                return number;
            }
            else pickRandomNumber();
            return string.Empty;
        }

        private void setNumbers()
        {
            if (Numbers.Count() == 0) Numbers = BingoCards.Singleton.UniqueNumbers;
        }

    }
}
