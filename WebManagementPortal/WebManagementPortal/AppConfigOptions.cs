using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebManagementPortal
{
    public static class AppConfigOptions
    {
        public static string MindSageWebUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["mindsageWebUrl"] ?? string.Empty; ;
            }
        }
    }
}
