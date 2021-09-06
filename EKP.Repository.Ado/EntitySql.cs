using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Repository.Ado
{
    /// <summary>
    /// 实体sql语句拼接
    /// </summary>
    internal class EntitySql
    {
        /// <summary>
        /// select语句模板(获取单个实体)
        /// </summary>
        public static string SelectSql<T>(int id) where T : class
        {
            var tabelName = typeof(T).Name;
            return string.Format("select * from {0} where {0}.Id = {1}", tabelName, id);
        }

        /// <summary>
        /// select语句模板(获取多个实体)
        /// </summary>
        public static string SelectSql<T>(string where = null)
        {
            var tabelName = typeof(T).Name;
            return string.Format("select * from {0} where {1}", tabelName, string.IsNullOrEmpty(where) ? "1=1" : where);
        }

        public static string SelectSql<T>(int top, string where = null)
        {
            var tabelName = typeof(T).Name;
            return string.Format("select top {0} * from {1} where {2}", top, tabelName, string.IsNullOrEmpty(where) ? "1=1" : where);
        }

        /// <summary>
        /// Add语句
        /// </summary>
        public static string AddSql<T>(T entity) where T : class
        {
            var insertTemplate = "insert into {0} ({1})values({2})";
            var tabelName = typeof(T).Name;
            var fields = string.Empty;
            var setting = string.Empty;
            var propertys = entity.GetType().GetProperties();

            foreach (var p in propertys)
            {
                if (p.Name.ToLower() == "id") continue;

                var value = p.GetValue(entity, null);
                fields += string.Format("[{0}], ", p.Name);
                if (value == null)
                    setting += string.Format("null, ");
                else
                    setting += string.Format("'{0}', ", value);
            }

            if (!string.IsNullOrEmpty(fields) && fields.Length >= 2)
            {
                fields = fields.Substring(0, fields.Length - 2);
                setting = setting.Substring(0, setting.Length - 2);
            }

            //"SELECT @@IDENTITY AS 'Id'"会在插入后返回插入的自增id
            return string.Format(insertTemplate, tabelName, fields, setting) + " SELECT @@IDENTITY AS 'Id'";
        }

        /// <summary>
        /// update语句
        /// </summary>
        public static string UpdateSql<T>(T entity, params string[] fileds) where T : class
        {
            var updateTemplate = "update {0} {1} {2}";
            var tabelName = typeof(T).Name;
            var propertys = entity.GetType().GetProperties();
            var setting = "set ";
            var id = 0;

            foreach (var p in propertys)
            {
                if (p.Name.ToLower() == "id")
                {
                    id = Convert.ToInt32(p.GetValue(entity, null));
                    continue;
                }
                else if (fileds != null && fileds.Length != 0)
                {
                    if (!fileds.Contains(p.Name))
                        continue;
                }

                var value = p.GetValue(entity, null);
                if (value == null)
                    setting += string.Format("[{0}]={1}, ", p.Name, "null");
                else
                    setting += string.Format("[{0}]='{1}', ", p.Name, value);
            }
            setting = setting.Substring(0, setting.Length - 2);

            return string.Format(updateTemplate, tabelName, setting, " where id=" + id);
        }

        /// <summary>
        /// delete语句模板
        /// </summary>
        public static string DeleteTemplate<T>(T entity) where T : class
        {
            var tabelName = typeof(T).Name;
            var propertys = entity.GetType().GetProperties();
            var id = 0;

            foreach (var p in propertys)
            {
                if (p.Name.ToLower() == "id")
                {
                    id = Convert.ToInt32(p.GetValue(entity, null));
                    break;
                }
            }
            return string.Format("delete from {0} where Id = {1}", tabelName, id);
        }
    }
}
