using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*  先去NuGet裝上 AutoMapper  */

namespace ParamCheckHelper
{
	public class AutoMapperHelper
	{
		/// <summary>
		/// 使用AutoMapper，將兩種Class轉換（就只是AutoMapper最簡單的寫法，但寫成泛型好大家使用）
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="u"></param>
		/// <returns></returns>
		public static T ConvertModel<T, U>(U u)
		{
			var config = new MapperConfiguration(cfg => cfg.CreateMap<U, T>());
			var mapper = config.CreateMapper();
			return mapper.Map<T>(u);
		}
	}
}
