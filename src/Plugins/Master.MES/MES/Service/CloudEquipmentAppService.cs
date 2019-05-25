using Abp.Web.Models;
using Master.Dto;
using Master.MES.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.MES.Service
{

    public class CloudEquipmentAppService: MasterAppServiceBase
    {
        /// <summary>
        /// 云设备分页返回数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [DontWrapResult]
        public async virtual Task<ResultPageDto> GetPageResult(RequestPageDto request)
        {
            if (request.Page <= 0)
            {
                request.Page = 1;
            }
            if (request.Limit <= 0)
            {
                request.Limit = 20;
            }
            //接口地址
            string apiUrl = $"http://mes.imould.me/Ajax/ajaxapi.ashx?action=GetYunEquipmentList&page={request.Page}&pagesize={request.Limit}{request.Where}";

            Logger.Info(apiUrl);

            var pageResult=await Senparc.CO2NET.HttpUtility.Get.GetJsonAsync<CloudPageResultDto<CloudEquipmentDto>>(apiUrl);

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.Data.Totle,
                data = pageResult.Data.ObjList
            };

            return result;
        }
        /// <summary>
        /// 通过关键字查询有空的云设备
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [DontWrapResult]
        public async virtual Task<ResultPageDto> GetFreeEquipmentByKey(string keyword,int page=1,int limit=50)
        {
            //接口地址
            string apiUrl = $"http://mes.imould.me/Ajax/ajaxapi.ashx?action=GetYunEquipmentList&page={page}&pagesize={limit}&freetype=0&speciality="+ keyword;

            var pageResult = await Senparc.CO2NET.HttpUtility.Get.GetJsonAsync<CloudPageResultDto<CloudEquipmentDto>>(apiUrl);

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.Data.Totle,
                data = pageResult.Data.ObjList
            };

            return result;
        }
        /// <summary>
        /// 通过关键字查询云加工点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [DontWrapResult]
        public async virtual Task<ResultPageDto> GetCompanyByKey(string keyword, int page = 1, int limit = 50)
        {
            //接口地址
            string apiUrl = $"http://mes.imould.me/Ajax/ajaxapi.ashx?action=GetYunCompanyList&page={page}&pagesize={limit}&companyName=" + keyword;

            var pageResult = await Senparc.CO2NET.HttpUtility.Get.GetJsonAsync<CloudPageResultDto<CloudCompanyDto>>(apiUrl);

            var result = new ResultPageDto()
            {
                code = 0,
                count = pageResult.Data.Totle,
                data = pageResult.Data.ObjList
            };

            return result;
        }
        /// <summary>
        /// 通过公司名称获取云加工点
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public async  virtual Task<CloudEquipmentDto> FindByCompanyName(string companyName)
        {
            //接口地址
            string apiUrl = $"http://mes.imould.me/Ajax/ajaxapi.ashx?action=GetYunEquipmentList&page=1&pagesize=10&companyName=" + companyName;

            var pageResult = await Senparc.CO2NET.HttpUtility.Get.GetJsonAsync<CloudPageResultDto<CloudEquipmentDto>>(apiUrl);

            return pageResult.Data.ObjList.FirstOrDefault();
        }
    }
}
