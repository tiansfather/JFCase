using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Reflection;
using Master.Authentication;
using Master.Configuration;
using Master.Controllers;
using Master.EntityFrameworkCore;
using Master.Module;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace Master.Web.Controllers
{
    [AbpMvcAuthorize]
    public class UserController : MasterModuleControllerBase
    {
        public ITypeFinder TypeFinder { get; set; }
        public UserManager UserManager { get; set; }
        public RoleManager RoleManager { get; set; }
        public MasterConfiguration MasterConfiguration { get; set; }

        public ActionResult Test2()
        {
            var _valueExp = Expression.Constant("11111");
            var _constExp = Expression.Constant("$.TEST", typeof(string));
            var p=Expression.Parameter(typeof(User), "x");
            MemberExpression member = Expression.PropertyOrField(p, "Property");
            var expRes = Expression.Call( typeof(MasterDbContext).GetMethod("GetJsonValueString"),member, _constExp);
            var exp = Expression.Equal(expRes, _valueExp);
            var lamda=Expression.Lambda(expRes, p);

            var dlamda = DynamicExpressionParser.ParseLambda(typeof(User), typeof(bool), "@0(it)==\"11111\"", lamda);

            var c = UserManager.Repository.GetAll().Where("@0(it)", dlamda).Count();
            var c2 = UserManager.Repository.GetAll().Where(o => MasterDbContext.GetJsonValueString(o.Property, "$.TEST") == "11111").Count();
            return Content(c.ToString()+"-"+c2.ToString());
        }

        public IActionResult Test()
        {
            var types = TypeFinder.Find(o => o.FullName == "Master.WanTai.WanTaiProject");
            var type = types[0];
            AppDomain.CurrentDomain.Load(type.Assembly.GetName());
            var op = ScriptOptions.Default;
            op = op.WithImports(new string[] { "System", "System.Math", "Master.EntityFrameworkCore" });
            op = op.WithReferences(typeof(ModuleInfo).Assembly, typeof(MasterDbContext).Assembly);
            var func = ScriptRunner.EvaluateScript<int>("Id", op,Activator.CreateInstance(type)).Result;

            return Content(func.ToString());
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Account(string data)
        {
            var roles = await RoleManager.GetAll().ToListAsync();//获取所有角色;
            var user = await UserManager.GetByIdAsync(Convert.ToInt32(data));

            var userRoles = new List<string>();
            userRoles = (await UserManager.GetRolesAsync(user)).Select(o=>o.Name).ToList();

            var statusDefinitions = new List<StatusDefinition>();
            if (MasterConfiguration.EntityStatusDefinitions.ContainsKey(typeof(User)))
            {
                statusDefinitions = MasterConfiguration.EntityStatusDefinitions[typeof(User)];
            }

            ViewData["userroles"] = userRoles;
            ViewData["roles"] = roles;
            ViewData["data"] = data;
            ViewData["statusDefinitions"] = statusDefinitions;
            return View(user);
        }
        [AbpMvcAuthorize("Module.User.Button.Dimission")]
        public async Task<IActionResult> OffJob(string data)
        {
            var ids = data.Split(',').ToList().ConvertAll(o => long.Parse(o));
            var staffNames = await UserManager.GetAll().Where(o => ids.Contains(o.Id)).Select(o => o.Name).ToListAsync();

            ViewData["data"] = data;
            return View(staffNames);
        }
    }
}