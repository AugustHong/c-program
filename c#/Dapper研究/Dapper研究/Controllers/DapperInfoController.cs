using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/*
 先去 NuGet 裝上 Dapper 和 Dapper.Contrib 和 System.Data.SqlClient
 */
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using System.Text;
using System.Data;

namespace Dapper研究.Controllers
{
    public class DapperInfoController : Controller
    {
        // 連線字串
        private string connString
        {
            get
            {
                //抓我們寫在web.config裡的連線字串（裡面的dbContext是當時寫連線字串中寫的，如果改了這裡也要改）
                return System.Configuration.ConfigurationManager.ConnectionStrings["dbContext"].ToString();
            }
        }

        // GET: DapperInfo
        public string Index()
        {

            //Insert用Model();
            //Insert用DynamicParameters();
            //用List的();
            //SelectFirst();
            //SelectList();
            //得到多個結果DataSet的概念();
            //Select出DataTable型別或轉出List();
            //Select出DataSet型別();
            //傳入參數用DtatTable型別();

            return "測試完成";
        }

        public void Insert用Model()
        {
            // 其餘 Delete 和 Update 也可以用這個
            Test t = new Test
            {
                text = "Insert 用 Model",
                value = 1111111
            };

            using (var conn = new SqlConnection(connString))
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(@"INSERT INTO Test (text, value) VALUES (@text, @value) ");

                // 後面第2個參數是放 Object => 所以可以寫 {} 或傳 Model 進來
                conn.Execute(sql.ToString(), t);

                // 也可用非同步
                //conn.ExecuteAsync();
            }
        }

        public void Insert用DynamicParameters()
        {
            // 其餘 Delete 和 Update 也可以用這個
            string text = "Insert用DynamicParameters";
            int value = 2222222;

            using (var conn = new SqlConnection(connString))
            {
                StringBuilder sql = new StringBuilder();

                DynamicParameters parameters = new DynamicParameters();

                sql.Append(@"INSERT INTO Test (text, value) VALUES (@text, @value) ");

                parameters.Add("text", text);
                parameters.Add("value", value);

                // 後面第2個參數是放 Object => 所以可以寫 {} 或傳 Model 進來
                conn.Execute(sql.ToString(), parameters);

                // 也可用非同步
                //conn.ExecuteAsync();
            }
        }

