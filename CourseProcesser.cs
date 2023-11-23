using Dynastream.Fit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitDemo
{
    public static class CourseProcesser
    {
        internal static void Event(object sender, MesgEventArgs e)
        {
            throw new NotImplementedException();
            CourseProcess(e.mesg);
        }

        private static void CourseProcess(Mesg mesg)
        {
            foreach(var field in mesg.Fields)
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
