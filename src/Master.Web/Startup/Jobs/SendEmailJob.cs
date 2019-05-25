using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Master.Case;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.Web.Startup.Jobs
{
    public class SendEmailJob : BackgroundJob<SendEmailJobArgs>, ITransientDependency
    {
        public EmailLogManager EmailLogManager { get; set; }

        [UnitOfWork]
        public override void Execute(SendEmailJobArgs args)
        {
            var email = EmailLogManager.GetByIdAsync(args.EmailLogId).GetAwaiter().GetResult();
            Common.EmailHelper.SendMailAsync("", false, "", "", "", "", email.ToEmail, email.Title, email.Content).GetAwaiter().GetResult();
            email.Success = true;
        }
    }

    public class SendEmailJobArgs
    {
        public int EmailLogId { get; set; }
    }
}
