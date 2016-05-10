using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSagePageImporter
{
    static class Program
    {
        private static string HostName = "http://localhost:2528"; // Local
        //private static string HostName = "http://www.mindsage.org"; // Production

        static void Main(string[] args)
        {
            const string ImportFilePath = @"..\..\Imports\";
            const string ExportFilePath = @"..\..\Exports\";
            var fileEntries = Directory.GetFiles(ImportFilePath);
            foreach (string filePath in fileEntries)
            {
                var rawHtml = string.Empty;
                using (var reader = new StreamReader(filePath))
                    rawHtml = reader.ReadToEnd();

                var convertedHtml = rawHtml
                       .AddAngular()
                       .ReplaceAuthentication()
                       .RemoveTopRightCorner()
                       .ReplaceLinks();

                var fileName = filePath.Replace(ImportFilePath, string.Empty);
                using (var writer = new StreamWriter($"{ ExportFilePath }\\{ fileName }"))
                    writer.Write(convertedHtml);
            }
        }

        private static string AddAngular(this string html)
        {
            const string OriginalBody = "<body";
            const string ReplaceBodyWith = "<body ng-app=''";
            html = html.Replace(OriginalBody, ReplaceBodyWith);

            const string OriginalWordPressStyle = "<link rel='https://api.w.org/' href='http://mindkey.org/wordpress/wp-json/' />";
            const string AddAngularScript = "<script src='http://ajax.googleapis.com/ajax/libs/angularjs/1.3.14/angular.min.js'></script><link rel='https://api.w.org/' href='http://mindkey.org/wordpress/wp-json/' />";
            return html.Replace(OriginalWordPressStyle, AddAngularScript);
        }
        private static string ReplaceAuthentication(this string html)
        {
            const string Original = "<div id=\"mAuthentication\">mAuthentication</div>";
            var ReplaceWith = $"<div ng-include src=\"'{ HostName }/account/LoginSection'\"></div>";
            return html.Replace(Original, ReplaceWith);
        }
        private static string RemoveTopRightCorner(this string html)
        {
            const string Original = "class=\"sb-toggle-wrapper\"";
            return html.Replace(Original, string.Empty);
        }
        private static string ReplaceLinks(this string html)
        {
            var links = new List<KeyValuePair<string, string>>
            {
                //new KeyValuePair<string,string>("http://mindkey.org/wordpress/","home.html"),
                new KeyValuePair<string,string>("http://mindkey.org/wordpress/index.php/our-approach/", "approach.html"),
                new KeyValuePair<string,string>("http://mindkey.org/wordpress/index.php/our-organisation/", "organisation.html"),
                new KeyValuePair<string,string>("http://mindkey.org/wordpress/index.php/what-is-mindsage/", "whatmindsage.html"),
                new KeyValuePair<string,string>("http://mindkey.org/wordpress/index.php/softskill-necessity/", "necessity.html"),
                new KeyValuePair<string,string>("http://mindkey.org/wordpress/index.php/why-start-now/", "whystart.html"),
                new KeyValuePair<string,string>("http://mindkey.org/wordpress/index.php/seminars/", "seminars.html"),
                new KeyValuePair<string,string>("http://mindkey.org/wordpress/index.php/21_century_skills/", "21century.html"),
                new KeyValuePair<string,string>("http://mindkey.org/wordpress/index.php/why-mindsage/", "whymindsage.html"),

                // Links
                new KeyValuePair<string, string>("href=\"http://mindkey.org/wordpress\"", $"href=\"{ HostName }\""), // Logo
                new KeyValuePair<string, string>("<a href=\"http://mindkey.org/wordpress/\">Home</a>", $"<a href=\"{ HostName }\">Home</a>") // Footer
            };
            foreach (var link in links) html = html.Replace(link.Key, link.Value);

            return html;
        }
    }
}
