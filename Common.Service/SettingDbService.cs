using Common.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Common.Service
{
    public abstract class SettingDbService<T> : QueryDbService<T> where T : DbModel, new()
    {
        public SettingDbService(string tableName, string sqlTablePart) : base(tableName, sqlTablePart)
        {
        }

        public abstract bool Add(T entity, MySqlConnection conn = null);

        public virtual bool Update(T entity, T oldEntity, MySqlConnection conn)
        {
            return this.Update(entity, oldEntity.DataNum, conn);
        }

        public abstract bool Update(T entity, int primaryKey, MySqlConnection conn);

        public virtual int Delete(List<T> deleteList,string primaryKey)
        {
            if (deleteList == null || !deleteList.Any()) return 0;
            string strSql = "delete from {0} where {1} in ({2});";
            var primayKeyStringBuilder = new StringBuilder();

            var deleteCount = deleteList.Count;
            for (int i = 0; i < deleteCount; i++)
            {
                if (i > 0)
                {
                    primayKeyStringBuilder.Append(",");
                }
                primayKeyStringBuilder.Append(deleteList[i].DataNum);
            }
            if (string.IsNullOrWhiteSpace(primayKeyStringBuilder.ToString())) return 0;

            strSql = string.Format(strSql, TableName, primaryKey, primayKeyStringBuilder.ToString());
            return SqlHelper.ExecuteNonQueryWithConn(strSql);
        }

        protected virtual string PrimaryKey { get { return "DataNum"; } }

        public virtual List<T> GetInfos(string[] orderByFileds = null, MySqlConnection conn = null)
        {
            StringBuilder orderByFiledStr = new StringBuilder();
            string orderBy = string.Empty;
            if (orderByFileds != null && orderByFileds.Any())
            {
                foreach (var filed in orderByFileds)
                {
                    if (orderByFiledStr.Length > 0)
                    {
                        orderByFiledStr.Append(",");
                    }
                    orderByFiledStr.Append(filed);
                }
                if (orderByFiledStr.Length > 0)
                {
                    orderByFiledStr.Insert(0, "orderBy ");
                }
            }
            string strSql = string.Format("select * from {0} {1}", TableName, orderByFiledStr.ToString());
            return base.SearchMappedResults(conn, strSql, 1);
        }

        public abstract DataDbValidateResult DataDbValidate(T entity, out T hasItem, List<T> allInfos, MySqlConnection conn=null);

        public virtual void ImportFilter(List<T> excelModels, System.Threading.CancellationToken token, out List<T> repeatList, out List<T> conflictList, out List<T> addList, out Dictionary<T, T> updateDic)
        {
            repeatList = new List<T>();
            conflictList = new List<T>();
            addList = new List<T>();
            updateDic = new Dictionary<T, T>();

            if (excelModels == null || !excelModels.Any()) return;

            using (var conn = SqlHelper.GetConnection())
            {
                conn.Open();
                var allInfos = GetInfos(conn: conn);
                foreach (var excelModel in excelModels)
                {
                    token.ThrowIfCancellationRequested();
                    T hasDbItem;
                    var validateResult = DataDbValidate(excelModel, out hasDbItem, allInfos, conn);
                    switch (validateResult)
                    {
                        case DataDbValidateResult.Add:
                            addList.Add(excelModel);
                            break;
                        case DataDbValidateResult.Repeat:
                            repeatList.Add(excelModel);
                            break;
                        case DataDbValidateResult.Update:
                            if (hasDbItem == null)
                            {
                                conflictList.Add(excelModel);

                            }
                            else
                            {
                                excelModel.DataNum = hasDbItem.DataNum;
                                updateDic.Add(excelModel, hasDbItem);
                            }
                            break;
                        case DataDbValidateResult.Conflict:
                            conflictList.Add(excelModel);
                            break;
                        case DataDbValidateResult.Ignore:
                            addList.Add(excelModel);
                            break;
                        default:
                            conflictList.Add(excelModel);
                            break;
                    }
                }
            }
        }

        public virtual void ImportInfos(List<T> addList, Dictionary<T,T> updateDic, System.Threading.CancellationToken token, out int addSuccessCount, out int updateSuccessCount, Action<int, int> importProgressAction)
        {
            int allCount = 0;
            if (addList != null)
            {
                allCount += addList.Count;
            }
            if (updateDic != null)
            {
                allCount += updateDic.Count;
            }
            addSuccessCount = 0;
            updateSuccessCount = 0;

            using (MySqlConnection conn = SqlHelper.GetConnection())
            {
                conn.Open();
                if (addList != null && addList.Any())
                {
                    foreach (var item in addList)
                    {
                        token.ThrowIfCancellationRequested();
                        if (item == null) continue;
                        try
                        {
                            if (Add(item, conn))
                            {
                                addSuccessCount++;
                                if (importProgressAction != null)
                                {
                                    importProgressAction.Invoke(addSuccessCount, allCount);
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                if (updateDic != null && updateDic.Any())
                {
                    foreach (var item in updateDic)
                    {
                        if (item.Key == null) continue;
                        if (Update(item.Key, item.Value, conn))
                        {
                            updateSuccessCount++;
                            if (importProgressAction != null)
                            {
                                importProgressAction.Invoke(addSuccessCount, allCount);
                            }
                        }
                    }
                }
            }
        }
    }
}
