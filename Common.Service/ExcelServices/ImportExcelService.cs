using Common.Domain;
using Framework.CustomException;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Service.ExcelServices
{
    public abstract class ImportExcelService<T> : ExportExcelService<T> where T : DbModel, new()
    {

        public List<T> GetDataFromExcel(string filePath, CancellationToken token, out StringBuilder errorMsg, out int excelRowCount, Action<int, int> getDataProgressAction)
        {
            errorMsg = new StringBuilder();
            excelRowCount = 0;
            var list = new List<T>();
            if (CellHeadListByIndex == null || !CellHeadListByIndex.Any())
            {
                throw new TException<ImportExcelException>("CellHeadListByIndex is null");
            }
            bool foundHeader = false;
            using (var fs = System.IO.File.Open(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                int readCount = 0;
                int allCount = 0;

                HSSFWorkbook workbook = new HSSFWorkbook(fs);
                int sheetCount = workbook.NumberOfSheets;
                if (sheetCount == 0) return null;

                HSSFSheet sheet;
                for (int index = 0; index < sheetCount; index++)
                {
                    sheet = workbook.GetSheetAt(index) as HSSFSheet;
                    if (sheet == null) continue;
                    if (sheet.PhysicalNumberOfRows <= 1) continue;

                    token.ThrowIfCancellationRequested();

                    int endRow = sheet.FirstRowNum + Common.Pub.DEFAULT_EXCEL_SEARCHROWCOUNT;
                    int dataStartRow = -1;
                    CellHeadListByIndex = GetTitleIdx(sheet, endRow, CellHeadListByIndex, out dataStartRow, out foundHeader);
                    if (dataStartRow <= 0) continue;
                    if (!foundHeader) continue;


                    #region readSheet
                    for (int rowIndex = dataStartRow; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        IRow row = sheet.GetRow(rowIndex);
                        if (row == null) continue;
                        if (row.Cells.Any(c => c.CellType == CellType.Blank)) continue;
                        token.ThrowIfCancellationRequested();

                        allCount += sheet.LastRowNum;

                        T dbModel = ReadCells(row, CellHeadListByIndex);
                        if (dbModel == null) continue;
                        list.Add(dbModel);
                        if (getDataProgressAction != null)
                        {
                            getDataProgressAction.Invoke(++readCount, allCount);
                        }
                    }
                    #endregion 
                }

            }
            ValidateHasHead(foundHeader, CellHeadListByIndex);

            excelRowCount = list.Count;
            return FilterData(list);
        }

        private IList<ExcelInfo> GetTitleIdx(HSSFSheet sheet, int endRow, IList<ExcelInfo> excelInfos, out int dataStartRow, out bool foundHeader)
        {
            dataStartRow = -1;
            foundHeader = false;

            if (excelInfos == null) return null;

            for (int rowIndex = sheet.FirstRowNum; rowIndex < endRow + sheet.FirstRowNum; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                if (row == null) continue;
                foreach (var headInfo in excelInfos)
                {
                    for (int cellIndex = 0; cellIndex < row.PhysicalNumberOfCells; cellIndex++)
                    {
                        ICell cell = row.GetCell(cellIndex);
                        if (cell == null) continue;
                        if (cell.CellType != CellType.String) continue;
                        string strCellValue = cell.StringCellValue;
                        if (string.IsNullOrWhiteSpace(strCellValue)) continue;

                        if (strCellValue.Equals(headInfo.Name, StringComparison.CurrentCultureIgnoreCase))
                        {
                            headInfo.Index = cellIndex;
                        }
                    }
                    //CellIndex初始化时值为-1，如果存在-1 说明有必要列未找到
                    var hasItem = excelInfos.FirstOrDefault(item => item.Index == -1);
                    if (hasItem == null)
                    {
                        //success
                        dataStartRow = rowIndex + 1;
                        foundHeader = true;
                    }
                }
            }
            return excelInfos;
        }

        private void ValidateHasHead(bool foundHeader, IList<ExcelInfo> importCellHeadList)
        {
            if (!foundHeader)
            {
                StringBuilder message = new StringBuilder();
                string str = string.Format("Excel文件格式错误。 扫描文件前 {0} 行，未发现列头，或所含信息列数量不符。", Common.Pub.DEFAULT_EXCEL_SEARCHROWCOUNT);
                StringBuilder sb = new StringBuilder();
                StringBuilder scarcityTitle = new StringBuilder();
                foreach (var cellHead in importCellHeadList)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("，");
                    }
                    sb.Append(cellHead.Name);

                    if (cellHead.Index == -1)
                    {
                        if (scarcityTitle.Length > 0)
                        {
                            scarcityTitle.Append("，");
                        }
                        scarcityTitle.Append(cellHead.Name);
                    }
                }
                message.AppendLine(str);
                message.AppendLine("信息列至少包含：");
                message.Append(sb.ToString());
                message.AppendLine("。");

                throw new TException<ImportExcelException>(message.ToString());
            }
        }

        /// <summary>
        /// 读取列的具体实现
        /// </summary>
        /// <param name="row"></param>
        /// <param name="sheet"></param>
        /// <param name="importExcelInfos"></param>
        /// <returns></returns>
        protected virtual T ReadCells(IRow row, IList<ExcelInfo> cellHeadListByIndex)
        {
            var entity = new T();

            foreach (var item in cellHeadListByIndex)
            {
                var cell = row.GetCell(item.Index);
                if (cell != null)
                {
                    var value = GetCellValue(cell);
                    value = ValueConvert(value,item.Property);
                    value = (value == null || (value as string) == "") ? null : Convert.ChangeType(value, item.Property.PropertyType, System.Globalization.CultureInfo.InvariantCulture);
                    item.Property.SetValue(entity, value, null);
                }
            }

            return entity;
        }

        protected virtual object ValueConvert(object value, System.Reflection.PropertyInfo propertyInfo)
        {
            return value;
        }

        object GetCellValue(ICell cell)
        {
            switch (cell.CellType)
            {
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                        return cell.DateCellValue;
                    else
                        return cell.NumericCellValue;
                case CellType.Formula:
                    return cell.CellFormula;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Error:
                    return cell.ErrorCellValue;
                case CellType.Unknown:
                case CellType.Blank:
                case CellType.String:
                default:
                    return cell.StringCellValue;
            }
        }

        /// <summary>
        /// 导入Excel函数需要重写过滤策略,过滤excel内部重复、冲突的数据
        /// </summary>
        /// <param name="rawDataList"></param>
        /// <returns></returns>
        protected virtual List<T> FilterData(List<T> rawDataList)
        {
            return rawDataList;
        }
        
    }
}
