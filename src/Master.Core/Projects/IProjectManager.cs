using Master.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.Projects
{
    public interface IProjectManager: IDataModule<Project,int>
    {
        /// <summary>
        /// 通过项目编号查找项目，若不存在，则新项目
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        Task<Project> GetByProjectSNOrInsert(string projectSN);
    }
}
