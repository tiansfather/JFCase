using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Authentication
{
    public enum LoginResultType : byte
    {
        Success = 1,

        InvalidUserName,

        InvalidPassword,

        UserIsNotActive,

        InvalidTenancyName,

        TenantIsNotActive,

        UnknownExternalLogin,

        LockedOut
    }
}
