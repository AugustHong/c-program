using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;  //要讀取excel
using System.Data;  //DataSet用的
using System.Windows.Forms;  //要去 參考/加入參考 新增

/*
	如果有出現OleDb的問題，請去這裡 https://devmanna.blogspot.com/2017/03/sql-server-excel-microsoftaceoledb120.html 安裝
*/


namespace ImportExcel
{
    class Program
    {
        [STAThreadAttribute]  //Dialog要用的
        static void Main(string[] args)
        {

            //用類似sql的語法來讀出
            DataSet ds = null;
            OleDbConnection ExcelConn = null;

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

            //取得副檔名
            string extension = System.IO.Path.GetExtension(filePath);

            switch (extension)
            {
                case ".xls":
                    // Excel 2003 版本連線字串
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0; HDR=YES; IMEX=1;'";
                    ExcelConn = new OleDbConnection(strConn);
                    ExcelConn.Open();
                    break;

                case ".xlsx":
                    // Excel 2007 以上版本連線字串
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";
                    ExcelConn = new OleDbConnection(strConn);
                    ExcelConn.Open();
                    break;

                default:
                    return;
            }


            //獲取所有的 sheet 表
            DataTable dtSheetName = ExcelConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

		//DataSet把他當作一個類似Excel的儲存資料型別
            ds = new DataSet();

            for (int i = 0; i < dtSheetName.Rows.Count; i++)
            {
			//DataTable就像Excel的table
                DataTable dt = new DataTable();
                dt.TableName = "table" + i.ToString();

                //獲取表名
                sheetName = dtSheetName.Rows[i]["TABLE_NAME"].ToString();

			//取得資料
                OleDbDataAdapter oleda = new OleDbDataAdapter("select * from [" + sheetName + "]", ExcelConn);

			//將取到的資料，填入DataTable
                oleda.Fill(dt);

			//把DataTable加入至DataSet中
                ds.Tables.Add(dt);
            }

            //關閉連線，釋放資源
            ExcelConn.Close();
            ExcelConn.Dispose();


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

						/*
							出來的row.ItemArray[i]是Object型別，所以要轉型
							String a = row.ItemArray[i].ToString();
							int b = Convert.Toint32(row.ItemArray[i].ToString());
							DateTime c = Convert.ToDateTime(row.ItemArray[i].ToString());
						*/

				Console.WriteLine("");
                    }
                }
            }

            Console.Read();
        }
    }
}
