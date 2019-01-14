using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;   //要用SecureString要引用這個（SecureString是較為安全的一種string，主要用於password）
using System.Security.Cryptography;  //要用TripleDES加密
using System.Text;
using System.Threading.Tasks;

namespace Hong.TripleDESHelper
{

	/// <summary>
	/// 擴充方法（class一定要是靜態的)
	/// 讓string 變成SecureString
	/// </summary>
	public static class SecureStringConvert
	{
		//讓string 變成SecureString
		public static SecureString ToSecureString(this string value)
		{

			SecureString result = new SecureString();

			if (!string.IsNullOrEmpty(value))
			{
				//將每個字母組進去
				foreach (char c in value)
				{
					result.AppendChar(c);
				}
			}

			return result;
		}
	}


	/// <summary>
	/// TripleDES加密
	/// </summary>
	public class TripleDESHelper
	{
		/// <summary>
		/// 獲取加密key的 md5 hash，最终DES加密的時候使用這个hash值
		/// </summary>
		/// <param name="key">原始key值</param>
		/// <param name="useHash">是否要使用hash值（預設為true）</param>
		/// <returns></returns>
		public static byte[] GetKeyMd5Hash(string key, bool useHash = true)
		{
			//先取得 將文字轉成byte[]
			byte[] result = UTF8Encoding.UTF8.GetBytes(key);

			//如果他選擇要用hash
			if (useHash)
			{
				//使用md5進行hash
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				result = hashmd5.ComputeHash(result);

				//清空
				hashmd5.Clear();
			}

			return result;
		}

		/// <summary>
		/// TripleDES 加密
		/// </summary>
		/// <param name="toEncrypt">要加密的文字</param>
		/// <param name="key">要當作key的文字</param>
		/// <param name="useHash">是否要使用hash值（預設為true）</param>
		/// <returns></returns>
		public static string DesEncrypt(string toEncrypt, string key, bool useHash = true)
		{
			//取得key的hash，或key 的文字轉byte[]形式
			byte[] privateKey = GetKeyMd5Hash(key, useHash);

			//將要加密的資料轉成byte[]型式
			byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

			//建立TripleDES的組件
			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
			{
				//得到的key
				Key = privateKey,

				//下2個先固定這樣寫
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			};

			//開始進行加密（都先照這樣寫即可）
			ICryptoTransform cTransform = tdes.CreateEncryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			tdes.Clear();

			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}

		/// <summary>
		/// TripleDES解密
		/// </summary>
		/// <param name="toDecrypt">要解密的文字</param>
		/// <param name="key">要當作key的文字</param>
		/// <param name="useHash">是否要使用hash值（預設為true）</param>
		/// <returns></returns>
		public static string DesDecrypt(string toDecrypt, string key, bool useHash = true)
		{
			//取得key的hash，或key 的文字轉byte[]形式
			byte[] privateKey = GetKeyMd5Hash(key, useHash);

			//先base64解密 因為加密的時候最後走了一道base64加密
			byte[] enBytes = Convert.FromBase64String(toDecrypt);

			//建立TripleDES的組件
			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
			{
				Key = privateKey,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			};

			//進行解密的作業（一樣都先照這樣寫即可）
			ICryptoTransform cTransform = tdes.CreateDecryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(enBytes, 0, enBytes.Length);
			tdes.Clear();

			return Encoding.UTF8.GetString(resultArray);

		}
	}
}
