using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CloudBackup.WebApp.Core
{
    public class CustomAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpcontext = context.GetHttpContext();
            if (httpcontext.User.Identity.IsAuthenticated && httpcontext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value == "ismail.erden")
                return true;
            else
                return false;
        }
    }
}
