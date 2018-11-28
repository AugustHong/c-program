using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BinaryFileHelper
{

    /*  
        
        本內容出自http://toimy.blogspot.com/2010/03/c.html
        僅作練習、學習之用
             
        */


    /// <summary>
    /// 做二進制的寫檔及讀檔
    /// </summary>
    public class BinaryFileHelper
    {
        /// <summary>
        /// 寫入二進制資料
        /// </summary>
        /// <param name="fileName">檔名（完整路徑）</param>
        /// <param name="binData">二進制資料</param>
        /// <returns></returns>
        public static bool BinWrite(string fileName, byte[] binData)
        {
            try
            {
                //開啟建立檔案
                FileStream myFile = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite);

                //二進制寫檔
                BinaryWriter myWriter = new BinaryWriter(myFile);
                myWriter.Write(binData);

                //關閉
                myWriter.Close();
                myFile.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        /// <summary>
        /// 讀取二進制資料
        /// </summary>
        /// <param name="fileName">檔名（完整路徑）</param>
        /// <param name="saveBinData">儲存讀取出來的二進制資料</param>
        /// <returns></returns>
        public static bool BinRead(string fileName, ref byte[] saveBinData)
        {

            try
            {
                //開啟檔案
                FileStream myFile = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite);

                //引用myReader類別
                BinaryReader myReader = new BinaryReader(myFile);

                //取得長度（因為是byte，所以要看長度）
                int len = Convert.ToInt32(myFile.Length);

                //讀取位元陣列
                saveBinData = myReader.ReadBytes(len);

                //讀取資料
                //釋放資源
                myReader.Close();
                myFile.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 寫字串成二進制檔
        /// </summary>
        /// <param name="fileName">檔名（完整路徑）</param>
        /// <param name="Data">字串資料</param>
        /// <returns></returns>
        public static bool StringWrite(string fileName, string Data)
        {
            try
            {
                //建立檔案
                FileStream myFile = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                //寫檔
                StreamWriter myWriter = new StreamWriter(myFile);

                //建立位元陣列
                myWriter.Write(Data);

                //釋放資源
                myWriter.Close();
                myFile.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        /// <summary>
        /// 讀取二進制檔轉成字串
        /// </summary>
        /// <param name="fileName">檔名（完整路徑）</param>
        /// <param name="saveData">儲存讀取出來的字串</param>
        /// <returns></returns>
        public static bool StringRead(string fileName, ref string saveData)
        {
            try
            {
                //開啟檔案
                FileStream myFile = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                //引用myReader類別
                StreamReader myReader = new StreamReader(myFile);

                //得到長度
                int len = Convert.ToInt32(myFile.Length);

                //讀取位元陣列
                saveData = myReader.ReadToEnd();

                //讀取資料
                //釋放資源
                myReader.Close();
                myFile.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}

