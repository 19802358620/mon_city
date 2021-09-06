using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace EKP.Base.Identity
{
    /// <summary>
    /// 名    称：ApplicationUserManager
    /// 作    者：胡政
    /// 参考：http://www.dotblogs.com.tw/ageoldmemories/archive/2013/12/19/135285.aspx
    /// 创建时间：2015-08-28
    /// 联系方式：13436053642
    /// 描    述：用户信息管理
    /// </summary>
    public class ApplicationUserManager : UserManager<IdentityUser, string>
    {
        public ApplicationUserManager(IUserStore<IdentityUser, string> userStore)
            : base(userStore)
        {

        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var userStore = new UserStore();
            var manager = new ApplicationUserManager(userStore);

            manager.UserValidator = new UserValidator<IdentityUser, string>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true //获取一个值，该值指示 Active Directory 服务器中存储的电子邮件地址是否必须是唯一的。
            };
            manager.PasswordHasher = new PasswordHasher();

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<IdentityUser, string>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<IdentityUser, string>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<IdentityUser, string>(
                        dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }
    }
}