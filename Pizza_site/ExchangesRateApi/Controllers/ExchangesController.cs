using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangesRateApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangesController : ControllerBase
    {
        private readonly ILogger<ExchangesController> _logger;

        public ExchangesController(ILogger<ExchangesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<ExchangesForecast> Get()
        {
            List<ExchangesForecast> list = new List<ExchangesForecast>();
            list.Add(new ExchangesForecast
            {
                nameFrom = "UAH",
                nameTo = "RUB",
                coefficient = 3,
                Date = DateTime.Now
            });
            list.Add(new ExchangesForecast
            {
                nameFrom = "UAH",
                nameTo = "DOL",
                coefficient = 0.27,
                Date = DateTime.Now
            });
            
            return list.ToArray();
        }
    }
}
