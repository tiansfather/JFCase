using Abp.Authorization;
using Abp.Domain.Uow;
using Master.Entity;
using Master.MES.Dtos;
using Master.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Master.MES.Service
{
    [AbpAuthorize]
    public class VideoAppService:MasterAppServiceBase<Resource,int>
    {
        /// <summary>
        /// 提交视频信息
        /// </summary>
        /// <param name="videoDtos"></param>
        /// <returns></returns>
        public virtual async Task SubmitVideos(List<VideoDto> videoDtos)
        {
            var manager = Manager as ResourceManager;
            //先删除已经不在提交过来的数据中存在的数据
            var deleteIds = await manager.GetAll()
                .Where(o => o.ResourceType == "Video")
                .Where(o => !videoDtos.Select(v => v.Id).Contains(o.Id))
                .Select(o => o.Id).ToListAsync();
            await manager.DeleteAsync(deleteIds);


            //遍历所有提交的数据
            foreach(var videoDto in videoDtos)
            {
                Resource resource = null;
                if (videoDto.Id == 0)
                {
                    resource = new Resource()
                    {

                    };
                }
                else
                {
                    resource = await manager.GetByIdAsync(videoDto.Id);
                }

                resource.ResourceName = videoDto.VideoName;
                resource.Sort = videoDto.Sort;
                resource.ResourceType = "Video";
                resource.Remarks = videoDto.Remarks;
                resource.ResourceContent = videoDto.VideoContent;
                resource.SetPropertyValue("VideoPath", videoDto.VideoPath);

                await manager.SaveAsync(resource);
            }
        }

        public override async Task<object> GetById(int primary)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var resource = await Manager.GetByIdFromCacheAsync(primary);
                return VideoDto.FromResource(resource);
            }
        }

        /// <summary>
        /// 返回所有视频信息
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<VideoDto>> GetAllList()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var resources = await Manager.GetAll().Where(o => o.ResourceType == "Video" && o.IsActive).OrderBy(o => o.Sort)
                .ToListAsync();

                return resources.Select(resource => VideoDto.FromResource(resource));
            }
            
        }

    }
}
