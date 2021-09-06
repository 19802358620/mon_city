using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ge.Infrastructure.Pager;

namespace Ge.Infrastructure.Metronicv
{
    /// <summary>
    /// 名    称：JqgridParam
    /// 作    者：胡政
    /// 创建时间：2015-08-22
    /// 联系方式：13436053642
    /// 描    述：Jqgrid分页所需参数(通用)
    /// </summary>
    public class JqgridParam : PagerParameter
    {
        private int page = 1;
        private int pageSize = 15;
        private string sortBy = "Id";
        private string sortOrder = "asc";

        ///<summary>
        /// 当前页位置
        /// </summary> 
        public virtual int Page
        {
            get { return page; }
            set { page = value; }
        }

        /// <summary>
        /// 每页显示的条目数
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        /// <summary>
        /// 条目起始位置
        /// </summary>
        public override int Skip
        {
            get { return PageSize * (Page - 1); }
        }

        /// <summary>
        /// 条目数
        /// </summary>
        public override int Take
        {
            get { return PageSize; }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortBy
        {
            get { return sortBy; }
            set { sortBy = value; }
        }

        /// <summary>
        /// 排序规则（"asc"和"desc"）
        /// </summary>
        public string SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }
    }

    /// <summary>
    /// 名    称：JqgridParam
    /// 作    者：胡政
    /// 创建时间：2015-08-22
    /// 联系方式：13436053642
    /// 描    述：Jqgrid分页所需参数(Ef实体)
    /// </summary>
    public class JqgridParam<TEntity> : PagerParameter
    {
        private int page = 1;
        private int pageSize = 15;
        private string sortBy = "Id";
        private string sortOrder = "asc";
        private Expression<Func<TEntity, bool>> where = entity => true;

        /// <summary>
        /// 查询条件
        /// </summary>
        public virtual Expression<Func<TEntity, bool>> Where
        {
            get { return where; }
            set { where = value; }
        }

        ///<summary>
        /// 当前页位置
        /// </summary>
        public virtual int Page
        {
            get { return page; }
            set { page = value; }
        }

        /// <summary>
        /// 每页显示的条目数
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortBy
        {
            get { return sortBy; }
            set { sortBy = value; }
        }

        /// <summary>
        /// 排序规则（"asc"和"desc"）
        /// </summary>
        public string SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        /// <summary>
        /// 条目起始位置
        /// </summary>
        public override int Skip
        {
            get { return PageSize * (Page - 1); }
        }

        /// <summary>
        /// 条目数
        /// </summary>
        public override int Take
        {
            get { return PageSize; }
        }
    }

    /// <summary>
    /// 名    称：JqgridParam
    /// 作    者：胡政
    /// 创建时间：2015-08-22
    /// 联系方式：13436053642
    /// 描    述：Jqgrid分页所需参数(Sql)
    /// </summary>
    public class JqgridSqlParam<TEntity> : JqgridParam<TEntity>
    {
        public new string Where { get; set; }

        /// <summary>
        /// 查询调用 删除状态 默认查询未删
        /// </summary>
        public string IsDeleted {
            get { return "undeleted"; }
        }
    }

    /// <summary>
    /// 名    称：JqgridResult
    /// 作    者：胡政
    /// 创建时间：2015-08-22
    /// 联系方式：13436053642
    /// 描    述：Jqgrid分页数据源
    /// </summary>
    public class JqgridResult<TModel> : PagerResult<TModel>
    {
        public JqgridResult()
            : base()
        {

        }

        public JqgridResult(PagerParameter param)
            : base(param)
        {
            
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get { return TotalRecords % PageSize == 0 ? TotalRecords / PageSize : TotalRecords / PageSize + 1; }
        }

    }
}
