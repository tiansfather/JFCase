using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Web.Configuration
{
    public interface IApplicationBuilderConfigurer
    {
        void Configure(IApplicationBuilder app);
    }
}
