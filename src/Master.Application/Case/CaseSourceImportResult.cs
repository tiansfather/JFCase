using Abp.Dependency;
using Master.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Case
{
    /// <summary>
    /// 案源导入结果
    /// </summary>
    public class CaseSourceImportResult
    {
        public string City { get; set; }
        public string Court1 { get; set; }
        public string Court2 { get; set; }
        public string SourceSN { get; set; }
        public string AnYou { get; set; }
        public string Lawyer { get; set; }
        public string TrialPeople { get; set; }
        public string ValidDate { get; set; }
        public string SourceFile { get; set; }
        public CaseSourceUpdateDto CaseSourceUpdateDto { get; set; }
        public string ErrMsg { get; set; }
        public bool Exist { get; set; }
        public bool Valid { get; set; }
        public async Task GenerateCaseSource()
        {
            using (var baseTreeManagerWrapper=IocManager.Instance.ResolveAsDisposable<BaseTreeManager>())
            {
                var baseTreeManager = baseTreeManagerWrapper.Object;
                this.CaseSourceUpdateDto = new CaseSourceUpdateDto()
                {
                    SourceSN = this.SourceSN
                };
                using (var caseSourceManagerWrapper = IocManager.Instance.ResolveAsDisposable<CaseSourceManager>())
                {
                    var caseSource = await caseSourceManagerWrapper.Object.GetAll().Where(o => o.SourceSN == this.SourceSN).FirstOrDefaultAsync();
                    if (caseSource!=null)
                    {
                        this.Exist = true;
                        CaseSourceUpdateDto.Id = caseSource.Id;
                    }
                }
                var allBaseTrees=await baseTreeManager.GetAllList();

                var allAnYous = await baseTreeManager.GetTypeNodesByKnowledgeName("案由");
                var allCities= await baseTreeManager.GetTypeNodesByKnowledgeName("城市");
                var allCourts=await baseTreeManager.GetTypeNodesByParentKnowledgeName("城市");
                

                #region 城市案由法院的关联验证
                var node_city = allCities.FirstOrDefault(o => o.DisplayName == City);
                if (node_city == null)
                {
                    throw new Exception("城市不在分类中存在");
                }
                this.CaseSourceUpdateDto.CityId = node_city.Id;
                var node_anyou = allAnYous.FirstOrDefault(o => o.DisplayName == AnYou);
                if (node_anyou == null)
                {
                    throw new Exception("案由不在分类中存在");
                }
                this.CaseSourceUpdateDto.AnYouId = node_anyou.Id;
                if (!string.IsNullOrEmpty(Court1))
                {
                    var node_court1 = allCourts.FirstOrDefault(o => o.DisplayName == Court1);
                    if (node_court1 == null)
                    {
                        throw new Exception("一审法院不在分类中存在");
                    }
                    this.CaseSourceUpdateDto.Court1Id = node_court1.Id;
                }
                if (!string.IsNullOrEmpty(Court2))
                {
                    var node_court2 = allCourts.FirstOrDefault(o => o.DisplayName == Court2);
                    if (node_court2 == null)
                    {
                        throw new Exception("二审法院不在分类中存在");
                    }
                    this.CaseSourceUpdateDto.Court2Id = node_court2.Id;
                } 
                #endregion

                this.CaseSourceUpdateDto.ValidDate = Convert.ToDateTime(this.ValidDate);

                #region 解析代理律师
                var lawyerFirms = this.Lawyer.Split('\n').Where(o=>!string.IsNullOrEmpty(o)).Select(o=> {
                    var lawyerFirm = new LawyerFirm();
                    if (o.IndexOf('：') > 0)
                    {
                        lawyerFirm.FirmName = o.Split('：')[1];
                        lawyerFirm.lawyer = o.Split('：')[0];
                        //var lawyers = o.Split('：')[0].Split('、');
                        //lawyFirm.lawyer1 = lawyers[0];
                        //if (lawyers.Length > 1)
                        //{
                        //    lawyFirm.lawyer2 = lawyers[1];
                        //}
                        //if (lawyers.Length > 2)
                        //{
                        //    lawyFirm.lawyer3 = lawyers[2];
                        //}
                    }
                    else
                    {
                        lawyerFirm.FirmName = o;
                    }
                    return lawyerFirm;
                });
                this.CaseSourceUpdateDto.LawyerFirms = lawyerFirms.ToList();
                #endregion

                #region 解析审判组织
                var trialPeople = this.TrialPeople.Split('\n').Where(o=>!string.IsNullOrEmpty(o)).Select(o => {
                    var person = new TrialPerson();
                    if (o.IndexOf("审判长") >= 0)
                    {
                        person.TrialRole = TrialRole.审判长;
                        person.Name = o.Replace("审判长", "");
                    }
                    if (o.IndexOf("审判员") >= 0)
                    {
                        person.TrialRole = TrialRole.审判员;
                        person.Name = o.Replace("审判员", "");
                    }
                    if (o.IndexOf("书记员") >= 0)
                    {
                        person.TrialRole = TrialRole.书记员;
                        person.Name = o.Replace("书记员", "");
                    }
                    return person;
                });
                this.CaseSourceUpdateDto.TrialPeople = trialPeople.ToList();
                #endregion

                this.CaseSourceUpdateDto.Normalize();

                this.Valid = true;
            }
            
        }
    }
}
