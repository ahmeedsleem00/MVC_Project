using Demo.PL.Models;
using Demo.PL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{

    [Authorize]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScopedService _scoped1;
        private readonly IScopedService _scoped2;
        private readonly ISingeltonService _singelton1;
        private readonly ISingeltonService _singelton2;
        private readonly ITransientService _transient1;
        private readonly ITransientService _transient2;

        public HomeController(ILogger<HomeController> logger,IScopedService scoped1
               ,IScopedService scoped2
               ,ISingeltonService singelton1
             ,ISingeltonService singelton2
             ,ITransientService transient1
             ,ITransientService transient2
            )
        {
            _logger     = logger;
            _scoped1    = scoped1;
            _scoped2    = scoped2;
            _singelton1 = singelton1;
            _singelton2 = singelton2;
            _transient1 = transient1;
            _transient2 = transient2;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Home/TestLifeTime
        public string TestLifeTime()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"Scoped01 :: {_scoped1.GetGuid()}\n");
            builder.Append($"Scoped02 :: {_scoped2.GetGuid()}\n\n");
            builder.Append($"singelton1 :: {_singelton1.GetGuid()}\n");
            builder.Append($"singelton2 :: {_singelton2.GetGuid()}\n\n");
            builder.Append($"transient1 :: {_transient1.GetGuid()}\n");
            builder.Append($"transient2 :: {_transient2.GetGuid()}");
            return builder.ToString();      
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
