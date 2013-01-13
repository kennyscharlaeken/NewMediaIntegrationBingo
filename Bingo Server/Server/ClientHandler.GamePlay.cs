using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class ClientHandler
    {

        // Resolve all player en register admin requests
        private void resolveMessage(string code, byte[] msg)
        {
            _code = code;
            switch (code)
            {
                case ServerCodes.CLIENT_CODE_LOGIN:
                    if (!IsPlayer) onLogin(); else rejectCode();
                    break;
                case ServerCodes.CLIENT_CODE_PICTURE:
                    _bufferoverflow = true;
                    _code = ServerCodes.CLIENT_CODE_PICTURE;
                    _overflowsize = int.Parse(Helper.convertToString(new byte[] { msg[2], msg[3], msg[4], msg[5] }));
                    writeToMemory(msg, 6);
                    break;
                case ServerCodes.SERVER_CODE_CLIENT_DC:
                    onDC();
                    break;
                case ServerCodes.CLIENT_ADMIN:
                    onAdmin(code,msg);
                    break;
                case ServerCodes.CLIENT_CODE_BINGO:
                    onBingo();
                    break;
                case ServerCodes.SERVER_CODE_READYP:
                    firePlayerReady();
                    break;
                default:
                    rejectCode();
                    break;
            }
        }

        // Resolve all Admin requests
        private void resolverAdminMessage(string code,byte[] msg)
        {
            if(Admin)
            {
                string admincode = Helper.convertToString(new byte[]{msg[0],msg[1]});
                switch (admincode)
                {
                    case ServerCodes.SERVER_CODE_PAUSE:
                        onPauseGame();
                     break;
                    case ServerCodes.SERVER_CODE_RESUME:
                        onResumeGame();
                        break;
                    case ServerCodes.SERVER_CODE_STOP:
                        onStopGame();
                        break;
                }
            }
        }

      
       
        private void rejectCode()
        {
            sendNAck();
            firePlayerCodeRejected(_player, _code);
        }


        #region RESOLVE_MESSAGES

        private void onDC()
        {
            _server.removePlayer(_player);
            disconnect();
            firePlayerLeft(_player);
        }
        private void onLogin()
        {
            _player.Ip = _ip.Address;
            if (_server.addPlayer(_player))
            {
                sendAck();
                IsPlayer = true;
                firePlayerJoined(_player);
            }
            else
            {
                IsPlayer = false;
                sendNAck();
                firePlayerRejected(_player);
            }
        }
        private void onAdmin(string code,byte[] msg)
        {
            if (Admin == true)
            {
                resolverAdminMessage(code,msg);
            }
            else
            {
                if (IsPlayer)
                {
                    Admin = true;
                    firePlayerOped(_player);
                    sendAck();
                }
                else rejectCode();
            }            
        }
       
        private void onBingo()
        {
            if (IsPlayer)
            {
                if (!_server.playerWin(_player)) rejectCode(); else firePlayerCalledBingo(_player);
            }
            else rejectCode();
        }

        private void onPauseGame()
        {
            _server.pauseGame();
        }

        private void onResumeGame()
        {
            _server.resumeGame();
        }

        private void onStopGame()
        {
            _server.stopGame();
        }


        #endregion


        

    }
}
