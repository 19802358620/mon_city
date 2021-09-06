using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Metronicv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.LearningResourceClass
{
    public interface IResourceClassService : IEkpEntityService<T_LearningResourceClass>
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="includePath"></param>
        /// <returns></returns>
        JqgridResult<T> GetPager<T>(ResourceClassPagerParam param, params string[] includePath) where T : class, new();
        
    }

    public class ResourceClassService : EkpEntityService<T_LearningResourceClass>,IResourceClassService
    {
        public JqgridResult<T> GetPager<T>(ResourceClassPagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = string.Empty;
            var rows=EkpDbService.GetDt(sql).ToList<T>();
            var count = EkpDbService.GetCount(sql);
            return new JqgridResult<T>(param)
            {
                Rows = rows,
                TotalRecords = count,

            };
        }
    }
}
