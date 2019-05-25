using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Master.Domain;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Master.Module
{
    /// <summary>
    /// 基于模块的领域基类，继承基于实体的领域基类，并封装模块数据获取、过滤及表单提交
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class ModuleServiceBase<TEntity, TPrimaryKey> : DomainServiceBase<TEntity, TPrimaryKey>, IDataModule<TEntity, TPrimaryKey>
        where TEntity : class, IHaveProperty, IEntity<TPrimaryKey>, new()
    {
        //public IModuleInfoManager ModuleManager { get; set; }
        //public IRepository<ModuleInfo, int> ModuleRepository { get; set; }

        //public ModuleServiceBase(IModuleInfoManager moduleManager,
        //    IRepository<ModuleInfo, int> moduleRepository)
        //{
        //    _moduleManager = moduleManager;
        //    _moduleRepository = moduleRepository;
        //}
        //public virtual  IQueryable<TEntity> GetInterModuleData(string baseSql="")
        //{
        //    //var moduleInfo = await _moduleManager.GetModuleInfoWithColumnsAsync(typeof(TEntity).Name);
        //    //var moduleType = await GetModelTypeAsync(moduleKey);
        //    //var queryType = typeof(IQueryable<>).MakeGenericType(moduleType);
        //    //var context = _moduleInfoRepository.GetDbContext();
        //    //var qry = context.GetType().GetTypeInfo().GetMethod("Set").MakeGenericMethod(moduleType).Invoke(context, null);

        //    //var fromSqlMethod = typeof(RelationalQueryableExtensions).GetTypeInfo().GetDeclaredMethods("FromSql")
        //    //.Single(mi => mi.GetParameters().Length == 3).MakeGenericMethod(moduleType);

        //    //var baseqry = fromSqlMethod.Invoke(null, new object[] { qry, BuildFilterSql(moduleInfo), new object[] { } }) as IQueryable;
        //    //var qry = GetDefaultQueryable();

        //    //if (!baseSql.ToString().IsNullOrWhiteSpace())
        //    //{
        //    //    qry = qry.FromSql(baseSql);
        //    //}
        //    //return qry;
        //}
        #region 过滤器
        public virtual IQueryable<TEntity> GetFilteredQuery(string moduleKey = "")
        {

            //创建人与修改人默认包含
            var qry = GetAll().Include("CreatorUser").Include("LastModifierUser").AsQueryable();

            //todo:模块的全局过滤
            //var moduleInfo = (Resolve<IModuleInfoManager>().GetModuleInfo(moduleKey.IsNullOrWhiteSpace() ? typeof(TEntity).Name : moduleKey)).GetAwaiter().GetResult();

            //var filter = new QueryBuilder<TEntity>(qry);
            var filter = qry;
            return filter;
        }

        //public virtual IQueryable<TEntity> GetUnFilterdQuery()
        //{
        //    //QueryBuilder<TEntity> qry = new QueryBuilder<TEntity>(Repository.GetAll().IgnoreQueryFilters());
        //    var qry = GetAll().IgnoreQueryFilters();
        //    return qry;
        //}
        #endregion

        #region 数据扁平化
        public virtual async Task<IEnumerable<IDictionary<string, object>>> GetModulePlainList(string moduleKey, string whereCondition)
        {
            var moduleInfoManager = Resolve<IModuleInfoManager>();

            var moduleInfo = await moduleInfoManager.GetModuleInfo(moduleKey);
            var query = GetFilteredQuery(moduleKey).Where(whereCondition);

            return await moduleInfoManager.GetModuleDataListAsync(moduleInfo, query);
        }

        public virtual async Task<IEnumerable<IDictionary<string, object>>> GetModulePlainList(string moduleKey, Expression<Func<TEntity, bool>> whereCondition)
        {
            var moduleInfoManager = Resolve<IModuleInfoManager>();

            var moduleInfo = await moduleInfoManager.GetModuleInfo(moduleKey);
            var query = GetFilteredQuery(moduleKey).Where(whereCondition);

            return await moduleInfoManager.GetModuleDataListAsync(moduleInfo, query);
        }
        #endregion

        #region 表单增加修改提交通用实现

        /// <summary>
        /// 读取自定义属性
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="entity"></param>
        /// <param name="Datas"></param>
        public virtual Task FillEntityPropertyFromDatas(ModuleInfo moduleInfo, TEntity entity, IDictionary<string, string> Datas)
        {
            return Task.Run(() =>
            {
                //读取属性
                foreach (var column in moduleInfo.ColumnInfos.Where(o => o.IsPropertyColumn))
                {
                    if (Datas.ContainsKey(column.ColumnKey))
                    {
                        //需要根据不同列类型，进行类型换
                        //entity.SetPropertyValue(column.ColumnKey, Datas[column.ColumnKey]);
                        entity.SetPropertyValue(column.ColumnKey, column.GetStrongTypeValue(Datas));
                    }
                }
            });


        }
        /// <summary>
        /// 进行自定义控件数据的赋值
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="entity"></param>
        /// <param name="Datas"></param>
        public virtual async Task FillEntityCustomizeDatas(ModuleInfo moduleInfo, TEntity entity, IDictionary<string, string> Datas)
        {
            foreach (var column in moduleInfo.ColumnInfos.Where(o => o.ColumnType == ColumnTypes.Customize))
            {
                if (Datas.ContainsKey(column.ColumnKey))
                {
                    //获取列对应的控件
                    var control = column.GetCustomizeControl();
                    var context = new ColumnWriteContext() { Datas = Datas, Entity = entity, ColumnInfo = column };
                    //写入数据
                    await control.Write(context);
                }
            }
        }
        /// <summary>
        /// 根据提交数据建立模块的实体数据
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="Datas"></param>
        /// <returns></returns>
        public virtual async Task<object> DoAdd(ModuleInfo moduleInfo, IDictionary<string, string> Datas)
        {
            //
            var entity = await DoAdd(Datas);

            //如果是实际IMayHaveTenant接口的且当前登录方为账套，则写入账套信息
            if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)) && AbpSession.TenantId.HasValue)
            {
                (entity as IMayHaveTenant).TenantId = AbpSession.TenantId.Value;
            }

            //如果是自定义模块提交，则需要建立模块外键关联
            if (!moduleInfo.IsInterModule)
            {
                (entity as ModuleData).ModuleInfoId = moduleInfo.Id;
            }

            //读取属性
            await FillEntityPropertyFromDatas(moduleInfo, entity, Datas);                        

            //保存
            var newId = await InsertAndGetIdAsync(entity);

            Datas["Id"] = newId.ToString();
            await CurrentUnitOfWork.SaveChangesAsync();

            //自定义控件赋值
            await FillEntityCustomizeDatas(moduleInfo, entity, Datas);

            return entity;
        }
        /// <summary>
        /// 根据提交数据修改模块实体
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="Datas"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<object> DoEdit(ModuleInfo moduleInfo, IDictionary<string, string> Datas, object id)
        {
            //var entityType = typeof(TEntity);
            ////获取旧实体
            //var query = Repository.GetAll();
            //if (entityType.HasPropertis())
            //{
            //    query = query.Include("Properties");
            //}
            //TEntity entity = await query.Where(o => o.Id == id).SingleAsync();
            var key=(TPrimaryKey)Convert.ChangeType(id, typeof(TPrimaryKey));
            //将数据读入实体
            var entity = await DoEdit(Datas, key);

            //读取属性
            await FillEntityPropertyFromDatas(moduleInfo, entity, Datas);

            await UpdateAsync(entity);

            //保存
            await CurrentUnitOfWork.SaveChangesAsync();

            //自定义控件赋值
            await FillEntityCustomizeDatas(moduleInfo, entity, Datas);

            return entity;
        }
        public virtual async Task<object> DoMultiEdit(ModuleInfo moduleInfo, IDictionary<string, string> Datas, IEnumerable<object> ids)
        {
            var entityType = typeof(TEntity);
            //获取旧实体
            var query = Repository.GetAll();

            var entities = await query.Where(o => ids.Contains(o.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                //新的实体
                await LoadEntityFromDatas(Datas, entity);
                //读取属性
                await FillEntityPropertyFromDatas(moduleInfo, entity, Datas);
            }

            //保存
            await CurrentUnitOfWork.SaveChangesAsync();

            foreach (var entity in entities)
            {
                //自定义控件赋值
                await FillEntityCustomizeDatas(moduleInfo, entity, Datas);
            }

            return entities;
        }
        #endregion
    }
}