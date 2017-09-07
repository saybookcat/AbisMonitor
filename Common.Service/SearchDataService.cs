
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Framework.DataExtension;
using MySql.Data.MySqlClient;

namespace Common.Service
{
    public static class SearchDataService
    {
        #region 可取消的分页查询
        /// <summary>
        ///     可取消的分页查询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableNames"></param>
        /// <param name="totalCountList"></param>
        /// <param name="sqlTablePart"></param>
        /// <param name="sqlWherePart"></param>
        /// <param name="totalCount"></param>
        /// <param name="lineCount"></param>
        /// <param name="pageNum"></param>
        /// <param name="searchResults"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static List<T> SearchDataForPager<T>(List<string> tableNames, List<int> totalCountList, string sqlTablePart,
            string sqlWherePart, int totalCount, int lineNum, int pageNum,
            Func<MySqlConnection, string, int, List<T>> searchMappedResults, CancellationToken token, string connStr = null) where T : class
        {
            var queryList = new List<T>();
            int startId = totalCount == 0 ? 0 : (pageNum - 1) * lineNum;
            int dispId = totalCount == 0 ? 0 : startId + 1;

            //当前表中的行数
            int lineCountInCurTable = lineNum,
                //之前取得的数量
                recCountBeforeThisTable = 0,
                //此次之前的数量
                recCountBeforeThisFetch = 0,
                //此次取得的数量
                lineCountThisFetch = 0;
            using (MySqlConnection conn = SqlHelper.GetConnection(connStr))
            {
                conn.Open();

                for (int i = 0; i < tableNames.Count; i++)
                {
                    token.ThrowIfCancellationRequested();

                    if (startId >= totalCountList[i])
                    {
                        startId -= totalCountList[i];
                        continue;
                    }
                    recCountBeforeThisTable = queryList.Count;
                    lineCountThisFetch = Math.Min(lineCountInCurTable, totalCountList[i]);
                    lineCountThisFetch = Math.Min(lineCountThisFetch, Common.Pub.DEFAULT_DBFETCHCOUNT);

                    while (true)
                    {
                        token.ThrowIfCancellationRequested();

                        recCountBeforeThisFetch = queryList.Count;
                        string where = string.IsNullOrWhiteSpace(sqlWherePart) ? "" : sqlWherePart;
                        string strSql = string.Format(sqlTablePart, tableNames[i], where, startId,
                            Math.Min(lineCountThisFetch, totalCount - dispId + 1));

                        List<T> items = searchMappedResults(conn, strSql, dispId);

                        if (items != null)
                        {
                            queryList.AddRange(items);
                            dispId += items.Count;
                        }

                        int currentCount = queryList.Count;
                        if (currentCount >= lineNum || currentCount <= recCountBeforeThisFetch ||
                            (currentCount - recCountBeforeThisTable) >= totalCountList[i])
                        {
                            break;
                        }
                        startId += currentCount - recCountBeforeThisFetch;
                        if (startId >= totalCountList[i]) break;

                        lineCountThisFetch = currentCount - recCountBeforeThisFetch;
                        if (lineCountThisFetch <= 0) break;
                    }
                    if (queryList.Count == lineNum)
                    {
                        break;
                    }
                    if (queryList.Count > lineNum)
                    {
                        queryList.RemoveRange(lineNum, queryList.Count - lineNum);
                        break;
                    }
                    startId = 0;
                    lineCountInCurTable = lineNum - queryList.Count;
                }
            }
            return queryList;
        }
        #endregion

