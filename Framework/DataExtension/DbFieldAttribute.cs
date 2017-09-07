using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DataExtension
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DbFieldAttribute : System.Attribute
    {
        /// <summary>
        /// 描述数据模型中的数据库字段，如果属性中没有DbField描述，则采用Property.Name作为数据库字段映射
        /// </summary>
        public string DbField { get; set; }

        public DbFieldAttribute(string dbField)
        {
            this.DbField = dbField;
        }
    }
}
