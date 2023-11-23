using Dynastream.Fit;

namespace FitDemo
{
    public static class SoftwareProcesser
    {
        public static void Event(object sender, MesgEventArgs e)
        {
            Console.WriteLine($"------------------{e.mesg.Name}---------------------");
            ProcessSoftware(e.mesg);
        }

        public static void ProcessSoftware(Mesg software)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();


            foreach (var field in software.Fields)
            {
                var value = field.GetValue();
                switch (field.Name)
                {
                    case "MessageIndex":
                        Console.WriteLine($"消息下标:{value}");
                        break;
                    case "Version":
                        Console.WriteLine($"版本:{value}");
                        break;
                    case "PartNumber":
                        Console.WriteLine($"码表:{encoding.GetString((byte[])value)}");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