        #region 可取消的所有结果集查询
        /// <summary>
        /// 可取消的所有结果集查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableNames"></param>
        /// <param name="totalCountList"></param>
        /// <param name="sqlTablePart"></param>
        /// <param name="sqlWherePart"></param>
        /// <param name="totalCount"></param>
        /// <param name="searchResults"></param>
        /// <param name="token"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static List<T> SearchAllData<T>(List<string> tableNames, List<int> totalCountList,
           string sqlTablePart,
           string sqlWherePart,
           int totalCount, Func<MySqlConnection, string, int, List<T>> searchMappedResults, CancellationToken token, string connStr = null)
           where T : class
        {
            var queryList = new List<T>();
            if (totalCount <= 0) return null;


            using (MySqlConnection conn = SqlHelper.GetConnection(connStr))
            {
                conn.Open();

                int startIdx = 0, dispId = startIdx + 1;
                //每次取多少行
                int lineCountInCurTable = Common.Pub.DEFAULT_DBFETCHCOUNT;

                for (int i = 0; i < tableNames.Count; i++)
                {
                    token.ThrowIfCancellationRequested();

                    if (startIdx >= totalCountList[i])
                    {
                        startIdx -= totalCountList[i];
                        continue;
                    }

                    //当前表中的行数
                    int recCountBeforeThisTable = queryList.Count;
                    //此次取的行数
                    int lineCountThisFetch = Math.Min(lineCountInCurTable, totalCountList[i]);
                    lineCountThisFetch = Math.Min(lineCountThisFetch, Common.Pub.DEFAULT_DBFETCHCOUNT);

                    string where = string.IsNullOrWhiteSpace(sqlWherePart) ? "" : sqlWherePart;
                    while (true)
                    {
                        token.ThrowIfCancellationRequested();

                        //此次之前的数量
                        int recCountBeforeThisFetch = queryList.Count;


                        string strSql =
                            string.Format(sqlTablePart, tableNames[i], where, startIdx,
                                Math.Min(lineCountThisFetch, totalCount - dispId + 1));

                        List<T> items = searchMappedResults(conn, strSql, dispId);


                        if (items != null)
                        {
                            queryList.AddRange(items);
                            dispId += items.Count;
                        }
                        int allItemCount = queryList.Count;
                        if (allItemCount >= totalCount || allItemCount <= recCountBeforeThisFetch ||
                            (allItemCount - recCountBeforeThisTable) >= totalCountList[i])
                        {
                            break;
                        }
                        startIdx += allItemCount - recCountBeforeThisFetch;
                        if (startIdx >= totalCountList[i]) break;

                        lineCountThisFetch = allItemCount - recCountBeforeThisFetch;
                        if (lineCountThisFetch <= 0) break;
                    }
                    if (queryList.Count == totalCount)
                    {
                        break;
                    }
                    if (queryList.Count > totalCount)
                    {
                        queryList.RemoveRange(totalCount, queryList.Count - totalCount);
                        break;
                    }
                    startIdx = 0;
                    lineCountInCurTable = totalCount - queryList.Count;
                }
            }
            return queryList;
        }
        #endregion

        public static IEnumerable<T> SearchData<T>(string commandText) where T : class, new()
        {
            return SearchData<T>(null, commandText);
        }

        public static IEnumerable<T> SearchData<T>(MySqlConnection conn, string commandText) where T : class, new()
        {
            IEnumerable<T> list = null;
            if (conn == null)
            {
                using (conn = SqlHelper.GetConnection())
                {
                    conn.Open();

                    list = SearchDataByMap<T>(conn, commandText);
                }
            }
            else
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                list = SearchDataByMap<T>(conn, commandText);
            }
            return list;
        }

        private static IEnumerable<T> SearchDataByMap<T>(MySqlConnection conn, string commandText) where T : class, new()
        {
            var dataSet = MySqlHelper.ExecuteDataset(conn, commandText);
            if (dataSet == null) return null;
            if (dataSet.Tables.Count == 0) return null;
            var dataTable = dataSet.Tables[0];
            if (dataTable == null) return null;
            if (dataTable.Rows == null || dataTable.Rows.Count == 0) return null;
            //return dataTable.ToList<T>();
            var mapper = new DbFieldMapper<T>();
            return mapper.Map(dataTable);
        }
    }
}
