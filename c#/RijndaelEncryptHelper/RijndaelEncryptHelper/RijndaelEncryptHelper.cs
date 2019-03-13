using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RijndaelEncryptHelper
{
	public class RijndaelEncryptHelper
	{
		/// <summary>
		/// key 長度可為:128bit or 192 bit or 256 bit
		/// </summary>
		public string key = "KEY_KEY_KEY_KEY_KEY_KEY_KEY_KEY_";
		/// <summary>
		/// IV( Initialization Vector) 長度固定為 128 bit
		/// </summary>
		public string iv = "IVIVIVIVIVIVIVIV";


		/// <summary>
		/// 建構子
		/// </summary>
		/// <param name="key">key 長度可為:128bit or 192 bit or 256 bit</param>
		/// <param name="iv">IV( Initialization Vector) 長度固定為 128 bit</param>
		public RijndaelEncryptHelper(string key, string iv)
		{
			if (!string.IsNullOrWhiteSpace(key) && (key.Length == 16 || key.Length == 24 || key.Length == 32)) { this.key = key; }
			if (!string.IsNullOrWhiteSpace(iv) && iv.Length == 16) { this.iv = iv; }
		}

		/// <summary>
		/// Encrypt 將文字加密回傳暗碼
		/// </summary>
		/// <param name="data">要加密的資料</param>
		/// <returns></returns>
		public String Encrypt(String data)
		{
			if (!String.IsNullOrWhiteSpace(data))
			{
				byte[] encrypted;
				
				using (Rijndael rijndael = Rijndael.Create())
				{
					rijndael.Key = Encoding.UTF8.GetBytes(key);
					rijndael.IV = Encoding.UTF8.GetBytes(iv);

					//在衍生類別中覆寫時，使用指定的 System.Security.Cryptography.SymmetricAlgorithm.Key 屬性和初始化向量
					//(System.Security.Cryptography.SymmetricAlgorithm.IV) 建立對稱加密子物件。
					ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);

					using (MemoryStream msEncrypt = new MemoryStream())
					{
						using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
						{
							using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
							{
								swEncrypt.Write(data.Trim());
							}

							encrypted = msEncrypt.ToArray();
						}
					}
				}
				return Convert.ToBase64String(encrypted);
			}
			return "";
		}


		/// <summary>
		/// 將暗碼轉成明碼
		/// </summary>
		/// <param name="data">要解密的資料</param>
		/// <returns></returns>
		public string Decrypt(String data)
		{
			string plaintext = string.Empty;
			if (!String.IsNullOrWhiteSpace(data))
			{
				using (Rijndael rijndael = Rijndael.Create())
				{
					byte[] cipherText = Convert.FromBase64String(data);

					rijndael.Key = Encoding.UTF8.GetBytes(key);
					rijndael.IV = Encoding.UTF8.GetBytes(iv);

					ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

					using (MemoryStream msDecrypt = new MemoryStream(cipherText))
					{
						using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
						{
							using (StreamReader srDecrypt = new StreamReader(csDecrypt))
							{                            
								plaintext = srDecrypt.ReadToEnd();
							}
						}
					}
				}

				return plaintext;
			}
			return plaintext;
		}
	}
}
