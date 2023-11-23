using Dynastream.Fit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitDemo
{

    public class FieldInfo
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Units { get; set; }
        public int Index { get; set; }
        public bool IsHide { get; set; }
    }
    public class RecordInfo
    {
        public FieldInfo Speed { get; set; }
        public FieldInfo Distance { get; set; }
        public FieldInfo Temperature { get; set; }
        public FieldInfo HeartRate { get; set; }
        public FieldInfo Cadence { get; set; }
        public FieldInfo PositionLat { get; set; }
        public FieldInfo PositionLong { get; set; }
        public FieldInfo Altitude { get; set; }
        public FieldInfo Grade { get; set; }
        public FieldInfo EnhancedSpeed { get; set; }
    }

    public static class RecordProcesser
    {
        public static void Event(object sender, MesgEventArgs e)
        {
            ProcessRecord(e.mesg);
        }


        public static void ProcessRecord(Mesg record)
        {
            var infoList = new List<FieldInfo>();
            foreach (var field in record.Fields)
            {
                var value = field.GetValue();
                switch (field.Name)
                {
                    case "Speed":
                        infoList.Add(new FieldInfo { Index = 0, Value = Tools.ConvertToKmPerHour(value), Name = "瞬时速度", Units = "km/h" });
                        break;
                    case "Distance":
                        infoList.Add(new FieldInfo { Value = Math.Round(Convert.ToDouble(value) / 1000, 2), Name = "里程", Units = "km" });
                        break;
                    case "Temperature":
                        infoList.Add(new FieldInfo { Value = value, Name = "温度", Units = field.Units });
                        break;
                    case "HeartRate":
                        infoList.Add(new FieldInfo { Value = value, Name = "心率", Units = field.Units });
                        break;
                    case "Cadence":
                        infoList.Add(new FieldInfo { Value = value, Name = "踏频", Units = field.Units });
                        break;
                    case "PositionLat":
                        infoList.Add(new FieldInfo { IsHide = true, Value = value, Name = "纬度", Units = field.Units });
                        break;
                    case "PositionLong":
                        infoList.Add(new FieldInfo { IsHide = true, Value = value, Name = "经度", Units = field.Units });
                        break;
                    case "Altitude":
                        infoList.Add(new FieldInfo { Value = value, Name = "海拔", Units = field.Units });
                        break;
                    case "Grade":
                        infoList.Add(new FieldInfo { Value = value, Name = "坡度", Units = field.Units });
                        break;
                    case "EnhancedSpeed":
                        infoList.Add(new FieldInfo { Value = Tools.ConvertToKmPerHour(value), Name = "Enhanced速度", Units = "km/h" });
                        break;
                    case "EnhancedAltitude":
                        infoList.Add(new FieldInfo { Value = value, Name = "Enhanced海拔", Units = field.Units });
                        break;
                    case "Timestamp":

                        var tt = new TimeSpan(Convert.ToInt64(value + "00"));

                        infoList.Add(new FieldInfo { Index = -1, Value = tt, Name = "时间", Units = field.Units });
                        break;
                    default:
                        Console.WriteLine($"{field.Name}: {value}");
                        break;
                }
            }
            ///打印info的内容
            var enSpeed = infoList.FirstOrDefault(i => i.Name.Equals("Enhanced速度"));
            var speed = infoList.FirstOrDefault(i => i.Name.Equals("瞬时速度"));
            var altitude = infoList.FirstOrDefault(i => i.Name.Equals("海拔"));
            var enAltitube = infoList.FirstOrDefault(i => i.Name.Equals("Enhanced海拔"));

            Console.WriteLine(string.Join("\t", infoList.Where(i => !i.IsHide).OrderBy(i => i.Index).Select(x => $"{x.Name}:{Tools.FormateNumber(x.Value)}{x.Units}\t")));
        }



    }
}
