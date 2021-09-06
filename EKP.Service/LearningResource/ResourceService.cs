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
            string sql = "select top 99.99999999 percent T_LearningResource.*,(T_User.RealName) as TeacherName,tempT.ClassId, tempT.ClassName" +
                         " from T_LearningResource inner join(" +
                         " select A.ResourceId," +
                         " ((select ClassId+ ',' from V_LearningResourceClass where ResourceId=A.ResourceId for XML Path(''))) AS ClassId," +
                         " (select Name +',' from V_LearningResourceClass where ResourceId=A.ResourceId for XML Path('')) as ClassName" +
                         " from V_LearningResourceClass A Group by A.ResourceId) tempT on T_LearningResource.Id=tempT.ResourceId" +
                         " left join T_User  on T_LearningResource.UserId=T_User.Id";
            sql = string.Format(sql);
            var rows = EkpDbService.GetDt(sql).ToList<T>();
            var count = EkpDbService.GetCount(sql);

            return new JqgridResult<T>(param)
            {
                Rows = rows,
                TotalRecords = count,
            };
        }
    }

}
