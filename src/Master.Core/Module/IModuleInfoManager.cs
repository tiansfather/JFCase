using Abp.Authorization;
using Master.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Master.Module
{
    public interface IModuleInfoManager : IData<ModuleInfo, int>
    {
        /// <summary>
        /// 获取一个实体的管理类
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        DomainServiceBase GetManager(Type entityType);
        // <summary>
        /// 通过模块key获取模块实体，包含模块列信息
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <returns></returns>
        Task<ModuleInfo> GetModuleInfo(string moduleKey);
        /// <summary>
        /// 通过权限名获取对应的按钮信息
        /// </summary>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        Task<ModuleButton> FindButtonByPermissionName(string permissionName);
        void RemoveModuleInfoCache(string moduleKey, int tenantId);
        Task<IEnumerable<IDictionary<string, object>>> GetModuleDataListAsync(ModuleInfo moduleInfo, IQueryable query);
        Task<IEnumerable<IDictionary<string, object>>> GetModuleDataListAsync(ModuleInfo moduleInfo, string whereCondition = "", string orderBy = "");
        /// <summary>
        /// 填充模块实体的扁平化数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="moduleInfo"></param>
        /// <param name="columnFilterExpression"></param>
        /// <returns></returns>
        Task<IEnumerable<IDictionary<string, object>>> FillModuleDataListAsync<TEntity>(IEnumerable<TEntity> entities, ModuleInfo moduleInfo, Expression<Func<ColumnInfo, bool>> columnFilterExpression = null) where TEntity : class, new();
        /// <summary>
        /// 获取模块的基本查询
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        IQueryable GetQuery(ModuleInfo moduleInfo);
        /// <summary>
        /// 通用表单添加提交
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="Datas"></param>
        /// <returns></returns>
        Task ManageFormAdd(ModuleInfo moduleInfo, IDictionary<string, string> Datas);
        /// <summary>
        /// 通用表单修改提交
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="Datas"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task ManageFormEdit(ModuleInfo moduleInfo, IDictionary<string, string> Datas, int id);
        /// <summary>
        /// 通用表单批量修改
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="Datas"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task ManageFormMultiEdit(ModuleInfo moduleInfo, IDictionary<string, string> Datas, IEnumerable<int> ids);
        /// <summary>
        /// 设置模块的列定义
        /// </summary>
        /// <param name="columnInfos"></param>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        Task UpdateColumns(IList<ColumnInfo> columnInfos, ModuleInfo moduleInfo);
        /// <summary>
        /// 设置模块的按钮定义
        /// </summary>
        /// <param name="btns"></param>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        Task UpdateBtns(IList<ModuleButton> btns, ModuleInfo moduleInfo);
        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        Task AddModuleInfo(ModuleInfo moduleInfo);
        /// <summary>
        /// 获取所有的模块权限
        /// </summary>
        /// <returns></returns>
        IList<Permission> GetAllModulePermissions();
    }
}
