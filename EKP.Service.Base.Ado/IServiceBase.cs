using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Pager;

namespace EKP.Service.Base.Ado
{
    /// <summary>
    /// 名    称：IServiceBase
    /// 作    者：胡政
    /// 创建时间：2015-08-21
    /// 联系方式：13436053642
    /// 描    述：实体基础操作服务接口
    /// </summary>
    public interface IServiceBase<TEntity> where TEntity : class, new()
    {
        #region 查询
        /// <summary>
        /// 获取DataSet数据集
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        DataSet GetDs(string where);
        /// <summary>
        /// 获取DataSet数据集
        /// </summary>
        /// <param name="top">获取条数</param>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        /// <returns></returns>

        DataSet GetDs(int top, string where);

        /// <summary>
        /// 获取DataTable数据集
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        DataTable GetDt(string where);
        /// <summary>
        /// 获取DataTable数据集
        /// </summary>
        /// <param name="top">获取条数</param>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        /// <returns></returns>
        DataTable GetDt(int top, string where);
        /// <summary>
        /// 获取DataTable分页数据
        /// </summary>
        DataTable GetDtPager(PagerParameter param, string where, out int totalCount);

        /// <summary>
        /// 获取DataReader数据集
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        DbDataReader GetDr(string where);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="top">获取条数</param>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        /// <returns></returns>
        DbDataReader GetDr(int top, string where);

        /// <summary>
        ///  查询是否存在数据
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        bool IsExist(string where);
        /// <summary>
        ///  根据查询是否存在数据
        /// </summary>
        /// <param name="id">实体id</param>
        bool IsExist(int id);
        #endregion

        #region 查询扩展
        /// <summary>
        /// 根据查询条件返回List实体集合
        /// </summary>
        List<TEntity> GetList(string where);
        List<TEntity> GetList(int top, string where);
        /// <summary>
        /// 根据多个id值返回List实体集合
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetList(params int[] ids);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        PagerResult<TModel> GetPager<TModel>(PagerParameter param, string where) where TModel : class, new();

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <returns></returns>
        TEntity GetEntiy(string where);
        /// <summary>
        /// 通过id获取单个实体
        /// </summary>
        TEntity GetEntiy(int id);
        /// <summary>
        /// 通过多个id获取实体集合
        /// </summary>
        List<TEntity> GetEntiys(List<int> id);
        #endregion

        #region 插入
        /// <summary>
        /// 插入单个实体
        /// </summary>
        void Add(TEntity entity);
        /// <summary>
        /// 插入多个实体
        /// </summary>
        void Add(params TEntity[] entitys);
        #endregion

        #region 编辑
        /// <summary>
        /// 更新单个实体
        /// </summary>
        void Update(TEntity entity);
        /// <summary>
        /// 更新单个实体，指定更新哪些列
        /// </summary>
        void Update(TEntity entity, params string[] fields);
        /// <summary>
        /// 更新单个实体，指定忽略哪些列
        /// </summary>
        void UpdateIgnore(TEntity entity, params string[] fields);
        /// <summary>
        /// 更新多个实体
        /// </summary>
        void Update(params TEntity[] entitys);
        /// <summary>
        /// 更新多个实体，指定更新哪些列
        /// </summary>
        void Update(TEntity[] entitys, params string[] fields);
        #endregion

        #region 删除
        /// <summary>
        /// 删除单个实体
        /// </summary>
        void Delete(TEntity entity);
        /// <summary>
        /// 删除多个实体
        /// </summary>
        void Delete(params TEntity[] entitys);

        Dialog Delete(int[] ids, bool isFlag = true);
        #endregion
    }
}
