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
    
    public partial class T_Notice
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> InvalidDateTime { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public string Link { get; set; }
        public string LinkName { get; set; }
        public string Accessory { get; set; }
        public string AccessoryName { get; set; }
        public string Remark { get; set; }
    }
}
