using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PackageChallenge.Model.Models;
using PackagingChallenge.Service.Interfaces;

namespace PackagingChallenge.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ApiController]
    public class v1Controller : ControllerBase
    {
        private readonly IPackagingService packagingService;

        public v1Controller(IPackagingService packagingService)
        {
            this.packagingService = packagingService;
        }
        
        [HttpPost]
        public ActionResult pack([FromBody] List<Package> package)
        {
            if (!ModelState.IsValid)
                return new JsonResult(BadRequest(ModelState));

            var result = packagingService.ProcessPackages(package);

            return new JsonResult(result);
        }
    }
}