using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EKP.Repository.Ef
{
    /// <summary>
    /// 数据库实体管理
    /// </summary>
    public class SetEntityState
    {
        private readonly DbContext _context;

        public SetEntityState(DbContext context)
        {
            this._context = context; 
        }

        private void SetState<T>(T entity, EntityState state) where T : class
        {
            this._context.Entry(entity).State = state;
        }

        public void Update<T>(T entity, string[] fileds = null) where T : class
        {
            entity = this._context.Entry(entity).Entity;
            this._context.Set<T>().Attach(entity); 

            //设置需要被修改的字段
            //不能再将contextd的状态标识为“Modified”，否者会更新所有字段
            if (fileds != null)
            {
                var stateEntry = ((IObjectContextAdapter) this._context).ObjectContext.
                    ObjectStateManager.GetObjectStateEntry(entity);
                foreach (var t in fileds)
                {
                    stateEntry.SetModifiedProperty(t);
                }
            }
            else
            {
                this.SetState(entity, EntityState.Modified);
            }
        }

        public void Delete<T>(T entity) where T : class
        {
            entity = this._context.Entry(entity).Entity;
            this._context.Set<T>().Attach(entity);
            this.SetState(entity, EntityState.Deleted);
        }

        public void Add<T>(T entity) where T : class
        {
            this._context.Set<T>().Add(entity);
            this.SetState(entity, EntityState.Added);
        }

        public void SaveChange()
        {
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
