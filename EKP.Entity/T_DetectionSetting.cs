//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EKP.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_DetectionSetting
    {
        public int Id { get; set; }
        public string ViewAnswerMode { get; set; }
        public Nullable<int> DetectionId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string CreateIp { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string IsDeleted { get; set; }
        public Nullable<int> SiteId { get; set; }
    }
}
