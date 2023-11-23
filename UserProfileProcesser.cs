using Dynastream.Fit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitDemo
{
    internal class UserProfileProcesser
    {
        /// <summary>
        /// 身高体重
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void Event(object sender, MesgEventArgs e)
        {
            Console.WriteLine($"------------------{e.mesg.Name}---------------------");
            UserProfileProcess(e.mesg);
        }

        private static void UserProfileProcess(Mesg mesg)
        {
            foreach (var field in mesg.Fields)
            {
                switch (field.Name)
                {
                    case "Height":
                        Console.WriteLine($"身高:{field.GetValue()} {field.Units}");
                        break;
                    case "Weight":
                        Console.WriteLine($"体重:{field.GetValue()} {field.Units}");
                        break;
                    case "PostionSetting":
                        Console.WriteLine($"Setting:{field.GetValue()}");
                        break;
                    default:
                        Console.WriteLine($"{field.Name}={field.GetValue()} {field.Units}");
                        break;
                }
            }
        }
    }
}
