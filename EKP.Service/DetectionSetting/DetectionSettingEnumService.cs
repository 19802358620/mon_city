using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Class;
using EKP.Service.Detection;
using EKP.Service.Question;
using EKP.Service.Subject;
using EKP.Service.DetectionReply;
using EKP.Service.User;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Pager;
using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EKP.Service.DetectionSetting
{
    /// <summary>
    /// 思考练习设置管理
    /// </summary>
    public interface IDetectionSettingService : IEkpEntityService<T_DetectionSetting>
    {

    }

    /// <summary>
    /// 思考练习设置管理
    /// </summary>
    public class DetectionSettingService : EkpEntityService<T_DetectionSetting>, IDetectionSettingService
    {


    }
}
