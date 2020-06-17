using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Dapper研究.Extentions
{
    public static class DataTableExtention
    {
        /// <summary>
        ///  DataTable 擴充 (得到 所有的 Columns 列表)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> GetColumnNameList(this DataTable dt)
        {
            List<string> result = new List<string>();
            if (dt != null)
            {
                foreach (DataColumn c in dt.Columns)
                {
                    result.Add(c.ToString());
                }
            }

            return result;
        }

        #region IEnumerable 轉 DataTable
        /// <summary>
        /// 擴充 IEnumerable to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var dt = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];

                dt.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType); //解決DataSet 不支援 System.Nullable<>
            }
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        /// <summary>
        /// 擴充 IEnumerable to DataTable (但去掉 特定欄位)
        /// 後面傳入 要去掉的欄位 名稱 即可
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="filterColumns"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection, List<string> filterColumns)
        {
            filterColumns = filterColumns == null ? new List<string>() : filterColumns;

            var props = typeof(T).GetProperties();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var dt = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];

                dt.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType); //解決DataSet 不支援 System.Nullable<>
            }

            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj;

                        // 名稱 不能在 filter 之中
                        if (!filterColumns.Contains(pi.Name))
                        {
                            obj = pi.GetValue(collection.ElementAt(i), null);
                        }
                        else
                        {
                            obj = null;
                        }

                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }
        #endregion


        #region DataTable 轉 List

        /// <summary>
        /// 擴充 DataTable to List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="filterColumns">篩選掉的欄位名稱List</param>
        /// <returns></returns>
        public static List<T> DTToList<T>(this DataTable table, List<string> filterColumns = null)
        {
            filterColumns = filterColumns == null ? new List<string>() : filterColumns;

            if (table == null)
            {
                return new List<T>();
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows, filterColumns);
        }

        /// <summary>
        ///  由上面 ToList 呼叫
        ///  作用： 將 DataTable 的 Row 傳轉換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <param name="filterColumns"></param>
        /// <returns></returns>
        private static List<T> ConvertTo<T>(List<DataRow> rows, List<string> filterColumns = null)
        {
            filterColumns = filterColumns == null ? new List<string>() : filterColumns;

            List<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row, filterColumns);
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// 由上面 ConvertTo 呼叫
        /// 作用： 建立 T 物件 Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="filterColumns"></param>
        /// <returns></returns>
        private static T CreateItem<T>(DataRow row, List<string> filterColumns = null)
        {
            filterColumns = filterColumns == null ? new List<string>() : filterColumns;

            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        if (value == System.DBNull.Value || filterColumns.Contains(column.ColumnName) || value.ToString() == "null")
                        {
                            if (prop.PropertyType.Name.ToLower() == "string")
                                prop.SetValue(obj, string.Empty, null);
                            else
                                prop.SetValue(obj, null, null);
                        }
                        else
                        {

                            switch (prop.PropertyType.Name.ToLower())
                            {
                                case "datetime":
                                    prop.SetValue(obj, DateTime.Parse(value.ToString()), null);
                                    break;
                                case "decimal":
                                    prop.SetValue(obj, decimal.Parse(value.ToString()), null);
                                    break;
                                case "int32":
                                    prop.SetValue(obj, int.Parse(value.ToString()), null);
                                    break;

                                default:
                                    if (prop.PropertyType.FullName.Contains("System.DateTime"))
                                    {
                                        prop.SetValue(obj, DateTime.Parse(value.ToString()), null);
                                    }
                                    else if (prop.PropertyType.FullName.Contains("System.Decimal"))
                                        prop.SetValue(obj, decimal.Parse(value.ToString()), null);
                                    else if (prop.PropertyType.FullName.Contains("System.Int32"))
                                        prop.SetValue(obj, int.Parse(value.ToString()), null);
                                    else
                                        prop.SetValue(obj, value, null);
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        throw;
                    }
                }
            }

            return obj;
        }

        #endregion
    }
}