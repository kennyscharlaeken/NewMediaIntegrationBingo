using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameplay
{
    public sealed class BingoCards
    {
        // Private Fields
        private static BingoCards _singleton=null;
        private static string _keyword = "BINGO";
        private static byte _max_rows = 5;
        private static byte _min_random_number = 1;
        private static byte _jump_random_number = 15;
        private static byte _max_cards = 3;
        private static byte _cap_random_number = _min_random_number;
        private static List<List<string>> _cards=new List<List<string>>();

        // Properties
        public BingoCards Singleton 
        {
            get
            {
                if (_singleton == null) _singleton = new BingoCards();
                return _singleton;
            }
        }

        // Ctor(s)
        private BingoCards()
        { }
        
        // Private Methods      
        private List<string> scrambleACard(string keyword)
        {
            Random rnd = new Random();
            char[] col = keyword.ToUpper().ToCharArray();
            List<string> card = new List<string>();

            for (byte ci = 0; ci < col.Length; ci++)
            {
                for (byte ri = 0; ri < _max_rows; ri++)
                {
                    string id = getRandomID(col[ci], card, rnd,_cap_random_number);
                    card.Add(id);
                    _cap_random_number += _jump_random_number;
                }                             
            }
            return card;
        }

        private string getRandomID(char p, List<string> card, Random rnd,byte caprnd)
        {
            byte rndnumber = (byte)rnd.Next(caprnd, caprnd+_jump_random_number);
            string id = p + rndnumber.ToString();
            if (!card.Contains(id)) return id; else return getRandomID(p, card, rnd,caprnd);
        }

        private bool isUniqueCard(List<string> card)
        {
            return _cards.Contains(card);
        }

        // Public Methods
        public List<List<string>> scrambleAllCards(byte players)
        {
            for (byte p = 0; p < players; p++)
			{
			   for (byte c = 0; c < _max_cards; c++)
               {
                    List<string> card = getUniqueCard();             
               }
			}          
            return _cards;
        }



        private List<string> getUniqueCard()
        {
 	        throw new NotImplementedException();
        }

       



    }
}
