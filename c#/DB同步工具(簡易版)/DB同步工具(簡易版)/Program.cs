using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB同步工具_簡易版_
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("本程式是將 來源的 資料庫 內容 同步至 目標的 資料庫 (即 目標DB的內容將會全部跟來源DB的內容一樣)");
            Console.WriteLine("============================================================================================================");

            // 先問來源
            string sourceSqlConnection = string.Empty;
            string sourceDB_Name = string.Empty;
            List<string> sourceTableList = new List<string>();

            // 得到 連線字串+DB名稱+資料表列表
            Console.WriteLine("請輸入來源端資訊：");
            sourceSqlConnection = AskSqlConnection(sourceTableList, out sourceDB_Name);

            // 得到 ColnumList+PkList+選定的 Table
            List<ColnumLayout> sourceColnumList = new List<ColnumLayout>();
            List<string> sourcePkList = new List<string>();
            string sourceTable = AskTable(sourceColnumList, sourcePkList, sourceSqlConnection, sourceTableList);

            Console.WriteLine("============================================================================================================");

            // 再來問 目標
            string goalSqlConnection = string.Empty;
            string goalDB_Name = string.Empty;
            List<string> goalTableList = new List<string>();

            // 得到 連線字串+DB名稱+資料表列表
            Console.WriteLine("請輸入目標端資訊：");
            goalSqlConnection = AskSqlConnection(goalTableList, out goalDB_Name);

            // 這邊跟上面不一樣的地方是 (一定要確保 目標的 結構+PK完完全全和 來源的 相同)
            bool sourceSameToGoal = false;

            // 得到 ColnumList+PkList+選定的 Table
            List<ColnumLayout> goalColnumList = new List<ColnumLayout>();
            List<string> goalPkList = new List<string>();
            string goalTable = string.Empty;

            while (sourceSameToGoal == false)
            {
                goalTable = AskTable(goalColnumList, goalPkList, goalSqlConnection, goalTableList);
                bool success = SourceIsSameToGoal(sourceColnumList, sourcePkList, goalColnumList, goalPkList);
                if (success)
                {
                    sourceSameToGoal = true;
                }
                else
                {
                    goalColnumList = new List<ColnumLayout>();
                    goalPkList = new List<string>();
                    goalTable = string.Empty;
                }
            }

            // ColnumNameList
            List<string> colnumNameList = sourceColnumList.Select(x => x.欄位名稱).ToList();

            Console.WriteLine("============================================================================================================");

            // 最精彩的實作部份要來了
            // 得到所有的資料
            List<Dictionary<string, string>> sourceAllDatas = GetAllDatas(sourceSqlConnection, sourceTable, colnumNameList);
            List<Dictionary<string, string>> goalAllDatas = GetAllDatas(goalSqlConnection, goalTable, colnumNameList);

            // 開始比對 (因為 PK 確認過是一樣的，所以才直接拿 sourcePkList 的)
            Match(sourceAllDatas, goalAllDatas, sourcePkList, goalSqlConnection, goalTable);

            // 結束
            Console.WriteLine("============================================================================================================");
            Console.WriteLine("執行完畢");
            Console.ReadLine();
        }

        /// <summary>
        ///  得到 DB 連線字串
        ///  格式一定要長的是 Data Source=tcp:GREEN-THINK,49172;Initial Catalog=DBHQTRACK;Persist Security Info=True;User ID=sa;Password=root
        /// </summary>
        /// <param name="sqlConnection"></param>
        static string GetDBName(string sqlConnection)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(sqlConnection))
            {
                List<string> tmp = sqlConnection.Split(';').ToList();
                if (tmp.Count() > 1)
                {
                    string tmpDBName = tmp[1];
                    List<string> tmp2 = tmpDBName.Split('=').ToList();
                    if (tmp2.Count() > 1)
                    {
                        result = tmp2[1].TrimStart().TrimEnd();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  得到 此 DB 的所有 Table
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <returns></returns>
        static List<string> GetTableList(string sqlConnection)
        {
            List<string> result = new List<string>();

            if (!string.IsNullOrEmpty(sqlConnection))
            {
                string DBName = GetDBName(sqlConnection);
                if (!string.IsNullOrEmpty(DBName))
                {
                    try
                    {
                        using (var conn = new SqlConnection(sqlConnection))
                        {
                            StringBuilder sql = new StringBuilder();

                            sql.Append($@" SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' ORDER BY TABLE_NAME");

                            List<string> sqlResult = conn.Query<string>(sql.ToString()).ToList();
                            result.AddRange(sqlResult);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"連線字串 發生錯誤： {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("連線字串格式錯誤");
                }
            }
            else
            {
                Console.WriteLine("連線字串是空的");
            }

            return result;
        }

        /// <summary>
        /// 詢問 連線字串
        /// </summary>
        /// <param name="tableList"></param>
        /// <param name=""></param>
        /// <returns></returns>
        static string AskSqlConnection(List<string> tableList, out string DB_Name)
        {
            string result = string.Empty;
            DB_Name = string.Empty;

            while (tableList.Count() <= 0)
            {
                Console.WriteLine("請輸入連線字串");
                Console.WriteLine("格式： Data Source=DB連線;Initial Catalog=資料庫名稱;Persist Security Info=True;User ID=使用者ID;Password=密碼");
                Console.Write("請輸入： ");

                string sqlConnection = Console.ReadLine();

                List<string> tmp = GetTableList(sqlConnection);

                if (tmp.Count() > 0)
                {
                    DB_Name = GetDBName(sqlConnection);
                    tableList.AddRange(tmp);
                    result = sqlConnection;
                }
            }

            return result;
        }

        /// <summary>
        ///  輸出讓他選 Table 的格式
        /// </summary>
        /// <param name="tableList"></param>
        static void PrintTableList(List<string> tableList)
        {
            int l = tableList.Count();
            for (var i = 0; i < l; i++)
            {
                Console.WriteLine($"{i}.{tableList[i]}");
            }
        }

        /// <summary>
        ///  得到 Colnum 列表
        /// </summary>
        /// <param name="colnumList"></param>
        /// <param name="tableName"></param>
        /// <param name="sqlConnection"></param>
        static List<ColnumLayout> GetColnumList(string tableName, string sqlConnection)
        {
            List<ColnumLayout> result = new List<ColnumLayout>();

            try
            {
                using (var conn = new SqlConnection(sqlConnection))
                {
                    StringBuilder sql = new StringBuilder();

                    sql.Append($@"SELECT a.TABLE_NAME as 表格名稱,
                                        b.COLUMN_NAME as 欄位名稱, 
                                        b.DATA_TYPE as 資料型別, 
                                        b.CHARACTER_MAXIMUM_LENGTH  as 最大長度, 
                                        b.COLUMN_DEFAULT as 預設值, 
                                        b.IS_NULLABLE  as 允許空值, 
                                            (SELECT	value FROM fn_listextendedproperty 
                                                (NULL, 'schema', 'dbo', 'table',  a.TABLE_NAME, 'column', default) 
                                                WHERE name='MS_Description' 
                                                and objtype='COLUMN' 
                                                and objname Collate Chinese_Taiwan_Stroke_CI_AS=b.COLUMN_NAME) as 欄位備註 
                                    FROM INFORMATION_SCHEMA.TABLES  a 
                                    LEFT JOIN INFORMATION_SCHEMA.COLUMNS b 
                                            ON (a.TABLE_NAME=b.TABLE_NAME) 
                                    WHERE a.TABLE_NAME = '{tableName}';");

                    List<ColnumLayout> sqlResult = conn.Query<ColnumLayout>(sql.ToString()).ToList();
                    result.AddRange(sqlResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"連線字串 發生錯誤： {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 得到 PK 列表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sqlConnection"></param>
        /// <returns></returns>
        static List<string> GetPkList(string tableName, string sqlConnection)
        {
            List<string> result = new List<string>();

            try
            {
                using (var conn = new SqlConnection(sqlConnection))
                {
                    StringBuilder sql = new StringBuilder();

                    sql.Append($@"SELECT COLUMN_NAME 
                                    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                                    WHERE TABLE_NAME = '{tableName}'
                                    AND CONSTRAINT_NAME like 'PK_%';");

                    List<string> sqlResult = conn.Query<string>(sql.ToString()).ToList();
                    result.AddRange(sqlResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"連線字串 發生錯誤： {ex.Message}");
            }

            return result;
        }

        /// <summary>
        ///  詢問要哪張 Table
        /// </summary>
        /// <param name="colnumList"></param>
        /// <param name="pkList"></param>
        /// <param name="sqlConnection"></param>
        /// <returns></returns>
        static string AskTable(List<ColnumLayout> colnumList, List<string> pkList, string sqlConnection, List<string> TableList)
        {
            string result = string.Empty;

            // 讓它 輸入要哪張表
            PrintTableList(TableList);

            // 得到 結構List 和 PK List

            while ((colnumList.Count() <= 0) || (pkList.Count() <= 0))
            {
                // 詢問 要哪張表
                Console.Write("請選擇一張資料表(請輸入代碼，如 0 or 1 …) ： ");
                int TableIndex = Helper.IntTryParse(Console.ReadLine());
                TableIndex = TableIndex >= TableList.Count() ? 0 : TableIndex;
                string Table = TableList[TableIndex];

                // 得到結構
                List<ColnumLayout> colnumTmp = GetColnumList(Table, sqlConnection);
                colnumList.AddRange(colnumTmp);

                // 得到 PK
                List<string> pkTmp = GetPkList(Table, sqlConnection);
                pkList.AddRange(pkTmp);

                // 顯示訊息
                if (colnumList.Count() <= 0)
                {
                    Console.WriteLine($"{Table} 並無 任何欄位");
                }

                if (pkList.Count() <= 0)
                {
                    Console.WriteLine($"{Table} 並沒有 任何 PK (本程式一定至少有一個 PK)");
                }

                result = Table;
            }

            return result;
        }

        /// <summary>
        /// 判斷 來源 和 目標 是否 欄位 + PK 全都一樣
        /// </summary>
        /// <param name="sourceColnumList"></param>
        /// <param name="sourcePkList"></param>
        /// <param name="goalColnumList"></param>
        /// <param name="goalPkList"></param>
        /// <returns></returns>
        static bool SourceIsSameToGoal(List<ColnumLayout> sourceColnumList, List<string> sourcePkList, List<ColnumLayout> goalColnumList, List<string> goalPkList)
        {
            bool result = true;

            // 判斷 Colnum
            if (sourceColnumList.Count() != goalColnumList.Count())
            {
                Console.WriteLine($"來源欄位數為 {sourceColnumList.Count()}, 目標欄位數為 {goalColnumList.Count()}  ，不相同！");
                return false;
            }
            else
            {
                foreach (var item in sourceColnumList)
                {
                    bool haveIn = goalColnumList.Where(x => x.Equal(item)).Count() > 0;
                    if (!haveIn)
                    {
                        Console.WriteLine($"{item.欄位名稱} 在 目標端 不存在 或 長度 … 等 其他不相同");
                        return false;
                    }
                }
            }

            // 判斷 PK
            if (sourcePkList.Count() != goalPkList.Count())
            {
                Console.WriteLine($"來源PK數為 {sourcePkList.Count()}, 目標PK數為 {goalPkList.Count()}  ，不相同！");
                return false;
            }
            else
            {
                foreach (var item in sourcePkList)
                {
                    bool haveIn = goalPkList.Where(x => x.Equals(item)).Count() > 0;
                    if (!haveIn)
                    {
                        Console.WriteLine($"{item} 在 目標端 並不是 PK");
                        return false;
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  得到 資料表內的所有資料
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="Table"></param>
        /// <returns></returns>
        static List<Dictionary<string, string>> GetAllDatas(string sqlConnection, string tableName, List<string> colnumNameList)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            try
            {
                using (var conn = new SqlConnection(sqlConnection))
                {
                    StringBuilder sql = new StringBuilder();

                    sql.Append($@" SELECT * FROM {tableName}");

                    List<object> sqlResult = conn.Query<object>(sql.ToString()).ToList();

                    // 轉出來會是 DapperRow 這個鬼東西 => 把他處理一下 拿去轉成 Directory<string, string>
                    List<Dictionary<string, string>> trueResult = sqlResult.SqlResultConvertToDir(colnumNameList);
                    result.AddRange(trueResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"連線字串 發生錯誤： {ex.Message}");
            }

            return result;
        }

        /// <summary>
        ///  比對 主邏輯
        /// </summary>
        /// <param name="source"></param>
        /// <param name="goal"></param>
        static void Match(List<Dictionary<string, string>> source, List<Dictionary<string, string>> goal, List<string> pkList, string goalSqlConnection, string goalTable)
        {
            List<Dictionary<string, string>> insertList = new  List<Dictionary<string, string>>();
            List<Dictionary<string, string>> updateList = new  List<Dictionary<string, string>>();
            List<Dictionary<string, string>> deleteList = new  List<Dictionary<string, string>>();

            // 先跑來源
            foreach (var item in source)
            {
                // 為了 省效能 (如果找到 目標也有此筆的話， 到時候 remove掉)
                int goalRemoveIndex = -1;

                // 是否是 insert
                bool isInsert = true;

                for (var i = 0; i < goal.Count(); i++)
                {
                    // 找出 PK 相同的 (就單一那一筆)
                    Dictionary<string, string> goalItem = goal[i];
                    bool pkSame = item.DictionaryStringStringEqual(goalItem, pkList);
                    if (pkSame)
                    {
                        // 再來判斷 是否全部一樣 (如果有不一樣的話 => update)
                        bool same = item.DictionaryStringStringEqual(goalItem);
                        if (!same)
                        {
                            // 如果不一樣 => update
                            updateList.Add(item);
                        }

                        // 如果確定 PK 一樣(到時候 換 foreach 目標時，不用再跑 => 先 remove掉)
                        goalRemoveIndex = i;

                        // 有相同 PK 就不可能是 Insert
                        isInsert = false;

                        break;
                    }
                }

                // 刪除
                if (goalRemoveIndex >= 0)
                {
                    goal.RemoveAt(goalRemoveIndex);
                }

                // 如果是 Insert 加入它
                if (isInsert)
                {
                    insertList.Add(item);
                }
            }

            // 剩下的 goal 就都是 要刪掉的了
            deleteList.AddRange(goal);

            // 執行 組 SQL 語法
            List<string> sqls = MakeSql(insertList, updateList, deleteList, goalTable, pkList);

            // 要在 頭尾 加上 可以 insert PK
            sqls.Insert(0, $"SET IDENTITY_INSERT {goalTable} ON");
            sqls.Add($"SET IDENTITY_INSERT {goalTable} OFF");

            // 組出sql 語法
            string sql = string.Empty;
            foreach (var s in sqls)
            {
                sql += $"{s};\n";
            }

            // 寫檔 + 執行 sql
            if (!string.IsNullOrEmpty(sql))
            {
                // 先寫個檔
                using(StreamWriter sw = new StreamWriter("result.sql"))
                {
                    sw.Write(sql);
                }

                // 再來執行 sql 語法
                try
                {
                    using (var conn = new SqlConnection(goalSqlConnection))
                    {
                        conn.Execute(sql);
                    }

                    Console.WriteLine("執行 SQL 成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"執行 SQL 寫入時，發生錯誤： {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("沒有 組出 SQL 語法");
            }
        }

        /// <summary>
        ///  組成 Sql 語法
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="update"></param>
        /// <param name="delete"></param>
        /// <param name="goalTable"></param>
        /// <param name="goalColnumList"></param>
        /// <param name="pkList"></param>
        /// <returns></returns>
        static List<string> MakeSql(List<Dictionary<string, string>> insert, List<Dictionary<string, string>> update, List<Dictionary<string, string>> delete, string goalTable, List<string> pkList)
        {
            List<string> result = new List<string>();

            List<string> insertSql = MakeInsertSql(insert, goalTable);
            result.AddRange(insertSql);

            List<string> updateSql = MakeUpdateSql(update, goalTable, pkList);
            result.AddRange(updateSql);

            List<string> deleteSql = MakeDeleteSql(delete, goalTable, pkList);
            result.AddRange(deleteSql);

            return result;
        }

        /// <summary>
        ///  組出 Insert 語法
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="goalTable"></param>
        /// <param name="goalColnumList"></param>
        /// <returns></returns>
        static List<string> MakeInsertSql(List<Dictionary<string, string>> insert, string goalTable)
        {
            List<string> result = new List<string>();

            foreach (var item in insert)
            {
                string sql = MakeInsertSql(item, goalTable);
                if (!string.IsNullOrEmpty(sql))
                {
                    result.Add(sql);
                }
            }

            return result;
        }

        /// <summary>
        ///  組單一 Sql 語法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="goalTable"></param>
        /// <returns></returns>
        static string MakeInsertSql(Dictionary<string, string> item, string goalTable)
        {
            string result = string.Empty;

            // 全部屬性名稱
            List<string> AttributeName = item.Keys.ToList();
            List<string> valueList = new List<string>();

            foreach (var name in AttributeName)
            {
                string o = item[name];

                if (o == null)
                {
                    valueList.Add("null");
                }
                else
                {
                    string v = o.ToString();
                    valueList.Add($"'{v}'");
                }
            }

            result = $"INSERT INTO {goalTable} ({string.Join(", ", AttributeName)}) VALUES ({string.Join(", ", valueList)}) ";

            return result;
        }

        /// <summary>
        ///  組出 update 語法
        /// </summary>
        /// <param name="update"></param>
        /// <param name="goalTable"></param>
        /// <param name="pkList"></param>
        /// <returns></returns>
        static List<string> MakeUpdateSql(List<Dictionary<string, string>> update, string goalTable, List<string> pkList)
        {
            List<string> result = new List<string>();

            foreach (var item in update)
            {
                string sql = MakeUpdateSql(item, goalTable, pkList);
                if (!string.IsNullOrEmpty(sql))
                {
                    result.Add(sql);
                }
            }

            return result;
        }

        /// <summary>
        ///  組單一 Sql 語法 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="goalTable"></param>
        /// <param name="pkList"></param>
        /// <returns></returns>
        static string MakeUpdateSql(Dictionary<string, string> item, string goalTable, List<string> pkList)
        {
            string result = string.Empty;

            List<string> where = new List<string>();
            List<string> set = new List<string>();

            // 全部屬性名稱
            List<string> AttributeName = item.Keys.ToList();

            // 去查屬性寫值
            foreach (var name in AttributeName)
            {
                string o = item[name];

                string v = o == null ? "null" : "'" + o.ToString() + "'";

                bool isPk = pkList.Where(p => p == name).Count() > 0;

                if (isPk)
                {
                    where.Add($" {name} = {v}");
                }
                else
                {
                    set.Add($" {name} = {v}");
                }
            }

            result = $"UPDATE {goalTable} SET {string.Join(", ", set)} WHERE {string.Join(" AND ", where)} ";

            return result;
        }

        /// <summary>
        ///  組出 Delete 語法
        /// </summary>
        /// <param name="delete"></param>
        /// <param name="goalTable"></param>
        /// <param name="pkList"></param>
        /// <returns></returns>
        static List<string> MakeDeleteSql(List<Dictionary<string, string>> delete, string goalTable, List<string> pkList)
        {
            List<string> result = new List<string>();

            foreach (var item in delete)
            {
                string sql = MakeDeleteSql(item, goalTable, pkList);
                if (!string.IsNullOrEmpty(sql))
                {
                    result.Add(sql);
                }
            }

            return result;
        }

        /// <summary>
        ///  組出單一 sql 語法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="goalTable"></param>
        /// <param name="pkList"></param>
        /// <returns></returns>
        static string MakeDeleteSql(Dictionary<string, string> item, string goalTable, List<string> pkList)
        {
            string result = string.Empty;

            List<string> where = new List<string>();

            // 去查屬性寫值
            foreach (var name in pkList)
            {
                string o = item[name];
                string v = o == null ? "null" : "'" + o.ToString() + "'";
                where.Add($" {name} = {v}");
            }

            result = $"DELETE {goalTable} WHERE {string.Join(" AND ", where)} ";

            return result;
        }
    }

    public class ColnumLayout
    {
        public string 表格名稱 { get; set; }

        public string 欄位名稱 { get; set; }

        public string 資料型別 { get; set; }

        public int? 最大長度 { get; set; }

        public string 預設值 { get; set; }

        public string 允許空值 { get; set; }

        public string 欄位備註 { get; set; }
    }


    #region 相關 Helper

    public static class Helper
    {
        /// <summary>
		/// 上面的正規化 如果用 . 來切 會變成要輸入 \. (有點像 js 會遇到的狀況)
		/// 解法： 自已寫
		/// </summary>
		/// <param name="source"></param>
		/// <param name="splitStr"></param>
		/// <returns></returns>
		public static List<string> Split(this string source, string splitStr)
        {
            List<string> result = new List<string>();

            string tmpSource = source;

            // 如果是 空的，就直接回傳 空字串
            if (string.IsNullOrEmpty(source))
            {
                return new List<string> { "" };
            }

            // 如果切割字串是 null 回傳整個
            if (splitStr == null)
            {
                result.Add(source);
                return result;
            }

            int len = tmpSource.Length;

            // 如果 切割字串是 空字串，就每個字母來切
            if (splitStr == string.Empty)
            {
                for (var i = 0; i < len; i++)
                {
                    string tmp = source.Substring(i, 1);
                    result.Add(tmp);
                }
                return result;
            }

            // 其餘照著切
            int splitStrLen = splitStr.Length;

            // 判斷是否有進去
            bool haveI = false;

            // 位置
            int pos = tmpSource.IndexOf(splitStr);

            // 直到結束
            while (pos >= 0)
            {
                haveI = true;
                string tmp = string.Empty;

                if (pos == 0)
                {
                    tmp = string.Empty;
                }
                else
                {
                    tmp = tmpSource.Substring(0, pos);
                }

                result.Add(tmp);

                // 算出要延後幾位
                int diff = pos + splitStrLen;

                // 切割 (讓剩下的繼續跑)
                tmpSource = tmpSource.Substring(diff);

                // 重算位置
                pos = tmpSource.IndexOf(splitStr);

                // 如果 最後一次砍完剩下 空字串 => 要 push 進去
                if (string.IsNullOrEmpty(tmpSource))
                {
                    result.Add(string.Empty);
                }
                else
                {
                    if (pos < 0)
                    {
                        result.Add(tmpSource);
                    }
                }
            }

            // 如果一開始就查不到 => 直接回傳 自己
            if (haveI == false)
            {
                result.Add(source);
            }

            return result;
        }

        /// <summary>
        ///  把用 try parse 寫成2行的部份 轉成一行
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int IntTryParse(string source)
        {
            int result = 0;
            if (string.IsNullOrEmpty(source)) { return 0; }
            int.TryParse(source, out result);
            return result;
        }

        /// <summary>
        ///  判斷是否相同
        /// </summary>
        /// <param name="source"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public static bool Equal(this ColnumLayout source, ColnumLayout goal)
        {
            bool result = false;

            if (
                (source.欄位名稱 == goal.欄位名稱) &&
                (source.資料型別 == goal.資料型別) &&
                (source.最大長度 == goal.最大長度) 
                )
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        ///  物件相等
        /// </summary>
        /// <param name="source"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public static bool ObjEqual(this object source, object goal)
        {
            bool result = true;

            try
            {
                Type st = source.GetType();
                Type gt = goal.GetType();

                // 全部屬性名稱
                List<string> sourceAttributeName = st.GetProperties().Select(x => x.Name).ToList();
                List<string> goalAttributeName = gt.GetProperties().Select(x => x.Name).ToList();

                // 如果 屬性名稱全一樣
                if (sourceAttributeName.Count() != goalAttributeName.Count())
                {
                    return false;
                }
                else
                {
                    foreach (var item in sourceAttributeName)
                    {
                        bool haveIn = goalAttributeName.Where(g => g == item).Count() > 0;
                        if (!haveIn)
                        {
                            return false;
                        }
                    }
                }

                // 接下來 判斷值是否一樣 (因為上面判斷過 屬性名稱全一樣 => 所以可以 直接用就行)
                foreach (var name in sourceAttributeName)
                {
                    string so = st.GetProperty(name).GetValue(source).ToString();
                    string go = gt.GetProperty(name).GetValue(goal).ToString();

                    if (so != go)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return result;
        }

        /// <summary>
        ///  物件相等
        /// </summary>
        /// <param name="source"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public static bool DictionaryStringStringEqual(this Dictionary<string, string> source, Dictionary<string, string> goal)
        {
            bool result = true;

            try
            {
                // 全部屬性名稱
                List<string> sourceAttributeName = source.Keys.ToList();
                List<string> goalAttributeName = goal.Keys.ToList();

                // 如果 屬性名稱全一樣
                if (sourceAttributeName.Count() != goalAttributeName.Count())
                {
                    return false;
                }
                else
                {
                    foreach (var item in sourceAttributeName)
                    {
                        bool haveIn = goalAttributeName.Where(g => g == item).Count() > 0;
                        if (!haveIn)
                        {
                            return false;
                        }
                    }
                }

                // 接下來 判斷值是否一樣 (因為上面判斷過 屬性名稱全一樣 => 所以可以 直接用就行)
                foreach (var name in sourceAttributeName)
                {
                    string so = source[name];
                    string go = goal[name];
                    if (so != go)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return result;
        }

        /// <summary>
        ///  物件相等 (但可輸入 屬性名稱 作條件 => 即判斷部份而已)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public static bool ObjEqual(this object source, object goal, List<string> filter)
        {
            bool result = true;

            try
            {
                Type st = source.GetType();
                Type gt = goal.GetType();

                // 如果爆錯 => 到 try catch 就會回傳 false
                foreach (var name in filter)
                {
                    string so = st.GetProperty(name).GetValue(source).ToString();
                    string go = gt.GetProperty(name).GetValue(goal).ToString();

                    if (so != go)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return result;
        }

        /// <summary>
        ///  物件相等 (但可輸入 屬性名稱 作條件 => 即判斷部份而已)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public static bool DictionaryStringStringEqual(this Dictionary<string, string> source, Dictionary<string, string> goal, List<string> filter)
        {
            bool result = true;

            try
            {
                // 如果爆錯 => 到 try catch 就會回傳 false
                foreach (var name in filter)
                {
                    string so = source[name];
                    string go = goal[name];

                    if (so != go)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return result;
        }

        /// <summary>
        ///  因為 Dapper 取出來的 物件 很怪 所以特別處理一下
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> SqlResultConvertToDir(this List<object> source, List<string> colnumNameList)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            try
            {
                foreach (var r in source)
                { 
                    string tmp = r.ToString();
                    tmp = tmp.Remove(0, 12);  // 先去掉 {DapperRow, 這句
                    tmp = tmp.Remove(tmp.Length - 2, 2);  // 去掉最尾巴的 '}

                    Dictionary<string, string> item = new Dictionary<string, string>();

                    // 因為有可能給的順序 跟 查出來的順序不一樣 (所以給的 colnumNameList 不一定 出來的欄位順序一樣)
                    // 但 sql 出來的格式一定是一樣的
                    string splitStr = "***^*^*##&&**$$!%%";
                    foreach (var colnumName in colnumNameList)
                    {
                        // 格式： {ID = '14', HCONN = '97', QNAME = 'TT.UPDATE.TEST', SYSNAME = '0',=', STOPCODE = '0'}
                        string first = $"{colnumName} = '";
                        string last = $"', {colnumName} = '";
                        string format = $"{splitStr}{colnumName}{splitStr}";  // 用一個比較不常用到的 (如果真的撞到就算自已運氣差吧)

                        tmp = tmp.Replace(last, format).Replace(first, format);
                    }

                    List<string> afterSplit = tmp.Split(splitStr);

                    for (var i = 1; i < afterSplit.Count(); i+= 2)
                    {
                        string key = afterSplit[i];

                        // 記得 要丟回給 sql 時  'ssa'dd' 的資料 => 'ssa''dd'
                        string value = afterSplit[i + 1].Replace("'", "''");
                        item.Add(key, value);
                    }
                    
                    result.Add(item);
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }

    #endregion
}
