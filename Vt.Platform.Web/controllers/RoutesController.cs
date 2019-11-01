using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vt.Platform.Web.controllers
{
    [Route("api/[controller]")]
    public class RoutesController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        private readonly IHostingEnvironment _hostingEnvironment;

        public RoutesController(
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            IHostingEnvironment hostingEnvironment)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var p = _hostingEnvironment.WebRootPath;

            var results = _actionDescriptorCollectionProvider.ActionDescriptors.Items;
            var exceptions = new[] {"api/Routes", ""};
            var pages = results.Select(x => x.AttributeRouteInfo.Template).Except(exceptions);

            return Ok(new {
              pages = pages,
              content = Directory.EnumerateFiles(p, "*.*", SearchOption.AllDirectories)
            });
        }
    }
}
