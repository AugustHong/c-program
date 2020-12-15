using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using System.Text;
using System.Data;

namespace Dapper研究.DAL
{
      public class DapperRepository<TEntity> : IDapperRepository<TEntity>
        where TEntity : class
      {
            private DapperDbContext<TEntity> context { get; set; }

            public DapperRepository()
            {
                  this.context = new DapperDbContext<TEntity>();
            }

            /// <summary>
            ///  Insert 、 Update 、 Delete 基本操作
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="instance"></param>
            /// <returns></returns>
            private int BaseOperation(string sql, TEntity instance)
            {
                  using (SqlConnection conn = new SqlConnection(context.DbConnectionString))
                  {
                        try
                        {
                              int result = conn.Execute(sql, instance);
                              return result;
                        }
                        catch (Exception ex)
                        {
                              System.Diagnostics.Debug.WriteLine(ex.Message);
                              return 0;
                        }
                  }
            }

            /// <summary>
            ///  相關 Range 的 基礎
            /// </summary>
            /// <param name="sql"></param>
            /// <returns></returns>
            private List<TEntity> BaseRange(string sql, List<TEntity> source, bool haveTransaction)
            {
                  List<TEntity> error = new List<TEntity>();

                  using (SqlConnection conn = new SqlConnection(context.DbConnectionString))
                  {
                        if (haveTransaction)
                        {
                              conn.Open();
                              using (var tran = conn.BeginTransaction())
                              {
                                    try
                                    {
                                          foreach (var item in source)
                                          {
                                                try
                                                {
                                                      conn.Execute(sql, item);
                                                }
                                                catch
                                                {
                                                      error.Add(item);
                                                      throw new Exception();
                                                }
                                          }

                                          tran.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                          System.Diagnostics.Debug.WriteLine(ex.Message);
                                          tran.Rollback();
                                    }
                              }
                        }
                        else
                        {
                              foreach (var item in source)
                              {
                                    try
                                    {
                                          conn.Execute(sql, item);
                                    }
                                    catch (Exception ex)
                                    {
                                          System.Diagnostics.Debug.WriteLine(ex.Message);
                                          error.Add(item);
                                    }
                              }
                        }
                  }

                  return error;
            }

            /// <summary>
            ///  如果得到的是 null ， 轉成 空的
            /// </summary>
            /// <param name="source"></param>
            /// <returns></returns>
            private TEntity NullToEntity(TEntity source)
            {
                  if (source == null)
                  {
                        return (TEntity)Activator.CreateInstance(typeof(TEntity));
                  }
                  else
                  {
                        return source;
                  }
            }

            /// <summary>
            ///  查詢 select top 的基本寫法
            /// </summary>
            /// <param name="query"></param>
            /// <param name="top"></param>
            /// <returns></returns>
            private List<TEntity> BaseSelectTop(List<TEntity> query, int top)
            {
                  List<TEntity> result = new List<TEntity>();

                  int index = 1;

                  foreach (var item in query)
                  {
                        if (index <= top)
                        {
                              result.Add(item);
                              index++;
                        }
                  }

                  return result;
            }

            /// <summary>
            /// 新增一筆
            /// </summary>
            /// <param name="instance">資料</param>
            /// <returns>是否成功</returns>
            public int Insert(TEntity instance)
            {
                  return this.BaseOperation(context.InsertString, instance);
            }

            /// <summary>
            /// 新增多筆
            /// </summary>
            /// <param name="source">多筆資料</param>
            /// <param name="haveTransaction">是否使用 TransactionScope</param>
            /// <returns>失敗項目</returns>
            public List<TEntity> InsertRange(List<TEntity> source, bool haveTransaction)
            {
                  return this.BaseRange(context.InsertString, source, haveTransaction);
            }

            /// <summary>
            /// 修改一筆 (拿全部 PK 去查再修改)
            /// </summary>
            /// <param name="instance">資料</param>
            /// <returns>影響筆數</returns>
            public int Update(TEntity instance)
            {
                  return this.BaseOperation(context.UpdateString, instance);
            }

            /// <summary>
            ///  修改 多筆 (自行輸入 where 條件)，且只修改幾個欄位(自行輸入)
            /// </summary>
            /// <param name="instance">要修改的欄位項目</param>
            /// <param name="where">條件</param>
            /// <returns>影響筆數</returns>
            public int Update(Dictionary<string, object> instance, Dictionary<string, object> where)
            {
                  List<string> instanceKeys = instance.Keys.ToList();
                  List<string> whereKeys = where.Keys.ToList();

                  using (SqlConnection conn = new SqlConnection(context.DbConnectionString))
                  {
                        try
                        {
                              DynamicParameters parameters = new DynamicParameters();

                              string sql = $"UPDATE {context.TableName} SET {string.Join(", ", instanceKeys.Select(n => n + " = @" + n))} WHERE {string.Join(" AND ", whereKeys.Select(n => n + " = @" + n))};";

                              if (whereKeys.Count() <= 0)
                              {
                                    sql = sql.Replace(" WHERE ", string.Empty);
                              }

                              foreach (var key in instanceKeys)
                              {
                                    parameters.Add(key, instance[key]);
                              }

                              foreach (var key in whereKeys)
                              {
                                    parameters.Add(key, where[key]);
                              }

                              int result = conn.Execute(sql, parameters);
                              return result;
                        }
                        catch (Exception ex)
                        {
                              System.Diagnostics.Debug.WriteLine(ex.Message);
                              return 0;
                        }
                  }
            }

            /// <summary>
            /// 修改多筆 (多個 update ，等同 Update(TEntity instance) 的多筆版)
            /// </summary>
            /// <param name="source">資料</param>
            /// <param name="haveTransaction">是否使用 TransactionScope</param>
            /// <returns>失敗項目</returns>
            public List<TEntity> UpdateRange(List<TEntity> source, bool haveTransaction)
            {
                  return this.BaseRange(context.UpdateString, source, haveTransaction);
            }

            /// <summary>
            /// 刪除一筆 (拿全部 PK 去查再修改)
            /// </summary>
            /// <param name="instance">資料</param>
            /// <returns>影響筆數</returns>
            public int Delete(TEntity instance)
            {
                  return this.BaseOperation(context.DeleteString, instance);
            }

            /// <summary>
            /// 刪除多筆 (拿 輸入的 where 條件 去刪除)
            /// </summary>
            /// <param name="where">條件</param>
            /// <returns>影響筆數</returns>
            public int Delete(Dictionary<string, object> where)
            {
                  List<string> whereKeys = where.Keys.ToList();

                  using (SqlConnection conn = new SqlConnection(context.DbConnectionString))
                  {
                        try
                        {
                              DynamicParameters parameters = new DynamicParameters();

                              string sql = $"DELETE {context.TableName} WHERE {string.Join(" AND ", whereKeys.Select(n => n + " = @" + n))};";

                              if (whereKeys.Count() <= 0)
                              {
                                    sql = sql.Replace(" WHERE ", string.Empty);
                              }

                              foreach (var key in whereKeys)
                              {
                                    parameters.Add(key, where[key]);
                              }

                              int result = conn.Execute(sql, parameters);
                              return result;
                        }
                        catch (Exception ex)
                        {
                              System.Diagnostics.Debug.WriteLine(ex.Message);
                              return 0;
                        }
                  }
            }

            /// <summary>
            /// 刪除多筆 (多個 delete ，等同 Delete(TEntity instance) 的多筆版)
            /// </summary>
            /// <param name="source">資料</param>
            /// <param name="haveTransaction">是否使用 TransactionScope</param>
            /// <returns>失敗項目</returns>
            public List<TEntity> DeleteRange(List<TEntity> source, bool haveTransaction)
            {
                  return this.BaseRange(context.DeleteString, source, haveTransaction);
            }

            /// <summary>
            ///  查詢 (用 PK 來查)
            /// </summary>
            /// <returns></returns>
            public TEntity Select(TEntity instance)
            {
                  TEntity query;
                  using (SqlConnection conn = new SqlConnection(context.DbConnectionString))
                  {
                        query = conn.Query<TEntity>(context.SelectString, instance).FirstOrDefault();
                  }
                  return this.NullToEntity(query);
            }

            /// <summary>
            /// 查詢 (用你輸入的PK來查)
            /// </summary>
            /// <param name="where">條件</param>
            /// <returns>結果</returns>
            public IQueryable<TEntity> Select(Dictionary<string, object> where)
            {
                  List<string> whereKeys = where.Keys.ToList();

                  using (SqlConnection conn = new SqlConnection(context.DbConnectionString))
                  {
                        try
                        {
                              DynamicParameters parameters = new DynamicParameters();

                              string sql = $"SELECT * FROM {context.TableName} WHERE {string.Join(" AND ", whereKeys.Select(n => n + " = @" + n))};";

                              if (whereKeys.Count() <= 0)
                              {
                                    sql = sql.Replace(" WHERE ", string.Empty);
                              }

                              foreach (var key in whereKeys)
                              {
                                    parameters.Add(key, where[key]);
                              }

                              var result = conn.Query<TEntity>(sql, parameters).AsQueryable();
                              return result;
                        }
                        catch (Exception ex)
                        {
                              System.Diagnostics.Debug.WriteLine(ex.Message);
                              return new List<TEntity>().AsQueryable();
                        }
                  }
            }

            /// <summary>
            /// 查詢 (用你輸入的PK來查)
            /// </summary>
            /// <param name="where">條件</param>
            /// <param name="orderBy">order by 的項目</param>
            /// <returns>結果</returns>
            public IQueryable<TEntity> Select(Dictionary<string, object> where, List<string> orderBy)
            {
                  List<string> whereKeys = where.Keys.ToList();

                  using (SqlConnection conn = new SqlConnection(context.DbConnectionString))
                  {
                        try
                        {
                              DynamicParameters parameters = new DynamicParameters();

                              string sql = $"SELECT * FROM {context.TableName} WHERE {string.Join(" AND ", whereKeys.Select(n => n + " = @" + n))} ORDER BY {string.Join(", ", orderBy)};";

                              if (whereKeys.Count() <= 0)
                              {
                                    sql = sql.Replace(" WHERE ", string.Empty);
                              }

                              if (orderBy.Count() <= 0)
                              {
                                    sql = sql.Replace(" ORDER BY ", string.Empty);
                              }

                              foreach (var key in whereKeys)
                              {
                                    parameters.Add(key, where[key]);
                              }

                              var result = conn.Query<TEntity>(sql, parameters).AsQueryable();
                              return result;
                        }
                        catch (Exception ex)
                        {
                              System.Diagnostics.Debug.WriteLine(ex.Message);
                              return new List<TEntity>().AsQueryable();
                        }
                  }
            }

            /// <summary>
            /// 查詢 第一筆
            /// </summary>
            /// <param name="where">條件</param>
            /// <returns>第一筆</returns>
            public TEntity SelectFirst(Dictionary<string, object> where)
            {
                  TEntity result = this.Select(where).FirstOrDefault();
                  return this.NullToEntity(result);
            }

            /// <summary>
            /// 查詢 第一筆
            /// </summary>
            /// <param name="where">條件</param>
            /// <param name="orderBy">order by 的項目</param>
            /// <returns>第一筆</returns>
            public TEntity SelectFirst(Dictionary<string, object> where, List<string> orderBy)
            {
                  TEntity result = this.Select(where, orderBy).FirstOrDefault();
                  return this.NullToEntity(result);
            }

            /// <summary>
            /// 查詢 最後一筆
            /// </summary>
            /// <param name="where">條件</param>
            /// <returns>最後一筆</returns>
            public TEntity SelectLast(Dictionary<string, object> where)
            {
                  TEntity result = this.Select(where).LastOrDefault();
                  return this.NullToEntity(result);
            }

            /// <summary>
            /// 查詢 最後一筆
            /// </summary>
            /// <param name="where">條件</param>
            /// <param name="orderBy">order by 的項目</param>
            /// <returns>最後一筆</returns>
            public TEntity SelectLast(Dictionary<string, object> where, List<string> orderBy)
            {
                  TEntity result = this.Select(where, orderBy).LastOrDefault();
                  return this.NullToEntity(result);
            }

            /// <summary>
            /// 查詢 前 幾筆
            /// </summary>
            /// <param name="where">條件</param>
            /// <param name="top">前幾筆</param>
            /// <returns>前幾筆資料</returns>
            public List<TEntity> SelectTop(Dictionary<string, object> where, int top)
            {
                  List<TEntity> query = this.Select(where).ToList();
                  return this.BaseSelectTop(query, top);
            }

            /// <summary>
            /// 查詢 前 幾筆
            /// </summary>
            /// <param name="where">條件</param>
            /// <param name="orderBy">order by 的項目</param>
            /// <param name="top">前幾筆</param>
            /// <returns>前幾筆資料</returns>
            public List<TEntity> SelectTop(Dictionary<string, object> where, List<string> orderBy, int top)
            {
                  List<TEntity> query = this.Select(where, orderBy).ToList();
                  return this.BaseSelectTop(query, top);
            }

            /// <summary>
            /// 查詢全部
            /// </summary>
            /// <returns>結果</returns>
            public IQueryable<TEntity> SelectAll()
            {
                  using (SqlConnection conn = new SqlConnection(context.DbConnectionString))
                  {
                        try
                        {
                              string sql = $"SELECT * FROM {context.TableName};";
                              var result = conn.Query<TEntity>(sql).AsQueryable();
                              return result;
                        }
                        catch (Exception ex)
                        {
                              System.Diagnostics.Debug.WriteLine(ex.Message);
                              return new List<TEntity>().AsQueryable();
                        }
                  }
            }

            /// <summary>
            /// 查詢全部
            /// </summary>
            /// <param name="orderBy">order by 的項目</param>
            /// <returns>結果</returns>
            public IQueryable<TEntity> SelectAll(List<string> orderBy)
            {
                  using (SqlConnection conn = new SqlConnection(context.DbConnectionString))
                  {
                        try
                        {
                              string sql = $"SELECT * FROM {context.TableName} ORDER BY {string.Join(", ", orderBy)};";

                              if (orderBy.Count() <= 0)
                              {
                                    sql = sql.Replace(" ORDER BY ", string.Empty);
                              }

                              var result = conn.Query<TEntity>(sql).AsQueryable();
                              return result;
                        }
                        catch (Exception ex)
                        {
                              System.Diagnostics.Debug.WriteLine(ex.Message);
                              return new List<TEntity>().AsQueryable();
                        }
                  }
            }
      }
}