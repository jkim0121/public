using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HelloAspCore.Controllers
{
    [Route("api/pi")]
    [ApiController]
    public class MainController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Pi()
        {
            return Math.PI.ToString();
        }

        [HttpGet("{precision}")]
        public ActionResult<string> Pi(int precision)
        {
            return Math.PI.ToString(string.Format("F{0}", precision));
        }

    }
}
