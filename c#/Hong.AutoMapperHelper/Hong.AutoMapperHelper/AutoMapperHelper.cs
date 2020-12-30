using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hong.AutoMapperHelper
{
    public static class AutoMapperHelper
    {
        /// <summary>
        /// AutoMapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="u"></param>
        /// <returns></returns>
        public static T Convert<U, T>(this U u)
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<U, T>());
                var mapper = config.CreateMapper();
                return mapper.Map<T>(u);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return default(T);
            }
        }
    }
}
