using KEACanteenREST.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KEACanteenREST.Controllers
{
    [Route("api")]
    public class RootController : Controller
    {
        private IUrlHelper _urlHelper;

        public RootController(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if (mediaType == "application/vnd.sysint.hateoas+json")
            {
                var links = new List<LinkDto>();

                links.Add(
                    new LinkDto(_urlHelper.Link("GetRoot", new { }),
                    "self",
                    "GET"));
                links.Add(
                   new LinkDto(_urlHelper.Link("GetAllData", new { }),
                   "records",
                   "GET"));
                links.Add(
                  new LinkDto(_urlHelper.Link("CreateMeasurement", new { }),
                  "create_record",
                  "POST"));

                return Ok(links);
            }

            return NoContent();
        }
    }
}
