using Master.Projects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Web.Models;
using Abp.Authorization;
using Master.MES.Dtos;
using Abp.AutoMapper;
using Master.Storage.Domains;
using Master.Dto;
using Master.EntityFrameworkCore;

namespace Master.MES.Service
{
    [AbpAuthorize]
    public class BomAppService:MasterAppServiceBase<Part,int>
    {
        public IProjectManager ProjectManager { get; set; }
        public PartManager PartManager { get; set; }
        public override async Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            var pageResult = await GetPageResultQueryable(request);
         //   var data = (await pageResult.Queryable.ToListAsync()).ConvertAll(PageResultConverter);
       
            var tenants = await pageResult.Queryable.ToListAsync();

            var data = new List<object>();
            foreach (var q in tenants)
            {
                var pareDto = q.MapTo<PartDto>();
                var materialRequire = Resolve<MaterialRequireManager>().GetAll().Where(o => o.RequireSource == "BOM" && o.RequireSourceId == q.Id && o.ProjectId == q.ProjectId).FirstOrDefault();
                if (materialRequire != null)
                {
                    var status = "未采购";
                    if (materialRequire.IsUsed)
                    {
                        status = "已领用";
                    }
                    else if (materialRequire.IsReceived)
                    {
                        status = "已入库";
                    }
                    else if (materialRequire.IsBuyed)
                    {
                        status = "已采购";
                    }
                    else if (materialRequire.IsBuyed)
                    {
                        status = "已询价";
                    }
                pareDto.MaterialStatus = status;
                   
                }
                data.Add(pareDto);
            }
             var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.RowCount,
                data = data
            };

            return result;
        }

        /// <summary>
        /// BOM树数据接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        [DontWrapResult]
        public virtual async Task<string> GetBomTree(string id,string name,int level=-1)
        {
            
            object result=new object();
            
            if (level == -1)
            {
                //初始请求获取所有项目的年份
                result = await ProjectManager.GetAll().GroupBy(o => "Year"+o.CreationTime.Year).Select(o => new { id=o.Key,name=o.Key.Replace("Year",""),isParent=o.Count()>0}).ToListAsync();
            }
            else if (level == 0)
            {
                //按年份请求月份，此时id对应年份
                result = await ProjectManager.GetAll().Where(o => o.CreationTime.Year == int.Parse(id.Replace("Year", ""))).GroupBy(o => int.Parse(id.Replace("Year", "")) +"-"+ o.CreationTime.Month).Select(o => new { id = o.Key, name = o.Key, isParent = o.Count() > 0 }).ToListAsync();
            }
            else if (level == 1)
            {
                var datearr = id.Split('-');
                //按年份月份请求对应的项目信息，此时id对应年-月
                result = await ProjectManager.GetAll().Where(o => o.CreationTime.Year == int.Parse(datearr[0])&& o.CreationTime.Month== int.Parse(datearr[1])).Select(o => new { id ="Project"+ o.Id,projectId=o.Id, name = o.ProjectSN, isParent = true }).ToListAsync();
            }
            else if (level == 2)
            {
                //按项目编号去找此项目所有bom
                var parts = await PartManager.GetAll().Where(o => o.ProjectId ==int.Parse(id.Replace("Project",""))).OrderBy(o=>o.Sort).ToListAsync();
                result = parts.Select(o => new { id = o.Id, name = o.PartName,projectId=o.ProjectId, partSN = o.PartSN,parentId=o.ParentId==null?("Project"+o.ProjectId):o.ParentId.Value.ToString(), isParent = parts.Count(p => p.ParentId == o.Id) > 0 });
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
        /// <summary>
        /// 移动零件节点
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="targetNodeId"></param>
        /// <param name="moveType"></param>
        /// <returns></returns>
        public virtual async Task MoveTreeNode(int nodeId,int targetNodeId,string moveType)
        {
            var manager = Manager as PartManager;
            switch (moveType)
            {
                case "inner":
                    await manager.MoveInner(nodeId, targetNodeId);
                    break;
                case "prev":
                    await manager.MovePrev(nodeId, targetNodeId);
                    break;
                case "next":
                    await manager.MoveNext(nodeId, targetNodeId);
                    break;
            }
        }
        /// <summary>
        /// 获取bom信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<PartDto> GetBomInfo(int id)
        {
            var bom = await Manager.GetByIdAsync(id);
            var bomDto = bom.MapTo<PartDto>(); 
            //var materialRequire  = Resolve<MaterialRequireManager>().GetAll().Where(o => o.RequireSource == "BOM" && o.RequireSourceId == id && o.ProjectId==bom.ProjectId).FirstOrDefault();
            //if (materialRequire != null) {
            //    var status = "未采购";
            //    if (materialRequire.IsUsed)
            //    {
            //        status = "已领用";
            //    }
            //    else if (materialRequire.IsReceived){
            //        status = "已入库";
            //    }
            //    else if (materialRequire.IsBuyed){
            //        status = "已采购";
            //    }
            //    else if (materialRequire.IsBuyed)
            //    {
            //        status = "已询价";
            //    }
            //    bomDto.MaterialStatus = status;
            //}
            return bomDto;
        }

        /// <summary>
        /// 更新BOM
        /// </summary>
        /// <param name="partDto"></param>
        /// <returns></returns>
        public virtual async Task SubmitBomInfo(PartDto partDto)
        {
            Part bom = null;
            if (partDto.Id > 0)
            {
                bom =  await Manager.GetByIdAsync(partDto.Id);
                partDto.MapTo(bom);
            }
            else
            {
                var project = await ProjectManager.GetByIdAsync(partDto.ProjectId);
                bom = await PartManager.GenerateNewPart(project, partDto.PartName, partDto.PartSpecification, partDto.PartNum);

                bom.Material = partDto.Material;
                bom.MeasureMentUnit = partDto.MeasureMentUnit;
                bom.EnableBuy = partDto.EnableBuy;
                bom.EnableProcess = partDto.EnableProcess;
                bom.EnableStorage = partDto.EnableStorage;

                bom.MaterialCode = partDto.MaterialCode;
                bom.RequireDate = partDto.RequireDate;
            }
            
            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
