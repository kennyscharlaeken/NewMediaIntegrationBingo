﻿using System;

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
        public const string SERVER_CODE_STOP = "ST";
        public const string SERVER_NO_CODE = "NC";

        // Client
        // 76(d) 73(d)
        public const string CLIENT_CODE_LOGIN = "LI";
        public const string CLIENT_CODE_PICTURE = "PI";
        public const string CLIENT_CODE_END_FILE = "EF";
        public const string CLIENT_ADMIN = "AD";

    }
}