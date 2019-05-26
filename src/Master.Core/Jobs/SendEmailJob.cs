using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Master.Case;
using Master.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.Core.Jobs
{
    public class SendEmailJob : BackgroundJob<SendEmailJobArgs>, ITransientDependency
    {
        public EmailLogManager EmailLogManager { get; set; }

        [UnitOfWork]
        public override void Execute(SendEmailJobArgs args)
        {

            var email = EmailLogManager.GetByIdAsync(args.EmailLogId).GetAwaiter().GetResult();
            Common.EmailHelper.SendMailAsync(
                SettingManager.GetSettingValueAsync(SettingNames.mail_smtpServer).Result,
                Convert.ToBoolean(SettingManager.GetSettingValueAsync(SettingNames.mail_ssl).Result),
                SettingManager.GetSettingValueAsync(SettingNames.mail_username).Result,
                SettingManager.GetSettingValueAsync(SettingNames.mail_pwd).Result,
                SettingManager.GetSettingValueAsync(SettingNames.mail_nickname).Result,
                SettingManager.GetSettingValueAsync(SettingNames.mail_fromname).Result,
                email.ToEmail, email.Title, email.Content).GetAwaiter().GetResult();
            email.Success = true;
        }
    }

    public class SendEmailJobArgs
    {
        public int EmailLogId { get; set; }
    }
}
