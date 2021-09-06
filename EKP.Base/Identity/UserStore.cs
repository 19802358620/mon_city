using System;
using System.Data.Entity;
using System.Threading.Tasks;
using EKP.Entity;
using Microsoft.AspNet.Identity;

namespace EKP.Base.Identity
{
    /// <summary>
    /// 名    称：UserStore
    /// 作    者：胡政
    /// 参考：http://www.dotblogs.com.tw/ageoldmemories/archive/2013/12/19/135285.aspx
    /// 创建时间：2015-08-28
    /// 联系方式：13436053642
    /// 描    述：用户管理
    /// </summary>
    public partial class UserStore :
        IUserPasswordStore<IdentityUser, object>
    {
        private readonly EKP_JSEntities _db;

        public UserStore()
        {
            this._db = new EKP_JSEntities();
        }

        #region IUserPasswordStore<IdentityUser, Key>

        public Task CreateAsync(IdentityUser user)
        {
            //this._db.ContactSecurity.Add(user);
            //this._db.SaveChanges();
            return this._db.SaveChangesAsync();
        }

        public Task DeleteAsync(IdentityUser user)
        {
            //this._db.ContactSecurity.Remove(user);
            return this._db.SaveChangesAsync();
        }

        public Task<IdentityUser> FindByIdAsync(object userId)
        {
            //return this._db.T_User.FirstOrDefaultAsync(u => u.Id == userId);
            return null;
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            //return this._db.T_User.FirstOrDefaultAsync(u => u.UserName == userName);
            return null;
        }

        public Task UpdateAsync(IdentityUser user)
        {
            this._db.Entry<IdentityUser>(user).State = EntityState.Modified;
            return this._db.SaveChangesAsync();
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            //return Task.FromResult(user.PassWord);
            return null;
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            //return Task.FromResult(!string.IsNullOrEmpty(user.PassWord));
            return null;
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            //user.Password = passwordHash;
            return Task.FromResult(0);
        }

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this._db != null)
            {
                this._db.Dispose();
            }
        }

        #endregion

        #endregion
    }
}