using System;

namespace Server
{
    public struct ServerCodes
    {
        // Server
        public const string SERVER_CODE_KICKED = "KI";
        public const string SERVER_CODE_SHUTDOWN = "SD";
        public const string SERVER_CODE_ERROR = "ER";
        public const string SERVER_CODE_CLIENT_DC = "DC";
        public const string SERVER_CODE_CLIENT_CARD = "CC";
        public const string SERVER_CODE_START = "ST";
        public const string SERVER_CODE_PAUSE = "PS";
        public const string SERVER_CODE_RESUME = "RS";
        public const string SERVER_CODE_STOP = "SO";
        public const string SERVER_NO_CODE = "NC";
        public const string SERVER_CODE_WIN = "WI";
        public const string SERVER_CODE_ACK="AK";
        public const string SERVER_CODE_NACK = "NK";
        public const string SERVER_CODE_NUMBER = "NB";
        public const string SERVER_CODE_READYP = "RP";

        // Client
        // 76(d) 73(d)
        public const string CLIENT_CODE_LOGIN = "LI";
        public const string CLIENT_CODE_PICTURE = "PI";
        public const string CLIENT_CODE_END_FILE = "EF";
        public const string CLIENT_ADMIN = "AD";
        public const string CLIENT_CODE_BINGO = "BI";

        // Admin
        public const string ADMIN_CODE_GENERATECARDS = "AGC";

    }
}