        public void 用List的()
        {
            // 用 List 的變數 會讓 一句話 變成重複
            List<Test> t = new List<Test>
            {
                new Test
                {
                    text = "List1",
                    value = 1
                },
                new Test
                {
                    text = "List2",
                    value = 2
                }
            };

            using (var conn = new SqlConnection(connString))
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(@"INSERT INTO Test (text, value) VALUES (@text, @value) ");

                // 後面第2個參數是放 Object => 所以可以寫 {} 或傳 Model 進來
                conn.Execute(sql.ToString(), t);

                // 執行 會變成 執行2 次 Insert 然後各自的變數對應到各自的值

                // 也可用非同步
                //conn.ExecuteAsync();
            }
        }

        public void SelectFirst()
        {
            int range = 300;

            using (var conn = new SqlConnection(connString))
            {
                StringBuilder sql = new StringBuilder();

                DynamicParameters parameters = new DynamicParameters();

                sql.Append(@"SELECT * FROM Test WHERE value <= @range");

                parameters.Add("range", range);

                // 後面第2個參數是放 Object => 所以可以寫 {} 或傳 Model 進來
                SelectTestResult result = conn.QueryFirst<SelectTestResult>(sql.ToString(), parameters);

                // 也可用非同步
                //conn.QueryFirstAsync();
            }
        }

        public void SelectList()
        {
            int range = 300;

            using (var conn = new SqlConnection(connString))
            {
                StringBuilder sql = new StringBuilder();

                DynamicParameters parameters = new DynamicParameters();

                sql.Append(@"SELECT * FROM Test WHERE value <= @range");

                parameters.Add("range", range);

                // 後面第2個參數是放 Object => 所以可以寫 {} 或傳 Model 進來
                List<SelectTestResult> result = conn.Query<SelectTestResult>(sql.ToString(), parameters).ToList();

                // 也可用非同步
                //conn.QueryAsync();
            }
        }

        public void 得到多個結果DataSet的概念()
        {
            int minRange = 300;
            int maxRange = 500;

            using (var conn = new SqlConnection(connString))
            {
                StringBuilder sql = new StringBuilder();

                DynamicParameters parameters = new DynamicParameters();

                sql.Append(@"SELECT * FROM Test WHERE value <= @minRange;");
                sql.Append(@"SELECT * FROM Test WHERE value > @maxRange;");

                parameters.Add("minRange", minRange);
                parameters.Add("maxRange", maxRange);

                // 可以看到 有2張結果 (在 ADO.NET 中會用 DataSet) 但這邊一樣可以處理
                SqlMapper.GridReader result = conn.QueryMultiple(sql.ToString(), parameters);
                List<SelectTestResult> result1 = result.Read<SelectTestResult>().ToList();   // 讀到第一個
                List<SelectTestResult> result2 = result.Read<SelectTestResult>().ToList();   // 讀到第二個
                // 就會依序讀下去 (像 Queue 一樣，讀完全沒了) => 像這次有2個，所以就讀2次得到結果

                // 這邊試試看 讀第3次 看會不會錯誤 => ANS : 會爆錯喔 ~
                // List<SelectTestResult> result3 = result.Read<SelectTestResult>().ToList();
            }
        }

        /// <summary>
        ///  特別： 平常用 SP 的話 不太會用 Query，但這裡用 Excute的 可以轉回List
        /// </summary>
        public void Select出DataTable型別或轉出List()
        {
            DataTable dt = new DataTable();
            int range = 300;

            using (var conn = new SqlConnection(connString))
            {
                StringBuilder sql = new StringBuilder();

                DynamicParameters parameters = new DynamicParameters();

                sql.Append(@"SELECT * FROM Test WHERE value <= @range");

                parameters.Add("range", range);

                // 後面第2個參數是放 Object => 所以可以寫 {} 或傳 Model 進來
                var dataReader = conn.ExecuteReader(sql.ToString(), parameters);

                // 也可用非同步
                //conn.ExecuteReaderAsync();

                // 也可用這個轉成 List<T> (但轉為 dataReader 就會不見 所以後面的 dt.Load就會沒有了)
                // 所以 List 和 DataTable 只能選一個來做，不管哪個前哪個後 同時出現後面的一定錯
                //List<SelectTestResult> rr = dataReader.Parse<SelectTestResult>().ToList();

                // 得到 DataTable
                dt.Load(dataReader);

                /*
                    如果要轉成 List<List<object>> 物件 可以這樣做
                    List<List<object>> data = new List<List<object>>();

                    while (dataReader.Read())
                    {
                        List<object> row = new List<object>();
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            row.Add(dataReader.GetValue(i));
                        }
                        data.Add(row);
                    }
                 */
            }
        }

        public void Select出DataSet型別()
        {
            DataSet ds = new DataSet();

            int minRange = 300;
            int maxRange = 500;

            using (var conn = new SqlConnection(connString))
            {
                StringBuilder sql = new StringBuilder();

                DynamicParameters parameters = new DynamicParameters();

                sql.Append(@"SELECT * FROM Test WHERE value <= @minRange;");
                sql.Append(@"SELECT * FROM Test WHERE value > @maxRange;");

                parameters.Add("minRange", minRange);
                parameters.Add("maxRange", maxRange);

                var dataReader = conn.ExecuteReader(sql.ToString(), parameters);

                // 表名
                string[] TableNameLists = (new List<string> { "Min", "Max" }).ToArray();
                ds.Load(dataReader, LoadOption.PreserveChanges, TableNameLists);

                // 缺點： 你必需要先知道有多少表會出來，才能造相對應的 TableNameList 
                // 缺點 可以用下面的 方法二
                // 否則還是用 ADO 的方法
                /*
                    DataSet result = new DataSet();
                    SqlCommand Cmmd = new SqlCommand(sbSql.ToString(), conn);
                    SqlDataAdapter myAdapter = new SqlDataAdapter(Cmmd);   
                    // 但這種的不能用傳參數的，都是要組好的純文字 
                    // (例如： SELECT * FROM Test WHERE value <= 300; ) 這300 就要先組好

                    myAdapter.Fill(result);

                    // 請看下方的 GetTableDataBySQL(List<string> sqls)
                 */
            }


            // 上面所說的問題解法
            DataSet ds2 = new DataSet();
            using (var conn = new SqlConnection(connString))
            {
                StringBuilder sql = new StringBuilder();

                DynamicParameters parameters = new DynamicParameters();

                sql.Append(@"SELECT * FROM Test WHERE value <= @minRange;");
                sql.Append(@"SELECT * FROM Test WHERE value > @maxRange;");

                parameters.Add("minRange", minRange);
                parameters.Add("maxRange", maxRange);

                var dataReader = conn.ExecuteReader(sql.ToString(), parameters);
                //dataReader.NextResult  (可以用 While (dataReader.NextResult) 跑 把每筆都 產生一個DataTable 再 Add 進去
                // ds.Table 也是一種方法 => 就看要怎麼做了)

                bool haveNext = true;
                int currentIndex = 1;
                while (haveNext)
                {
                    DataTable dt = new DataTable();
                    dt.TableName =  $"xxxx_{currentIndex}";   //也可自行設 TableName
                    dt.Load(dataReader);   // Load 完自己就會跑到下個結果了
                    ds2.Tables.Add(dt);
                    currentIndex++;
                    haveNext = !dataReader.IsClosed;   // 如果 沒東西 會是 Closed
                }
            }
        }

        public void 傳入參數用DtatTable型別()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[1] { new DataColumn("VALUE", typeof(int)) });

            // 加入資料
            dt.Rows.Add(222);
            dt.Rows.Add(333);
            dt.Rows.Add(666);

            using (var conn = new SqlConnection(connString))
            {
                StringBuilder sql = new StringBuilder();

                DynamicParameters parameters = new DynamicParameters();

                // 這一段是 因為要用 DataTable 要先建立 CREATE TYPE (要建立這個結構，下面的才能用)
                // 裡面的內容 要跟傳入的 DataTable 是一樣的
                // 最好先建好
                //sql.Append(@"CREATE TYPE [dbo].[TestV] AS TABLE(
                //                [VALUE] int
                //            )");

                sql.Append(@"SELECT * FROM Test WHERE value in (Select * from @Source);");

                // 加完要 Drop 掉 (不會太快 Drop 掉 => 不然很容易找不到)
                // 可以等得到結果 (已回傳)  再Drop 掉 ； 或者 根本就在資料庫建好 不要在 主sql 前後加上這些
                //sql.Append(@"Drop type [dbo].[TestV]");

                // 後面的 是你的 DataTable 的 資料類型 (上面第一個 sql 所建立的就是為了它)
                var dp = dt.AsTableValuedParameter("dbo.TestV");
                parameters.Add("Source", dp);

                // 後面第2個參數是放 Object => 所以可以寫 {} 或傳 Model 進來
                List<SelectTestResult> result = conn.Query<SelectTestResult>(sql.ToString(), parameters).ToList();

                // 也可用非同步
                //conn.QueryAsync();
            }
        }

        public void 使用Stored_Procedure()
        {
            // 因為這個測試資料庫沒有 SP => 故這個一定不會成功

            using (var conn = new SqlConnection(connString))
            {
                //準備參數 (後面 dbType 是傳入的資料類型， 而 direction 是決定是 Input 還是 Output 還是 ReturnValue)
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Param1", "abc", DbType.String, ParameterDirection.Input);
                parameters.Add("@OutPut1", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Return1", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                // 要特別寫 commandType: CommandType.StoredProcedure
                /*
                    也可以寫成
                    string sql = @"declare @rr int;
                                    declare @rv int;
                                    exec MyStoredProcedure @Param1 ,@rr output, @rv;
                                    select @rr, @rv;
                                    "

                    然後用 conn.Query() 來做也行
                 */
                conn.Execute("MyStoredProcedure", parameters, commandType: CommandType.StoredProcedure);

                //接回Output值
                int outputResult = parameters.Get<int>("@OutPut1");
                //接回Return值
                int returnResult = parameters.Get<int>("@Return1");
            }
        }

        public void Dapper中使用Transaction()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                //交易 (一定要先 conn.Open過才能用)
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // conn.Execute(strSql, datas);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        tran.Rollback();
                    }
                }
            }

            // 注意： 也還是可以使用 TransactionScope
        }

        public void Dapper使用像EF的功能()
        {
            // 這功能沒測過 => 只做筆記用，實際盡量不要用
            SelectTestResult t = new SelectTestResult();
            SelectTestResult ut = new SelectTestResult();
            SelectTestResult dt = new SelectTestResult();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                // Insert
                long r = conn.Insert<SelectTestResult>(t);

                // Select
                SelectTestResult rr = conn.Get<SelectTestResult>(2);

                // Update
                bool rrr = conn.Update<SelectTestResult>(ut);

                // Delete
                bool rrrr = conn.Delete<SelectTestResult>(dt);

                // 刪掉全部
                bool all = conn.DeleteAll<SelectTestResult>();
            }
        }

        /// <summary>
        ///  自行準備T-SQL , WHERE 條件自行準備 => 產出 DataSet
        /// </summary>
        /// <param name="Sqls"></param>
        /// <returns></returns>
        public DataSet GetTableDataBySQL(List<string> sqls)
        {
            DataSet result = new DataSet();

            using (var conn = new SqlConnection(connString))
            {
                StringBuilder sbSql = new StringBuilder();

                foreach (string sql in sqls)
                {
                    sbSql.Append(sql + ";");
                }

                SqlCommand Cmmd = new SqlCommand(sbSql.ToString(), conn);
                SqlDataAdapter myAdapter = new SqlDataAdapter(Cmmd);

                myAdapter.Fill(result);
            }


            return result;
        }
    }

    public class Test
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class SelectTestResult
    {
        public decimal id { get; set; }
        public string text { get; set; }
        public int value { get; set; }
    }
}