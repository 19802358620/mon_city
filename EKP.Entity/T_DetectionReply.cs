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
    
    public partial class T_DetectionReply
    {
        public int Id { get; set; }
        public Nullable<int> DetectionHandId { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public string Value { get; set; }
        public Nullable<double> Score { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string IsDeleted { get; set; }
    }
}
