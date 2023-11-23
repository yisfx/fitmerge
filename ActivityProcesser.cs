using Dynastream.Fit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitDemo
{
    public static class ActivityProcesser
    {
        public static void Event(object sender, MesgEventArgs e)
        {
            Console.WriteLine($"------------------{e.mesg.Name}---------------------");
            ActivityProcess(e.mesg);
        }

        private static void ActivityProcess(Mesg mesg)
        {
            var fieldList = mesg.Fields;

            foreach (var field in fieldList)
            {
                switch (field.Name)
                {
                    case "Timestamp":
                        Console.WriteLine($"时间:{field.GetValue()} {field.Units}");
                        break;
                    case "TotalTimerTime":
                        Console.WriteLine($"TotalTimerTime:总时长:{field.GetValue()} {field.Units}");
                        break;
                    case "NumSessions":
                        Console.WriteLine($"圈数:{field.GetValue()} {field.Units}");
                        break;
                    case "Type":
                        Console.WriteLine($"类型:{field.GetValue()} {field.Units}");
                        break;
                    case "Event":
                        Console.WriteLine($"事件:{field.GetValue()} {field.Units}");
                        break;
                    case "EventType":
                        Console.WriteLine($"事件类型:{field.GetValue()} {field.Units}");
                        break;
                    case "LocalTimestamp":
                        Console.WriteLine($"本地时间戳:{field.GetValue()} {field.Units}");
                        break;
                    case "EventGroup":
                        Console.WriteLine($"事件组:{field.GetValue()} {field.Units}");
                        break;
                    default:
                        Console.WriteLine($"{field.Name}:{field.GetValue()} {field.Units}");
                        break;
                }

            }
        }
    }
}
