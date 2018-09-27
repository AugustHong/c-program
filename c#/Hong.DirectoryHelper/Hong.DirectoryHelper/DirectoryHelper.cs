using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Compression;  //一般壓縮（不用密碼的）

using Ionic.Zip;  //有密碼的壓縮（但要先去NuGet裝上DotNetZip才行使用）

namespace Hong.DirectoryHelper
{
    /// <summary>
    /// 資料夾輔助器（新建、刪除、壓縮、解壓縮）
    /// </summary>
    public class DirectoryHelper
    {
        public string path { get; set; }
        public string dirName { get; set; }

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="path">路徑</param>
        /// <param name="dirName">資料夾名稱</param>
        /// <param name="createForNotExist">如果此資料夾未建立，是否要建立</param>
        public DirectoryHelper(string path, string dirName, bool createForNotExist = true)
        {
            //如果使用者沒給/，自己加
            this.path = path.Substring(path.Length - 1) == "/" || path.Substring(path.Length - 2) == "\\" ? path : path + "/";
            this.dirName = dirName;

            //如果資料夾未存在，預設會是幫他建立
            if (createForNotExist && !Directory.Exists(this.path + this.dirName)) { Create(); }
        }

        /// <summary>
        /// 建立資料夾
        /// </summary>
        /// <param name="isExistAction">如果已存在是否要刪掉重建</param>
        public void Create(bool isExistAction = false)
        {
            //如果資料夾已存在，看他給的參數決定是否刪掉重建
            if (Directory.Exists(path + dirName))
            {
                if (isExistAction) { Delete(); }
                else { return; }
            }
            else {
                Directory.CreateDirectory(path + dirName);
            }
        }

        /// <summary>
        /// 刪除資料夾
        /// </summary>
        public void Delete()
        {
            //如果資料夾不存在，不動作
            if (!Directory.Exists(path + dirName)) { return; }
            else {
                Directory.Delete(path + dirName);
            }
        }

        /// <summary>
        /// 把資料夾變成zip（無密碼）
        /// </summary>
        /// <param name="destination">目的地（請輸入詳細路徑+檔名+.zip）</param>
        public void MarkZip(string destination)
        {
            //如果後面不是.zip型式，則不作
            if (Path.GetExtension(destination) != ".zip") { return; }

            //如果這zip存在，先砍掉
            if (File.Exists(destination)) { File.Delete(destination); }

            //如果資料夾不存在，不動作
            if (!Directory.Exists(path + dirName)) { return; }
            else
            {
                try
                {
                    //路徑相同可能會發生錯誤（所以用try catch）
                    System.IO.Compression.ZipFile.CreateFromDirectory(path + dirName, destination);
                }
                catch (Exception e)
                {
                    throw new Exception("錯誤：請詳見" + e.Message);
                }
            }
        }

        /// <summary>
        /// 把資料夾變成zip（有密碼的）
        /// </summary>
        /// <param name="destination">目的地（請輸入詳細路徑+檔名+.zip）</param>
        /// <param name="passwd">密碼</param>
        public void MarkZip(string destination, string passwd)
        {
            //如果後面不是.zip型式，則不作
            if (Path.GetExtension(destination) != ".zip") { return; }

            //如果這zip存在，先砍掉
            if (File.Exists(destination)) { File.Delete(destination); }

            //如果資料夾不存在，不動作
            if (!Directory.Exists(path + dirName)) { return; }
            else
            {
                try
                {
                    //路徑相同可能會發生錯誤（所以用try catch）
                    //有密碼的壓縮
                    using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(destination))
                    {
                        zip.Password = passwd;
                        //也有zip.AddFile(路徑)，但這裡是DirectoryHelper所以都是用Direcotry來做即可（要的話把檔案放進
                        //資料夾內再壓縮即可）
                        zip.AddDirectory(path + dirName);
                        zip.Save();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("錯誤：請詳見" + e.Message);
                }
            }
        }


        /// <summary>
        /// 把zip解壓縮
        /// </summary>
        /// <param name="source">來源地（請輸入詳細路徑+檔名+.zip）</param>
        public void Extract(string source)
        {
            //如果後面不是.zip型式，則不作
            if (Path.GetExtension(source) != ".zip") { return; }

            //如果這zip不存在，不做
            if (!File.Exists(source)) { return; }

            //如果資料夾已存在，刪掉
            if (Directory.Exists(path + dirName)) { Delete(); }

            try
            {
                //路徑相同可能會發生錯誤（所以用try catch）
                System.IO.Compression.ZipFile.CreateFromDirectory(source, path + dirName);
            }
            catch (Exception e)
            {
                throw new Exception("錯誤：請詳見" + e.Message);
            }

        }

        /// <summary>
        /// 把zip解壓縮（有密碼的）
        /// </summary>
        /// <param name="source">來源地（請輸入詳細路徑+檔名+.zip）</param>
        /// <param name="passwd">密碼</param>
        /// <param name="zipName">Zip的檔名，好用於移動檔案新增</param>
        public void Extract(string source, string passwd, string zipName = "test.zip")
        {
            //如果後面不是.zip型式，則不作
            if (Path.GetExtension(source) != ".zip") { return; }

            //如果這zip不存在，不做
            if (!File.Exists(source)) { return; }

            //這裡跟上面不一樣，他會把東西都拆開來，但不會產生Directory
            //所以要先有資料夾，再把zip移到這個資料夾下，再拆開
            //如果資料夾不存在，新建
            if (!Directory.Exists(path + dirName)) { Create(); }

            //如果他給的zip檔名未有.zip則幫他加
            zipName = zipName.Substring(zipName.Length - 4) == ".zip" ? zipName : zipName + ".zip";

            //新的zip位置
            string newZipPath = path + dirName + "/" + zipName;
            File.Move(source, newZipPath);

            try
            {
                //路徑相同可能會發生錯誤（所以用try catch）

                using (var zip = Ionic.Zip.ZipFile.Read(newZipPath))
                {
                    foreach (var zipEntry in zip)
                    {
                        zip.Password = passwd;
                        zipEntry.Extract(path + dirName, ExtractExistingFileAction.OverwriteSilently);
                    }
                }

                //把zip砍掉
                File.Delete(newZipPath);
            }
            catch (Exception e)
            {
                throw new Exception("錯誤：請詳見" + e.Message);
            }
        }
    }
}
