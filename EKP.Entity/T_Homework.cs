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
    
    public partial class T_Homework
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Request { get; set; }
        public string Link { get; set; }
        public string LinkName { get; set; }
        public string Attachment { get; set; }
        public string AttachmentName { get; set; }
        public string AnswerAttachment { get; set; }
        public string AnswerAttachmentName { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<System.DateTime> StartDateTime { get; set; }
        public Nullable<System.DateTime> EndDateTime { get; set; }
        public string Status { get; set; }
        public string ScoreDegree { get; set; }
        public string Remark { get; set; }
        public string IsDeleted { get; set; }
    }
}
