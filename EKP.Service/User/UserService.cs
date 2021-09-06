using System;
using System.Collections.Generic;
using System.Linq;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Class;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.User
{
    /// <summary>
    /// 用户管理接口
    /// </summary>
    public interface IUserService : IEkpEntityService<T_User>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(UserPagerParam param, params string[] includePath) where T : UserPagerModel, new();

        /// <summary>
        /// 根据ip获取用户
        /// </summary>
        T_User GetUserByIp(string ip);

        /// <summary>
        /// 获取所有的老师
        /// </summary>
        JqgridResult<T> GetTeacherPager<T>(UserPagerParam param, params string[] includePath) where T : class, new();
    }

    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserService : EkpEntityService<T_User>, IUserService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(UserPagerParam param, params string[] includePath) where T : UserPagerModel, new()
        {
            var classService = Ioc.GetService<IClassService>();

            var sql = "select top 99.99999999 percent T_User.* {0} from T_User {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_User.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_Role"))
            {
                sqlSelect += " ,(T_Role.Name)RoleName ";
                sqlJoin += " left join T_Role on T_Role.Id = T_User.RoleId ";
            }
            if (includePath.Contains("T_Site"))
            {
                sqlSelect += " ,(T_Site.Name)SiteName ";
                sqlJoin += " left join T_Site on T_Site.Id = T_User.SiteId ";
            }


            //查询
            if (param.KeyWord != null)
                sqlWhere += string.Format(" and (T_User.Account like '%{0}%' or T_User.RealName like '%{0}%' or T_User.Email like '%{0}%') ", param.KeyWord);
            if (!string.IsNullOrEmpty(param.Gender))
                sqlWhere += string.Format(" and T_User.Gender like '%{0}%' ", param.Gender);
            if (param.RoleId != null)
                sqlWhere += string.Format(" and T_User.RoleId = {0} ", param.RoleId);
            if (param.SiteId != null)
                sqlWhere += string.Format(" and T_User.SiteId = '{0}' ", param.SiteId);
            if (!string.IsNullOrEmpty(param.Status))
                sqlWhere += string.Format(" and T_User.Status = '{0}' ", param.Status);
            if (!string.IsNullOrEmpty(param.Grade))
                sqlWhere += string.Format(" and T_Role.Grade = '{0}' ", param.Grade);
            if(param.ClassId != null)
                sqlWhere += string.Format(" and ',' + T_User.ClassIds + ',' like '%,{0},%' ", param.ClassId);
            if (param.ClassIds != null)
                sqlWhere += string.Format(" and ',{0},' like '%,' + T_User.ClassIds + ',%' ", param.ClassIds);

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by {0}{1} {2} ",
                    param.SortBy.Contains(".") ? string.Empty : "T_User.", param.SortBy, param.SortOrder);

            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);
            var pager = EkpDbService.GetPager<T>(sql, param);

            //填充班级
            var classIds = new List<int>();
            var classes = new List<T_Class>();
            pager.Rows.FindAll(row => !string.IsNullOrEmpty(row.ClassIds)).ForEach(row => {
                row.ClassIds.Split(',').ToList().ForEach(idstr => { classIds.Add(Convert.ToInt32(idstr)); });
            });
            classes = classService.GetList(classIds.ToArray());
            pager.Rows.FindAll(row => !string.IsNullOrEmpty(row.ClassIds)).ForEach(row => {
                row.ClassIds.Split(',').ToList().ForEach(idstr => {
                    var class_ = classes.FirstOrDefault(c => c.Id == Convert.ToInt32(idstr));
                    if (class_ == null) return;
                    row.ShowClasses += "," + class_.Name ;
                });
                row.ShowClasses = row.ShowClasses.RelpaceFirst(",", string.Empty);
            });

            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        }

        /// <summary>
        /// 根据ip获取用户
        /// </summary>
        public T_User GetUserByIp(string ip)
        {
            var users = this.GetList("Ips is not null and Ips != ''");

            foreach (var user in users)
            {
                if (IpsHelper.IsInIps(ip, user.Ips))
                {
                    return user;
                }
            }

            return null;
        }

        public JqgridResult<T> GetTeacherPager<T>(UserPagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_User.* from T_User where RoleId=137";
            var pager = EkpDbService.GetPager<T>(sql, param);
            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        
        }

    }
}
