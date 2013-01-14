using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class Server
    {
        // MESSAGES
        private const string MSG_SERVER_START = "Bingo server started on {0}:{1}. Waiting on players...";
        private const string MSG_SERVER_STOP = "Bingo server stopped.";
        private const string MSG_SENDING_BINGOCARDS = "Sending bingocards to {0} player(s)...";
        private const string MSG_CARD_SEND = "Send cards to {0}";
        private const string MSG_NUMBER_SEND = "Send the number {0} to {1} player(s)";
        private const string MSG_PLAYER_WON = "Player {0} has bingo !";
    }
}
