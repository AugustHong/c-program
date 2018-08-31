using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;  //要讀取excel
using System.Data;  //DataSet用的
using System.Windows.Forms;

namespace ImportExcel
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {

            //用類似sql的語法來讀出
            DataSet ds = null;
            OleDbConnection conn;

            string strConn = string.Empty;   //連線字串名稱
            string sheetName = string.Empty;  //sheet名稱

            //開啟檔案
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "選擇文件";
            openFileDialog.Filter = "Excel2010以後|*.xlsx|Excel2010前|*.xls";
            openFileDialog.FileName = string.Empty;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.DefaultExt = "xlsx";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel) { return; }

            string filePath = openFileDialog.FileName;  //檔案路徑

            try
            {
                // Excel 2003 版本連線字串
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0; HDR=YES; IMEX=1;'";
                conn = new OleDbConnection(strConn);
                conn.Open();
            }
            catch
            {
                // Excel 2007 以上版本連線字串
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";
                conn = new OleDbConnection(strConn);
                conn.Open();
            }

            //獲取所有的 sheet 表
            DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

            ds = new DataSet();

            for (int i = 0; i < dtSheetName.Rows.Count; i++)
            {
                DataTable dt = new DataTable();
                dt.TableName = "table" + i.ToString();

                //獲取表名
                sheetName = dtSheetName.Rows[i]["TABLE_NAME"].ToString();

                OleDbDataAdapter oleda = new OleDbDataAdapter("select * from [" + sheetName + "]", conn);

                oleda.Fill(dt);

                ds.Tables.Add(dt);
            }

            //關閉連線，釋放資源
            conn.Close();
            conn.Dispose();


            //如果ds是有資料的
            if(ds != null && ds.Tables.Count > 0)
            {
                //把每個table跑過（如果想要一個一個看，用for(var i = 0; i < ds.Tables.Count;i++){ds.Tables[i]}
                foreach (DataTable table in ds.Tables)
                {
                    //每個row都跑過
                    foreach (DataRow row in table.Rows)
                    {
                        for(var i = 0; i < row.ItemArray.Count(); i++)
                        {
                            Console.Write("{0} ", row.ItemArray[i]);
                        }
                        Console.WriteLine("");
                    }
                }
            }

            Console.Read();
        }
    }
}
