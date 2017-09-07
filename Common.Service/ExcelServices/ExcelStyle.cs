using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service.ExcelServices
{
    public class ExcelStyle
    {
        #region excel style

        /// <summary>
        /// 创建Sheet时初始化Style
        /// </summary>
        public void InitSheetStyle()
        {
            _titleStyle = null;
            _headerStyle = null;
            _headerLeftStyle = null;
            _centerStyle = null;
            _leftSytle = null;
            _centerRedStyle = null;
            _centerOrangeStyle = null;

            _statisticSytle = null;
            _blackLeftStyle = null;
        }

        private ICellStyle _titleStyle;

        public ICellStyle GetStyleTitle(HSSFWorkbook workbook)
        {
            return _titleStyle ?? (_titleStyle = CreateCellStyle(workbook, HSSFColor.COLOR_NORMAL, 16,
                HorizontalAlignment.Center, false, false));
        }

        private ICellStyle _headerStyle;

        public ICellStyle HeaderStyle(HSSFWorkbook workbook)
        {
            return _headerStyle ?? (_headerStyle = CreateCellStyle(workbook, HSSFColor.COLOR_NORMAL, 0,
                HorizontalAlignment.Center, true, true));
        }

        private ICellStyle _headerLeftStyle;

        public ICellStyle HeaderLeftStyle(HSSFWorkbook workbook)
        {
            return _headerLeftStyle ?? (_headerLeftStyle = CreateCellStyle(workbook, HSSFColor.COLOR_NORMAL, 0,
                HorizontalAlignment.Left, true, true));
        }


        public ICellStyle SetHeaderStyle(HSSFWorkbook workbook, string propertyName)
        {
            return HeaderStyle(workbook);
        }

        private ICellStyle _centerStyle;

        public ICellStyle CenterStyle(HSSFWorkbook workbook)
        {
            return _centerStyle ??
                   (_centerStyle = CreateCellStyle(workbook, HSSFColor.COLOR_NORMAL, 0, HorizontalAlignment.Center,
                       false, true));
        }

        private ICellStyle _leftSytle;

        public ICellStyle LeftStyle(HSSFWorkbook workbook)
        {

            return _leftSytle ??
                   (_leftSytle = CreateCellStyle(workbook, HSSFColor.COLOR_NORMAL, 0, HorizontalAlignment.Left,
                       false, true));
        }

        private ICellStyle _centerRedStyle;

        public ICellStyle CenterRedStyle(HSSFWorkbook workbook)
        {
            return _centerRedStyle ?? (_centerRedStyle = CreateCellStyle(workbook, HSSFColor.Red.Index, 0,
                NPOI.SS.UserModel.HorizontalAlignment.Center, false, true));
        }


        private ICellStyle _centerOrangeStyle;

        public ICellStyle CenterOrangeStyle(HSSFWorkbook workbook)
        {
            return _centerOrangeStyle ?? (_centerOrangeStyle = CreateCellStyle(workbook, HSSFColor.Orange.Index, 0,
                NPOI.SS.UserModel.HorizontalAlignment.Center, false, true));
        }



        private ICellStyle _statisticSytle;

        public ICellStyle StatisticSytle(HSSFWorkbook workbook)
        {
            return _statisticSytle ?? (_statisticSytle = CreateCellStyle(workbook, HSSFColor.COLOR_NORMAL, 0,
                HorizontalAlignment.Left, true, false));
        }

        private ICellStyle _blackLeftStyle;

        public ICellStyle BlackLeftStyle(HSSFWorkbook workbook)
        {
            return _blackLeftStyle ??
                   (_blackLeftStyle = CreateCellStyle(workbook, HSSFColor.COLOR_NORMAL, 0, NPOI.SS.UserModel.HorizontalAlignment.Left,
                           false, false));
        }



        public ICellStyle CreateCellStyle(IWorkbook workbook, short fontColorIdx, int fontSize,
            HorizontalAlignment horizontalAlignment, bool isBold, bool hasBorder)
        {
            ICellStyle style = workbook.CreateCellStyle();

            if (hasBorder)
            {
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
            }

            style.Alignment = horizontalAlignment;
            style.VerticalAlignment = VerticalAlignment.Center;

            IFont font = workbook.CreateFont();

            font.Color = fontColorIdx;

            if (isBold)
                font.Boldweight = (short)FontBoldWeight.Bold;

            if (fontSize > 0)
                font.FontHeightInPoints = (short)fontSize;

            style.SetFont(font);

            return style;
        }

        public ISheet SetSheeetParams(ISheet sheet)
        {
            sheet.PrintSetup.Landscape = true;
            sheet.PrintSetup.Scale = 100;
            sheet.PrintSetup.FitHeight = 0;
            sheet.PrintSetup.FitWidth = 0;
            sheet.PrintSetup.PaperSize = (short)PaperSize.A4;

            return sheet;
        }

        #endregion
    }
}
