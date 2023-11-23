using Dynastream.Fit;
using System;

namespace FitDemo
{
    public static class FileIdProcesser
    {
        public static void Event(object sender, MesgEventArgs e)
        {
            Console.WriteLine($"------------------{e.mesg.Name}---------------------");
            Processor(e.mesg);
        }

        private static void Processor(Mesg mesg)
        {
            var fileId = mesg.Fields;

            foreach (var field in fileId)
            {
                switch (field.Name)
                {
                    case "SerialNumber":
                        Console.WriteLine($"序列码:{field.GetValue()}");
                        break;
                    case "TimeCreated":
                        var ts = Convert.ToInt64(Convert.ToString(field.GetValue()) + "000");
                        var t = new System.DateTime(ts);
                        Console.WriteLine($"创建时间:{t}  {field.Units}");
                        break;
                    case "ProductName":
                        Console.WriteLine($"产品名称:{Tools.ByteToString(field.GetValue())}");
                        break;
                    case "Manufacturer":
                        Console.WriteLine($"制造商:{field.GetValue()}");
                        break;
                    case "Product":
                        Console.WriteLine($"产品:{field.GetValue()}");
                        break;
                    case "Type":
                        Console.WriteLine($"文件类型:{field.GetValue()}");
                        break;
                    default:
                        Console.WriteLine($"{field.Name}: {field.GetValue()} {field.Units}");
                        break;
                }

            }
        }
    }
}
