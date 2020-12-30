using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hong.CookieHelper
{
    public static class CookieHelper
    {
        /// <summary>
        ///  建立 Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="expires"></param>
        /// <param name="value"></param>
        public static void CreateCookie(string cookieName, DateTime expires, string value, HttpResponseBase Response)
        {
            if (!string.IsNullOrWhiteSpace(cookieName) && Response != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName)
                {
                    Expires = expires,
                    Value = value
                };

                // 加入
                Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        ///  建立 Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="expires"></param>
        /// <param name="value"></param>
        public static void CreateCookie(string cookieName, DateTime expires, string value, HttpRequestBase Request)
        {
            if (!string.IsNullOrWhiteSpace(cookieName) && Request != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName)
                {
                    Expires = expires,
                    Value = value
                };

                // 加入
                Request.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 建立 Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="expires"></param>
        /// <param name="value"></param>
        /// <param name="controller"></param>
        public static void CreateCookie(string cookieName, DateTime expires, Dictionary<string, string> value, HttpResponseBase Response)
        {
            if (!string.IsNullOrWhiteSpace(cookieName) && Response != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName)
                {
                    Expires = expires
                };

                if (value != null)
                {
                    List<string> keys = value.Keys.ToList();
                    foreach (var key in keys)
                    {
                        cookie.Values.Add(key, value[key]);
                    }
                }

                // 加入
                Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 建立 Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="expires"></param>
        /// <param name="value"></param>
        /// <param name="controller"></param>
        public static void CreateCookie(string cookieName, DateTime expires, Dictionary<string, string> value, HttpRequestBase Request)
        {
            if (!string.IsNullOrWhiteSpace(cookieName) && Request != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName)
                {
                    Expires = expires
                };

                if (value != null)
                {
                    List<string> keys = value.Keys.ToList();
                    foreach (var key in keys)
                    {
                        cookie.Values.Add(key, value[key]);
                    }
                }

                // 加入
                Request.Cookies.Add(cookie);
            }
        }

        /// <summary>
        ///  修改 Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="expires"></param>
        /// <param name="value"></param>
        /// <param name="controller"></param>
        public static void UpdateCookie(string cookieName, DateTime expires, string value, HttpResponseBase Response)
        {
            if (!string.IsNullOrWhiteSpace(cookieName) && Response != null)
            {
                if (Response.Cookies[cookieName] != null)
                {
                    HttpCookie cookie = Response.Cookies[cookieName];
                    cookie.Expires = expires;
                    cookie.Value = value;

                    Response.Cookies.Set(cookie);
                }
            }
        }

        /// <summary>
        ///  修改 Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="expires"></param>
        /// <param name="value"></param>
        /// <param name="controller"></param>
        public static void UpdateCookie(string cookieName, DateTime expires, string value, HttpRequestBase Request)
        {
            if (!string.IsNullOrWhiteSpace(cookieName) && Request != null)
            {
                if (Request.Cookies[cookieName] != null)
                {
                    HttpCookie cookie = Request.Cookies[cookieName];
                    cookie.Expires = expires;
                    cookie.Value = value;

                    Request.Cookies.Set(cookie);
                }
            }
        }

        /// <summary>
        /// 修改 Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="expires"></param>
        /// <param name="value"></param>
        /// <param name="controller"></param>
        public static void UpdateCookie(string cookieName, DateTime expires, Dictionary<string, string> value, HttpResponseBase Response)
        {
            if (!string.IsNullOrWhiteSpace(cookieName) && Response != null)
            {
                if (Response.Cookies[cookieName] != null)
                {
                    HttpCookie cookie = Response.Cookies[cookieName];
                    cookie.Expires = expires;

                    if (value != null)
                    {
                        // 先清空
                        cookie.Values.Clear();

                        List<string> keys = value.Keys.ToList();
                        foreach (var key in keys)
                        {
                            cookie.Values.Add(key, value[key]);
                        }
                    }

                    Response.Cookies.Set(cookie);
                }
            }
        }

        /// <summary>
        /// 修改 Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="expires"></param>
        /// <param name="value"></param>
        /// <param name="controller"></param>
        public static void UpdateCookie(string cookieName, DateTime expires, Dictionary<string, string> value, HttpRequestBase Request)
        {
            if (!string.IsNullOrWhiteSpace(cookieName) && Request != null)
            {
                if (Request.Cookies[cookieName] != null)
                {
                    HttpCookie cookie = Request.Cookies[cookieName];
                    cookie.Expires = expires;

                    if (value != null)
                    {
                        // 先清空
                        cookie.Values.Clear();

                        List<string> keys = value.Keys.ToList();
                        foreach (var key in keys)
                        {
                            cookie.Values.Add(key, value[key]);
                        }
                    }

                    Request.Cookies.Set(cookie);
                }
            }
        }

        /// <summary>
        ///  清除 Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public static void ClearCookie(string cookieName, HttpResponseBase Response)
        {
            if (!string.IsNullOrWhiteSpace(cookieName) && Response != null)
            {
                if (Response.Cookies[cookieName] != null)
                {
                    HttpCookie cookie = Response.Cookies[cookieName];
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    cookie.Value = string.Empty;
                    cookie.Values.Clear();

                    Response.Cookies.Set(cookie);
                }
            }
        }

        /// <summary>
        ///  清除 Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public static void ClearCookie(string cookieName, HttpRequestBase Request)
        {
            if (!string.IsNullOrWhiteSpace(cookieName) && Request != null)
            {
                if (Request.Cookies[cookieName] != null)
                {
                    HttpCookie cookie = Request.Cookies[cookieName];
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    cookie.Value = string.Empty;
                    cookie.Values.Clear();

                    Request.Cookies.Set(cookie);
                }
            }
        }

        /// <summary>
        ///  清除所有 Cookie
        /// </summary>
        /// <param name="controller"></param>
        public static void ClearAllCookie(HttpResponseBase Response)
        {
            if (Response != null)
            {
                // 清除 Cookies
                foreach (HttpCookie cookie in Response.Cookies)
                {
                    ClearCookie(cookie.Name, Response);
                }
            }
        }

        /// <summary>
        ///  清除所有 Cookie
        /// </summary>
        /// <param name="controller"></param>
        public static void ClearAllCookie(HttpRequestBase Request)
        {
            if (Request != null)
            {
                // 清除 Cookies
                foreach (HttpCookie cookie in Request.Cookies)
                {
                    ClearCookie(cookie.Name, Request);
                }
            }
        }

        /// <summary>
        /// 得到 Cookie (只有 Value)
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName, HttpResponseBase Response)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(cookieName) && Response != null)
            {
                if (Response.Cookies[cookieName] != null)
                {
                    HttpCookie cookie = Response.Cookies[cookieName];
                    return cookie.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// 得到 Cookie (只有 Value)
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName, HttpRequestBase Request)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(cookieName) && Request != null)
            {
                if (Request.Cookies[cookieName] != null)
                {
                    HttpCookie cookie = Request.Cookies[cookieName];
                    return cookie.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// 得到 Cookie (只有 Values)
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <param name="Response"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName, string key, HttpResponseBase Response)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(cookieName) && Response != null)
            {
                if (Response.Cookies[cookieName] != null)
                {
                    HttpCookie cookie = Response.Cookies[cookieName];
                    if (cookie.Values[key] != null)
                    {
                        return cookie.Values[key];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 得到 Cookie (只有 Values)
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <param name="Response"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName, string key, HttpRequestBase Request)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(cookieName) && Request != null)
            {
                if (Request.Cookies[cookieName] != null)
                {
                    HttpCookie cookie = Request.Cookies[cookieName];
                    if (cookie.Values[key] != null)
                    {
                        return cookie.Values[key];
                    }
                }
            }

            return result;
        }
    }
}
