using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

[assembly: InternalsVisibleTo("Cherwell.Tests")]
namespace Cherwell.Controllers
{
    [Route("api")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private const double Length = 10.0;
        private static readonly char[] ValidCoordY = new[] { 'A', 'B', 'C', 'D', 'E', 'F', };
        private const int MinCoordX = 1;
        private const int MaxCoordX = 12;

        [Route("triangle")]
        [HttpGet]
        public ActionResult<string> GetTriangle(string coord)
        {
            var result = GetTriangleCore(coord);
            return result != null ? Ok(result) : BadRequest() as ActionResult;
        }

        [Route("coordinate")]
        [HttpPost]
        public ActionResult<string> GetCoordinate([FromBody] string input)
        {
            var result = GetCoordinateCore(input);
            return result != null ? Ok(result) : BadRequest() as ActionResult;
        }

        internal string GetTriangleCore(string coord)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(coord) == false && coord.Length >= 2)
                {
                    var y = Array.IndexOf(ValidCoordY, coord[0]) + 1;
                    if (int.TryParse(coord.Substring(1), out int x) && y >= 1 && x >= MinCoordX && x <= MaxCoordX)
                    {
                        var xs = new double[3];
                        var ys = new double[3];
                        xs[0] = (x - 1) / 2 * Length;
                        ys[0] = (y - 1) * Length;

                        if (x % 2 == 1)
                        {
                            ys[0] = ys[0] + Length;
                            xs[2] = xs[0];
                            ys[2] = ys[0] - Length;
                        }
                        else
                        {
                            xs[2] = xs[0] + Length;
                            ys[2] = ys[0] + Length;
                        }

                        ys[1] = ys[0];
                        xs[1] = xs[0] + Length;

                        var jArray = new JArray();
                        for (var i = 0; i < xs.Length; i++)
                        {
                            jArray.Add(JObject.Parse($"{{ \"x\" : {xs[i]}, \"y\" : {ys[i]},}}"));
                        }

                        return jArray.ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                // log ex here
            }

            return null;
        }

        internal string GetCoordinateCore(string input)
        {
            try
            {
                //input format : [ { "x" : x1, "y" : y1 }, { "x" : x2, "y" : y2 }, { "x" : x3, "y" : y3 } ]
                var points = JArray.Parse(input).Select(p => new { X = Convert.ToDouble(p["x"].ToString()), Y = Convert.ToDouble(p["y"].ToString()) });
                var horizontal = points.GroupBy(p => p.Y).First(g => g.Count() == 2).AsEnumerable().OrderBy(p => p.X).ToList();

                var groups = points.GroupBy(p => p.Y).OrderBy(g => g.Count());
                if (horizontal.Count() > 0)
                {
                    var rest = groups.FirstOrDefault(g => g.Count() == 1).ElementAt(0);

                    var width = horizontal[1].X - horizontal[0].X;
                    var height = Math.Abs(horizontal.FirstOrDefault(p => p.X == rest.X).Y - rest.Y);


                    if (width == height && width == Length)
                    {
                        var odd = horizontal[0].X == rest.X;

                        var letter = (char)(Convert.ToByte(Math.Floor(horizontal[0].Y / 10)) + 'A' - (odd ? 1 : 0));
                        var number = 2 * Convert.ToInt32(Math.Floor(horizontal[0].X / 10)) + 1 + (odd ? 0 : 1);

                        return $"{letter}{number}";
                    }
                }
            }
            catch (Exception ex)
            {
                // log ex here
            }

            return null;
        }
    }
}
