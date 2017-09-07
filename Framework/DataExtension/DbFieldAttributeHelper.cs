using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.DataExtension
{
    public static class DbFieldAttributeHelper
    {
        public static string DbFieldAttributeName = typeof(DbFieldAttribute).Name;
        public static string GetDbField(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName).GetCustomAttributes(false).FirstOrDefault(x => x.GetType().Name == DbFieldAttributeName);
            if (property != null)
            {
                return ((DbFieldAttribute)property).DbField;
            }
            return null;
        }
    }
}
