using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

using log4net;

using Deg.FrontEndWebServer.Properties;
using System.Security.Principal;

namespace Deg.FrontEndWebServer.Controllers
{
    [RoutePrefix("home")]
    [Authorize]
    public class HomeController : Controller
    {
        private ILog _log = LogManager.GetLogger(typeof(HomeController));
        private TimeZoneInfo _est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        private TimeZoneInfo _cst = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        private TimeZoneInfo _mst = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
        private TimeZoneInfo _pst = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

        [Route("{sessionID:Guid?}")]
        public ActionResult Index(Guid? sessionID)
        {
            try
            {

                var isValidUser = false;

                if (sessionID.HasValue)
                {
                    isValidUser = true;
                }
                else
                {
                    if (HttpContext.User is WindowsPrincipal)
                    {
                        var address = default(IPAddress);
                        try
                        {
                            address = new IPAddress(Request.UserHostAddress.Split('.').Select(part => Convert.ToByte(part)).ToArray());
                        }
                        catch
                        {
                            address = new IPAddress(new byte[] { 127, 0, 0, 1, });
                        }

                        var user = HttpContext.User as WindowsPrincipal;
                        if (user.IsInRole(WindowsBuiltInRole.User))
                        {
                            sessionID = WebShared.Instance.RegisterSession(user.Identity.Name, null, address, Request.UserAgent, true);
                            isValidUser = sessionID != Guid.Empty;
                        }
                        else if (user.IsInRole(WindowsBuiltInRole.Administrator))
                        {

                        }
                    }
                }

                if (isValidUser)
                {
                    ViewBag.SessionInfo = string.Format("session=\"{0}\"", sessionID.ToString().Trim('{', '}'));

#if DEBUG
                    ViewBag.Server = "localhost";
                    ViewBag.Port = 54312;
                    ViewBag.IsDebug = true;
#else
                    ViewBag.Server = "localhost";
                    ViewBag.Port = 80;
                    ViewBag.IsDebug = false;
#endif

                    return View();
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return RedirectToAction("Login", "AccountController");
        }

        [Route("getcontrol")]
        [HttpPost]
        public ActionResult GetControl()
        {
            try
            {
                using (var sr = new StreamReader(Request.InputStream))
                {
                    var content = sr.ReadToEnd();
                    var controlID = Convert.ToUInt64(content, 16);

                    var bytes = default(byte[]);
                    var control = string.Empty;
                    switch (controlID & 0x0000ffff)
                    {
                        case 0x00000002:
                            control = Resources.BaldayMonitor;
                            break;
                        case 0x00000003:
                            control = Resources.TickMonitor;
                            break;
                        case 0x00000004:
                            control = Resources.PjmDispatch;
                            break;
                    }

                    switch (controlID >> 16)
                    {
                        case 1:
                            control = control.Replace("ADA20289-0579-4E01-A02D-E11C7C977173", "pjm");
                            break;
                        case 2:
                            control = control.Replace("ADA20289-0579-4E01-A02D-E11C7C977173", "miso");
                            break;
                        case 3:
                            control = control.Replace("ADA20289-0579-4E01-A02D-E11C7C977173", "ercot");
                            break;
                        case 4:
                            control = control.Replace("ADA20289-0579-4E01-A02D-E11C7C977173", "caiso");
                            break;
                        case 5:
                            control = control.Replace("ADA20289-0579-4E01-A02D-E11C7C977173", "spp");
                            break;
                        case 6:
                            control = control.Replace("ADA20289-0579-4E01-A02D-E11C7C977173", "nyiso");
                            break;
                        case 7:
                            control = control.Replace("ADA20289-0579-4E01-A02D-E11C7C977173", "isone");
                            break;
                    }

                    bytes = Encoding.UTF8.GetBytes(control);
                    return new FileContentResult(bytes, "text/html");
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);

            }

            return new HttpStatusCodeResult(404);
        }

        [Route("markettime")]
        [HttpPost]
        public ActionResult MarketTime()
        {
            var dateTime = string.Empty;
            try
            {
                var market = default(string);
                using (var sr = new StreamReader(Request.InputStream))
                {
                    market = sr.ReadToEnd().Trim();
                }
               
                // TODO: The routine will move to FrontEndManager to get unique time for all web servers
                if (string.IsNullOrWhiteSpace(market) == false)
                {
                    var localTime = default(DateTime);

                    switch (market.ToLower())
                    {
                        case "pjm":
                        case "nyiso":
                        case "iso-ne":
                            localTime = DateTime.UtcNow.Add(_est.BaseUtcOffset);
                            if (localTime.IsDaylightSavingTime())
                            {
                                localTime = localTime.AddHours(1);
                            }
                            break;
                        case "miso":
                            localTime = DateTime.UtcNow.Add(_cst.BaseUtcOffset);
                            break;
                        case "ercot":
                        case "spp":
                            localTime = DateTime.UtcNow.Add(_cst.BaseUtcOffset);
                            if (localTime.IsDaylightSavingTime())
                            {
                                localTime = localTime.AddHours(1);
                            }
                            break;
                        case "caiso":
                            localTime = DateTime.UtcNow.Add(_pst.BaseUtcOffset);
                            if (localTime.IsDaylightSavingTime())
                            {
                                localTime = localTime.AddHours(1);
                            }
                            break;
                    }

                    if (localTime != default(DateTime))
                    {
                        dateTime = localTime.ToString("s");
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
            }

            return new ContentResult
            {
                Content = dateTime,
            };
        }
    }
}