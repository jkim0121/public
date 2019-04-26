using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;
using System.Runtime.Serialization.Json;

using log4net;
using System.IO;

namespace Deg.FrontEndWebServer.Controllers
{
    [RoutePrefix("admin")]
    public class AdminController : ApiController
    {
        private static ILog _log = LogManager.GetLogger(typeof(AdminController));

        [Route("activesessions/{adminsessionid:guid}")]
        [HttpGet]
        public string ActiveSessions(Guid sessionID)
        {
            return string.Empty;
        }

        [Route("forcelogout/{adminsessionid:guid}/{targetsessionid:guid}")]
        [HttpGet]
        public string ForceLogout(Guid sessionID, Guid targetID)
        {
            return string.Empty;
        }
    }
}