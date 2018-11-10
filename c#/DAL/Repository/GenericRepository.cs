using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using PUVAMS.Web.Domain;
using PUVAMS.Web.DAL.Interfaces;
using System.Linq.Expressions;

namespace PUVAMS.Web.DAL.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        //這裡使用Ioc控制反轉
        /// <summary>
        /// GlobalVariables：DbContext
        /// 準備DbContext的全域變數
        /// 放在你EF的Entities的名稱（例如：ABCEntities）
        /// </summary>
        private ABCEntities _dbContext;

        /// <summary>
        /// GlobalVariables：DbSet
        /// 準備DbSet的全域變數
        /// </summary>
        private DbSet<TEntity> _dbSet;

        /// <summary>
        /// GenericRepositoryConstructor
        /// 泛型_資料倉儲_建構子, 建構全域變數
        /// </summary>
        public GenericRepository()
        {
            _dbContext = new ABCEntities();
            _dbSet = _dbContext.Set<TEntity>();
        }

        /// <summary>
        /// 透過ID,取得該筆資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// 透過雙PrimaryKey取得對應一筆資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public TEntity GetByID(object id, object name)
        {
            return _dbSet.Find(id, name);
        }

        /// <summary>
        /// 透過輸入委派的判斷式來搜尋（其實等同於GetAll().Where();
        /// 使用方法： xxx.GetFilter(e => e.id == 3);
        /// </summary>
        /// <param name="paymentFlowID"></param>
        /// <returns></returns>
		public TEntity GetFilter(Expression<Func<TEntity, bool>> filter)
		{
			return _dbSet.Where(filter).FirstOrDefault();
		}


		/// <summary>
		/// 取得該實體所有資料
		/// </summary>
		/// <returns></returns>
		public IEnumerable<TEntity> GetAll()
        {
            IQueryable<TEntity> _Query = _dbSet;
            return _Query;
        }

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="obj_Entity"></param>
        public void Create(TEntity obj_Entity)
        {
            _dbContext.Entry(obj_Entity).State = EntityState.Added;
        }

        /// <summary>
        /// 修改資料
        /// </summary>
        /// <param name="obj_Entity"></param>
        public void Update(TEntity obj_Entity)
        {
            _dbContext.Entry(obj_Entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="obj_Entity"></param>
        public void Delete(object id)
        {
            TEntity _EntityToDelete = _dbSet.Find(id);
            _dbContext.Entry(_EntityToDelete).State = EntityState.Deleted;
        }

        /// <summary>
        /// 刪除資料(雙主鍵)
        /// </summary>
        /// <param name="obj_Entity"></param>
        public void Delete(object id, object name)
        {
            TEntity _EntityToDelete = _dbSet.Find(id, name);
            _dbContext.Entry(_EntityToDelete).State = EntityState.Deleted;
        }

        /// <summary>
        /// 將多筆異動資料, 同時寫回資料庫
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() > 0);
        }


    }
}