using System;
using System.Linq;
using EKP.Entity;
using EKP.Service.Role;
using EKP.Service.User;
using Ge.Infrastructure.Ioc;
using Microsoft.AspNet.Identity;
namespace EKP.Base.Identity
{
    /// <summary>
    /// 登录用户详细信息
    /// </summary>
    public class IdentityUser : IUser<string>
    {
        private T_User user = null;
        private readonly IUserService _userService = Ioc.GetService<IUserService>();
        private readonly IRoleService _roleService = Ioc.GetService<IRoleService>();

        public IdentityUser(T_User user)
        {
            this.user = user;
            this.Id = user.Id.ToString();
            this.UserName = user.Account.ToString();
        }

        /// <summary>
        /// 本次登录用户Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 本次登录用户Id
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 本次登录方式
        /// </summary>
        public LoginMethod LoginMethod { get; set; }

        /// <summary>
        /// 本次登录角色
        /// </summary>
        public string LoginRoleId { get; set; }

        /// <summary>
        /// 本次登录标识
        /// </summary>
        public string LoginRoleKey { get; set; }

        /// <summary>
        /// 当前登陆用户信息
        /// </summary>
        public T_User LoginUser
        {
            get { return user; }
        }

    }

    /// <summary>
    /// 登录方式枚举值
    /// </summary>
    public enum LoginMethod
    {
        Ip登录 = 1,
        后台登陆 = 2,
        前台登录 = 3,
        微信登录 = 4
    }
}