using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    /// <summary>
    /// Excel单元信息的Key和Value,例如：{{"UserName","姓名"，"20*256"},{"Age","年龄"},{"Student.Id","学生编号"}}
    /// </summary>
    public class ExcelInfo
    {
        public PropertyInfo Property { get; set; }

        public string Name { get; set; }

        public int Width { get; set; }

        public int Index { get; set; }
    }


    [AttributeUsage(AttributeTargets.All)]
    public sealed class ExcelInfoAttribute : Attribute
    {
        public string Name { get; set; }

        public int Width { get; set; }

        public int Index { get; set; }

        public ExcelInfoAttribute()
        {
            Width = 20;
        }

    }

    public class TypeMapper<T>
    {

        public List<ExcelInfo> ExcelInfos { get; set; } 


        public List<ExcelInfo> ExcelInfosByIndex { get; set; }

        public TypeMapper()
        {
            if (ExcelInfos == null)
            {
                ExcelInfos = new List<ExcelInfo>();
            }
            if (ExcelInfosByIndex == null)
            {
                ExcelInfosByIndex = new List<ExcelInfo>();
            }
            Analyze();
        }

        private void Analyze()
        {
            Type type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in properties)
            {
                var excelInfoAttribute = Attribute.GetCustomAttribute(prop, typeof(ExcelInfoAttribute)) as ExcelInfoAttribute;

                if (excelInfoAttribute != null)
                {
                    var excelInfo = new ExcelInfo() { Property = prop, Name = excelInfoAttribute.Name, Width = excelInfoAttribute.Width, Index = excelInfoAttribute.Index };
                    ExcelInfos.Add(excelInfo);
                    ExcelInfosByIndex.Add(excelInfo);
                }
            }
            ExcelInfosByIndex.Sort((x, y) => x.Index.CompareTo(y.Index));
        }
    }
}
