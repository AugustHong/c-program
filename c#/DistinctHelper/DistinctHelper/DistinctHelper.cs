using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DistinctHelper
{
	/// <summary>
	/// 要擴充比對Distinct（自訂義的Class在Distinct時都會沒效）
	/// 故有2種方法可以進行
	/// 1.對Distinct進行擴充
	/// 2.寫一個比對的Helper（for那Class的）
	/// </summary>
	/// 

	//來源資料：https://dotblogs.com.tw/larrynung/2012/09/18/74901
	//來源資料：https://blog.csdn.net/qq285679784/article/details/70766600


	///對Distinct進行擴充
	public static class EnumerableExtender
	{
		/// <summary>
		/// 對Distinct進行擴充
		/// 使用方法：
		/// 1.比對單個項目  var query = people.DistinctBy(p => p.Id);
		/// 2.比對多個項目 var query = people.DistinctBy(p => new { p.Id, p.Name });
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="source"></param>
		/// <param name="keySelector"></param>
		/// <returns></returns>
		public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				var elementValue = keySelector(element);
				if (seenKeys.Add(elementValue))
				{
					yield return element;
				}
			}
		}
	}


	class DistinctHelper
	{
		/// <summary>
		/// 自已寫的比對重複（要實作IEqualityComparer）
		/// 注意：原本是為每一個Class就要寫一個Comparer（都要實作IEqualityComparer，且要有Equals()、GetHashCode())
		/// 但這裡寫成泛形，就可for全部了
		/// PS:因為這邊是使用Reflection，故效能真的很差（建議還是用上面的方法即可，這裡主要是筆記用）
		/// 
		/// 使用方法：datas.Distinct(new PropertyComparer<Class>("Class含namespace之全名"));
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public class PropertyComparer<T> : IEqualityComparer<T>
		{
			//Reflection出來的屬性
			private PropertyInfo _PropertyInfo;

			/// <summary>
			/// 建構子
			/// </summary>
			/// <param name="propertyName">Class名稱</param>
			public PropertyComparer(string propertyName)
			{
				//儲存一個property物件，以用於之後的比較
				//store a reference to the property info object for use during the comparison
				_PropertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

				if (_PropertyInfo == null)
				{
					throw new ArgumentException(string.Format("{0} is not a property of type {1}.", propertyName, typeof(T)));
				}
			}

			#region IEqualityComparer<T> Members

			public bool Equals(T x, T y)
			{
				//得到x, y目前的值
				object xValue = _PropertyInfo.GetValue(x, null);
				object yValue = _PropertyInfo.GetValue(y, null);

				//如果x是null，則回傳y是不是null
				if (xValue == null) { return yValue == null; }

				//用預設的比較法來比其值（全部內容皆相同=>true）
				return xValue.Equals(yValue);

				//如果要自己寫for每個Class的話，就可在這個mathod回傳自己想比對的
				//例如： return (x.id == y.id ) && (x.name == y.name);
			}

			public int GetHashCode(T obj)
			{
				//get the value of the comparison property out of obj
				object propertyValue = _PropertyInfo.GetValue(obj, null);

				if (propertyValue == null)
					return 0;

				else
					return propertyValue.GetHashCode();
			}

			#endregion
		}
	}
}
