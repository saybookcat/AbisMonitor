using Common.Domain;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Common.Service
{
    public abstract class StatisticsDbService<T> where T : StatisticsModel, new()
    {
        public StatisticsDbService(string tableName,string sqlTablePart)
        {
            this.TableName = tableName;
            this.SqlTablePart = sqlTablePart;
        }

        protected string TableName { get; set; }

        protected string SqlTablePart { get; set; }

        protected const string BaseSqlTablePart = @"SELECT
	cast(sum(Result<>-1 AND Result<>2) AS DECIMAL) AS AllCount,
	cast(sum(Result=0) AS DECIMAL) AS SuccessCount,
	cast(sum(Result=-1 OR Result=2) AS DECIMAL) AS InvalidCount
FROM {0} {1};";

        public List<T> GetStatisticsList(DateTime startTime, DateTime endTime, int timeOffset, string sqlWherePart, out T allStatisticsInfo, System.Threading.CancellationToken token)
        {
            allStatisticsInfo = null;
            var list = new List<T>();

            List<StatisticsParameter> parameterList = ParamtersSplit(startTime, endTime, sqlWherePart, timeOffset);

            long successCount = 0;
            long invalidCount = 0;
            long totalCount = 0;
            long tag1 = 0;
            long tag2 = 0;
            using (var conn = SqlHelper.GetConnection())
            {
                conn.Open();

                for (int i = 0; i < parameterList.Count; i++)
                {
                    var oneDayModel = new T();

                    #region query List

                    foreach (var sql in parameterList[i].SqlList)
                    {
                        StatisticsModel onecModel = null;
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            var queryData = MySqlHelper.ExecuteDataset(conn, sql).Tables[0];
                            if (queryData != null && queryData.Rows.Count > 0)
                            {
                                onecModel = queryData.AsEnumerable().Select(item => new T()
                                {
                                    AllCount = DecimalToLongNotNull(item.Field<Decimal?>("AllCount")),
                                    SuccessCount = DecimalToLongNotNull(item.Field<Decimal?>("SuccessCount")),
                                    InvalidCount = DecimalToLongNotNull(item.Field<Decimal?>("InvalidCount")),
                                    Tag1 = queryData.Columns.Contains("Tag1") ? DecimalToLong(item.Field<Decimal?>("Tag1")) : null,
                                    Tag2 = queryData.Columns.Contains("Tag2") ? DecimalToLong(item.Field<Decimal?>("Tag2")) : null,
                                }).FirstOrDefault();
                            }
                        }
                        catch (MySqlException)
                        {
                            //表异常或不存在
                            onecModel = new T();
                        }
                        if (onecModel == null)
                        {
                            onecModel = new T();
                        }

                        oneDayModel.SuccessCount += onecModel.SuccessCount;
                        oneDayModel.AllCount += onecModel.AllCount;
                        oneDayModel.InvalidCount += onecModel.InvalidCount;

                        oneDayModel.Tag1 += onecModel.Tag1 ?? 0;
                        oneDayModel.Tag2 += onecModel.Tag2 ?? 0;
                    }

                    #endregion

                    oneDayModel.Disp = i + 1;
                    oneDayModel = Calc(oneDayModel.AllCount, oneDayModel.SuccessCount, oneDayModel.InvalidCount, oneDayModel);
                    oneDayModel.StartDate = parameterList[i].StartDate;
                    oneDayModel.EndDate = parameterList[i].EndDate;
                    if (oneDayModel.StartDate.TimeOfDay.TotalSeconds != timeOffset ||
                        oneDayModel.EndDate.TimeOfDay.TotalSeconds != timeOffset ||
                        oneDayModel.EndDate - oneDayModel.StartDate != TimeSpan.FromHours(24))
                    {
                        oneDayModel.Remarks ="非全天数据";
                    }

                    totalCount += oneDayModel.AllCount;

                    successCount += oneDayModel.SuccessCount;

                    invalidCount += oneDayModel.InvalidCount;
                    if (oneDayModel.Tag1 != null)
                    {
                        tag1 += oneDayModel.Tag1.Value;
                    }
                    if (oneDayModel.Tag2 != null)
                    {
                        tag2 += oneDayModel.Tag2.Value;
                    }


                    list.Add(oneDayModel);
                }
                allStatisticsInfo = Calc(totalCount, successCount, invalidCount, allStatisticsInfo);
                allStatisticsInfo.Tag1 = tag1;
                allStatisticsInfo.Tag2 = tag2;
                allStatisticsInfo.StartDate = startTime;
                allStatisticsInfo.EndDate = endTime;
            }
            return list;
        }


        #region Paramters Split
        private List<StatisticsParameter> ParamtersSplit(DateTime startTime, DateTime endTime, string sqlWherePart, double offset)
        {
            if (startTime >= endTime) return null;
            List<string> totalTableNames = SqlHelper.GetTableNameByDateTime(TableName, startTime, endTime);
            var list = new List<StatisticsParameter>();

            int dayCount = 0;

            bool flag = true;
            while (flag)
            {
                DateTime tempStart = startTime.Date.AddDays(dayCount).AddSeconds(offset);
                if (dayCount == 0)
                {
                    var fristModel = new StatisticsParameter();
                    if (startTime < tempStart)
                    {
                        fristModel.StartDate = startTime;
                        fristModel.EndDate = tempStart;
                        var sqlStr = BuildSql(fristModel.StartDate, fristModel.EndDate, SqlTablePart, sqlWherePart,
                            totalTableNames);
                        if (!string.IsNullOrWhiteSpace(sqlStr))
                        {
                            fristModel.SqlList.Add(sqlStr);
                            list.Add(fristModel);
                        }
                    }
                }
                DateTime tempEnd = tempStart.AddDays(1);
                if (tempStart <= startTime)
                {
                    tempStart = startTime;
                }
                if (tempEnd >= endTime)
                {
                    tempEnd = endTime;
                    DateTime endOffsetTime = endTime.Date.AddSeconds(offset);
                    if (tempEnd > endOffsetTime && endOffsetTime > tempStart)
                    {
                        var endModel = new StatisticsParameter() { StartDate = tempStart, EndDate = endOffsetTime };
                        var sqlStr = BuildSql(endModel.StartDate, endModel.EndDate, SqlTablePart, sqlWherePart,
                            totalTableNames);
                        if (!string.IsNullOrWhiteSpace(sqlStr))
                        {
                            endModel.SqlList.Add(sqlStr);
                            list.Add(endModel);
                        }
                        tempStart = endOffsetTime;
                    }

                    flag = false;
                }
                var dateModel = new StatisticsParameter()
                {
                    StartDate = tempStart,
                    EndDate = tempEnd
                };

                if (tempEnd.TimeOfDay.TotalMilliseconds != 0 && tempEnd.Date > tempStart.Date)
                {
                    DateTime tempEnd2 = tempEnd.Date;
                    string sql1 = BuildSql(tempStart, tempEnd2, SqlTablePart, sqlWherePart, totalTableNames);
                    if (!string.IsNullOrWhiteSpace(sql1))
                    {
                        dateModel.SqlList.Add(sql1);
                    }
                    string sql2 = BuildSql(tempEnd2, tempEnd, SqlTablePart, sqlWherePart, totalTableNames);
                    if (!string.IsNullOrWhiteSpace(sql2))
                    {
                        dateModel.SqlList.Add(sql2);
                    }
                }
                else
                {
                    if (tempStart >= tempEnd) //数据不合法
                    {
                        dayCount++;
                        continue;
                    }
                    string sql = BuildSql(tempStart, tempEnd, SqlTablePart, sqlWherePart, totalTableNames);
                    if (!string.IsNullOrWhiteSpace(sql))
                    {
                        dateModel.SqlList.Add(sql);
                    }
                }
                list.Add(dateModel);
                dayCount++;
            }
            return list;
        }

        private string BuildSql(DateTime startDate, DateTime endDate, string sqlTablePart, string sqlWherePart, List<string> totalTableNames)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}");
            var timeMatchCollection = reg.Matches(sqlWherePart);
            sqlWherePart = sqlWherePart ?? "";

            sqlWherePart = sqlWherePart.Replace(timeMatchCollection[0].Value,
                                startDate.ToString(FixedParamsPub.TIME_FORMAT_YMDHMS));
            sqlWherePart = sqlWherePart.Replace(timeMatchCollection[1].Value,
                endDate.ToString(FixedParamsPub.TIME_FORMAT_YMDHMS));
            string tableName = TableName + startDate.ToString(FixedParamsPub.TIME_FORMAT_YMD);
            if (!totalTableNames.Contains(tableName)) return null;

            var sqlStr = string.Format(sqlTablePart, tableName, sqlWherePart);
            return sqlStr;
        }
        #endregion

        #region  Converter and Calc
        private long? DecimalToLong(decimal? value)
        {
            if (value == null) return null;
            return long.Parse(value.ToString());
        }

        private long DecimalToLongNotNull(decimal? value)
        {
            if (value == null) return 0;
            return long.Parse(value.ToString());
        }

        private T Calc(long totalCount, long successCount, long invalidCount, T model)
        {
            if (model == null) model = new T();
            model.SuccessCount = successCount;
            model.AllCount = totalCount;
            model.InvalidCount = invalidCount;
            if (totalCount < 0) model.SuccessCount = 0;
            if (totalCount < 0) model.AllCount = 0;

            if ((totalCount == 0) || (successCount > totalCount))
            {
                model.SuccessPercent = "--";
            }
            else
            {
                double? ratio = successCount / ((double)totalCount);
                model.SuccessCount = successCount;
                model.SuccessPercent = String.Format("{0:F2}%", ratio * 100.0);
            }

            return model;
        }
        #endregion 
    }
}
