using System;
using Ge.Infrastructure.Metronicv;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.User;
using EKP.Service.LearningResource;
using EKP.Service.LearningResourceClass;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;

namespace EKP.Service.LearningResource
{
    public interface IResourceService : IEkpEntityService<T_LearningResource>
    {
        JqgridResult<T> GetPager<T>(ResourcePagerParam param, params string[] includePath) where T : class, new();
    }

    public class ResourceService : EkpEntityService<T_LearningResource>, IResourceService
    {
        public JqgridResult<T> GetPager<T>(ResourcePagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_LearningResource.* {0} from T_LearningResource {1} {2} {3}";
            //string sql = "select top 99.99999999 percent T_LearningResource.*,(T_User.RealName) as TeacherName,tempT.ClassId, tempT.ClassName,(Shared1.RealName) as SharedUser,(Shared2.Name) as SharedResource " +
            //             " from T_LearningResource" +
            //             " inner join( select A.ResourceId, ((select ClassId+ ',' from V_LearningResourceClass where ResourceId=A.ResourceId for XML Path(''))) AS ClassId, (select Name +',' from V_LearningResourceClass where ResourceId=A.ResourceId for XML Path('')) as ClassName from V_LearningResourceClass A Group by A.ResourceId) tempT on T_LearningResource.Id=tempT.ResourceId" +
            //             " left join T_User  on T_LearningResource.UserId=T_User.Id" +
            //             " left join T_User Shared1 on T_LearningResource.SharedUserId=Shared1.Id" +
            //             " left join T_Learningresource Shared2 on T_LearningResource.SharedResourceId=Shared2.Id" ;

            String SqlSelect = string.Empty;
            String SqlJoin = string.Empty;
            String SqlWhere = string.Empty;
            String SqlOrderBy = string.Empty;


            //链接查询
            SqlSelect += " ,(T_User.RealName) as TeacherName,tempT.ClassId, tempT.ClassName,(Shared1.RealName) as SharedUser ";

            SqlJoin += " left join( select A.ResourceId, ((select ClassId+ ',' from V_LearningResourceClass where ResourceId=A.ResourceId for XML Path(''))) AS ClassId, (select ClassName +',' from V_LearningResourceClass where ResourceId=A.ResourceId for XML Path('')) as ClassName from V_LearningResourceClass A Group by A.ResourceId) tempT on T_LearningResource.Id=tempT.ResourceId" +
                      " left join T_User  on T_LearningResource.UserId=T_User.Id" +
                      " left join T_User Shared1 on T_LearningResource.SharedUserId=Shared1.Id";            

            //条件查询 
            if (param.KeyWord != null )
                SqlWhere += string.Format(" Where T_LearningResource.Name like '%{0}%' or T_LearningResource.Type like '%{0}%' or T_User.RealName like '%{0}%'", param.KeyWord);
            //老师查看
            if(param.UserId != 0 && param.RoleId == 137)
            {
                if (string.IsNullOrEmpty(SqlWhere))
                    SqlWhere += string.Format(" Where ");
                else
                    SqlWhere += string.Format(" and ");
                SqlWhere += string.Format( " T_LearningResource.UserId={0}",param.UserId);         
            }

            //班级筛选
            if (param.ClassIdSearch != 0)
            {
                if (string.IsNullOrEmpty(SqlWhere))
                    SqlWhere += string.Format(" Where ");
                else
                    SqlWhere += string.Format(" and ");
                SqlWhere += string.Format(" TempT.ClassId like '%{0}%'", param.ClassIdSearch);
            }

            //学生查看
            if (param.RoleId == 138)
            {
                SqlWhere = string.Empty;
                SqlJoin = string.Empty;
                SqlSelect = " ,(T_User.RealName) as TeacherName";
                SqlOrderBy = string.Empty;
                SqlJoin += " left join T_learningResourceClass on T_LearningResource.Id=T_LearningResourceClass.ResourceId " +
                           " left join T_User on T_LearningResource.UserId = T_User.Id" ;
                SqlWhere += string.Format(" where ClassId = {0} ",param.ClassIds );

                //条件查询 
                if (param.KeyWord != null)
                    SqlWhere += string.Format(" and T_LearningResource.Name like '%{0}%' or T_LearningResource.Type like '%{0}%' or T_User.RealName like '%{0}%'", param.KeyWord);
            }

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                SqlOrderBy = string.Format(" order by T_LearningResource.{0} {1} ", param.SortBy, param.SortOrder);

            sql = string.Format(sql, SqlSelect, SqlJoin, SqlWhere,SqlOrderBy);
            var pager = EkpDbService.GetPager<T>(sql, param);


            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        }
    }

}
