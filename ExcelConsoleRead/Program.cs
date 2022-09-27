using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace ExcelConsoleRead
{
    class ReadExcelFile
    {
        public Range range;

        public ReadExcelFile(Range range)
        {
            this.range = range;
        }

        public void ReadExcel(int tableRows, int tableColumns)
        {
            for (int i = 1; i <= tableRows; i++)
            {
                for (int j = 1; j <= tableColumns; j++)
                {
                    //new line
                    if (j == 1)
                        Console.Write("\r\n");

                    //write the value to the console
                    if (range.Cells[i, j] != null && range.Cells[i,j].Value != null)
                        Console.Write(range.Cells[i, j].Value.ToString() + "\t");
                }
            }
        }
    }
    class ExcelConsoleRead
    {
        static void Main()
        {
            Application excelApp = new Application();
            Workbook wb = excelApp.Workbooks.Open(@"C:\Users\user\Documents\GUESTS LIST.xlsx");
            Worksheet ws = wb.Worksheets[1];
            Range range = ws.UsedRange;

            int rows = range.Rows.Count;
            int columns = range.Columns.Count;

            ReadExcelFile excelFile = new ReadExcelFile(range);

            excelFile.ReadExcel(rows, columns);

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(range);
            Marshal.ReleaseComObject(ws);

            //close and release
            wb.Close();
            Marshal.ReleaseComObject(wb);

            //quit and release
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);
        }
    }
}
