using CloudBackup.Database.Entity;
using CloudBackup.Database.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CloudBackup.WebApp.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public DpOperations operations;
        private IMemoryCache _cache;
        public BaseController(IMemoryCache cache)
        {
            _cache = cache;
            operations = new DpOperations();
        }
        public int GetOrganizationId()
        {
            return GetOrganizationBinding().OrganizationId;
        }
        public string GetUserName()
        {
            return User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value;
        }
        public int GetUserId()
        {
            return Convert.ToInt32( User.Claims.FirstOrDefault(s => s.Type == "UserId").Value);
        }
        public AddressBinding GetOrganizationBinding()
        {
            string url = Request.Host.Host;
            int port = Convert.ToInt32(Request.Host.Port);
            string cacheKey = url + ":" + port;
            AddressBinding bind = _cache.Get<AddressBinding>(cacheKey);
            if (bind == null)
            {
                AddressBinding binding = operations.Organization.GetAddress(url, port);
                _cache.Set<AddressBinding>(cacheKey, binding);
                return binding;
            }
            else
                return bind;
        }


    }
}
