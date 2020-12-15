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
      public class DapperDbContext<TEntity>
      {
            public string DbConnectionString = string.Empty;
            public string TableName = string.Empty;
            public List<string> pkList = new List<string>();
            public List<string> allColnumNameList = new List<string>();
            public List<string> canInsertColnumList = new List<string>();
            public List<string> notPkList = new List<string>();

            public string SelectString = string.Empty;
            public string InsertString = string.Empty;
            public string UpdateString = string.Empty;
            public string DeleteString = string.Empty;

            public DapperDbContext()
            {
                  this.DbConnectionString = GetDbConnection();
                  this.TableName = GetTableName();
                  this.pkList = GetAllPk();
                  this.InsertString = GetInsertString();
                  this.UpdateString = GetUpdateString();
                  this.DeleteString = GetDeleteString();
                  this.SelectString = GetSelectString();
            }

            /// <summary>
            /// 給定 連線字串 (可自己修改)
            /// </summary>
            /// <returns></returns>
            private string GetDbConnection()
            {
                  // 連線字串自己寫死設定，以後再改成彈性的
                  return System.Configuration.ConfigurationManager.ConnectionStrings["dbContext"].ToString();
            }

            /// <summary>
            ///  得到資料表名稱
            /// </summary>
            /// <returns></returns>
            private string GetTableName()
            {
                  return typeof(TEntity).Name;
            }

            /// <summary>
            ///  找出所有的 PK
            /// </summary>
            /// <returns></returns>
            private List<string> GetAllPk()
            {
                  List<string> result = new List<string>();

                  using (SqlConnection conn = new SqlConnection(this.DbConnectionString))
                  {
                        try
                        {
                              DynamicParameters parameters = new DynamicParameters();

                              string sql = @"SELECT COLUMN_NAME 
                                    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                                    WHERE TABLE_NAME = @TableName
                                    AND CONSTRAINT_NAME like 'PK_%';";

                              parameters.Add("TableName", this.TableName);
                              result = conn.Query<string>(sql, parameters).ToList();
                              return result;
                        }
                        catch (Exception ex)
                        {
                              System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                  }

                  return result;
            }

            /// <summary>
            ///  組出 Select 基本語法
            /// </summary>
            /// <returns></returns>
            private string GetSelectString()
            {
                  if (pkList.Count() <= 0)
                  {
                        return $"SELECT * FROM {this.TableName};";
                  }
                  else
                  {
                        return $"SELECT * FROM {this.TableName} WHERE {string.Join(" AND ", this.pkList.Select(n => n + " = @" + n))};";
                  }
            }

            /// <summary>
            ///  先找出 全部 的欄位 + 組出 Insert 語法
            /// </summary>
            /// <returns></returns>
            private string GetInsertString()
            {
                  string result = string.Empty;

                  using (SqlConnection conn = new SqlConnection(this.DbConnectionString))
                  {
                        try
                        {
                              DynamicParameters parameters = new DynamicParameters();

                              string sql = @"SELECT b.COLUMN_NAME as ColnumName,
                                     (SELECT is_identity 
                                        FROM sys.columns c
                                        INNER JOIN sys.tables ts ON ts.OBJECT_ID = c.OBJECT_ID
                                        where c.name = b.COLUMN_NAME 
                                        AND ts.name = a.TABLE_NAME) 
                                        AS is_identity
		                            FROM INFORMATION_SCHEMA.TABLES  a 
		                            LEFT JOIN INFORMATION_SCHEMA.COLUMNS b ON (a.TABLE_NAME=b.TABLE_NAME) 
		                            WHERE a.TABLE_NAME = @TableName
		                            ORDER BY a.TABLE_NAME, ordinal_position;";

                              parameters.Add("TableName", this.TableName);
                              var colnumDetail = conn.Query<dynamic>(sql, parameters).ToList();

                              // 開始組出 Insert 字串
                              foreach (var c in colnumDetail)
                              {
                                    string colnumName = c.ColnumName;
                                    bool is_identity = c.is_identity;

                                    this.allColnumNameList.Add(colnumName);
                                    if (is_identity == false)
                                    {
                                          this.canInsertColnumList.Add(colnumName);
                                    }
                              }

                              this.notPkList = this.allColnumNameList.Except(this.pkList).ToList();

                              result = $"INSERT INTO {TableName} ({string.Join(", ", canInsertColnumList)}) VALUES ({string.Join(", ", canInsertColnumList.Select(c => "@" + c))});";
                        }
                        catch (Exception ex)
                        {
                              System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                  }

                  return result;
            }

            /// <summary>
            ///  組出 基本 Update 語法
            /// </summary>
            /// <returns></returns>
            private string GetUpdateString()
            {
                  if (pkList.Count() <= 0)
                  {
                        return $"UPDATE {this.TableName} SET {string.Join(", ", this.notPkList.Select(n => n + " = @" + n))};";
                  }
                  else
                  {
                        return $"UPDATE {this.TableName} SET {string.Join(", ", this.notPkList.Select(n => n + " = @" + n))} WHERE {string.Join(" AND ", this.pkList.Select(n => n + " = @" + n))};";
                  }
            }

            /// <summary>
            ///  組出 基本 Delete 語法
            /// </summary>
            /// <returns></returns>
            private string GetDeleteString()
            {
                  return $"DELETE {this.TableName} WHERE {string.Join(" AND ", this.pkList.Select(n => n + " = @" + n))};";
            }
      }

}