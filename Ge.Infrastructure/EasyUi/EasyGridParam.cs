using System;
using System.Linq.Expressions;
using Ge.Infrastructure.Pager;

namespace Ge.Infrastructure.EasyUi
{
    /// <summary>
    /// easyui datagrid查询需要的参数(Ef)
    /// 这个分页参数支持实体的分页，包括单字段排序、查询过滤、 left join查询。不适合有其他需求（例如使用了group by、not in等子句）的分页，复杂查询请用sql语句或者linq语句进行查询。
    /// </summary>
    public class EasyGridParam<TEntity> : PagerParameter
    {
        private int page = 1;
        private int rows = 15;
        private string sort = "Id";
        private string order = "ASC";
        private Expression<Func<TEntity, bool>> where = entity => true;


        /// <summary>
        /// 当前页
        /// </summary>
        public virtual int Page
        {
            get { return page; } 
            set { page = value; }
        }

        /// <summary>
        /// 每页条目数
        /// </summary>
        public virtual int Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public virtual string Sort
        {
            get { return sort; }
            set { sort = value; }
        }

        /// <summary>
        /// 排序规则（“ASC或者DESC”）
        /// </summary>
        public virtual string Order
        {
            get { return order; }
            set { order = value; }
        }

        /// <summary>
        /// 查询条件(EF)
        /// </summary>
        public virtual Expression<Func<TEntity, bool>> Where {
            get { return where; } 
            set { where = value; } 
        }

        /// <summary>
        /// 链接查询包含的表
        /// </summary>
        public virtual string[] IncludePath { get; set; }

        /// <summary>
        /// 条目起始位置
        /// </summary>
        public override int Skip
        {
            get { return Rows * (Page - 1); }
        }

        /// <summary>
        /// 每页条目数
        /// </summary>
        public override int Take
        {
            get { return Rows; }
            
        }
    }

    /// <summary>
    /// easyui datagrid查询需要的参数(Sql语句)
    /// </summary>
    public class EasyGridSqlParam<TEntity> : EasyGridParam<TEntity>
    {
        public new string Where { get; set; }
    }
}
