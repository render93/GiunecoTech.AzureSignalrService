using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeOrder.Hubs;
using CoffeeOrder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeOrder.Controllers
{
    public class CoffeeController : Controller
    {
        private readonly IHubContext<CoffeeOrderHub> _coffeeHub;

        public CoffeeController(IHubContext<CoffeeOrderHub> coffeeHub)
        {
            _coffeeHub = coffeeHub;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OrderCaffee([FromBody]Order mdl)
        {
            await _coffeeHub.Clients.AllExcept(new List<string>(){mdl.SenderId}).SendAsync("NewOrder", mdl);
            return Accepted(1); //return order id
        }
    }
}