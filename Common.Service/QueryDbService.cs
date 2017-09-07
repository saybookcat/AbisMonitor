
using Common.Domain;
using Framework.DataExtension;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Service
{
    public abstract class QueryDbService<T> where T:DbModel,new()
    {
        public QueryDbService(string tableName,string sqlTablePart)
        {
            this.TableName = tableName;
            this.SqlTablePart = sqlTablePart;
        }

        /// <summary>
        /// 仅表名前缀部分
        /// 示例：mt_RawData_
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        ///  示例："select * from {0} {1} Order by RecTime,RecTimeUsec LIMIT {2},{3} ;"
        /// </summary>
        public string SqlTablePart { get; private set; }

        public readonly string BaseSqlTablePart = "select * from {0} {1} Order by RecTime,RecTimeUsec LIMIT {2},{3} ;";

        public virtual List<T> SearchMappedResults(MySqlConnection conn, string strSql)
        {
            var list = SearchDataService.SearchData<T>(conn, strSql);
            return list == null ? null : list.ToList();
        }

        public virtual List<T> SearchMappedResults(MySqlConnection conn, string strSql, int dispId)
        {
            var list = SearchMappedResults(conn, strSql);

            list = SetDisp(list, dispId);

            return list;
        }
        
        /// <summary>
        /// 业务相关，设置DispId
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dispId"></param>
        /// <returns></returns>
        protected virtual List<T> SetDisp(List<T> list, int dispId)
        {
            if (list == null || list.Count == 0) return list;
            list.ForEach(item => item.Disp = dispId++);
            return list;
        }
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="tableNames">待查询表集合</param>
        /// <param name="totalCountList">待查询表对应的数量</param>
        /// <param name="sqlWherePart">查询条件部分，即从"where"起及之后的部分</param>
        /// <param name="totalCount">查询总数</param>
        /// <param name="lineNum">每页数量</param>
        /// <param name="pageNum">页数</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public List<T> SearchDataForPager(List<string> tableNames, List<int> totalCountList, string sqlWherePart, int totalCount, int lineNum, int pageNum, CancellationToken token)
        {
            return SearchDataService.SearchDataForPager(tableNames, totalCountList, SqlTablePart, sqlWherePart, totalCount, lineNum, pageNum, SearchMappedResults, token);
        }
    }
}
