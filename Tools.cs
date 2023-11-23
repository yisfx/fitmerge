using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitDemo
{
    public static class Tools
    {
        public static string FormateNumber(object value)
        {
            if(value is TimeSpan timeSpan)
            {
                return timeSpan.ToString();
            }
            if(value is DateTime dateTime)
            {
                return dateTime.ToString(@"yyyy-MM-dd hh:mm:ss");
            }

            return Math.Round(Convert.ToDouble(value), 2).ToString("0.00");
        }

        public static double ConvertToKmPerHour(object value)
        {
            return Math.Round(Convert.ToDouble(value) * 3.6, 2);
        }


        public static string ByteToString(object value)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetString((byte[])value);
        }
    }
}
