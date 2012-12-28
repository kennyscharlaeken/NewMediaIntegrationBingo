using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameplay
{
    [Serializable()]
    public class BingoCard : List<string>
    {
        private const double CARD_SAME_RATIO = 0.8;

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                BingoCard temp = obj as BingoCard;
                if (temp != null)
                {
                    return check(temp);
                }
            }
            return false;
        }

        private bool check(BingoCard c)
        {
            int matched = 0;
            if (c.Count != this.Count) return false;
            for (int i = 0; i < c.Count; i++)
            {
                if (this[i].Equals(c[i])) matched++;
            }
            if (matched==0) return false;
            double ratio = (double)(matched) / (double)(Count);
            if (ratio <= CARD_SAME_RATIO)return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
