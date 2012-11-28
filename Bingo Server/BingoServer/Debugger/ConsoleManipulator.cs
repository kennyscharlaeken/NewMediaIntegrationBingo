using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoServer.Debugger
{
    public class ConsoleManipulator
    {

        public static void InterpretAction(string action)
        {
            Type console = GameConsole.Singleton.GetType();
            char splitter = '+'; 
            action = action.ToLower();
            string method = action.Replace(' ', splitter);
            string[] result = method.Split(splitter);
            try
            {
                object[] para = new object[result.Count()-1];
                int ri = 1;
                for (int i = 0; i < result.Count() - 1; i++)
                {
                    para[i] = result[ri];
                    ri++;
                }
                console.GetMethod(result[0]).Invoke(GameConsole.Singleton,para);
            }
            catch (Exception ex)
            {
                console.GetMethod("echo").Invoke(GameConsole.Singleton,new Object[]{String.Format("Invalid command '{0}'",action)});
            }
            
        }

    }
}
