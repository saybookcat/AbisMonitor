using Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Common.Service
{
    public static class SqlHelper
    {
        private static string _connectionStr;
        public static string ConnectionStr
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_connectionStr))
                {
                    return _connectionStr;
                }
                _connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                return _connectionStr;
            }
        }

        public static MySqlConnection GetConnection(string connStr = null)
        {
            if (string.IsNullOrWhiteSpace(connStr))
            {
                return new MySqlConnection(ConnectionStr);
            }
            return new MySqlConnection(connStr);
        }

        public static bool IsConnection(string connStr)
        {
            try
            {
                using (MySqlConnection conn = GetConnection(connStr))
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #region GetTableCount
        /// <summary>
        /// 根据表名查询所有表的记录数，返回存在的表和记录数的对和几何，和记录总数
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="parameStr"></param>
        /// <param name="token"></param>
        /// <param name="allCount"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetTablesCount(List<string> tableNames, string parameStr, System.Threading.CancellationToken token, out int allCount)
        {
            allCount = 0;
            if (tableNames == null || !tableNames.Any()) return null;
            //var noTableNames = new List<string>();
            var tablesCountDic = new Dictionary<string, int>();
            string hasTableNameQueryStr =
                        "select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_NAME='{0}';";

            using (var conn = SqlHelper.GetConnection())
            {
                conn.Open();
                try
                {
                    parameStr = parameStr.ParameStrFilter();
                    foreach (var tableName in tableNames)
                    {
                        token.ThrowIfCancellationRequested();
                        var sqlStr1 = string.Format(hasTableNameQueryStr, tableName);
                        if (NoHasTableName(conn, sqlStr1))
                        {
                            continue;
                        }
                        string strSql2 = string.IsNullOrWhiteSpace(parameStr)
                            ? string.Format("select count(*) from {0};", tableName)
                            : string.Format("select count(*) from {0} {1};", tableName, parameStr);
                        try
                        {
                            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(conn, strSql2))
                            {
                                if (reader.Read() && !reader.IsDBNull(0))
                                {
                                    int count = reader.GetInt32(0);
                                    tablesCountDic.Add(tableName, count);
                                    allCount += count;
                                }
                            }
                        }
                        catch (MySqlException msex)
                        {
                            //1146:表不存在，1356：视图异常，无法打开（视图引用的表不存在）
                            if (msex.Number == 1146 || msex.Number == 1356 || msex.Number == 1029)
                            {
                                continue;
                            }
                        }
                    }
                    return tablesCountDic;

                }
                catch (NullReferenceException)
                {
                    allCount = 0;
                    throw;
                }
                catch (Exception)
                {
                    allCount = 0;
                    throw;
                }

            }
        }

        private static string ParameStrFilter(this string parameStr)
        {
            if (!string.IsNullOrWhiteSpace(parameStr))
            {
                //针对left join查询时的参数处理，后续仍需修改。
                parameStr = parameStr.ReplaceInsensitive("having", "where");
            }
            return parameStr;
        }

        

        private static bool NoHasTableName(MySqlConnection conn, string sqlStr)
        {
            var tmpHasTable = MySqlHelper.ExecuteScalar(conn, sqlStr);
            if (tmpHasTable == null || string.IsNullOrWhiteSpace(tmpHasTable.ToString()))
            {
                return true;
            }
            return false;
        }
        #endregion

        public static int ExecuteNonQueryWithConn(string commandText, MySqlConnection conn = null)
        {
            int result;
            if (conn == null)
            {
                using (conn = SqlHelper.GetConnection())
                {
                    result = MySqlHelper.ExecuteNonQuery(conn, commandText);
                }
            }
            else
            {
                result = MySqlHelper.ExecuteNonQuery(conn, commandText);
            }
            return result;
        }

        /// <summary>
        /// 根据表名、起始日期和结束日期构建所有要查询的表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static List<string> GetTableNameByDateTime(string tableName, DateTime startDate, DateTime endDate)
        {
            var tableNameList = new List<string>();
            int count = (endDate.Date - startDate.Date).Days;
            if (endDate.TimeOfDay.TotalSeconds != 0)
            {
                count++;
            }
            for (int i = 0; i < count; i++)
            {
                tableNameList.Add(tableName + startDate.ToString(Common.FixedParamsPub.TIME_FORMAT_YMD));
                startDate = startDate.AddDays(1);
            }
            return tableNameList;
        }

    }
}
