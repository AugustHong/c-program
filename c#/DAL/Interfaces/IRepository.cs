using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PUVAMS.Web.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 取得單筆資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetByID(object id);

        /// <summary>
        /// 取得單筆資料(雙主鍵)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        TEntity GetByID(object id, object name);

        /// <summary>
        /// 透過輸入委派的判斷式來搜尋（其實等同於GetAll().Where();
        /// 使用方法： xxx.GetFilter(e => e.id == 3);
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
		TEntity GetFilter(Expression<Func<TEntity, bool>> filter);


		/// <summary>
		/// 取得所有資料
		/// </summary>
		/// <returns></returns>
		IEnumerable<TEntity> GetAll();

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="obj_Entity"></param>
        void Create(TEntity obj_Entity);

        /// <summary>
        /// 修改資料
        /// </summary>
        /// <param name="obj_Entity"></param>
        void Update(TEntity obj_Entity);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="obj_Entity"></param>
        void Delete(object id);

        /// <summary>
        /// 刪除資料(雙主鍵)
        /// </summary>
        /// <param name="obj_Entity"></param>
        void Delete(object id, object name);

        /// <summary>
        /// 寫回資料庫
        /// </summary>
        /// <returns></returns>
        bool SaveChanges();
    }
}