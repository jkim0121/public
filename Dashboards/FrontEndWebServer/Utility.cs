using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

using mscoree;

namespace Deg.FrontEndWebServer
{
    public class Utility
    {
        public static IList<AppDomain> GetAppDomains()
        {
            var domains = new List<AppDomain>();
            var enumHandle = IntPtr.Zero;
            var host = new CorRuntimeHost();
            try
            {
                host.EnumDomains(out enumHandle);
                object domain = null;
                while (true)
                {
                    host.NextDomain(enumHandle, out domain);
                    if (domain == null)
                    {
                        break;
                    }

                    domains.Add((AppDomain)domain);
                }
                return domains;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                host.CloseEnum(enumHandle);
                Marshal.ReleaseComObject(host);
            }
        }
    }
}
