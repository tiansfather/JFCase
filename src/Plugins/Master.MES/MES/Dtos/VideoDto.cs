using Master.Entity;
using Master.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    /// <summary>
    /// 视频
    /// </summary>
    public class VideoDto
    {
        public int Id { get; set; }
        public string VideoName { get; set; }
        public string VideoContent { get; set; }
        public string VideoPath { get; set; }
        public string Remarks { get; set; }
        public int Sort { get; set; }

        public Resource ToResource()
        {
            var resource = new Resource();
            resource.ResourceName = VideoName;
            resource.Sort = Sort;
            resource.ResourceType = "Video";
            resource.Remarks = Remarks;
            resource.ResourceContent = VideoContent;
            resource.SetPropertyValue("VideoPath", VideoPath);

            return resource;
        }

        public static VideoDto FromResource(Resource resource)
        {
            return new VideoDto()
            {
                Id = resource.Id,
                VideoName = resource.ResourceName,
                VideoPath = resource.GetPropertyValue<string>("VideoPath"),
                VideoContent = resource.ResourceContent,
                Sort = resource.Sort,
                Remarks = resource.Remarks
            };
        }
    }
}
