//----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using ComputeWebJobsSDKQueue.Models;
using ComputeWebJobsSDKQueue.Repositories;

namespace MindSageWebJob
{
    public class Functions
    {
        /// <summary>
        /// อัพเดทข้อมูล comments และ discussions
        /// </summary>
        /// <param name="userprofileStr">ข้อมูลผู้ใช้ที่ขอทำการอัพเดท</param>
        public static async Task UpdateCommentsAndDiscussions([QueueTrigger("update-user-profile")] UpdateUserProfileMessage userprofile)
        {
            if (userprofile == null) return;

            var commentRepo = new CommentRepository();
            var requireUpdateComments = (await commentRepo.GetCommentByRelatedUserProfileId(userprofile.UserProfileId)).ToList();
            foreach (var comment in requireUpdateComments)
            {
                if (comment.CreatedByUserProfileId == userprofile.UserProfileId)
                {
                    comment.CreatorDisplayName = userprofile.DisplayName;
                    comment.CreatorImageUrl = userprofile.ProfileImageUrl;
                }
                var relatedDiscussions = comment.Discussions.Where(it => it.CreatedByUserProfileId == userprofile.UserProfileId);
                foreach (var discussion in relatedDiscussions)
                {
                    discussion.CreatorDisplayName = userprofile.DisplayName;
                    discussion.CreatorImageUrl = userprofile.ProfileImageUrl;
                }

                await commentRepo.UpdateComment(comment);
            }
        }
    }
}
