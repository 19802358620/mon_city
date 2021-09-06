using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using EKP.Entity;
using EKP.Repository.Ado;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Pager;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.Base.Ado
{
    /// <summary>
    /// 名    称：ServiceBase
    /// 作    者：胡政
    /// 创建时间：2015-08-21
    /// 联系方式：13436053642
    /// 描    述：实体基础操作服务接口
    /// </summary>
    public abstract class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class, new()
    {
        private readonly IDbContextAdapter<EKP_JSEntities> dbContextAdapte = null;

        protected ServiceBase()
        {
            dbContextAdapte = CreateDbContextAdapter();
        }

        private IDbContextAdapter<EKP_JSEntities> CreateDbContextAdapter()
        {
            return new DbContextAdapter<EKP_JSEntities>();
        }

        #region 查询
        /// <summary>
        /// 获取DataSet数据集
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        public virtual DataSet GetDs(string where)
        {
            var ds = dbContextAdapte.GetDs<TEntity>(where);
            return ds;
        }
        /// <summary>
        /// 获取DataSet数据集
        /// </summary>
        /// <param name="top">获取条数</param>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        /// <returns></returns>
        public virtual DataSet GetDs(int top, string where)
        {
            var ds = dbContextAdapte.GetDs<TEntity>(top, where);
            return ds;
        }

        /// <summary>
        /// 获取DataTable数据集
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        public virtual DataTable GetDt(string where)
        {
            var dt = dbContextAdapte.GetDt<TEntity>(where);
            return dt;
        }
        /// <summary>
        /// 获取DataTable数据集
        /// </summary>
        /// <param name="top">获取条数</param>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        /// <returns></returns>
        public virtual DataTable GetDt(int top,string where)
        {
            var dt = dbContextAdapte.GetDt<TEntity>(top, where);
            return dt;
        }

        /// <summary>
        /// 获取DataTable分页数据
        /// </summary>
        public virtual DataTable GetDtPager(PagerParameter param, string where, out int totalCount)
        {
            var dt = dbContextAdapte.GetDtPager<TEntity>(param, where, out totalCount);
            return dt;
        }

        /// <summary>
        /// 获取DataReader数据集
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        public virtual DbDataReader GetDr(string where)
        {
            var dr = dbContextAdapte.GetDr<TEntity>(where);
            return dr;
        }
        /// <summary>
        /// 获取DataReader数据集
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual DbDataReader GetDr(int top, string where)
        {
            var dr = dbContextAdapte.GetDr<TEntity>(top, where);
            return dr;
        }

        /// <summary>
        ///  查询是否存在数据
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        public virtual bool IsExist(string where)
        {
            return dbContextAdapte.IsExist<TEntity>(where);
        }
        /// <summary>
        ///  根据查询是否存在数据
        /// </summary>
        /// <param name="id">实体id</param>
        public virtual bool IsExist(int id)
        {
            return IsExist(string.Format("id = {0}", id));
        }
        #endregion

        #region 查询扩展
        /// <summary>
        /// 根据查询条件返回List实体集合
        /// </summary>
        public virtual List<TEntity> GetList(string where)
        {
            var dt = this.GetDt(where);
            var list = ObjectMapper.Mapper<TEntity>(dt);
            return list;
        }
        /// <summary>
        /// 根据查询条件返回List实体集合
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual List<TEntity> GetList(int top,string where)
        {
            var dt = this.GetDt(top, where);
            var list = ObjectMapper.Mapper<TEntity>(dt);
            return list;
        }

        /// <summary>
        /// 根据多个id值返回List实体集合
        /// </summary>
        public virtual List<TEntity> GetList(params int[] ids)
        {
            var where = " 1=0 ";
            ids.ToList().ForEach(id => where += string.Format(" or id={0} ", id));
            return GetList(where);
        }

        /// <summary>
        /// 分页
        /// </summary>
        public virtual PagerResult<TModel> GetPager<TModel>(PagerParameter param, string where) where TModel : class, new()
        {
            int totalRecords = 0;
            var rows = GetDtPager(new PagerParameter()
            {
                Skip = param.Skip,
                Take = param.Take
            }, where, out totalRecords).ToList<TModel>();
            return new PagerResult<TModel>
            {
                Rows = rows,
                TotalRecords = totalRecords,
                PageSize = param.Take
            };
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        public TEntity GetEntiy(string where)
        {
            string sql = string.Empty;
            DbCommand cmd = null;
            List<TEntity> entityList = null;

            entityList = this.GetList(where);
            return entityList.FirstOrDefault();
        }
        /// <summary>
        /// 通过id获取单个实体
        /// </summary>
        public TEntity GetEntiy(int id)
        {
            return GetEntiy(string.Format("Id = {0}", id));
        }

        /// <summary>
        /// 通过多个id获取实体集合
        /// </summary>
        public List<TEntity> GetEntiys(List<int> ids)
        {
            var deleteIdsStr = string.Empty;
            ids.ForEach(d =>
            {
                if (string.IsNullOrEmpty(deleteIdsStr))
                    deleteIdsStr += d;
                else
                    deleteIdsStr += "," + d;
            });
            var list = this.GetList(string.Format(" Id in ({0}) ", deleteIdsStr));
            return list;
        }
        #endregion

        #region 插入
        /// <summary>
        /// 插入单个实体
        /// </summary>
        public virtual void Add(TEntity entity)
        {
             dbContextAdapte.Add(entity);
        }

        /// <summary>
        /// 插入多个实体
        /// </summary>
        public virtual void Add(params TEntity[] entitys)
        {
            dbContextAdapte.Add(entitys);
        }
        #endregion

        #region 编辑
        /// <summary>
        /// 更新单个实体
        /// </summary>
        public virtual void Update(TEntity entity)
        {
            dbContextAdapte.Update(entity);
        }

        /// <summary>
        /// 更新单个实体，指定更新哪些列
        /// </summary>
        public void Update(TEntity entity, params string[] fields)
        {
            Update(new TEntity[] { entity }, fields);
        }

        /// <summary>
        /// 更新单个实体，指定忽略哪些列
        /// </summary>
        public void UpdateIgnore(TEntity entity, params string[] ignoreFields)
        {
            var propertys = entity.GetType().GetProperties();
            var fields = new List<string>();
            if (ignoreFields != null)
            {
                for (var i = 0; i < ignoreFields.Length; i++)
                    ignoreFields[i] = ignoreFields[i].ToLower();
                foreach (var p in propertys)
                {
                    if (ignoreFields.Contains(p.Name.ToLower()))
                        continue;
                    fields.Add(p.Name);
                }
            }
            Update(entity, fields.ToArray());
        }

        /// <summary>
        /// 更新多个实体
        /// </summary>
        public virtual void Update(params TEntity[] entitys)
        {
            dbContextAdapte.Update(entitys);
        }

        /// <summary>
        /// 更新多个实体，指定更新哪些列
        /// </summary>
        public virtual void Update(TEntity[] entitys, params string[] fields)
        {
            dbContextAdapte.Update(entitys.ToList(), fields);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除单个实体
        /// </summary>
        public virtual void Delete(TEntity entity)
        {
            dbContextAdapte.Delete(entity, null);
        }

        /// <summary>
        /// 删除多个实体
        /// </summary>
        public virtual void Delete(params TEntity[] entitys)
        {
            dbContextAdapte.Delete(entitys);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        public virtual Dialog Delete(int[] ids, bool isFlag = true)
        {
            var entitys = GetList(ids);
            if (isFlag)
            {
                PropertyInfo property = null;
                entitys.ForEach(e =>
                {
                    if (property == null)
                        property = e.GetType().GetProperty("IsDeleted");
                    property.SetValue(e, "deleted");
                });
                dbContextAdapte.Update(entitys.ToArray());
            }
            else
            {
                try
                {
                    dbContextAdapte.Delete(entitys.ToArray());

                }
                catch
                {
                    return DialogFactory.Create(DialogType.Error, "操作失败");
                }
            }

            return DialogFactory.Create(DialogType.Success, string.Empty, "操作成功");
        }
        #endregion
    }
}
