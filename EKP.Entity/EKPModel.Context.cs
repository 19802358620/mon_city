﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EKP_JSEntities : DbContext
    {
        public EKP_JSEntities()
            : base("name=EKP_JSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<T_AdmSetting> T_AdmSetting { get; set; }
        public DbSet<T_Authority> T_Authority { get; set; }
        public DbSet<T_Class> T_Class { get; set; }
        public DbSet<T_Detection> T_Detection { get; set; }
        public DbSet<T_DetectionHand> T_DetectionHand { get; set; }
        public DbSet<T_DetectionReply> T_DetectionReply { get; set; }
        public DbSet<T_DetectionSetting> T_DetectionSetting { get; set; }
        public DbSet<T_DictKey> T_DictKey { get; set; }
        public DbSet<T_DictValue> T_DictValue { get; set; }
        public DbSet<T_Homework> T_Homework { get; set; }
        public DbSet<T_HomeworkClass> T_HomeworkClass { get; set; }
        public DbSet<T_HomeworkSubmit> T_HomeworkSubmit { get; set; }
        public DbSet<T_Info> T_Info { get; set; }
        public DbSet<T_LearningResource> T_LearningResource { get; set; }
        public DbSet<T_LearningResourceClass> T_LearningResourceClass { get; set; }
        public DbSet<T_Notice> T_Notice { get; set; }
        public DbSet<T_NoticeClass> T_NoticeClass { get; set; }
        public DbSet<T_Project> T_Project { get; set; }
        public DbSet<T_ProjectInfo> T_ProjectInfo { get; set; }
        public DbSet<T_Question> T_Question { get; set; }
        public DbSet<T_Role> T_Role { get; set; }
        public DbSet<T_Site> T_Site { get; set; }
        public DbSet<T_Subject> T_Subject { get; set; }
        public DbSet<T_User> T_User { get; set; }
        public DbSet<V_HomeworkClass> V_HomeworkClass { get; set; }
        public DbSet<T_Video> T_Video { get; set; }
        public DbSet<T_VideoTree> T_VideoTree { get; set; }
    }
}