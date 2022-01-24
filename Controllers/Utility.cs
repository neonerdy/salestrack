

/*--------------------------------------------------
 *
 *  CORBIS 
 * 
 *  Version : 1.0
 *  Author  : Ariyanto
 *  E-mail  : neonerdy@gmail.com
 * 
 *  © 2021, All Right Reserved  
 * 
 *--------------------------------------------------
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

using SalesTrack.Models;



namespace SalesTrack.Controllers
{
    public class Utility
    {



        public static CustomFile CreateExcelFile(IWorkbook workbook, string excelName)
        {
            string contentType = "";
            MemoryStream tempStream = null;
            MemoryStream stream = null;
            try
            {
                // 1. Write the workbook to a temporary stream
                tempStream = new MemoryStream();
                workbook.Write(tempStream);
                // 2. Convert the tempStream to byteArray and copy to another stream
                var byteArray = tempStream.ToArray();
                stream = new MemoryStream();
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Seek(0, SeekOrigin.Begin);
                // 3. Set file content type
                contentType = workbook.GetType() == typeof(XSSFWorkbook) ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/vnd.ms-excel";
                // 4. set to custom file
                CustomFile file = new CustomFile();
                file = new CustomFile
                {
                    FileContents = stream.ToArray(),
                    ContentType = contentType,
                    FileName = excelName + ((workbook.GetType() == typeof(XSSFWorkbook)) ? ".xlsx" : "xls")
                };

                return file;
            }
            finally
            {
                if (tempStream != null) tempStream.Dispose();
                if (stream != null) stream.Dispose();
            }
        }



        public static IWorkbook WriteExcel(DataTable dt, string extension = "xlsx")
        {
            // Instantiate Wokrbook
            IWorkbook workbook;
            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                throw new Exception("The format '" + extension + "' is not supported.");
            }

            ISheet sheet1 = workbook.CreateSheet("Sheet 1");

            var font = workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "Calibri";
            font.IsBold = true;


            //make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < dt.Columns.Count; j++)
            {
                ICell cell = row1.CreateCell(j);
                string columnName = dt.Columns[j].ToString().Replace("_", " ");
                cell.CellStyle = workbook.CreateCellStyle();
                cell.CellStyle.SetFont(font);
                cell.SetCellValue(columnName);

            }

            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    string columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                    cell.CellStyle.WrapText = true; // NOT WORKING
                }
            }
            // Auto size columns
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < row1.LastCellNum; j++)
                {
                    sheet1.AutoSizeColumn(j);
                }
            }

            return workbook;
        }

    }


}