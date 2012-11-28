using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoServer
{
    public class Helper
    {

        public static string timeStamp()
        {
            return DateTime.Now.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("be-BE"));
        }

    }
}
