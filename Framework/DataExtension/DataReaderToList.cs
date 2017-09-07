using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DataExtension
{/// <summary>
 /// DataReader处理扩展
 /// </summary>
    public static class DataReaderToList
    {
        /// <summary>
        /// DataReader转化成List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> GetDataEntitys<T>(this DbDataReader reader) where T : new()
        {
            var list = new List<T>();

            if (reader == null)
                return list;

            HashSet<string> tableColumnNames = DataReaderToList.CollectColumnNames(reader); ;
            while (reader.Read())
            {
                var entity = new T();
                foreach (var propertyInfo in typeof(T).GetProperties())
                {
                    object retrievedObject = null;
                    if (tableColumnNames.Contains(propertyInfo.Name) &&
                        (retrievedObject = reader[propertyInfo.Name]) != null)
                    {
                        propertyInfo.SetValue(entity, retrievedObject, null);
                    }
                }
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// DataReader字段缓存HashSet,在ReaderEntity中使用。如下：
        /// HashSet<string> tableColumnNames tableColumnNames = tableColumnNames ?? CollectColumnNames(reader);
        /// while(dataReader.Read())
        /// {
        ///     var model=dataReader.ReaderEntity<Model>(tableColumnNames);
        /// }
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static HashSet<string> CollectColumnNames(this DbDataReader reader)
        {
            var collectedColumnInfo = new HashSet<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                collectedColumnInfo.Add(reader.GetName(i));
            }
            return collectedColumnInfo;
        }

        /// <summary>
        /// 从DataRecord中读取Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record"></param>
        /// <param name="tableColumnNames">参照上方CollectColumnNames(DbDataReader reader)方法</param>
        /// <returns></returns>
        public static T ReaderEntity<T>(this IDataRecord record, ref HashSet<string> tableColumnNames) where T : new()
        {
            var entity = new T();
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                object retrievedObject = null;
                var propertyInfoItem =
                    propertyInfo.GetCustomAttributes(typeof(DbFieldAttribute), false).FirstOrDefault();
                var dbField = propertyInfoItem as DbFieldAttribute;
                var propertyInfoName = dbField != null ? dbField.DbField : propertyInfo.Name;

                var hasColumnName =
                    tableColumnNames.FirstOrDefault(columnName => columnName.Equals(propertyInfoName, StringComparison.CurrentCultureIgnoreCase));
                if (hasColumnName != null)
                {
                    retrievedObject = record[propertyInfoName];
                    if (retrievedObject == null || retrievedObject is DBNull)
                    {
                        propertyInfo.SetValue(entity, null, null);
                    }
                    else
                    {
                        propertyInfo.SetValue(entity, retrievedObject, null);
                    }
                }
            }
            return entity;
        }

    }
}
