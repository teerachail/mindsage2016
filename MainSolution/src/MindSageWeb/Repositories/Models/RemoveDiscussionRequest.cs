﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories.Models
{
    public class RemoveDiscussionRequest
    {
        #region Properties

        public string ClassRoomId { get; set; }
        public string LessonId { get; set; }
        public string CommentId { get; set; }
        public string UserProfileId { get; set; }

        #endregion Properties
    }
}
