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
                var value = ConfigurationManager.AppSettings["mindsageWebUrl"] ?? string.Empty;
                return value;
            }
        }

        public static string PrimaryDBConnectionString
        {
            get
            {
                var value = ConfigurationManager.AppSettings["PrimaryDBConnectionString"] ?? string.Empty;
                return value;
            }
        }

        public static string PrimaryDBName
        {
            get
            {
                var value = ConfigurationManager.AppSettings["PrimaryDBName"] ?? string.Empty;
                return value;
            }
        }

        public static string CourseCatalogTableName
        {
            get
            {
                var value = ConfigurationManager.AppSettings["CourseCatalogTableName"] ?? string.Empty;
                return value;
            }
        }

        public static string LessonCatalogTableName
        {
            get
            {
                var value = ConfigurationManager.AppSettings["LessonCatalogTableName"] ?? string.Empty;
                return value;
            }
        }

        public static string ClassRoomTableName
        {
            get
            {
                var value = ConfigurationManager.AppSettings["ClassRoomTableName"] ?? string.Empty;
                return value;
            }
        }

        public static string ContractTableName
        {
            get
            {
                var value = ConfigurationManager.AppSettings["ContractTableName"] ?? string.Empty;
                return value;
            }
        }

        public static string UserProfileTableName
        {
            get
            {
                var value = ConfigurationManager.AppSettings["UserProfileTableName"] ?? string.Empty;
                return value;
            }
        }
    }
}
