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
        private int _lastnumber = 0;

        private List<string> _numbers = new List<string>();
        public List<string> Numbers { get { return _numbers; } set { _numbers = value; } }
     
        public int pickRandomNumber()
        {
            setNumbers();
            int rindex = _rand.Next(Numbers.Count()-1);
            int number=0;
            int.TryParse(Numbers[rindex],out number);
            if (number != _lastnumber)
            {
                _lastnumber = number;
                return number;
            }
            else pickRandomNumber();
            return 0;
        }

        private void setNumbers()
        {
            if (Numbers.Count() == 0) Numbers = BingoCards.Singleton.UniqueNumbers;
        }

    }
}
