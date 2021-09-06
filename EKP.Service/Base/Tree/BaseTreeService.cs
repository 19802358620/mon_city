using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using EKP.Service.Base.Ado;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.Base.Tree
{
    /// <summary>
    /// 名    称：IZlfService
    /// 作    者：胡政
    /// 创建时间：2015-08-30
    /// 联系方式：13436053642
    /// 描    述：   树形结构操作服务抽象类， 数据库表必须有id(int)、parentId(int)字段
    /// </summary>
    public abstract class BaseTreeService<TEntity> : ServiceBase<TEntity>,  IBaseTreeService<TEntity> where TEntity : class, new()
    {
        public virtual string PropertyName
        {
            get { return "Name"; }
        }

        /// <summary>
        /// 获取树
        /// </summary>
        public List<TModel> GetTree<TModel>(List<TEntity> dataSourse) where TModel : TreeNode<TModel>, new()
        {
            var list = new List<TModel>();

            dataSourse.ForEach(t =>
            {
                var parentId = typeof(TEntity).GetProperty("ParentId").GetValue(t);
                if (parentId == null || string.IsNullOrEmpty(parentId.ToString()))
                {
                    var model = ObjectMapper.Mapper<TEntity, TModel>(t);
                    model.Children = Children<TModel>(dataSourse, model.Id);
                    list.Add(model);
                }
            });

            return list;
        }

        /// <summary>
        /// 获取某个节点下的子树
        /// </summary>
        /// <param name="rootId">根节点id</param>
        public List<TModel> Children<TModel>(List<TEntity> dataSourse, int rootId) where TModel : TreeNode<TModel>, new()
        {
            var list = new List<TModel>();

            dataSourse.ForEach(t =>
            {
                var parentId = typeof(TEntity).GetProperty("ParentId").GetValue(t);
                if (Convert.ToInt32(parentId) == rootId)
                {
                    var model = ObjectMapper.Mapper<TEntity, TModel>(t);
                    model.Children = Children<TModel>(dataSourse, model.Id);
                    list.Add(model);
                }
            });

            return list;
        }

        /// <summary>
        /// 获取某个节点下的子节点集合，并将子节点集合扁平化
        /// </summary>
        public List<TreeNode> GetSomeChildren(List<TEntity> dataSourse, int rootId)
        {
            var list = new List<TreeNode>();

            dataSourse.ForEach(t =>
            {
                var parentId = typeof(TEntity).GetProperty("ParentId").GetValue(t);
                if (Convert.ToInt32(parentId) == rootId)
                {
                    var id = typeof(TEntity).GetProperty("Id").GetValue(t);
                    var name = Convert.ToString(typeof(TEntity).GetProperty(PropertyName).GetValue(t));
                    list.Add(new TreeNode
                    {
                        Id = Convert.ToInt32(id),
                        ParentId = Convert.ToInt32(parentId),
                        Name = name,
                        Children = null
                    });
                    list.AddRange(GetSomeChildren(dataSourse, Convert.ToInt32(id)));
                }
            });

            return list;
        }

        /// <summary>
        /// 获取某个节点的所有父节点集合
        /// </summary>
        public List<TreeNode> Parents(int nodeId)
        {
            var list = new List<TreeNode>();
            var entity = GetEntiy(nodeId);

            var temp = Int32.MaxValue;//防止死循环
            while (true)
            {
                if(temp < 0)break;

                var parentId = Convert.ToInt32(typeof(TEntity).GetProperty("ParentId").GetValue(entity));
                var parent = GetEntiy(parentId);
                if(parent == null) break;
                if (parentId > 0)
                {
                    var father = GetEntiy(string.Format("Id={0}", parentId));
                    var parentParentId = Convert.ToInt32(typeof(TEntity).GetProperty("ParentId").GetValue(parent));
                    var name = Convert.ToString(typeof(TEntity).GetProperty(PropertyName).GetValue(parent));
                    list.Add(new TreeNode
                    {
                        Id = parentId,
                        ParentId = parentParentId,
                        Name = name,
                        Children = null//获取子树
                    });
                }
                entity = parent;

                temp--;
            }
            return list;
        }
    }
}
