using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Cherwell.Controllers
{
    [Route("api")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Route("alltriangles")]
        [HttpGet]
        public ActionResult<string> GetAllTriangles()
        {
            try
            {
                return string.Empty;
            }
            catch (Exception ex)
            {
                // log ex here
                return BadRequest();
            }
        }

        [Route("coordinate")]
        [HttpPost]
        public ActionResult<string> GetCoordinate([FromBody] string input)
        {
            try
            {
                //input format : [ { "x" : x1, "y" : y1 }, { "x" : x2, "y" : y2 }, { "x" : x3, "y" : y3 } ]
                var points = JArray.Parse(input).Select(p => new { X = Convert.ToDouble(p["x"]), Y = Convert.ToDouble(p["y"]) });
                var horizontal = points.GroupBy(p => p.Y).First(g => g.Count() == 2).AsEnumerable().OrderBy(p => p.X).ToList();

                var groups = points.GroupBy(p => p.Y).OrderBy(g => g.Count());
                if (horizontal.Count() > 0)
                {
                    var rest = groups.FirstOrDefault(g => g.Count() == 1).ElementAt(0);

                    var height = Math.Abs(rest.Y - horizontal[0].Y);
                    var width = horizontal[1].Y - horizontal[0].Y;


                    if (width == height)
                    {
                        if (horizontal[0].X == rest.X && horizontal[0].Y > rest.Y)
                        {

                        }
                        else if (horizontal[0].X == rest.X && horizontal[0].Y < rest.Y)
                        {

                        }
                        else if (horizontal[1].X == rest.X && horizontal[0].Y > rest.Y)
                        {

                        }
                        else if (horizontal[1].X == rest.X && horizontal[0].Y < rest.Y)
                        {

                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                // log ex here
                return BadRequest();
            }
        }
    }
}
