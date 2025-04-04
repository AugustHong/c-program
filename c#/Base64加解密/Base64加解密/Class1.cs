﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base64加解密
{
    public static class Base64Helper
    {
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字串</param>
        /// <returns></returns>
        public static string Base64Encrypt(this string input)
        {
            return Base64Encrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字串</param>
        /// <param name="encode">字元編碼</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input, Encoding encode)
        {
            return Convert.ToBase64String(encode.GetBytes(input));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="input">需要解密的字串</param>
        /// <returns></returns>
        public static string Base64Decrypt(this string input)
        {
            return Base64Decrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="input">需要解密的字串</param>
        /// <param name="encode">字元的編碼</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input, Encoding encode)
        {
            return encode.GetString(Convert.FromBase64String(input));
        }
    }
}
