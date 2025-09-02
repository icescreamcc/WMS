using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Common
{
   public class ExcelHelper
    {
        /// <summary>
        /// 通过Stream获取Datatable
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileType">Excel文件类型（xls或xlsx）</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns></returns> 
        public static DataTable ConvertStreamToDataTable(Stream stream, string fileType, string sheetName, bool isFirstRowColumn = true, int FirstRow = 0)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            using (stream)
            {
                if (fileType == ".xlsx")
                {
                    workbook = new XSSFWorkbook(stream);//2007版本

                }
                else if (fileType == ".xls")
                {
                    workbook = new HSSFWorkbook(stream);//2003版本
                }
                else
                {
                    throw new Exception("Excel文件类型不正确");
                }
                if (sheetName != "")
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }

                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);//第一行数据
                                                    //IRow firstRow = sheet.GetRow(0);//第一行数据
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            var cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = string.IsNullOrEmpty(cell.StringCellValue) ? "" : cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        //startRow = sheet.FirstRowNum + 1;
                        startRow = FirstRow == 0 ? sheet.FirstRowNum + 1 : FirstRow;
                    }
                    else //第几行开始读取数据 
                    {
                        firstRow = sheet.GetRow(0);//第一行数据
                        for (int i = firstRow.FirstCellNum; i < cellCount; i++)
                        {
                            var cell = firstRow.GetCell(i);
                            if (cell != null && cell.IsMergedCell)
                            {
                                int mergedRegionIndex = GetMergedRegionIndex(sheet, cell.RowIndex, cell.ColumnIndex);
                                if (mergedRegionIndex >= 0)
                                {
                                    IRow row = sheet.GetRow(cell.RowIndex);
                                    var mergedCell = row.GetCell(cell.ColumnIndex);
                                    string mergedCellValue = string.IsNullOrEmpty(mergedCell.StringCellValue) ? "" : mergedCell.StringCellValue;
                                    if (mergedCellValue != null)
                                    {
                                        DataColumn column = new DataColumn(mergedCellValue);
                                        //data.Columns[column.ColumnName] = column;
                                        data.Columns.Add(column);
                                    }
                                }
                            }
                        }
                        ////startRow = sheet.FirstRowNum;
                        //for (int i = firstRow.FirstCellNum; i < cellCount; i++)
                        //{
                        //    ICell cell = firstRow.GetCell(i);
                        //    if (cell != null)
                        //    {
                        //        string cellValue = string.IsNullOrEmpty(cell.StringCellValue) ? "" : cell.StringCellValue;
                        //        if (cellValue != null)
                        //        {
                        //            DataColumn column = new DataColumn(i.ToString());
                        //            data.Columns.Add(column);
                        //            //DataColumn column = new DataColumn(cellValue);
                        //            //data.Columns.Add(column);
                        //        }
                        //    }

                        //}
                        startRow = FirstRow == 0 ? sheet.FirstRowNum : FirstRow;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        else
                        {
                            bool isNullRow = true;
                            for (int j = row.FirstCellNum; j < cellCount; ++j)
                            {
                                //需要判断当前行如果所有单元格都没有数据，则不添加到table中
                                var curCellVal = row.GetCell(j);
                                if (curCellVal != null && !string.IsNullOrWhiteSpace(curCellVal.ToString()))
                                {
                                    isNullRow = false;
                                    break;
                                }
                            }
                            if (!isNullRow)
                            {
                                DataRow dataRow = data.NewRow();
                                for (int j = row.FirstCellNum; j < cellCount; ++j)
                                {
                                    if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                    {
                                        dataRow[j] = row.GetCell(j).ToString();
                                        if (row.GetCell(j).CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(row.GetCell(j)))//如果类型是日期形式的，转成合理的日期字符串
                                        {
                                            dataRow[j] = row.GetCell(j).DateCellValue.ToString();
                                        }
                                        //if (row.GetCell(j).CellType == CellType.Formula)
                                        //{
                                        //    dataRow[j] = row.GetCell(j).NumericCellValue.ToString();
                                        //}
                                    }
                                }
                                data.Rows.Add(dataRow);
                            }
                        }

                    }
                }

                return data;
            } 
        
        }
        private static int GetMergedRegionIndex(ISheet sheet, int rowIndex, int colIndex)
        {
            for (int i = 0; i < sheet.NumMergedRegions; i++)
            {
                CellRangeAddress mergedRegion = sheet.GetMergedRegion(i);
                if (rowIndex >= mergedRegion.FirstRow && rowIndex <= mergedRegion.LastRow &&
                    colIndex >= mergedRegion.FirstColumn && colIndex <= mergedRegion.LastColumn)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 把DataTable的内容生产Excel并作为Stream输出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <returns></returns>
        public static Stream ConvertDataTableToStream(DataTable dt, string sheetName="Sheet1")
        {
            var ms = new NpoiMemoryStream();
            using (dt)
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(sheetName); 
                ICellStyle headerStyle = workbook.CreateCellStyle();
                headerStyle.FillForegroundColor = IndexedColors.Yellow.Index;
                headerStyle.FillPattern = FillPattern.SolidForeground;
                var headerRow = sheet.CreateRow(0);
                // handling header.
                foreach (DataColumn column in dt.Columns)
                {
                    var cell = headerRow.CreateCell(column.Ordinal);
                    cell.SetCellValue(column.Caption);
                    if (column.ColumnName.Contains("*"))
                        cell.CellStyle = headerStyle;
                }

                // handling value.
                int rowIndex = 1;
                foreach (DataRow row in dt.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in dt.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }

        class NpoiMemoryStream : MemoryStream
        {
            /// <summary>
            /// 获取流是否关闭
            /// </summary>
            public bool IsColse
            {
                get;
                private set;
            }

            public NpoiMemoryStream(bool colse = false)
            {
                IsColse = colse;
            }

            public override void Close()
            {
                if (IsColse)
                {
                    base.Close();
                }

            }
        }
    } 
}
