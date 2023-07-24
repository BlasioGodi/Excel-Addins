using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Text;

using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;

using IronXL;

namespace ExcelReaderTwo
{
    public class ExcelReader
    {
        static void Main()
        {
            ///<Interop>Using Excel Interop</Interop>
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook wb = excelApp.Workbooks.Open(@"D:\Git\Excel-Addins\References\CUEA Drawing List.xlsx");
            Excel.Worksheet ws = wb.Worksheets[1] ;
            Excel.Range excelRange = ws.UsedRange;

            int rows = excelRange.Rows.Count;
            int columns = excelRange.Columns.Count;

            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= columns; j++)
                {
                    if (j == 1)
                        Console.Write("\r\n");

                    if (excelRange.Cells[i,j]!=null && excelRange.Cells[i,j].Value != null)
                    {
                        Console.Write(excelRange.Cells[i, j].Value.ToString() + "\t");
                    }
                }
            }

            ///<IronXL>Getting data using IronXL</IronXL>
            WorkBook workBook = WorkBook.Load(@"C:\Users\user\Desktop\Computer Science\2. Projects\Projects4-Excel Addins\References\CUEA Drawing List.xlsx");
            WorkSheet workSheet = workBook.DefaultWorkSheet;

            ///<ExcelClear>Clear Excel and close the local files.</ExcelClear>
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(excelRange);
            Marshal.ReleaseComObject(ws);

            wb.Close();
            Marshal.ReleaseComObject(wb);

            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);
        }
    }
}