using Dynastream.Fit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitDemo
{
    public static class HrvProcesser
    {
        public static void Event(object sender, MesgEventArgs e)
        {
            throw new NotImplementedException();
            HrvProcess(e.mesg);
        }

        static void HrvProcess(Mesg mesg)
        {
            mesg.Fields.ToList().ForEach(field =>
            {
                var value = field.GetValue();
                switch (field.Name)
                {
                    case "Time":
                        Console.WriteLine($"Time:{value}");
                        break;
                    case "TimeFromLastHrv":
                        Console.WriteLine($"TimeFromLastHrv:{value}");
                        break;
                    default:
                        break;
                }
            });
        }
    }
}
