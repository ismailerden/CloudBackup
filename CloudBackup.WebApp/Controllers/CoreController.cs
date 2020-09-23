using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CloudBackup.WebApp.Controllers
{
    public class CoreController : BaseController
    {
        public CoreController(IMemoryCache cache) : base(cache)
        {

        }
        public IActionResult SSS()
        {
            return View();
        }
    }
}