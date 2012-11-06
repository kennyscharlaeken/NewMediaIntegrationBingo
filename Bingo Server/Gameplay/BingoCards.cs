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
        private static byte _max_cols = 5;
        private static byte _max_rows = 5;
        private static byte _min_random_number = 1;
        private static byte _jump_random_number = 15;
        private static byte _max_cards = 3;
        private static byte _cap_random_number = _min_random_number;
        private static List<BingoCard> _cards = new List<BingoCard>();

        // Properties
        public static BingoCards Singleton 
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
        private BingoCard scrambleACard()
        {
            Random rnd = new Random();
            BingoCard card = new BingoCard();
            byte caprndnumber = _min_random_number;

            for(byte ci = 0; ci < _max_cols; ci++)
            {
                for (byte ri = 0; ri < _max_rows; ri++)
                {
                    string id = getRandomID(card, rnd, caprndnumber);
                    card.Add(id);
                }
                caprndnumber += _jump_random_number;            
            }
            return card;
        }

        private string getRandomID(BingoCard card, Random rnd, int caprnd)
        {
            int rndnumber = (byte)rnd.Next(caprnd, caprnd+_jump_random_number);
            string id = rndnumber.ToString();
            if (!card.Contains(id)) return id; else return getRandomID(card, rnd,caprnd);
        }

        private bool containsCard(BingoCard card)
        {
            if (_cards.Count == 0) return false;
            return _cards.Contains(card);
        }


        // Public Methods
        public List<BingoCard> scrambleAllCards(byte players)
        {
            for (byte p = 0; p < players; p++)
			{
			   for (byte c = 0; c < _max_cards; c++)
               {
                   BingoCard card = scrambleACard();
                   if (!containsCard(card)) _cards.Add(card);
               }
			}          
            return _cards;
        }



       

       



    }
}
