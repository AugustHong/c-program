using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;  //先去參考=>加入參考=>選擇System.Windows.Forms按確定

namespace OpenFileDialog_Test
{
    class Program
    {
        [STAThreadAttribute] //要先加入這一行
        static void Main(string[] args)
        {
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

            string fileName = openFileDialog.FileName;
            Console.WriteLine(fileName);


            //開啟路徑
            string path = string.Empty;

            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "請選擇資料夾";
            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                path = dilog.SelectedPath;
            }

            Console.WriteLine(path);

            Console.Read();
        }
    }
}
