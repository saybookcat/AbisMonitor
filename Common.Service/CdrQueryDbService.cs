using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service
{
    public abstract class CdrQueryDbService<TDbModel, TCdrDbModel> : QueryDbService<TDbModel>
        where TDbModel : DbModel, new()
        where TCdrDbModel : CdrDbModel, new()
    {
        public CdrQueryDbService(string tableName, string sqlTablePart) : base(tableName, sqlTablePart)
        {
        }

        protected abstract QueryDbService<TDbModel> SetRawSignalDbService();

        private QueryDbService<TDbModel> _rawSignalDbService;
        protected QueryDbService<TDbModel> RawSignalDbService
        {
            get { return _rawSignalDbService ?? (_rawSignalDbService = SetRawSignalDbService()); }
        }

        public virtual List<TDbModel> SearchRawSignalByCdrIndex(DateTime dateTime, long cdrIndex, System.Threading.CancellationToken token)
        {
            var tableNames = new List<String>();
            string tableName = RawSignalDbService.TableName;
            //查询前后的一张表的数据记录，共三张表
            string tableName0 = tableName + dateTime.AddDays(-1).ToString(FixedParamsPub.TIME_FORMAT_YMD);
            tableNames.Add(tableName0);
            string tableName1 = tableName + dateTime.ToString(FixedParamsPub.TIME_FORMAT_YMD);
            tableNames.Add(tableName1);
            if (dateTime.Date < DateTime.Now.Date)
            {
                string tableName2 = tableName + dateTime.AddDays(1).ToString(FixedParamsPub.TIME_FORMAT_YMD);
                tableNames.Add(tableName2);
            }
            var sb = new StringBuilder();
            sb.Append(string.Format("where {0}={1} ", RawSignalCdrIndexKeyword(), cdrIndex));

            string rawSignalSqlTablePart = RawSignalDbService.SqlTablePart;
            int totalCount;
            var tableNamesDic = SqlHelper.GetTablesCount(tableNames, sb.ToString(), token, out totalCount);
            tableNamesDic = tableNamesDic ?? new Dictionary<string, int>();
            tableNames = tableNamesDic.Keys.ToList();
            List<int> totalCountList = tableNamesDic.Values.ToList();

            return SearchDataService.SearchAllData(tableNames, totalCountList, SqlTablePart, sb.ToString(), totalCount, SearchRawSignalMappedResults, token);
        }

        protected virtual string RawSignalCdrIndexKeyword()
        {
            return "CdrIndex";
        }

        private List<TDbModel> SearchRawSignalMappedResults(MySql.Data.MySqlClient.MySqlConnection conn, string strSql, int dispId)
        {
            return RawSignalDbService.SearchMappedResults(conn, strSql, dispId);
        }
    }
}
