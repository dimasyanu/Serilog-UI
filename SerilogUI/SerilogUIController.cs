using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace SerilogUI
{
    public class SerilogUIController : Controller
    {
        private readonly ILogger _logger;
        private readonly SerilogUIRepository _repo;

        public SerilogUIController(IConfiguration config, ILogger logger, SerilogUIRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Index() => Content(_repo.GetRawHtml(), "text/html");

        [HttpGet]
        public IActionResult GetFilenames() => Success(_repo.GetFilenames());

        [HttpGet]
        public IActionResult GetItems([FromQuery] FilterParam filter) => Success(_repo.GetItems(filter));

        [HttpGet]
        [Route("assets/{filename}")]
        public IActionResult GetAsset(string filename)
        {
            var contentType = "application/javascript";
            if (filename.EndsWith(".css")) {
                contentType = "text/css";
            }
            return Content(_repo.GetAsset("assets." + filename), contentType);
        }

        private IActionResult Success(object results, string message = "Successfully retrieved") => Ok(new {
            Data = results,
            Message = message,
            StatusCode = HttpStatusCode.OK
        });

    }
}
