using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper研究.DAL
{
      public interface IDapperRepository<TEntity>
       where TEntity : class
      {
            /// <summary>
            /// 新增一筆
            /// </summary>
            /// <param name="instance">資料</param>
            /// <returns>是否成功</returns>
            int Insert(TEntity instance);
            /// <summary>
            /// 新增多筆
            /// </summary>
            /// <param name="source">多筆資料</param>
            /// <param name="haveTransaction">是否使用 TransactionScope</param>
            /// <returns>失敗項目</returns>
            List<TEntity> InsertRange(List<TEntity> source, bool haveTransaction);
            /// <summary>
            /// 修改一筆 (拿全部 PK 去查再修改)
            /// </summary>
            /// <param name="instance">資料</param>
            /// <returns>影響筆數</returns>
            int Update(TEntity instance);
            /// <summary>
            ///  修改 多筆 (自行輸入 where 條件)，且只修改幾個欄位(自行輸入)
            /// </summary>
            /// <param name="instance">要修改的欄位項目</param>
            /// <param name="where">條件</param>
            /// <returns>影響筆數</returns>
            int Update(Dictionary<string, object> instance, Dictionary<string, object> where);
            /// <summary>
            /// 修改多筆 (多個 update ，等同 Update(TEntity instance) 的多筆版)
            /// </summary>
            /// <param name="source">資料</param>
            /// <param name="haveTransaction">是否使用 TransactionScope</param>
            /// <returns>失敗項目</returns>
            List<TEntity> UpdateRange(List<TEntity> source, bool haveTransaction);
            /// <summary>
            /// 刪除一筆 (拿全部 PK 去查再修改)
            /// </summary>
            /// <param name="instance">資料</param>
            /// <returns>影響筆數</returns>
            int Delete(TEntity instance);
            /// <summary>
            /// 刪除多筆 (拿 輸入的 where 條件 去刪除)
            /// </summary>
            /// <param name="where">條件</param>
            /// <returns>影響筆數</returns>
            int Delete(Dictionary<string, object> where);
            /// <summary>
            /// 刪除多筆 (多個 delete ，等同 Delete(TEntity instance) 的多筆版)
            /// </summary>
            /// <param name="source">資料</param>
            /// <param name="haveTransaction">是否使用 TransactionScope</param>
            /// <returns>失敗項目</returns>
            List<TEntity> DeleteRange(List<TEntity> source, bool haveTransaction);
            /// <summary>
            ///  查詢 (用 PK 來查)
            /// </summary>
            /// <returns></returns>
            TEntity Select(TEntity instance);
            /// <summary>
            /// 查詢 (用你輸入的PK來查)
            /// </summary>
            /// <param name="where">條件</param>
            /// <returns>結果</returns>
            IQueryable<TEntity> Select(Dictionary<string, object> where);
            /// <summary>
            /// 查詢 (用你輸入的PK來查)
            /// </summary>
            /// <param name="where">條件</param>
            /// <param name="orderBy">order by 的項目</param>
            /// <returns>結果</returns>
            IQueryable<TEntity> Select(Dictionary<string, object> where, List<string> orderBy);
            /// <summary>
            /// 查詢 第一筆
            /// </summary>
            /// <param name="where">條件</param>
            /// <returns>第一筆</returns>
            TEntity SelectFirst(Dictionary<string, object> where);
            /// <summary>
            /// 查詢 第一筆
            /// </summary>
            /// <param name="where">條件</param>
            /// <param name="orderBy">order by 的項目</param>
            /// <returns>第一筆</returns>
            TEntity SelectFirst(Dictionary<string, object> where, List<string> orderBy);
            /// <summary>
            /// 查詢 最後一筆
            /// </summary>
            /// <param name="where">條件</param>
            /// <returns>最後一筆</returns>
            TEntity SelectLast(Dictionary<string, object> where);
            /// <summary>
            /// 查詢 最後一筆
            /// </summary>
            /// <param name="where">條件</param>
            /// <param name="orderBy">order by 的項目</param>
            /// <returns>最後一筆</returns>
            TEntity SelectLast(Dictionary<string, object> where, List<string> orderBy);
            /// <summary>
            /// 查詢 前 幾筆
            /// </summary>
            /// <param name="where">條件</param>
            /// <param name="top">前幾筆</param>
            /// <returns>前幾筆資料</returns>
            List<TEntity> SelectTop(Dictionary<string, object> where, int top);
            /// <summary>
            /// 查詢 前 幾筆
            /// </summary>
            /// <param name="where">條件</param>
            /// <param name="orderBy">order by 的項目</param>
            /// <param name="top">前幾筆</param>
            /// <returns>前幾筆資料</returns>
            List<TEntity> SelectTop(Dictionary<string, object> where, List<string> orderBy, int top);
            /// <summary>
            /// 查詢全部
            /// </summary>
            /// <returns>結果</returns>
            IQueryable<TEntity> SelectAll();
            /// <summary>
            /// 查詢全部
            /// </summary>
            /// <param name="orderBy">order by 的項目</param>
            /// <returns>結果</returns>
            IQueryable<TEntity> SelectAll(List<string> orderBy);
      }
}
