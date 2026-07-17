using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Sbdms.Ltr.Api.Controllers.v2;

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class TestController(ILogger<TestController> _logger) : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetTest")]
        public IEnumerable<TestModel> Get()
        {
            _logger.LogWarning("GET method for Test Controller V2");

            return Enumerable.Range(1, 5).Select(index => new TestModel
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
