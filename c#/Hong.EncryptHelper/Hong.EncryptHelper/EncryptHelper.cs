using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace Hong.EncryptHelper
{
    /// <summary>
    /// 用於加解密相關資料，並和WPF做溝通
    /// </summary>
    public class EncryptHelper
    {
        //公私鑰（路徑）
        string privateKeyPath;
        public string publicKeyPath;


        /// <summary>
        /// 建構子（把公私鑰的路徑先給他）
        /// </summary>
        /// <param name="privateKeyPath">私鑰路徑</param>
        /// <param name="publicKeyPath">公鑰路徑</param>
        public EncryptHelper(string privateKeyPath, string publicKeyPath)
        {
            this.privateKeyPath = privateKeyPath;
            this.publicKeyPath = publicKeyPath;
        }


        /// <summary>
        /// 創建RSA公鑰私鑰
        /// </summary>
        public void CreateRSAKey()
        {
            //設置[公鑰私鑰]文件路徑
            //創建RSA對象
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);

            //生成RSA[公鑰私鑰]
            string privateKey = rsa.ToXmlString(true);
            string publicKey = rsa.ToXmlString(false);

            //將密鑰寫入指定路徑
            File.WriteAllText(privateKeyPath, privateKey);//文件內包含公鑰和私鑰
            File.WriteAllText(publicKeyPath, publicKey);//文件內只包含公鑰
        }


        /// <summary>
        /// 取得公鑰
        /// </summary>
        /// <returns></returns>
        public string GetPublicKey()
        {
            //公鑰不加密
            return File.ReadAllText(publicKeyPath);
        }


        /// <summary>
        /// 使用RSA進行加密的作業
        /// </summary>
        /// <param name="data">要被加密的資料（任何型態皆可，包含自訂的Class）</param>
        /// <param name="splitLen">分割的長度大小（每多少分一次段，預設200）</param>
        /// <param name="key">公鑰（如沒輸入會預設用你剛才設的路徑之公鑰）</param>
        /// <returns></returns>
        public byte[] RSAEncrypt(Object data, int splitLen = 200, string key = "")
        {

            //取得公鑰（如果是""則用自已的公鑰，不是則是別人傳過來的）
            string publicKey = string.IsNullOrEmpty(key) ? File.ReadAllText(publicKeyPath) : key;

            //創建RSA對象并載入[公鑰]
            RSACryptoServiceProvider rsaPublic = new RSACryptoServiceProvider(2048);
            rsaPublic.FromXmlString(publicKey);

            //先將資料進行二進制的序列化
            string jsonString = JsonConvert.SerializeObject(data);     //序列化
            byte[] publicValue = Encoding.UTF8.GetBytes(jsonString);   //轉為二進制

            //分割長度（不能大於256，也不能小於0）
            int len = splitLen > 256 || splitLen <= 0 ? 256 : splitLen;

            //總資料（要裝加密後的資料的）
            List<byte[]> totalData = new List<byte[]>();

            //當前的數量位置（要把每一個分割後，進行加密）
            int current = 0;   

            //跑迴圈並把值加密丟入進去
            while (current < publicValue.Length)
            {
                //實際現在可以取到的長度（因為在最後一段時，未必資料剛好等同於len）
                int canGetLen = publicValue.Length - current < len ? publicValue.Length - current : len;

                //創建一個buffer來進行複製用的
                byte[] buffer = new byte[canGetLen];

                //把他複製到buffer（分別為  從publicValue的第current位置開始  copy共len個位數資料 到 buffer 的第0位數） 
                Array.Copy(publicValue, current, buffer, 0, canGetLen);

                //把上面的buffer資料加密並加入進去
                totalData.Add(rsaPublic.Encrypt(buffer, false));

                //值增加（因為只有最後一段才會不等同於len，所以這邊可以直接增值len）
                current += len;
            }

            //總長度
            int byteLength = totalData.Sum(a => a.Length);

            //創新的byte[]是將剛才加密的List<byte[]>放入進去
            byte[] publicStr = new byte[byteLength];

            //重新歸零（要重新計算）
            current = 0;

            //把資料丟進去
            foreach (var item in totalData)
            {
                //把內容複製過去
                item.CopyTo(publicStr, current);

                //這邊固定256，因為加密後不管你給他多長他都是變成256（所以List<>裡的每一個長度都是256）
                current += 256;
            }

            return publicStr;
        }


        /// <summary>
        /// 進行RSA的解密
        /// </summary>
        /// <typeparam name="T">轉回類型（從JSON轉型成你要的類型）</typeparam>
        /// <param name="data">二進制的加密資料</param>
        /// <param name="r">你要傳入的結果（從JSON將資料轉成你傳入的類型，並把值給上）</param>
        /// <returns></returns>
        public string RSADecrypt<T>(byte[] data, ref T r)
        {
            //C#默認只能使用[私鑰]進行解密(想使用[私鑰加密]可使用第三方組件BouncyCastle來實現)
            string privateKey = File.ReadAllText(privateKeyPath);

            //創建RSA對象并載入[私鑰]
            RSACryptoServiceProvider rsaPrivate = new RSACryptoServiceProvider(2048);
            rsaPrivate.FromXmlString(privateKey);

            //加密後的資料長度都是256，所以分割時也是用256來分
            int len = 256;

            //資料的總長度
            int dataLen = data.Length;

            //總共會跑幾次（即總長度/256）
            int c = (dataLen / len);

            //計數器（計算目前弄到多少了）
            int count = 0;          

            //回傳的字串（JSON字串）
            string result = string.Empty;

            //跑每個分割的量（每次都是256）
            for (int i = 1; i <= c; i++)
            {
                //進行解密並變成byte[]
                byte[] b = rsaPrivate.Decrypt(data.Skip(count).Take(count + 256).ToArray(), false);

                //從進制資料轉成Json字串
                result += Encoding.UTF8.GetString(b);

                //增加值（每次固定256）
                count += 256;
            }

            //從Json轉回類別物件
            r = JsonConvert.DeserializeObject<T>(result);

            return result;
        }
    }
}
