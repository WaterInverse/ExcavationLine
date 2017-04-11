using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
//using NPOI.OpenXml4Net;

namespace Common
{
    public class ExcelOprate
    {
        private string excelFilePath;

        public string ExcelFilePath
        {
            get
            {
                return excelFilePath;
            }

            set
            {
                excelFilePath = value;
            }
        }

        public ExcelOprate()
        {
        }

        public ExcelOprate(string excelFilePath)
        {
            this.ExcelFilePath = excelFilePath;
        }


        /// <summary>
        /// 创建excel表
        /// </summary>
        /// <param name="sheetName">sheet名</param>
        /// <param name="matrix">数据</param>
        /// <param name="savePath">保存路径</param>
        public void CreateExcel(string sheetName, List<List<double>> dataList, string savePath)
        {
            IWorkbook workbook = null;
            if (savePath.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (savePath.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);

            #region 单元格创建样例（必须新建，否则报错）
            ////在第一行创建行  
            //IRow row = sheet.CreateRow(0);
            ////在第一行的第一列创建单元格  
            //ICell cell = row.CreateCell(0);
            //cell.SetCellValue("测试");
            #endregion

            int rows = dataList.Count;
            //int columns = matrix.Columns;
            if (rows == 0)
            {
                return;
            }
            for (int i = 0; i < rows; i++)
            {
                int columns = dataList[i].Count;
                IRow row = sheet.CreateRow(i);
                for (int j = 0; j < columns; j++)
                {
                    row.CreateCell(j).SetCellValue(dataList[i][j]);
                }
            }
            using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                workbook.Write(fs);//向打开的这个xls文件中写入并保存。  
            }
        }

        /// <summary>
        /// 读取excel中数据
        /// </summary>
        /// <param name="sheetName">sheet名</param>
        /// <returns></returns>
        public List<List<double>> ExcelToList(string sheetName)
        {
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<List<double>> dataList = new List<List<double>>();
            using (FileStream fs = new FileStream(ExcelFilePath, FileMode.Open, FileAccess.Read))
            {
                if (ExcelFilePath.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (ExcelFilePath.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);
            }
            try
            {               
                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    //List<ICell> rowt = sheet.GetRow(sheet.LastRowNum).Cells;
                    //string str1= rowt[6].ToString();
                    //Console.WriteLine(str1+str1.Length);
                    int rowCount = sheet.LastRowNum+1;
                    for (int i = 0; i < rowCount; i++)
                    {
                        List<ICell> row = sheet.GetRow(i).Cells;
                        int columnCount = row.Count;
                        List<double> columns = new List<double>();
                        for (int j = 0; j < columnCount; j++)
                        {
                            string str = row[j].ToString();
                            if (str.Length != 0 && str != " " && str != string.Empty && str != null)
                            {
                                columns.Add(Convert.ToDouble(sheet.GetRow(i).GetCell(j).ToString()));
                            }
                        }
                        dataList.Add(columns);
                    }

                }
                return dataList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
    }
}
