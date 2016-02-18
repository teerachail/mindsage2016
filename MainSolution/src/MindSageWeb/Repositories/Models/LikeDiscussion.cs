using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories.Models
{
    public class LikeDiscussion : LikeContent
    {
        #region Properties

        public string CommentId { get; set; }
        public string DiscussionId { get; set; }

        #endregion Properties
    }
}
