
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ICRC.Model
{
    public class ProgressHub : Microsoft.AspNet.SignalR.Hub
    {
        public string msg = "Initializing and Preparing...";
        public static double count = 0.1;
        public static bool loop= true;
        public static bool error = false;
        public static string errormsg = String.Empty;

        public static void starthub()
        {
            loop = true;
            ProgressHub hb = new ProgressHub();
            hb.CallLongOperation();            
        }

        public void CallLongOperation()
        {
            while (loop)
            {
                Thread.Sleep(100);
                if (count > 0)
                {
                    Clients.Caller.sendMessage(string.Format
                          (String.Format("{0:0.00}", count)));
                }
                if (error == true)
                {
                    Clients.Caller.sendMessage(errormsg.ToString());
                    error=false;
                    loop = false;
                }
                if (count == 100)
                    loop = false;
            }

            //string property1 = MySession.Current.perncetage;
            //// delay the process to see things clearly
            //Thread.Sleep(100);

                //if (x == 20)
                //    msg = "Loading Application Settings...";

                //else if (x == 40)
                //    msg = "Applying Application Settings...";

                //else if (x == 60)
                //    msg = "Loading User Settings...";

                //else if (x == 80)
                //    msg = "Applying User Settings...";

                //else if (x == 100)
                //    msg = "Process Completed!...";

                // call client-side SendMethod method
                //Clients.Caller.sendMessage(string.Format
                //        ("sdfsf"));
            
        }
    }
}