using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class ClientHandler
    {
        // MESSAGES
        private const int BUFFER_SIZE = 1024;
        private const string MSG_JOINED = "Player \"{0}\" joined.";
        private const string MSG_LEFT = "Player \"{0}\" has left.";
        private const string MSG_IMAGE = "Player \"{0}\" has changed his picture.";
        private const string MSG_REJECTED = "Player \"{0}\" has been rejected from the game";
        private const string MSG_OP = "Player \"{0}\" made OP";
        private const string MSG_CODE_REJECTED = "Rejected code \"{0}\" from \"{1}\" ";
        private const string MSG_BINGO = "Player \"{0}\" is the first who called BINGO";
    }
}
