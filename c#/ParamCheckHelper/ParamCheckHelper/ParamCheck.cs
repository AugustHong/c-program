using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Linq;

namespace ParamCheckHelper
{
	/// <summary>
	/// 做參數的確認（去除字元……等）
	/// </summary>
	public class ParamChecker
	{
		//要去掉的一些字元
		private static string[] BadChars = new string[] { "\\", ".", "~" };

		/// <summary>
		/// 字串的確認（把上述的BadChars中的字元處理掉）
		/// </summary>
		/// <param name="inputText">傳入字串</param>
		/// <returns></returns>
		public static string Check(string inputText)
		{
			if (!string.IsNullOrEmpty(inputText))
			{
				//將所有的BadChars字元跑過，並取代成空字串
				foreach (var c in BadChars)
				{
					inputText = inputText.Replace(c, string.Empty);
				}

				//轉成utf8字元byte[]
				byte[] b1 = Encoding.UTF8.GetBytes(inputText);
				byte[] b2 = b1.ToArray();

				//創建一個b3（要將b2的重新組合）
				byte[] b3 = new byte[b2.Length];
				for (int i = 0; i < b2.Length; i++)
				{
					b3[i] = b2[i];
				}

				//轉回字串
				string base64 = Convert.ToBase64String(b3);
				return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
			}
			else
			{
				if (inputText == null) { return null; }
			}

			return string.Empty;
		}

		/// <summary>
		/// 將Model作轉換
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <returns></returns>
		public static T CheckModel<T>(T t)
		{
			if (t == null){return default(T);}

			var model = AutoMapperHelper.ConvertModel<T, T>(t);

			if (model == null){return default(T);}

			//進行序列化的動作
			var serializableAttribute = t.GetType().GetCustomAttributes(typeof(SerializableAttribute), true).FirstOrDefault();


			if (serializableAttribute != null)
			{
				var base64 = Convert.ToBase64String(ObjectToBytes(model));
				var newObj = BytesToObject(Convert.FromBase64String(base64));
				return (T)newObj;
			}
			else {
				return model;
			}
		}

		/// <summary>
		/// 化為long?型別
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static long? ToOptionalLong(long? input)
		{
			long? returnValue = null;

			if (input.HasValue)
			{
				returnValue = ToLong(input.Value.ToString());
			}

			return returnValue;
		}

		/// <summary>
		/// 將字串化成long型別
		/// </summary>
		/// <param name="input">傳入字串</param>
		/// <param name="defaultValue">預設值</param>
		/// <returns></returns>
		public static long ToLong(string input, long defaultValue = 0)
		{
			try
			{
				//進行check
				var s2 = Check(input);

				if (!string.IsNullOrEmpty(s2))
				{
					//轉成long
					return long.Parse(s2);
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return defaultValue;
		}

		/// <summary>
		/// 字串化成int
		/// </summary>
		/// <param name="input">傳入字串</param>
		/// <param name="defaultValue">預設值</param>
		/// <returns></returns>
		public static int ToInt(string input, int defaultValue = 0)
		{
			try
			{
				//先進行check動作
				var s2 = Check(input);

				if (!string.IsNullOrEmpty(s2))
				{
					//轉成數字
					return int.Parse(s2);
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return defaultValue;
		}

		/// <summary>
		/// 將物件轉成byte[]
		/// </summary>
		/// <param name="obj">物件</param>
		/// <returns></returns>
		private static byte[] ObjectToBytes(object obj)
		{
			if (obj == null){return null;}

			var bf = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				//將物件轉成byte[]（進行序列化相關動作）
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
		}


		/// <summary>
		/// 將byte[]轉成物件
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private static object BytesToObject(byte[] bytes)
		{
			if (bytes == null){return null;}
			if(bytes.Length <= 0) { return new byte[0]; }

			var bf = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				//進行處理（把資料寫進MemorySteam中）
				//寫入資料
				ms.Write(bytes, 0, bytes.Length);
				//找尋位置
				ms.Seek(0, SeekOrigin.Begin);

				//進行反序列化
				return (object)bf.Deserialize(ms);
			}
		}
	}
}
