using Common.Domain;
using Framework.DataExtension;
using MySql.Data.MySqlClient;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Service.ExcelServices
{
    public abstract class ExportExcelService<T> where T:DbModel,new()
    {
        protected ExcelStyle ExcelStyle { get; private set; }
        Domain.TypeMapper<ExcelInfo> typeMapper;
        public ExportExcelService()
        {
            this.ExcelStyle = new ExcelStyle();
            typeMapper = new TypeMapper<ExcelInfo>();
        }

        private IList<ExcelInfo> _exprotCellHeadList;
        protected IList<ExcelInfo> ExportCellHeadList
        {
            get
            {
                return _exprotCellHeadList ?? (_exprotCellHeadList = typeMapper.ExcelInfos);
            }
        }

        private IList<ExcelInfo> _cellHeadListByIndex;
        protected IList<ExcelInfo> CellHeadListByIndex
        {
            get { return _cellHeadListByIndex ?? (_cellHeadListByIndex = typeMapper.ExcelInfosByIndex); }
            set { _cellHeadListByIndex = value; }
        }



        #region ExportExcel 
        /// <summary>
        /// 根据现有的既定行数，导出Excel文件
        /// </summary>
        /// <param name="allRows"></param>
        /// <param name="excelFileCount"></param>
        /// <param name="exportFileInfo"></param>
        /// <param name="tablesCountDic"></param>
        /// <param name="querySqlPart"></param>
        /// <param name="whereSqlPart"></param>
        /// <param name="token"></param>
        /// <param name="exportProgressAction"></param>
        /// <param name="exportFileCompletedAction"></param>
        public void ExportExcel(int allRows, int excelFileCount, ExportFileInfo exportFileInfo, Dictionary<string, int> tablesCountDic, string querySqlPart, string whereSqlPart, CancellationToken token, Action<int> exportProgressAction, Action<string> exportFileCompletedAction)
        {
            List<string> tableNameList = tablesCountDic.Keys.ToList();
            List<int> tableCountList = tablesCountDic.Values.ToList();
            int sheetCount = allRows / Pub.DEFAULT_EXCELEXPORT_SHEET_MAXCOUNT + allRows % Pub.DEFAULT_EXCELEXPORT_SHEET_MAXCOUNT == 0 ? 0 : 1;

            Func<int, string> getCurrectTableName = (int tableIndex) =>
            {
                return tableNameList[tableIndex];
            };

            using (MySqlConnection conn = SqlHelper.GetConnection())
            {
                conn.Open();

                int tableIndex = 0;
                int currectTableOccupancyRows = tableCountList[tableIndex]; //当前表的剩余记录数
                int currectTableStartRowIdx = 0;//当前表起始行号
                int dispId = 0;

                for (int fileIndex = 1; fileIndex <= excelFileCount; fileIndex++)
                {
                    string filePath = ExcelFileHelper.GetExportFileName(exportFileInfo.ExcelTitle, exportFileInfo.SaveDirectory, fileIndex, excelFileCount);


                    QueryDataReaderWriteExcel(conn, allRows, sheetCount, exportFileInfo, filePath, token, querySqlPart, tableNameList, tableCountList, whereSqlPart, ref tableIndex, ref currectTableOccupancyRows, ref currectTableStartRowIdx, ref dispId, exportProgressAction);
                    if (exportFileCompletedAction != null)
                    {
                        exportFileCompletedAction(filePath);
                    }
                }
            }
        }

        /// <summary>
        /// List导出到Excel
        /// </summary>
        /// <param name="allRows"></param>
        /// <param name="excelFileCount"></param>
        /// <param name="exportFileInfo"></param>
        /// <param name="list"></param>
        /// <param name="token"></param>
        /// <param name="exportProgressAction"></param>
        /// <param name="exportFileCompletedAction"></param>
        public void ExportListToExcel(int allRows, int excelFileCount, ExportFileInfo exportFileInfo, List<T> list, CancellationToken token, Action<int> exportProgressAction, Action<string> exportFileCompletedAction)
        {
            int sheetCount = allRows / Pub.DEFAULT_EXCELEXPORT_SHEET_MAXCOUNT + (allRows % Pub.DEFAULT_EXCELEXPORT_SHEET_MAXCOUNT == 0 ? 0 : 1);

            int currectListOccupancyRows = list.Count; //当前表的剩余记录数
            int dispId = 0;

            for (int fileIndex = 1; fileIndex <= excelFileCount; fileIndex++)
            {
                string filePath = ExcelFileHelper.GetExportFileName(exportFileInfo.ExcelTitle, exportFileInfo.SaveDirectory, fileIndex, excelFileCount);

                ListWriteExcelFile(allRows, sheetCount, exportFileInfo, filePath, list, token, ref currectListOccupancyRows, ref dispId, exportProgressAction);
                if (exportFileCompletedAction != null)
                {
                    exportFileCompletedAction.Invoke(filePath);
                }
            }

        }
        #endregion 

        #region DataToExcel
        private void QueryDataReaderWriteExcel(MySqlConnection conn, int allRows, int sheetCount, ExportFileInfo exportFileInfo, string filePath, CancellationToken token, string querySqlPart, List<string> tableNameList, List<int> tableCountList, string whereSqlPart, ref int tableIndex, ref int currectTableOccupancyRows, ref int currectTableStartRowIdx, ref int dispId, Action<int> exportProgressAction)
        {
            int currectSheetOccupancyRows = Pub.DEFAULT_EXCELEXPORT_SHEET_MAXCOUNT;
            int sheetIdx = 1; //sheet下标
            int currectExcelIdx = 0; //操作的excel操作行下标
            int currectExcelCount = 0;
            var workbook = new HSSFWorkbook();
            try
            {
                var sheet = CreateSheet(workbook, sheetCount, sheetIdx, exportFileInfo.ExcelTitle, out currectExcelIdx);


                int singleExcelMaxCount = Math.Min(Pub.DEFAULT_EXCEL_MAXCOUNT_FORINFO, allRows);
                while (currectExcelCount < singleExcelMaxCount)
                {
                    token.ThrowIfCancellationRequested();
                    int readerCount = Math.Min(Pub.DEFAULT_EXCELEXPORT_SHEET_MAXCOUNT, currectSheetOccupancyRows);
                    readerCount = Math.Min(readerCount, currectTableOccupancyRows);

                    if (readerCount > 0)
                    {
                        string commandText = CreateQuerySqlStrAction(querySqlPart, tableNameList[tableIndex], whereSqlPart, currectTableStartRowIdx, readerCount);

                        int writeCount;
                        sheet = DataReaderToSheet(workbook, sheet, conn, commandText, ref dispId, ref currectExcelIdx, exportProgressAction, token, out writeCount);

                        singleExcelMaxCount -= readerCount;
                        currectTableOccupancyRows -= readerCount;

                        currectTableStartRowIdx += writeCount;
                        currectExcelCount += writeCount;
                    }

                    #region state update

                    if (currectTableOccupancyRows <= 0 || readerCount == 0)
                    {
                        //切换table
                        currectTableStartRowIdx = 0;
                        if (tableIndex == tableCountList.Count - 1)
                        {
                            break;
                        }
                        currectTableOccupancyRows = tableCountList[++tableIndex];
                    }

                    if (singleExcelMaxCount >= Pub.DEFAULT_EXCEL_MAXCOUNT_FORINFO)
                    {
                        //Excel满了，退出当前Excel文件
                        break;
                    }

                    //切换Sheet
                    if ((singleExcelMaxCount > 0 && singleExcelMaxCount % Pub.DEFAULT_EXCELEXPORT_SHEET_MAXCOUNT == 0) || (currectSheetOccupancyRows == 0))
                    {
                        if (sheetIdx == sheetCount)
                        {
                            break;
                        }

                        sheet.ForceFormulaRecalculation = true;
                        sheetIdx++;
                        sheet = CreateSheet(workbook, sheetCount, sheetIdx, exportFileInfo.ExcelTitle, out currectExcelIdx);
                        currectSheetOccupancyRows = Pub.DEFAULT_EXCEL_MAXCOUNT_FORINFO - singleExcelMaxCount;
                    }
                    #endregion 

                }

                WriteWorkbookToFile(workbook, filePath);
            }
            finally
            {
                workbook.Clear();
                workbook = null;
                GC.Collect();
            }
        }


        protected void WriteWorkbookToFile(HSSFWorkbook workbook, string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Create))
            {
                workbook.Write(file);
            }
        }


        private void ListWriteExcelFile(int allRows, int sheetCount, ExportFileInfo exportFileInfo, string filePath, List<T> list, CancellationToken token, ref int currectListOccupancyRows, ref int dispId, Action<int> exportProgressAction)
        {
            int currectSheetOccupancyRows = Pub.DEFAULT_EXCELEXPORT_SHEET_MAXCOUNT;
            int sheetIdx = 1;
            int currectExcelIdx = 0; //当前excel操作行下标
            int currectExcelCount = 0;
            var workbook = new HSSFWorkbook();
            try
            {
                var sheet = CreateSheet(workbook, sheetCount, sheetIdx, exportFileInfo.ExcelTitle, out currectExcelIdx);

                int singleExcelMaxCount = Math.Min(Pub.DEFAULT_EXCEL_MAXCOUNT_FORINFO, allRows);
                while (currectExcelCount < singleExcelMaxCount)
                {
                    token.ThrowIfCancellationRequested();
                    int readerCount = Math.Min(Pub.DEFAULT_EXCELEXPORT_SHEET_MAXCOUNT, currectListOccupancyRows);
                    readerCount = Math.Min(readerCount, currectSheetOccupancyRows);

                    if (readerCount > 0)
                    {
                        var readList = list.GetRange(0, readerCount);
                        int writeCount = 0;
                        sheet = ListToSheet(workbook, sheet, readList, ref dispId, ref currectExcelIdx, exportProgressAction, token,out writeCount);
                        list.RemoveRange(0, readerCount);

                        singleExcelMaxCount -= readerCount;
                        currectListOccupancyRows -= readerCount;

                        currectExcelCount += writeCount;
                    }

                    #region state update

                    if (currectListOccupancyRows <= 0 || readerCount == 0)
                    {
                        break;
                    }

                    if (singleExcelMaxCount >= Pub.DEFAULT_EXCEL_MAXCOUNT_FORINFO)
                    {
                        //Excel满了，退出当前Excel文件
                        break;
                    }

                    //切换Sheet
                    if ((singleExcelMaxCount > 0 && singleExcelMaxCount % Pub.DEFAULT_EXCELEXPORT_SHEET_MAXCOUNT == 0) || (currectSheetOccupancyRows == 0))
                    {
                        if (sheetIdx == sheetCount)
                        {
                            break;
                        }

                        sheet.ForceFormulaRecalculation = true;
                        sheetIdx++;
                        sheet = CreateSheet(workbook, sheetCount, sheetIdx, exportFileInfo.ExcelTitle, out currectExcelIdx);
                        currectSheetOccupancyRows = Pub.DEFAULT_EXCEL_MAXCOUNT_FORINFO - singleExcelMaxCount;
                    }
                    #endregion 
                }

                WriteWorkbookToFile(workbook, filePath);
            }
            finally
            {
                workbook.Clear();
                workbook = null;
                GC.Collect();
            }
        }

        private ISheet ListToSheet(HSSFWorkbook workbook, ISheet sheet, IList<T> list, ref int dispId, ref int currectExcelIdx, Action<int> updateProgress, CancellationToken token,out int writeCount)
        {
            writeCount = 0;
            if (list == null) return sheet;
            foreach (var dbModel in list)
            {
                token.ThrowIfCancellationRequested();
                sheet = AddRowToSheet(workbook, dbModel, sheet, currectExcelIdx);
                updateProgress(dispId);
                dispId++;
                currectExcelIdx++;
                writeCount++;
            }
            return sheet;
        }


        #region DataReaderToSheet
        protected ISheet DataReaderToSheet(HSSFWorkbook workbook, ISheet sheet, MySqlConnection conn, string commandText, ref int dispId, ref int currectExcelIdx, Action<int> exportProgressAction, CancellationToken token,out int wirteCount)
        {
            wirteCount = 0;
            using (var reader = MySqlHelper.ExecuteReader(conn, commandText))
            {
                if (!reader.HasRows) return sheet;
                HashSet<string> tableNamesCollection = reader.CollectColumnNames();
                while (reader.Read())
                {
                    token.ThrowIfCancellationRequested();
                    if (reader.IsDBNull(0) && reader.IsDBNull(1)) continue;
                    var entity= ReaderDataRecordConvert(reader, ref tableNamesCollection);
                    if (entity == null )
                    {
                        continue;
                    }
                    entity.Disp = dispId;
                    sheet = AddRowToSheet(workbook, entity, sheet, currectExcelIdx);
                    exportProgressAction(dispId);
                    wirteCount++;
                    dispId++;
                    currectExcelIdx++;
                }
            }
            return sheet;
        }
        /// <summary>
        /// 自动读取DataRecord数据，如需可重写该方法（如直接读取IDataRecord数据 DataNum=(int)reader["DataNum"]）。
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <param name="tableNamesCollection"></param>
        /// <returns></returns>
        protected virtual T ReaderDataRecordConvert(IDataRecord dataRecord, ref HashSet<string> tableNamesCollection)
        {
            if (dataRecord == null) return default(T);
            var model = dataRecord.ReaderEntity<T>(ref tableNamesCollection);

            return model;
        }

        #endregion
        #endregion

        #region 样式扩展


        /// <summary>
        /// 扩展标题头样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual ICellStyle SetHeaderStyle(HSSFWorkbook workbook, PropertyInfo propertyInfo)
        {
            if (propertyInfo.Name.Contains("DataContent"))
            {
                return ExcelStyle.HeaderLeftStyle(workbook); //设置居左
            }
            return ExcelStyle.HeaderStyle(workbook); //设置Header剧中
        }

        /// <summary>
        /// 设置行的样式，常用在Cdr记录导出时，设置颜色
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual ICellStyle SetCdrRowForeground(HSSFWorkbook workbook, T model)
        {
            return null;
        }


        /// <summary>
        /// 设定某一列的样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="propertyName"></param>
        /// <param name="cdrRowStyle">如果设置了行样式，列的样式设置会被覆盖，仅返回行样式</param>
        /// <returns></returns>
        private ICellStyle SetCdrCellStyle(HSSFWorkbook workbook,PropertyInfo propertyInfo,T dbModel, ICellStyle cdrRowStyle)
        {
            if (cdrRowStyle != null)
            {
                return cdrRowStyle;
            }

            return ExcelStyle.CenterStyle(workbook);
        }

        /// <summary>
        /// 设置导出单元格数据格式
        /// </summary>
        /// <param name="exportCellHead"></param>
        /// <param name="value"></param>
        /// <param name="dbModel"></param>
        /// <returns></returns>
        protected virtual object CellValueConvert(PropertyInfo propertyInfo,object value, T dbModel)
        {
            return value;
        }

        #endregion 

        #region private methods

        #region Create Query Sql Str
        private string CreateQuerySqlStrAction(string querySqlPart, string tableName, string whereSqlPart, int startIdx, int readerCount)
        {
            string commandText = BuildQueryLimitCommandText(querySqlPart,
                                    tableName,
                                    whereSqlPart, startIdx, readerCount);
            return commandText;
        }

        private static string BuildQueryLimitCommandText(string querySqlPart, string tableName, string paramesStr,
          int limitStart,
          int limitEnd)
        {
            paramesStr = paramesStr ?? "";

            string commandText = string.Format(querySqlPart, tableName, paramesStr, limitStart, limitEnd);
            return commandText;
        }
        #endregion

        #region AddRowToSheet
        private ISheet AddRowToSheet(HSSFWorkbook workbook, T dbModel, ISheet sheet, int rowIndex)
        {
            IRow rowTmp = sheet.CreateRow(rowIndex);
            ICellStyle cdrRowStyle = SetCdrRowForeground(workbook, dbModel);
            for (int i = 0; i < ExportCellHeadList.Count; i++)
            {
                object properotyValue = null;
                System.Reflection.PropertyInfo properotyInfo = null;


                properotyInfo = ExportCellHeadList[i].Property;
                var type = properotyInfo.PropertyType;
                if (properotyInfo != null)
                {
                    properotyValue = properotyInfo.GetValue(dbModel, null);
                }
                if (properotyValue == null)
                {
                    properotyValue = string.Empty;
                }

                properotyValue = CellValueConvert(properotyInfo, properotyValue, dbModel);

                ICell cell = rowTmp.CreateCell(i);

                cell.CellStyle = SetCdrCellStyle(workbook,properotyInfo,dbModel, cdrRowStyle);

                if (properotyValue == null)
                {
                    cell.SetCellValue("");
                }
                else if (properotyValue is DateTime)
                {
                    cell.SetCellValue((DateTime)properotyValue);
                }
                else if (properotyValue is int)
                {
                    cell.SetCellValue((int)properotyValue);
                }
                else if (properotyValue is long)
                {
                    cell.SetCellValue((long)properotyValue);
                }
                else if (properotyValue is double)
                {
                    cell.SetCellValue((double)properotyValue);
                }
                else if (properotyValue is float)
                {
                    cell.SetCellValue((float)properotyValue);
                }
                else
                {
                    cell.SetCellValue(properotyValue.ToString());
                }
            }

            return sheet;
        }
        #endregion

        #region Create Sheet and Head
        private ISheet CreateSheet(HSSFWorkbook workbook, int sheetCount, int sheetIdx, string title, out int occupancyRows)
        {
            ExcelStyle.InitSheetStyle();
            occupancyRows = 0;
            ISheet sheet = workbook.CreateSheet("Sheet" + sheetIdx);
            string strTitle = title + (sheetCount == 1 ? "" : " (" + sheetIdx + ")");
            sheet = ExcelStyle.SetSheeetParams(sheet);
            occupancyRows = CreateSheetTitle(workbook, sheet, strTitle, occupancyRows);
            occupancyRows = CreateSheetHeads(workbook, sheet, occupancyRows);
            return sheet;
        }

        private int CreateSheetTitle(HSSFWorkbook workbook, ISheet sheet, string title, int titleOccupancyRows)
        {
            IRow row = sheet.CreateRow(titleOccupancyRows);
            row.HeightInPoints = 25;
            ICell cell = row.CreateCell(0);
            cell.CellStyle = ExcelStyle.GetStyleTitle(workbook);
            cell.SetCellValue(title);
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, ExportCellHeadList.Count - 1));

            titleOccupancyRows++;
            return titleOccupancyRows;
        }

        private int CreateSheetHeads(HSSFWorkbook workbook, ISheet sheet, int occupancyRows)
        {
            IRow row = sheet.CreateRow(occupancyRows);
            ICell cell = null;
            for (int i = 0; i < ExportCellHeadList.Count; i++)
            {
                sheet.SetColumnWidth(i, ExportCellHeadList[i].Width);
                cell = row.CreateCell(i);
                cell.CellStyle = SetHeaderStyle(workbook, ExportCellHeadList[i].Property);
                cell.SetCellValue(ExportCellHeadList[i].Name);
            }
            occupancyRows++;
            return occupancyRows;
        }

        #endregion

        #endregion

    }
}
