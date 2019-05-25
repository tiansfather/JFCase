using System;
using System.Collections.Generic;
using System.Text;

namespace Master.MES.Dtos
{
    public class CloudCompanyDto
    {
        public string CompanySN { get; set; }
        public string CompanyName { get; set; }
        public string Speciality { get; set; }
        public string Mobile { get; set; }
    }
    /// <summary>
    /// 云设备
    /// </summary>
    public class CloudEquipmentDto
    {
        public string ResSN { get; set; }
        public string CompanyName { get; set; }
        public string CompanySN { get; set; }
        public string EquipmentType { get; set; }
        public string EquipmentPic { get; set; }
        public string Brand { get; set; }
        public string Range { get; set; }
        public string BuyYear { get; set; }
        public decimal ProcessPrice { get; set; }
        public decimal Cost { get; set; }
        public int FreeDay { get; set; }
        public string Mobile { get; set; }
        public string Speciality { get; set; }
        public string Address { get; set; }
    }

    public class CloudDataDto<T>
    {
        public int Totle { get; set; }
        public int TotalPages { get; set; }
        public List<T> ObjList { get; set; }
    }

    public class CloudPageResultDto<T>
    {
        public int ErrCode { get; set; }
        public string Message { get; set; }
        public CloudDataDto<T> Data { get; set; }
    }
}
