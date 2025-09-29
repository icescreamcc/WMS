using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace External.Common.Extension
{
    public static class PropertyInfoExtension
    {
        public static void SetValueExtension(this PropertyInfo propInfo, object obj, object value)
        {
            if (propInfo.PropertyType.Equals(typeof(int))|| propInfo.PropertyType.Equals(typeof(double))||
                propInfo.PropertyType.Equals(typeof(float))|| propInfo.PropertyType.Equals(typeof(decimal)))
            {
                if (value == null)
                {
                    propInfo.SetValue(obj,0 );
                }
                else
                {
                    int v;
                    int.TryParse(value.ToString(), out v);
                    propInfo.SetValue(obj, v);
                }
            }
            else if (propInfo.PropertyType.Equals(typeof(DateTime)))
            {
                if (value == null)
                {
                    propInfo.SetValue(obj, DateTime.Parse("1900-1-1"));
                }
                else
                {
                    DateTime dt;
                    DateTime.TryParse(value.ToString(), out dt);
                    propInfo.SetValue(obj, dt);
                }
            }
            else
            {
                propInfo.SetValue(obj, value);
            }
        }
    }
}
