using System.Collections.Generic;
using System.Reflection;

namespace Ge.Infrastructure.Xml.XmlHelper
{
    /// <summary>
    /// 扩展IXmlHelper接口的方法
    /// </summary>
    public static class XmlHelperExtension
    {
        /// <summary>
        /// 选择匹配XPath表达式将匹配节点映射为实体集合List
        /// </summary>
        public static List<TEntity> GetXmlEntityListByXpath<TEntity>(this IXmlHelper xmlHealHelper, string xpath)
            where TEntity : XmlEntity, new()
        {
            var xmlNodes = xmlHealHelper.GetXmlNodeListByXpath(xpath);
            var xmlEntitys = new List<TEntity>();
            foreach (System.Xml.XmlNode node in xmlNodes)
            {
                var xmlEntity = GetEntityByNode<TEntity>(xmlHealHelper, node);
                xmlEntitys.Add(xmlEntity);
            }
            return xmlEntitys;
        }

        /// <summary>
        /// 将XmlNode节点映射为实体节点
        /// </summary>
        public static TEntity GetEntityByNode<TEntity>(this IXmlHelper xmlHealHelper, System.Xml.XmlNode node)
            where TEntity : XmlEntity, new()
        {
            var xmlEntity = new TEntity();
            if (node == null) return null;

            xmlEntity.EntityName = node.Name;
            xmlEntity.ChildEntitys = GetChildEntitys<TEntity>(xmlHealHelper, node);
            var propertys = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.SetProperty);
            foreach (var p in propertys)
            {
                if (node.Attributes != null && node.Attributes[p.Name] != null)
                {
                    p.SetValue(xmlEntity, node.Attributes[p.Name].Value, new object[]{});
                }
            }
            return xmlEntity;
        }

        /// <summary>
        /// 获取某个父节点的所有子节点实体集合
        /// </summary>
        public static List<TEntity> GetChildEntitys<TEntity>(this IXmlHelper xmlHealHelper, System.Xml.XmlNode fatherNode)
            where TEntity : XmlEntity, new()
        {
            if (fatherNode.ChildNodes.Count == 0) return null;

            var childEntitys = new List<TEntity>();
            foreach (System.Xml.XmlNode childNode in fatherNode.ChildNodes)
            {
                var childEntity = new TEntity();
                var propertys = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.SetProperty);
                childEntity.EntityName = childNode.Name;
                childEntity.ChildEntitys = GetChildEntitys<TEntity>(xmlHealHelper, childNode);
                if (childNode.Attributes != null) //反射设置实体属性值
                {
                    foreach (var p in propertys)
                    {
                        if (childNode.Attributes[p.Name] != null)
                        {
                            p.SetValue(childEntity, childNode.Attributes[p.Name].Value, new object[]{});
                        }
                    }
                }
                childEntitys.Add(childEntity);
            }
            return childEntitys;
        }
    }

    /// <summary>
    /// 基础Xml节点类
    /// </summary>
    public class XmlEntity
    {
        /// <summary>
        /// 节点名
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 子节点集合
        /// </summary>
        public object ChildEntitys { get; set; }
    }
}
