using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Class;
using EKP.Service.DictValue;
using EKP.Service.Role;
using EKP.Service.User;
using EKP.Base.Identity;
using EKP.Base.Controllers;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Mvc.Extensions;
using Ge.Infrastructure.Utilities;
using Microsoft.AspNet.Identity.Owin;
using EKP.Web.Areas.Base.Application;
using EKP.Adm;
using EKP.Base;
using EKP.Service.Site;
using EKP.Adm.Authority;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Excel;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserController : EntityController<T_User>
    {
        private readonly IUserService userService = Ioc.GetService<IUserService>();
        private readonly IRoleService roleService = Ioc.GetService<IRoleService>();
        private readonly ISiteService siteService = Ioc.GetService<ISiteService>();
        private readonly IDictValueService dictValueService = Ioc.GetService<IDictValueService>();
        private readonly IClassService classService = Ioc.GetService<IClassService>();

        public UserController()
            : base(Ioc.GetService<IUserService>())
        {


        }

        #region 公共

        /// <summary>
        /// 分页post
        /// </summary>
        [HttpPost]
        public ActionResult Pager(UserPagerParam param)
        {
            return Json(userService.GetPager<UserPagerModel>(param, "T_Role", "T_Site"));
        }

        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int? id, string account)
        {
            var user = new T_User();
            if (id != null)
                user = userService.GetEntiy(Convert.ToInt32(id));
            if (!string.IsNullOrWhiteSpace(account))
                user = userService.GetEntiy(string.Format(" Account = '{0}'", account));
            var role = roleService.GetEntiy(Convert.ToInt32(user.RoleId));
            var site = siteService.GetEntiy(Convert.ToInt32(user.SiteId));
            var detail = ObjectMapper.Mapper<T_User, UserPagerModel>(user);
            if (!string.IsNullOrEmpty(detail.ClassIds))
            {
                var classIds = ObjectMapper.Mapper<List<string>, List<int>>(detail.ClassIds.Split(',').ToList());
                var classes = classService.GetList(classIds.ToArray());
                detail.Classes = classes.ToModel<List<T_Class>, List<ClassPagerModel>>();
            }
            detail.RoleName = role.Name;
            detail.SiteName = site.Name;
            detail.PassWord = "******";
            return Json(detail);
        }

        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            return Json(userService.Delete(ids.ToArray()));
        }

        /// <summary>
        /// 账号重复性验证
        /// </summary>
        [HttpPost]
        public ActionResult ValidName(string name, int? siteId)
        {
            return Json(new
            {
                valid = !userService.IsExist(string.Format("Account = '{0}' and SiteId = '{1}' and IsDeleted = '{2}'", name, siteId, IsDelete.undeleted))
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 登录
        /// </summary>
        [AdmFilter.Disabled]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录Post
        /// </summary>
        [HttpPost]
        public ActionResult Login(UserLoginModel loginModel)
        {
            var result = new JavaScriptResult();

            if (!this.ModelValidate(loginModel).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(loginModel).FirstError.ErrorMessage));
            }
            var password = Md5Encrypt.Md5EncryptPassword(loginModel.Password).ToString().Trim();
            var user = userService.GetEntiy(string.Format("Account='{0}' and Password='{1}' and IsDeleted = '{2}'",
                loginModel.Name, password, IsDelete.undeleted));
            if (user == null)
            {
                return Json(DialogFactory.Create(DialogType.Error, "账号或密码错误"));
            }
            var identityUser = new IdentityUser(user);
            identityUser.LoginMethod = (LoginMethod)loginModel.LoginMethod;
            identityUser.LoginRoleId = user.RoleId.ToString();
            var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            signInManager.SignIn(identityUser, false);

            return Json(new
            {
                IsSucceed = true
            });
        }

        /// <summary>
        /// 切换角色Post
        /// </summary>
        [HttpPost]
        public ActionResult SwitchRole(int roleId)
        {
            var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            signInManager.SwitchRole(roleId.ToString());

            return Json(DialogFactory.Create(DialogType.Success));
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        [HttpPost]
        public ActionResult LoginOut()
        {
            var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            signInManager.SignOut();
            return Json(DialogFactory.Create(DialogType.Success, "退出成功！"));
        }

        /// <summary>
        /// 获取当前登陆人信息
        /// </summary>
        [HttpPost]
        public ActionResult LoginUserInfo()
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            if (loginInUser == null) return Json(null);

            var user = loginInUser.LoginUser;
            var role = roleService.GetEntiy(Convert.ToInt32(user.RoleId));
            var model = ObjectMapper.Mapper<T_User, UserPagerModel>(user);
            model.RoleName = role.Name;
            return Json(model);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public ActionResult ModifyPassword()
        {
            return View();
        }

        /// <summary>
        /// 修改密码Post
        /// </summary>
        [HttpPost]
        public ActionResult ModifyPassword(ResetPasswordModel model)
        {
            var loginInUser = ApplicationSignInManager.GetLoginUser();

            if (loginInUser == null)
            {
                return Json(DialogFactory.Create(DialogType.Error, "请登录！"));
            }

            if (loginInUser.LoginUser.Account != model.Account)
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数错误！"));
            }

            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }

            return Json(Edit("Account = '{0}'".Format2(model.Account), model));
        }

        #endregion

        #region 管理员

        /// <summary>
        /// 分页
        /// </summary>
        public ActionResult Pager()
        {
            return View();
        }

        /// <summary>
        /// 创建
        /// </summary>
        public ActionResult Create()
        {
            return PartialView();
        }

        /// <summary>
        /// 创建post
        /// </summary>
        [HttpPost]
        public ActionResult Create(UserCreateModel model)
        {
            //模型数据安全验证
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }
            var user = ObjectMapper.Mapper<UserCreateModel, T_User>(model);
            //密码不能为******
            if (user.PassWord.Equals("******"))
            {
                return Json(DialogFactory.Create(DialogType.Error, "错误", "密码串非法"));
            }
            user.PassWord = Md5Encrypt.Md5EncryptPassword(model.PassWord.Trim()).ToString();
            user.CreateTime = DateTime.Now;
            user.IsDeleted = IsDelete.undeleted.ToString();

            //ip格式验证
            var error = string.Empty;
            if (!IpsHelper.IsRightIps(model.Ips, out error))
            {
                return Json(DialogFactory.Create(DialogType.Error, error));
            }

            //ip冲突验证
            var ipUsers = userService.GetList("Ips is not null and Ips != '' and SiteId = '{0}'".Format2(model.SiteId));
            foreach (var u in ipUsers)
            {
                if (IpsHelper.IpsConflict(u.Ips, model.Ips))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("Ip段与用户“{0}”冲突，请修改！", u.Account)));
                }
            }

            //身份证重复验证
            if (userService.IsExist("IDNumber='{0}' and RoleId = '{1}' and SiteId = '{2}'".Format2(model.IDNumber, model.RoleId, model.SiteId)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "该身份证已被注册，请更换！"));
            }

            //手机重复验证
            if (userService.IsExist("Telephone='{0}' and RoleId = '{1}' and SiteId = '{2}'".Format2(model.Telephone, model.RoleId, model.SiteId)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "该手机已被注册，请更换！"));
            }

            //事务添加用户
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                trans.Add(user);
                var role = roleService.GetEntiy(string.Format(" Id = {0}", user.RoleId));
                trans.SaveChange();
            }

            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "创建成功！"));
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public ActionResult Edit()
        {
            return PartialView();
        }

        /// <summary>
        /// 编辑post
        /// </summary>
        [HttpPost]
        public ActionResult Edit(UserEditModel model)
        {
            var user = userService.GetEntiy(model.Id);

            model.Account = user.Account;
            if (model.PassWord.Equals("******"))
                model.PassWord = user.PassWord;
            else
                model.PassWord = Md5Encrypt.Md5EncryptPassword(model.PassWord.Trim()).ToString();

            //ip格式验证
            var error = string.Empty;
            if (!IpsHelper.IsRightIps(model.Ips, out error))
            {
                return Json(DialogFactory.Create(DialogType.Error, error));
            }

            //ip冲突验证
            var ipUsers = userService.GetList("Ips is not null and Ips != '' and Id != '{0}' and SiteId = '{1}'".Format2(model.Id, model.SiteId));
            foreach (var u in ipUsers)
            {
                if (IpsHelper.IpsConflict(u.Ips, model.Ips))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("Ip段与用户“{0}”冲突，请修改！", u.Account)));
                }
            }

            //身份证重复验证
            if (userService.IsExist("IDNumber='{0}' and RoleId = '{1}' and Id != '{2}'  and SiteId = '{3}'".Format2(model.IDNumber, model.RoleId, model.Id, model.SiteId)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "该身份证已被注册，请更换！"));
            }

            //手机重复验证
            if (userService.IsExist("Telephone='{0}' and RoleId = '{1}' and Id != '{2}'  and SiteId = '{3}'".Format2(model.Telephone, model.RoleId, model.Id, model.SiteId)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "该手机已被注册，请更换！"));
            }

            return Json(EditIgnore("id = {0}".Format2(model.Id), model, "PassWord", "SiteId", "CompanyId", "CreateTime", "IsDeleted"));
        }

        #endregion

        #region 教师

        /// <summary>
        /// 分页
        /// </summary>
        public ActionResult TeacherPager()
        {
            return View();
        }

        /// <summary>
        /// 老师数据获取post
        /// </summary>
        [HttpPost]
        public ActionResult TeacherPager(UserPagerParam param)
        {
            return Json(userService.GetTeacherPager<UserPagerModel>(param, "T_Role", "T_Site"));
        }

        /// <summary>
        /// 创建
        /// </summary>
        public ActionResult TeacherCreate()
        {
            return PartialView();
        }

        /// <summary>
        /// 创建post
        /// </summary>
        [HttpPost]
        public ActionResult TeacherCreate(TeacherCreateModel model)
        {
            //模型数据安全验证
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }
            var user = ObjectMapper.Mapper<TeacherCreateModel, T_User>(model);
            //密码不能为******
            if (user.PassWord.Equals("******"))
            {
                return Json(DialogFactory.Create(DialogType.Error, "错误", "密码串非法"));
            }
            user.PassWord = Md5Encrypt.Md5EncryptPassword(model.PassWord.Trim()).ToString();
            user.CreateTime = DateTime.Now;
            user.IsDeleted = IsDelete.undeleted.ToString();

            //ip格式验证
            var error = string.Empty;
            if (!IpsHelper.IsRightIps(model.Ips, out error))
            {
                return Json(DialogFactory.Create(DialogType.Error, error));
            }

            //ip冲突验证
            var ipUsers = userService.GetList("Ips is not null and Ips != '' and SiteId = '{0}'".Format2(model.SiteId));
            foreach (var u in ipUsers)
            {
                if (IpsHelper.IpsConflict(u.Ips, model.Ips))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("Ip段与用户“{0}”冲突，请修改！", u.Account)));
                }
            }

            //身份证重复验证
            if (userService.IsExist("IDNumber='{0}' and RoleId = '{1}' and SiteId = '{2}'".Format2(model.IDNumber, model.RoleId, model.SiteId)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "该身份证已被注册，请更换！"));
            }

            //手机重复验证
            if (userService.IsExist("Telephone='{0}' and RoleId = '{1}' and SiteId = '{2}'".Format2(model.Telephone, model.RoleId, model.SiteId)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "该手机已被注册，请更换！"));
            }

            //事务添加用户
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                trans.Add(user);
                var role = roleService.GetEntiy(string.Format(" Id = {0}", user.RoleId));
                trans.SaveChange();
            }

            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "创建成功！"));
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public ActionResult TeacherEdit()
        {
            return PartialView();
        }

        /// <summary>
        /// 编辑post
        /// </summary>
        [HttpPost]
        public ActionResult TeacherEdit(TeacherEditModel model)
        {
            var user = userService.GetEntiy(model.Id);

            model.Account = user.Account;
            if (model.PassWord.Equals("******"))
                model.PassWord = user.PassWord;
            else
                model.PassWord = Md5Encrypt.Md5EncryptPassword(model.PassWord.Trim()).ToString();

            //ip格式验证
            var error = string.Empty;
            if (!IpsHelper.IsRightIps(model.Ips, out error))
            {
                return Json(DialogFactory.Create(DialogType.Error, error));
            }

            //ip冲突验证
            var ipUsers = userService.GetList("Ips is not null and Ips != '' and Id != '{0}' and SiteId = '{1}'".Format2(model.Id, model.SiteId));
            foreach (var u in ipUsers)
            {
                if (IpsHelper.IpsConflict(u.Ips, model.Ips))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("Ip段与用户“{0}”冲突，请修改！", u.Account)));
                }
            }

            //身份证重复验证
            if (userService.IsExist("IDNumber='{0}' and RoleId = '{1}' and Id != '{2}'  and SiteId = '{3}'".Format2(model.IDNumber, model.RoleId, model.Id, model.SiteId)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "该身份证已被注册，请更换！"));
            }

            //手机重复验证
            if (userService.IsExist("Telephone='{0}' and RoleId = '{1}' and Id != '{2}'  and SiteId = '{3}'".Format2(model.Telephone, model.RoleId, model.Id, model.SiteId)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "该手机已被注册，请更换！"));
            }

            return Json(EditIgnore("id = {0}".Format2(model.Id), model, "PassWord", "SiteId", "CompanyId", "CreateTime", "IsDeleted"));
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        public ActionResult TeacherImportExcel()
        {
            return PartialView();
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        [HttpPost]
        public ActionResult TeacherImportExcel(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数错误。"));
            }

            var file = Server.MapPath(string.Format("~{0}", url));
            var sqlFile = "";
            var dt = Excel.ExcelToDataTable(file, sqlFile);
            var users = new List<T_User>();
            var loginUser = ApplicationSignInManager.GetLoginUser();

            //加入用户
            var siteId = App.Site.Id;
            var classes = classService.GetList("IsDeleted='{0}' and SiteId='{1}'".Format2(IsDelete.undeleted, siteId));
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString().Trim())) break;

                try
                {
                    var user = new T_User
                    {
                        Account = dt.Rows[i][0].ToString().Trim(),
                        Status = dt.Rows[i][1].ToString().Trim(),
                        RealName = dt.Rows[i][2].ToString().Trim(),
                        PassWord = string.IsNullOrEmpty(dt.Rows[i][3].ToString().Trim()) ? "123456" : dt.Rows[i][3].ToString().Trim(),
                        RoleId = 137,
                        Email = dt.Rows[i][4].ToString().Trim(),
                        Phone = dt.Rows[i][5].ToString().Trim(),
                        Telephone = dt.Rows[i][6].ToString().Trim(),
                        IDNumber = dt.Rows[i][7].ToString().Trim(),
                        Age = dt.Rows[i][8].ToInt(),
                        Province = dt.Rows[i][9].ToString().Trim(),
                        City = dt.Rows[i][10].ToString().Trim(),
                        Sex = dt.Rows[i][11].ToString().Trim(),
                        ClassIds = classes.FirstOrDefault(c => c.Name == dt.Rows[i][12].ToString().Trim())?.Id.ToString(),
                        SiteId = siteId,
                        CreateIp = HttpHelper.GetIP(),
                        CreateBy = Convert.ToInt32(loginUser.Id),
                        CreateTime = DateTime.Now,
                        IsDeleted = IsDelete.undeleted.ToString(),
                    };
                    if (string.IsNullOrEmpty(user.ClassIds))
                    {
                        return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据格式有误，班级填写错误", i + 1)));
                    }

                    user.PassWord = Md5Encrypt.Md5EncryptPassword(user.PassWord);
                    users.Add(user);
                }
                catch (Exception e)
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据格式有误！错误信息：{1}", i, e.Message)));
                }
            }

            //模型验证
            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                //字段验证
                if (!this.ModelValidate(users[i]).IsValid)
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据格式有误，错误信息：{1}", i + 1,
                                this.ModelValidate(users[i]).FirstError.ErrorMessage)));
                }

                //账号重复性验证
                if (users.FindAll(u => u.Account == users[i].Account && u.SiteId == users[i].SiteId).Count > 1)
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的账号“{1}”在Excel中已经出现过！", i + 1, users[i].Account)));
                }
                if (userService.IsExist("Account='{0}' and SiteId='{1}'".Format2(users[i].Account, users[i].SiteId)))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的账号“{1}”已经存在！", i + 1, users[i].Account)));
                }

                //身份证重复性验证
                if (!string.IsNullOrEmpty(users[i].IDNumber) && users.FindAll(u => u.IDNumber == users[i].IDNumber && u.SiteId == users[i].SiteId).Count > 1)
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的身份证“{1}”在Excel中已经出现过！", i + 1, users[i].IDNumber)));
                }
                if (!string.IsNullOrEmpty(users[i].IDNumber) && userService.IsExist("IDNumber='{0}' and SiteId='{1}'".Format2(users[i].IDNumber, users[i].SiteId)))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的身份证“{1}”已经存在！", i + 1, users[i].IDNumber)));
                }

                //手机重复性验证
                if (!string.IsNullOrEmpty(users[i].Telephone) && users.FindAll(u => u.Telephone == users[i].Telephone && u.SiteId == users[i].SiteId).Count > 1)
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的手机号码“{1}”在Excel中已经出现过！", i + 1, users[i].Telephone)));
                }
                if (!string.IsNullOrEmpty(users[i].Telephone) && userService.IsExist("Telephone='{0}' and SiteId='{1}'".Format2(users[i].Telephone, users[i].SiteId)))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的手机号码“{1}”已经存在！", i + 1, users[i].Telephone)));
                }

                //邮箱重复性验证
                if (!string.IsNullOrEmpty(users[i].Email) && users.FindAll(u => u.Email == users[i].Email && u.SiteId == users[i].SiteId).Count > 1)
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的邮箱“{1}”在Excel中已经出现过！", i + 1, users[i].Email)));
                }
                if (!string.IsNullOrEmpty(users[i].Email) && userService.IsExist("Email='{0}' and SiteId='{1}'".Format2(users[i].Email, users[i].SiteId)))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的邮箱“{1}”已经存在！", i + 1, users[i].Email)));
                }

            }
            //执行事务
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                users.ForEach(trans.Add);
                trans.SaveChange();
            }
            return Json(DialogFactory.Create(DialogType.Success, "导入数据成功！"));
        }

        #endregion

        #region 学生

        /// <summary>
        /// 分页
        /// </summary>
        public ActionResult StudentPager()
        {
            return View();
        }

        /// <summary>
        /// 创建
        /// </summary>
        public ActionResult StudentCreate()
        {
            return PartialView();
        }

        /// <summary>
        /// 创建post
        /// </summary>
        [HttpPost]
        public ActionResult StudentCreate(StudentCreateModel model)
        {
            //模型数据安全验证
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }
            var user = ObjectMapper.Mapper<StudentCreateModel, T_User>(model);
            //密码不能为******
            if (user.PassWord.Equals("******"))
            {
                return Json(DialogFactory.Create(DialogType.Error, "错误", "密码串非法"));
            }
            user.PassWord = Md5Encrypt.Md5EncryptPassword(model.PassWord.Trim()).ToString();
            user.CreateTime = DateTime.Now;
            user.IsDeleted = IsDelete.undeleted.ToString();

            //ip格式验证
            var error = string.Empty;
            if (!IpsHelper.IsRightIps(model.Ips, out error))
            {
                return Json(DialogFactory.Create(DialogType.Error, error));
            }

            //ip冲突验证
            var ipUsers = userService.GetList("Ips is not null and Ips != '' and SiteId = '{0}'".Format2(model.SiteId));
            foreach (var u in ipUsers)
            {
                if (IpsHelper.IpsConflict(u.Ips, model.Ips))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("Ip段与用户“{0}”冲突，请修改！", u.Account)));
                }
            }

            //身份证重复验证
            if (userService.IsExist("IDNumber='{0}' and RoleId = '{1}' and SiteId = '{2}'".Format2(model.IDNumber, model.RoleId, model.SiteId)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "该身份证已被注册，请更换！"));
            }

            //手机重复验证
            if (userService.IsExist("Telephone='{0}' and RoleId = '{1}' and SiteId = '{2}'".Format2(model.Telephone, model.RoleId, model.SiteId)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "该手机已被注册，请更换！"));
            }

            //事务添加用户
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                trans.Add(user);
                var role = roleService.GetEntiy(string.Format(" Id = {0}", user.RoleId));
                trans.SaveChange();
            }

            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "创建成功！"));
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public ActionResult StudentEdit()
        {
            return PartialView();
        }

        /// <summary>
        /// 编辑post
        /// </summary>
        [HttpPost]
        public ActionResult StudentEdit(StudentEditModel model)
        {
            var user = userService.GetEntiy(model.Id);

            model.Account = user.Account;
            if (model.PassWord.Equals("******"))
                model.PassWord = user.PassWord;
            else
                model.PassWord = Md5Encrypt.Md5EncryptPassword(model.PassWord.Trim()).ToString();

            //ip格式验证
            var error = string.Empty;
            if (!IpsHelper.IsRightIps(model.Ips, out error))
            {
                return Json(DialogFactory.Create(DialogType.Error, error));
            }

            //ip冲突验证
            var ipUsers = userService.GetList("Ips is not null and Ips != '' and Id != '{0}' and SiteId = '{1}'".Format2(model.Id, model.SiteId));
            foreach (var u in ipUsers)
            {
                if (IpsHelper.IpsConflict(u.Ips, model.Ips))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("Ip段与用户“{0}”冲突，请修改！", u.Account)));
                }
            }

            //身份证重复验证
            if (userService.IsExist("IDNumber='{0}' and RoleId = '{1}' and Id != '{2}'  and SiteId = '{3}'".Format2(model.IDNumber, model.RoleId, model.Id, model.SiteId)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "该身份证已被注册，请更换！"));
            }

            //手机重复验证
            if (userService.IsExist("Telephone='{0}' and RoleId = '{1}' and Id != '{2}'  and SiteId = '{3}'".Format2(model.Telephone, model.RoleId, model.Id, model.SiteId)))
            {
                return Json(DialogFactory.Create(DialogType.Error, string.Empty, "该手机已被注册，请更换！"));
            }

            return Json(EditIgnore("id = {0}".Format2(model.Id), model, "PassWord", "SiteId", "CompanyId", "CreateTime", "IsDeleted"));
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        public ActionResult StudentImportExcel()
        {
            return PartialView();
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        [HttpPost]
        public ActionResult StudentImportExcel(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数错误。"));
            }

            var file = Server.MapPath(string.Format("~{0}", url));
            var sqlFile = "";
            var dt = Excel.ExcelToDataTable(file, sqlFile);
            var users = new List<T_User>();
            var loginUser = ApplicationSignInManager.GetLoginUser();

            //加入用户
            var siteId = App.Site.Id;
            var classes = classService.GetList("IsDeleted='{0}' and SiteId='{1}'".Format2(IsDelete.undeleted, siteId));
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString().Trim())) break;

                try
                {
                    var user = new T_User
                    {
                        Account = dt.Rows[i][0].ToString().Trim(),
                        Status = dt.Rows[i][1].ToString().Trim(),
                        RealName = dt.Rows[i][2].ToString().Trim(),
                        PassWord = string.IsNullOrEmpty(dt.Rows[i][3].ToString().Trim()) ? "123456" : dt.Rows[i][3].ToString().Trim(),
                        RoleId = 138,
                        Email = dt.Rows[i][4].ToString().Trim(),
                        Phone = dt.Rows[i][5].ToString().Trim(),
                        Telephone = dt.Rows[i][6].ToString().Trim(),
                        IDNumber = dt.Rows[i][7].ToString().Trim(),
                        Age = dt.Rows[i][8].ToInt(),
                        Province = dt.Rows[i][9].ToString().Trim(),
                        City = dt.Rows[i][10].ToString().Trim(),
                        Sex = dt.Rows[i][11].ToString().Trim(),
                        ClassIds = classes.FirstOrDefault(c => c.Name == dt.Rows[i][12].ToString().Trim())?.Id.ToString(),
                        SiteId = siteId,
                        CreateIp = HttpHelper.GetIP(),
                        CreateBy = Convert.ToInt32(loginUser.Id),
                        CreateTime = DateTime.Now,
                        IsDeleted = IsDelete.undeleted.ToString(),
                    };
                    if (string.IsNullOrEmpty(user.ClassIds))
                    {
                        return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据格式有误，班级填写错误", i + 1)));
                    }
                    if(loginUser.LoginUser.RoleId == 137 && !loginUser.LoginUser.ClassIds.Split(',').ToList().Contains(user.ClassIds))
                    {
                        return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据格式有误，你不是该班级的教师", i + 1)));
                    }

                    user.PassWord = Md5Encrypt.Md5EncryptPassword(user.PassWord);
                    users.Add(user);
                }
                catch (Exception e)
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据格式有误！错误信息：{1}", i, e.Message)));
                }
            }

            //模型验证
            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                //字段验证
                if (!this.ModelValidate(users[i]).IsValid)
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据格式有误，错误信息：{1}", i + 1,
                                this.ModelValidate(users[i]).FirstError.ErrorMessage)));
                }

                //账号重复性验证
                if (users.FindAll(u => u.Account == users[i].Account && u.SiteId == users[i].SiteId).Count > 1)
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的账号“{1}”在Excel中已经出现过！", i + 1, users[i].Account)));
                }
                if (userService.IsExist("Account='{0}' and SiteId='{1}'".Format2(users[i].Account, users[i].SiteId)))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的账号“{1}”已经存在！", i + 1, users[i].Account)));
                }

                //身份证重复性验证
                if (!string.IsNullOrEmpty(users[i].IDNumber) && users.FindAll(u => u.IDNumber == users[i].IDNumber && u.SiteId == users[i].SiteId).Count > 1)
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的身份证“{1}”在Excel中已经出现过！", i + 1, users[i].IDNumber)));
                }
                if (!string.IsNullOrEmpty(users[i].IDNumber) && userService.IsExist("IDNumber='{0}' and SiteId='{1}'".Format2(users[i].IDNumber, users[i].SiteId)))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的身份证“{1}”已经存在！", i + 1, users[i].IDNumber)));
                }

                //手机重复性验证
                if (!string.IsNullOrEmpty(users[i].Telephone) && users.FindAll(u => u.Telephone == users[i].Telephone && u.SiteId == users[i].SiteId).Count > 1)
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的手机号码“{1}”在Excel中已经出现过！", i + 1, users[i].Telephone)));
                }
                if (!string.IsNullOrEmpty(users[i].Telephone) && userService.IsExist("Telephone='{0}' and SiteId='{1}'".Format2(users[i].Telephone, users[i].SiteId)))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的手机号码“{1}”已经存在！", i + 1, users[i].Telephone)));
                }

                //邮箱重复性验证
                if (!string.IsNullOrEmpty(users[i].Email) && users.FindAll(u => u.Email == users[i].Email && u.SiteId == users[i].SiteId).Count > 1)
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的邮箱“{1}”在Excel中已经出现过！", i + 1, users[i].Email)));
                }
                if (!string.IsNullOrEmpty(users[i].Email) && userService.IsExist("Email='{0}' and SiteId='{1}'".Format2(users[i].Email, users[i].SiteId)))
                {
                    return Json(DialogFactory.Create(DialogType.Error, string.Format("第{0}行数据的邮箱“{1}”已经存在！", i + 1, users[i].Email)));
                }

            }
            //执行事务
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                users.ForEach(trans.Add);
                trans.SaveChange();
            }
            return Json(DialogFactory.Create(DialogType.Success, "导入数据成功！"));
        }

        #endregion
    }
}