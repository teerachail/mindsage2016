using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class GetStudentListRespond
    {
        #region Properties

        public string id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int CommentPercentage { get; set; }
        public int OnlineExtrasPercentage { get; set; }
        public int SocialParticipationPercentage { get; set; }

        #endregion Properties
    }
}
