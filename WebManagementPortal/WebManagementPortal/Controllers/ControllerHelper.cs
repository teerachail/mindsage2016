using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebManagementPortal.Controllers
{
    public static class ControllerHelper
    {
        public static IEnumerable<SelectListItem> GetExtraContentType(string selectedType = null)
        {
            selectedType = selectedType ?? string.Empty;
            var extraContentType = ExtraContentType.Video;
            Enum.TryParse<ExtraContentType>(selectedType, out extraContentType);
            return new List<SelectListItem>
            {
                new SelectListItem { Value = ExtraContentType.Video.ToString(), Text = ExtraContentType.Video.ToString(), Selected = extraContentType == ExtraContentType.Video },
                new SelectListItem { Value = ExtraContentType.Audio.ToString(), Text = ExtraContentType.Audio.ToString(), Selected = extraContentType == ExtraContentType.Audio },
                new SelectListItem { Value = ExtraContentType.File.ToString(),  Text = ExtraContentType.File.ToString(),  Selected = extraContentType == ExtraContentType.File },
                new SelectListItem { Value = ExtraContentType.Game.ToString(),  Text = ExtraContentType.Game.ToString(),  Selected = extraContentType == ExtraContentType.Game },
            };
        }
        public static string ConvertToIconUrl(string contentString)
        {
            var content = ExtraContentType.Video;
            Enum.TryParse<ExtraContentType>(contentString, out content);
            return ConvertToIconUrl(content);
        }
        public static string ConvertToIconUrl(this ExtraContentType content)
        {
            switch (content)
            {
                case ExtraContentType.Video:
                    return "";
                case ExtraContentType.Audio:
                    return "";
                case ExtraContentType.File:
                    return "";
                case ExtraContentType.Game:
                    return "";
                default:
                    return "";
            }
        }
    }

    public enum ExtraContentType
    {
        Video = 1,
        Audio = 2,
        File = 3,
        Game = 4,
    }
}
