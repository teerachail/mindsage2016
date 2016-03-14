using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class GetCommentRespond
    {
        #region Properties

        public bool IsPrivateAccount { get; set; }
        public bool IsDiscussionAvailable { get; set; }
        public IEnumerable<CommentInformation> Comments { get; set; }

        #endregion Properties
    }
}
