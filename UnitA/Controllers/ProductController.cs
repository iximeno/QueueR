using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitX;


namespace UnitX.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
       
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Product Get(string Id, bool sold = false)
        {
            var target = (from p in ProgramA.Products where p.Id.ToString("N") == Id select p).FirstOrDefault();
            if (target == null) return null;
            if (target.SoldDate == null && sold)
            {
                target.SoldDate = DateTime.UtcNow;
                ProgramA.MyQueue.PushIt(Id, "Solds");

            }
            else
            {
                if (sold) return null;
            }
            return target;
        }


    }
}
