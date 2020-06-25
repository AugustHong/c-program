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
                conn.Open();
                
                //交易 (一定要先 conn.Open過才能用)
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        //string delStr = "Delete Test4 where id =2;";
                        //conn.Execute(delStr, transaction:tran, commandType: CommandType.Text);

                        //string instStr = "Insert into Test4 (id, kk) values (2, 'yy');";
                        //conn.Execute(instStr, transaction: tran, commandType: CommandType.Text);


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

    public static class DapperDebugHelper
    {
        /// <summary>
        ///  Dapper 中 Insert 超出上限只會回 sql 本身丟出的 二進位或字串超出上限
        ///  並無法知道是哪一欄
        ///  注意： 只會特別去找 是 "字串" 類型的欄位 其他數字類大小就不會去找
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<string> DebugForInsertStringOverLength(string sql, DynamicParameters parameters, string connString)
        {
            List<string> result = new List<string>();

            if (!string.IsNullOrEmpty(sql) && !string.IsNullOrEmpty(connString))
            {
                // 先得到所要的資料
                // 要的參數
                string tableName = string.Empty;
                List<string> tableColnumName = new List<string>();
                List<string> inputVarName = new List<string>();

                HandleBaseInsertSql(sql, out tableName, tableColnumName, inputVarName);

                // 如果 傳入的 和 接收的 數目不合 要提示
                if (tableColnumName.Count() != inputVarName.Count())
                {
                    result.Add("輸入的參數個數和接收的個數不同");
                }

                if (!string.IsNullOrEmpty(tableName))
                {
                    List<dynamic> data;   // 得到的 資料表各欄位資料

                    // 去取到 資料表詳細
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string querySql = @"SELECT b.COLUMN_NAME as 欄位名稱, 
                                        b.DATA_TYPE as 資料型別, 
                                        b.CHARACTER_MAXIMUM_LENGTH  as 最大長度
                                        FROM INFORMATION_SCHEMA.TABLES  a 
                                        LEFT JOIN INFORMATION_SCHEMA.COLUMNS b ON (a.TABLE_NAME=b.TABLE_NAME) 
                                        WHERE a.TABLE_NAME = @TableName ;";

                        DynamicParameters querypar = new DynamicParameters();
                        querypar.Add("TableName", tableName);

                        data = conn.Query<dynamic>(querySql, querypar).ToList();
                        if (data.Count() <= 0)
                        {
                            result.Add("查無此資料表的資料");
                        }
                        else
                        {
                            // 去把 DynamicParameters 解析出來
                            List<string> parNameList = new List<string>();
                            List<string> parValueList = new List<string>();

                            GetDynamicParametersNameAndValue(parameters, parNameList, parValueList);

                            if ((parNameList.Count() != tableColnumName.Count()) || (parNameList.Count() != inputVarName.Count()))
                            {
                                result.Add("在 DynamicParameters 得到的個數 和你寫在 sql 語法中的參數數目不合");
                            }
                            else
                            {
                                // 對這幾個 字串類型的才做處理
                                List<string> canJudgeList = new List<string> { "text", "char" };

                                // 去做判斷了 (跑過所有的 parName)
                                for (var i = 0; i < tableColnumName.Count(); i++)
                                {
                                    string tableColnum = tableColnumName[i];
                                    string inputVar = inputVarName[i];
                                    string parValue = parValueList[i];

                                    dynamic item = data.Where(d => d.欄位名稱 == tableColnum).FirstOrDefault();
                                    if (item != null)
                                    {
                                        string type = item.資料型別;

                                        if (canJudgeList.Where(c => type.Contains(c)).Count() > 0)
                                        {
                                            int maxLen =  item.最大長度;

                                            if (parValue.Length > maxLen)
                                            {
                                                result.Add($"{tableColnum} 欄位的最大長度是 {maxLen} ， 而你 輸入的變數 {inputVar} 其值是 {parValue} 長度是 {parValue.Length} 已超過上限，請修改");
                                            }
                                            else
                                            {
                                                System.Diagnostics.Debug.WriteLine($"{tableColnum} 欄位的最大長度是 {maxLen} ， 而你輸入的 變數名稱是 {inputVar} 其值是 {parValue}  長度是 {parValue.Length}");
                                            }
                                        }
                                        else
                                        {
                                            System.Diagnostics.Debug.WriteLine($"{tableColnum} 欄位的類型是 {type} 而你輸入的 變數名稱是 {inputVar} 其值是 {parValue} ， 故不特別去判斷 字串長度是否超過");
                                        }
                                    }
                                    else
                                    {
                                        result.Add($"{tableColnum} -- 查無此 欄位名稱");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Add("沒有輸入資料表名稱");
                }
            }

            return result;
        }

        /// <summary>
        ///  將 Insert 語法 轉成所要的資料
        /// </summary>
        /// <param name="tableName">表格名稱</param>
        /// <param name="tableColnumName">資料表的欄位名稱</param>
        /// <param name="inputVarName">輸入的變數名稱</param>
        private static void HandleBaseInsertSql(string sql, out string tableName, List<string> tableColnumName, List<string> inputVarName)
        {
            tableName = string.Empty;

            if (!string.IsNullOrEmpty(sql))
            {
                // 先進行分割
                // source = insert into Table (a, b, c, d, e) values (@aa, @bb, @cc, @dd, @ee);
                // 1 => insert into Table (a, b, c, d, e( values (@aa, @bb, @cc, @dd, @ee(;
                // 分解成 => insert into Table a, b, c, d, e  values @aa, @bb, @cc, @dd, @ee ;
                // Index =>         0             1             2          3                 4
                List<string> data = sql.Replace(")", "(").Split('(').ToList();

                // 得到 資料表名稱
                if (data.Count() > 0)
                {
                    // 真實資料可能不會給你這麼好， 可能有很多空餘的空白
                    // 所以先用 空白 分開 => 再從尾巴數 第1個不是 空白的
                    List<string> item = data[0].Split(' ').ToList();
                    string tN = item.Where(x => !string.IsNullOrEmpty(x.Trim())).LastOrDefault();
                    tableName = string.IsNullOrEmpty(tN) ? string.Empty : tN;
                }

                // 得到 資料表的欄位名稱
                if (data.Count() > 1)
                {
                    // 先把多餘的空白去掉
                    string source = data[1].Replace(" ", string.Empty);

                    // 用 , 分開
                    List<string> item = source.Split(',').ToList();
                    tableColnumName.AddRange(item);
                }

                // 得到 輸入的參數名稱
                if (data.Count() > 3)
                {
                    // 先把多餘的空白去掉 和 輸入的 @ 去掉
                    string source = data[3].Replace(" ", string.Empty).Replace("@", string.Empty);

                    // 用 , 分開
                    List<string> item = source.Split(',').ToList();
                    inputVarName.AddRange(item);
                }
            }
        }

        /// <summary>
        /// 把 DynamicParameters 解析出 Name 和 Value
        /// Value 都用 string 先來接
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="parNameList"></param>
        /// <param name="parValueList"></param>
        private static void GetDynamicParametersNameAndValue(DynamicParameters parameters, List<string> parNameList, List<string> parValueList)
        {
            foreach (var paramName in parameters.ParameterNames)
            {
                parNameList.Add(paramName);

                string parValue = string.Empty;
                if (parameters.Get<dynamic>(paramName) is System.Collections.IList)
                {
                    parValue = string.Join(", ", parameters.Get<dynamic>(paramName));
                }
                else
                {
                    parValue = Convert.ToString(parameters.Get<dynamic>(paramName));
                }
                parValueList.Add(parValue);
            }
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