using Dynastream.Fit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitDemo
{
    public static class ClimbProProcesser
    {
        internal static void Event(object sender, MesgEventArgs e)
        {
            throw new NotImplementedException();
            ClimbProProcess(e.mesg);
        }

        private static void ClimbProProcess(Mesg mesg)
        {
            var fieldList = mesg.Fields;
            foreach (var field in fieldList)
            {
                switch (field.Name)
                {
                    default:
                        Console.WriteLine($"{field.Name}:{field.GetValue()}");
                        break;
                }
            }
        }
    }
}
