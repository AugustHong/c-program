using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Compression;

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
        /// 把資料夾變成zip
        /// </summary>
        /// <param name="destination">目的地（請輸入詳細路徑+檔名+.zip）</param>
        public void MarkZip(string destination)
        {
            //如果後面不是.zip型式，則不作
            if (Path.GetExtension(destination) != ".zip") { return; }

            //如果這zip不存在，不動作
            if (File.Exists(destination)) { return; }

            //如果資料夾不存在，不動作
            if (!Directory.Exists(path + dirName)) { return; }
            else
            {
                try
                {
                    //路徑相同可能會發生錯誤（所以用try catch）
                    ZipFile.CreateFromDirectory(path + dirName, destination);
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

            //如果這zip不存在，不動作
            if (File.Exists(source)) { return; }

            //如果資料夾已存在，刪掉
            if (Directory.Exists(path + dirName)) { Delete(); }
            else
            {
                try
                {
                    //路徑相同可能會發生錯誤（所以用try catch）
                    ZipFile.CreateFromDirectory(source, path + dirName);
                }
                catch (Exception e)
                {
                    throw new Exception("錯誤：請詳見" + e.Message);
                }
            }
        }
    }
}
