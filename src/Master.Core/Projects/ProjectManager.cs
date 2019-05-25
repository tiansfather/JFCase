using Abp.UI;
using Master.Domain;
using Master.Module;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Projects
{
    public class ProjectManager : ModuleServiceBase<Project, int>, IProjectManager
    {
        public async override Task ValidateEntity(Project entity)
        {
            if (string.IsNullOrEmpty(entity.ProjectSN))
            {
                throw new UserFriendlyException(L("编号不能为空"));
            }

            //不允许相同编号存在
            if ( entity.Id > 0 && await Repository.CountAsync(o =>o.ProjectSN == entity.ProjectSN && o.Id != entity.Id) > 0)
            {
                throw new UserFriendlyException(L("相同编号已存在"));
            }
            if (entity.Id == 0 && await Repository.CountAsync(o => o.ProjectSN == entity.ProjectSN) > 0)
            {
                throw new UserFriendlyException(L("相同编号已存在"));
            }
            await base.ValidateEntity(entity);
        }
        /// <summary>
        /// 通过项目编号查找项目，若不存在，则新项目
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public virtual async Task<Project> GetByProjectSNOrInsert(string projectSN)
        {
            var project = await Repository.GetAll().Where(o => o.ProjectSN == projectSN).FirstOrDefaultAsync();
            if (project == null)
            {
                project = new Project()
                {
                    ProjectSN = projectSN,
                };
                await Repository.InsertAsync(project);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            return project;
        }
    }
}
