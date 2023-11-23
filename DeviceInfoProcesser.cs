using Dynastream.Fit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitDemo
{
    internal class DeviceInfoProcesser
    {
        internal static void Event(object sender, MesgEventArgs e)
        {
            Console.WriteLine($"------------------{e.mesg.Name}---------------------");
            DeviceInfoProcess(e.mesg);
        }

        /// <summary>
        /// 外设信息
        /// 心率带，踏频计
        /// </summary>
        /// <param name="mesg"></param>
        private static void DeviceInfoProcess(Mesg mesg)
        {
            foreach (var field in mesg.Fields)
            {
                switch (field.Name)
                {
                    case "ProductName":

                        var encoder = new System.Text.ASCIIEncoding();
                        var name = encoder.GetString((byte[])field.GetValue());
                        Console.WriteLine($"设备名:{name}");
                        break;
                    case "DeviceType":
                        Console.WriteLine($"设备类型:{field.GetValue()} {field.Units}");
                        break;
                    case "AntDeviceNumber":
                        Console.WriteLine($"Ant设备号:{field.GetValue()} {field.Units}");
                        break;
                    case "AntTransmissionType":
                        Console.WriteLine($"Ant传输类型:{field.GetValue()} {field.Units}");
                        break;
                    case "SourceType":
                        Console.WriteLine($"源类型:{field.GetValue()} {field.Units}");
                        break;
                    case "DeviceIndex":
                        Console.WriteLine($"设备索引:{field.GetValue()} {field.Units}");
                        break;
                    case "SoftwareVersion":
                        Console.WriteLine($"软件版本:{field.GetValue()} {field.Units}");
                        break;
                    case "SerialNumber":
                        Console.WriteLine($"序列号:{field.GetValue()} {field.Units}");
                        break;
                    case "HardwareVersion":
                        Console.WriteLine($"硬件版本:{field.GetValue()} {field.Units}");
                        break;
                    default:
                        Console.WriteLine($"{field.Name}:{field.GetValue()} {field.Units}");
                        break;
                }
            }
        }
    }
}
