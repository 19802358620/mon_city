using System.Collections.Generic;
using EKP.Service.Base.Ado;

namespace EKP.Service.Base.Tree
{
    /// <summary>
    /// 名    称：IZlfService
    /// 作    者：胡政
    /// 创建时间：2015-08-30
    /// 联系方式：13436053642
    /// 描    述：   树形结构操作服务接口， 数据库表必须有id(int)、parentId(int)字段
    /// </summary>
    public interface IBaseTreeService<TEntity> : IEkpEntityService<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 获取树
        /// </summary>
        List<TModel> GetTree<TModel>(List<TEntity> dataSourse) where TModel : TreeNode<TModel>, new();

        /// <summary>
        /// 获取某个节点下的子树
        /// </summary>
        /// <param name="rootId">根节点id</param>
        List<TModel> Children<TModel>(List<TEntity> dataSourse, int rootId) where TModel : TreeNode<TModel>, new();

        /// <summary>
        /// 获取某个节点下的子节点集合，并将子节点集合扁平化
        /// </summary>
        List<TreeNode> GetSomeChildren(List<TEntity> dataSourse, int rootId);

        /// <summary>
        /// 获取某个节点的所有父节点集合
        /// </summary>
        List<TreeNode> Parents(int nodeId);
    }
}
