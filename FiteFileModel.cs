using Dynastream.Fit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitDemo
{
    public class FiteFileModel
    {
        public Mesg ActivityFileMsg { get; set; }

        public List<Mesg> DeviceInfoMsg { get; set; }

        public Mesg FileIdMsg { get; set; }

        public List<Mesg> SessionMsg { get; set; }

        public Mesg UserProfileMsg { get; set; }

        public List<Mesg> RecordMsg { get; set; }

        public List<Mesg> SoftwareMsg { get; set; }
    }

    public static class MesgExtion
    {
        public static T GetFieldValue<T>(this Mesg mesg, string fieldName)
        {
            var f = mesg.Fields.FirstOrDefault(f => f.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
            if (f == null)
            {
                return default(T);
            }
            return (T)f.GetValue();
        }

        public static void SetFieldValue<T>(this Mesg mesg, string fieldName, T value)
        {
            foreach (var field in mesg.Fields)
            {
                if (field.Name.Equals(fieldName))
                {
                    //var sss = new Field(field.Num, field.Type, field.Scale, field.Offset, field.Units, field.IsAccumulated, field.IsExpandedField);
                }
            }

            var f = mesg.Fields.FirstOrDefault(f => f.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
            if (f == null)
            {
                return;
            }

            f.SetValue(value);
            var vv = f.GetValue();


            f = mesg.Fields.FirstOrDefault(f => f.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
            if (f == null)
            {
                return;
            }


            vv = f.GetValue();
        }
    }
}
