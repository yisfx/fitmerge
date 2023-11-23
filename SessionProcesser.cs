using Dynastream.Fit;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitDemo
{
    public static class SessionProcesser
    {
        public static void Event(object sender, MesgEventArgs e)
        {
            Console.WriteLine($"------------------{e.mesg.Name}---------------------");
            SessionProcess(e.mesg);
        }

        static float CalcAvgGrade(FiteFileModel baseFit, FiteFileModel additionFit)
        {
            var baseRecord = baseFit.RecordMsg.Where(r => r.Fields.Any(f => f.Name.Equals("Grade", StringComparison.OrdinalIgnoreCase)));

            var additionRecord = additionFit.RecordMsg.Where(r => r.Fields.Any(f => f.Name.Equals("Grade", StringComparison.OrdinalIgnoreCase)));


            var avg = (baseRecord.Select(r => r.Fields.First(f => f.Name.Equals("Grade")).GetValue()).Sum(f => (float)f) +
                additionRecord.Select(r => r.Fields.First(f => f.Name.Equals("Grade")).GetValue()).Sum(f => (float)f))
                / (baseRecord.Count() + additionRecord.Count());
            return avg;
        }

        static float CalcAvgHeartRate(FiteFileModel baseFit, FiteFileModel additionFit)
        {
            var baseRecord = baseFit.RecordMsg.Where(r => r.Fields.Any(f => f.Name.Equals("HeartRate", StringComparison.OrdinalIgnoreCase)));

            var additionRecord = additionFit.RecordMsg.Where(r => r.Fields.Any(f => f.Name.Equals("HeartRate", StringComparison.OrdinalIgnoreCase)));


            var avg = (baseRecord.Select(r => r.Fields.First(f => f.Name.Equals("HeartRate")).GetValue()).Sum(f => (byte)f) +
                additionRecord.Select(r => r.Fields.First(f => f.Name.Equals("HeartRate")).GetValue()).Sum(f => (byte)f))
                / (baseRecord.Count() + additionRecord.Count());



            return avg;
        }

        static float CalcAvgCadence(FiteFileModel baseFit, FiteFileModel additionFit)
        {
            var baseRecord = baseFit.RecordMsg.Where(r => r.Fields.Any(f => f.Name.Equals("Cadence", StringComparison.OrdinalIgnoreCase)));

            var additionRecord = additionFit.RecordMsg.Where(r => r.Fields.Any(f => f.Name.Equals("Cadence", StringComparison.OrdinalIgnoreCase)));


            var avg = (baseRecord.Select(r => r.Fields.First(f => f.Name.Equals("Cadence")).GetValue()).Sum(f => (byte)f) +
                additionRecord.Select(r => r.Fields.First(f => f.Name.Equals("Cadence")).GetValue()).Sum(f => (byte)f))
                / (baseRecord.Count() + additionRecord.Count());

            return avg;
        }


        internal static List<Mesg> Calc(FiteFileModel baseFit, FiteFileModel additionFit)
        {
            var result = new List<Mesg>();
            var baseF = baseFit.SessionMsg.FirstOrDefault();

            var addition = additionFit.SessionMsg.FirstOrDefault();
            var elapsedTime = baseF.GetFieldValue<float>("TotalElapsedTime") + addition.GetFieldValue<float>("TotalElapsedTime");
            var totalTimerTime = baseF.GetFieldValue<float>("TotalTimerTime") + addition.GetFieldValue<float>("TotalTimerTime");
            var distance = baseF.GetFieldValue<float>("TotalDistance") + addition.GetFieldValue<float>("TotalDistance");


            var avgHeartRate = CalcAvgHeartRate(baseFit, additionFit);

            var avgCadence = CalcAvgCadence(baseFit, additionFit);

            var avgPower = (baseF.GetFieldValue<UInt16>("AvgPower") * baseF.GetFieldValue<float>("TotalTimerTime") +
                                              addition.GetFieldValue<UInt16>("AvgPower") * addition.GetFieldValue<float>("TotalTimerTime")) / totalTimerTime;


            var avgGrade = CalcAvgGrade(baseFit, additionFit);
            var avgAltitude = (baseFit.RecordMsg.Select(r => r.Fields.First(f => f.Name.Equals("Altitude")).GetValue()).Sum(f => (float)f) +
                               additionFit.RecordMsg.Select(r => r.Fields.First(f => f.Name.Equals("Altitude")).GetValue()).Sum(f => (float)f))
                               / (baseFit.RecordMsg.Count + additionFit.RecordMsg.Count);
            var avgTemperature = (baseFit.RecordMsg.Select(r => r.Fields.First(f => f.Name.Equals("Temperature")).GetValue()).Sum(f => (SByte)f) +
                                              additionFit.RecordMsg.Select(r => r.Fields.First(f => f.Name.Equals("Temperature")).GetValue()).Sum(f => (SByte)f))
                               / (baseFit.RecordMsg.Count + additionFit.RecordMsg.Count);


            foreach (var field in baseF.Fields)
            {
                var additionField = addition.Fields.FirstOrDefault(x => x.Name == field.Name);

                switch (field.Name)
                {
                    case "StartTime":
                        Console.WriteLine($"开始时间:{field.GetValue()}");
                        break;
                    case "TotalAscent":

                        var a = addition.GetFieldValue<UInt16>("TotalAscent") + baseF.GetFieldValue<UInt16>("TotalAscent");
                        baseF.SetFieldValue("TotalAscent", a);
                        break;
                    case "TotalDescent":
                        var b = addition.GetFieldValue<UInt16>("TotalDescent") + baseF.GetFieldValue<UInt16>("TotalDescent");
                        baseF.SetFieldValue("TotalDescent", b);
                        break;

                    case "MaxSpeed":
                        var c = Math.Max(addition.GetFieldValue<float>("MaxSpeed"), baseF.GetFieldValue<float>("MaxSpeed"));
                        baseF.SetFieldValue("MaxSpeed", c);
                        break;
                    case "MaxHeartRate":
                        var d = Math.Max(addition.GetFieldValue<byte>("MaxHeartRate"), baseF.GetFieldValue<byte>("MaxHeartRate"));
                        baseF.SetFieldValue("MaxHeartRate", d);
                        break;
                    case "MaxCadence":
                        var e = Math.Max(addition.GetFieldValue<byte>("MaxCadence"), baseF.GetFieldValue<byte>("MaxCadence"));
                        baseF.SetFieldValue("MaxCadence", e);
                        break;

                    case "MaxPower":
                        var f = Math.Max(addition.GetFieldValue<ushort>("MaxPower"), baseF.GetFieldValue<ushort>("MaxPower"));
                        baseF.SetFieldValue("MaxPower", f);
                        break;

                    case "MaxAltitude":
                        var g = Math.Max(addition.GetFieldValue<float>("MaxAltitude"), baseF.GetFieldValue<float>("MaxAltitude"));
                        baseF.SetFieldValue("MaxAltitude", g);
                        break;

                    case "MaxTemperature":
                        var h = Math.Max(addition.GetFieldValue<sbyte>("MaxTemperature"), baseF.GetFieldValue<sbyte>("MaxTemperature"));
                        baseF.SetFieldValue("MaxTemperature", h);
                        break;
                    case "TotalElapsedTime":
                        var i = addition.GetFieldValue<float>("TotalElapsedTime") + baseF.GetFieldValue<float>("TotalElapsedTime");
                        baseF.SetFieldValue("TotalElapsedTime", i);
                        break;
                    case "TotalTimerTime":
                        var j = addition.GetFieldValue<float>("TotalTimerTime") + baseF.GetFieldValue<float>("TotalTimerTime");
                        baseF.SetFieldValue("TotalTimerTime", j);
                        break;
                    case "TotalDistance":
                        var k = addition.GetFieldValue<float>("TotalDistance") + baseF.GetFieldValue<float>("TotalDistance");
                        baseF.SetFieldValue("TotalDistance", k);
                        break;
                    case "ThresholdPower":
                        var l = Math.Max(addition.GetFieldValue<ushort>("ThresholdPower"), baseF.GetFieldValue<ushort>("ThresholdPower"));
                        baseF.SetFieldValue("ThresholdPower", l);
                        break;
                    case "TotalWork":
                        var m = addition.GetFieldValue<uint>("TotalWork") + baseF.GetFieldValue<uint>("TotalWork");
                        baseF.SetFieldValue("TotalWork", m);
                        break;
                    case "TotalMovingTime":
                        var n = addition.GetFieldValue<float>("TotalMovingTime") + baseF.GetFieldValue<float>("TotalMovingTime");
                        baseF.SetFieldValue("TotalMovingTime", n);
                        break;
                    case "TotalCalories":
                        var o = addition.GetFieldValue<UInt16>("TotalCalories") + baseF.GetFieldValue<UInt16>("TotalCalories");
                        baseF.SetFieldValue("TotalCalories", o);
                        break;
                    case "EnhancedMaxSpeed":
                        var p = Math.Max(addition.GetFieldValue<float>("EnhancedMaxSpeed"), baseF.GetFieldValue<float>("EnhancedMaxSpeed"));
                        baseF.SetFieldValue("EnhancedMaxSpeed", p);
                        break;
                    case "EnhancedMaxAltitude":
                        var q = Math.Max(addition.GetFieldValue<float>("EnhancedMaxAltitude"), baseF.GetFieldValue<float>("EnhancedMaxAltitude"));
                        baseF.SetFieldValue("EnhancedMaxAltitude", q);
                        break;
                    case "AvgPower":
                        baseF.SetFieldValue("AvgPower", avgPower);
                        break;
                    case "AvgSpeed":
                        var r = distance / totalTimerTime;
                        baseF.SetFieldValue("AvgSpeed", r);
                        Console.WriteLine($"平均速度:{Tools.ConvertToKmPerHour(field.GetValue())} km/h");
                        break;
                    case "AvgHeartRate":
                        baseF.SetFieldValue("AvgHeartRate", avgHeartRate);
                        break;
                    case "AvgCadence":
                        baseF.SetFieldValue("AvgCadence", avgCadence);
                        break;
                    case "AvgAltitude":
                        baseF.SetFieldValue("AvgAltitude", avgAltitude);
                        break;
                    case "AvgGrade":
                        baseF.SetFieldValue("AvgGrade", avgGrade);
                        break;
                    case "AvgTemperature":
                        baseF.SetFieldValue("AvgTemperature", avgTemperature);
                        break;
                    case "EnhancedAvgSpeed":
                        var s = distance / totalTimerTime;
                        baseF.SetFieldValue("EnhancedAvgSpeed", s);
                        break;
                    case "EnhancedAvgAltitude":
                        baseF.SetFieldValue("EnhancedAvgAltitude", avgAltitude);
                        break;
                    default:
                        Console.WriteLine($"{field.Name}:{field.GetValue()}");
                        break;
                }
            }

            result.Add(baseF);
            return result;
        }

        private static void SessionProcess(Mesg mesg)
        {
            var fieldList = mesg.Fields;
            foreach (var field in fieldList)
            {
                switch (field.Name)
                {
                    case "StartTime":
                        Console.WriteLine($"开始时间:{field.GetValue()}");
                        break;
                    case "TotalAscent":
                        Console.WriteLine($"总爬升:{field.GetValue()} {field.Units}");
                        break;
                    case "TotalDescent":
                        Console.WriteLine($"总下降:{field.GetValue()} {field.Units}");
                        break;
                    case "AvgSpeed":
                        Console.WriteLine($"平均速度:{Tools.ConvertToKmPerHour(field.GetValue())} km/h");
                        break;
                    case "MaxSpeed":
                        Console.WriteLine($"最大速度:{Tools.ConvertToKmPerHour(field.GetValue())} km/h");
                        break;
                    case "AvgHeartRate":
                        Console.WriteLine($"平均心率:{field.GetValue()} {field.Units}");
                        break;
                    case "MaxHeartRate":
                        Console.WriteLine($"最大心率:{field.GetValue()} {field.Units}");
                        break;
                    case "AvgCadence":
                        Console.WriteLine($"平均踏频:{field.GetValue()} {field.Units}");
                        break;
                    case "MaxCadence":
                        Console.WriteLine($"最大踏频:{field.GetValue()} {field.Units}");
                        break;
                    case "AvgPower":
                        Console.WriteLine($"平均功率:{field.GetValue()} {field.Units}");
                        break;
                    case "MaxPower":
                        Console.WriteLine($"最大功率:{field.GetValue()} {field.Units}");
                        break;
                    case "AvgAltitude":
                        Console.WriteLine($"平均海拔:{field.GetValue()} {field.Units}");
                        break;
                    case "MaxAltitude":
                        Console.WriteLine($"最大海拔:{field.GetValue()} {field.Units}");
                        break;
                    case "AvgGrade":
                        Console.WriteLine($"平均坡度:{field.GetValue()} {field.Units}");
                        break;
                    case "MaxTemperature":
                        Console.WriteLine($"最高温度:{field.GetValue()} {field.Units}");
                        break;
                    case "AvgTemperature":
                        Console.WriteLine($"平均温度:{field.GetValue()} {field.Units}");
                        break;
                    case "TotalElapsedTime":
                        Console.WriteLine($"总时长:{field.GetValue()} {field.Units}");
                        break;
                    case "TotalTimerTime":
                        Console.WriteLine($"运动时长:{field.GetValue()} {field.Units}");
                        break;
                    case "TotalDistance":
                        Console.WriteLine($"总里程:{field.GetValue()} {field.Units}");
                        break;
                    case "ThresholdPower":
                        Console.WriteLine($"阈值功率:{field.GetValue()} {field.Units}");
                        break;
                    case "TotalWork":
                        Console.WriteLine($"总功:{field.GetValue()} {field.Units}");
                        break;
                    case "TotalMovingTime":
                        Console.WriteLine($"移动时长:{field.GetValue()} {field.Units}");
                        break;
                    case "TotalCalories":
                        Console.WriteLine($"总卡路里:{field.GetValue()} {field.Units}");
                        break;
                    case "Sport":
                        Console.WriteLine($"运动类型:{field.GetValue()}");
                        break;
                    case "SubSport":
                        Console.WriteLine($"运动子类型:{field.GetValue()}");
                        break;
                    case "EnhancedMaxSpeed":
                        Console.WriteLine($"最大速度:{Tools.ConvertToKmPerHour(field.GetValue())} km/h");
                        break;
                    case "EnhancedAvgSpeed":
                        Console.WriteLine($"平均速度:{Tools.ConvertToKmPerHour(field.GetValue())} km/h");
                        break;
                    case "EnhancedAvgAltitude":
                        Console.WriteLine($"平均海拔:{field.GetValue()} {field.Units}");
                        break;
                    case "EnhancedMaxAltitude":
                        Console.WriteLine($"最高海拔:{field.GetValue()} {field.Units}");
                        break;
                    default:
                        Console.WriteLine($"{field.Name}:{field.GetValue()}");
                        break;
                }
            }
        }
    }
}
