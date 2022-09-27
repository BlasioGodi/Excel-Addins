using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

using IronXL;

namespace ExcelPlugin1
{
    public partial class ExcelReader : Form
    {
        public ExcelReader()
        {
            InitializeComponent();
        }

        private DataTable ExcelReading(string fileName)
        {
            WorkBook workBook = WorkBook.Load(fileName);

            //// Work with a single WorkSheet.
            ////you can pass static sheet name like Sheet1 to get that sheet
            ////WorkSheet sheet = workbook.GetWorkSheet("Sheet1");
            WorkSheet workSheet = workBook.DefaultWorkSheet;

            return workSheet.ToDataTable();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void ReadExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFile.FileName;
                string fileExt = Path.GetExtension(filePath);

                if (fileExt.CompareTo(".xlsx") == 0 || fileExt.CompareTo(".xls") == 0)
                {
                    try
                    {
                        DataTable data = ExcelReading(filePath);
                       
                        dataGridView1.Visible = true;
                        dataGridView1.DataSource = data;

                        //Excel cleanup after task execution
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    catch(Exception except)
                    {
                        MessageBox.Show(except.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }            
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
