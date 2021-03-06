module app.journals {
    'use strict';

    class JournalController {

        private userprofile: any;
        public message: string;
        public targetComment: any;
        public targetDiscussion: any;
        public deleteComment: boolean;
        public MyComments = [];
        public discussions = [];
        private requestedCommentIds = [];

        static $inject = ['$scope', 'content', 'targetUserId', 'likes', 'app.shared.ClientUserProfileService', 'app.shared.DiscussionService', 'app.shared.CommentService', 'app.lessons.LessonService'];
        constructor(private $scope, public content, public targetUserId, private likes, private svc: app.shared.ClientUserProfileService, private discussionSvc: app.shared.DiscussionService, private commentSvc: app.shared.CommentService, private lessonSvc: app.lessons.LessonService) {
            this.userprofile = this.svc.GetClientUserProfile();
        }


        public GetWeeks() {
            var usedWeekNo = {};
            var lessonWeeks = [];

            if (this.MyComments.length != 0) {
                var NewCommentlessonWeekNo = this.userprofile.CurrentLessonNo;
                lessonWeeks.push(NewCommentlessonWeekNo);
                usedWeekNo[NewCommentlessonWeekNo] = 1;
            }

            for (var index = 0; index < this.content.Comments.length; index++) {

                var lessonWeekNo = this.content.Comments[index].LessonWeek;
                if (usedWeekNo.hasOwnProperty(lessonWeekNo)) continue;
                lessonWeeks.push(lessonWeekNo);
                usedWeekNo[lessonWeekNo] = 1;
            }
            return lessonWeeks;
        }

        public GetComments(week: number) {
            var qry = this.content.Comments.filter(it=> it.LessonWeek == week);
            return qry;
        }
        
        public showDiscussion(item: any, open: boolean) {
            this.GetDiscussions(item);
            return !open;
        }

        public GetDiscussions(comment) {
            if (comment == null) return;

            const NoneDiscussion = 0;
            if (comment.TotalDiscussions <= NoneDiscussion) return;
            if (this.requestedCommentIds.filter(it=> it == comment.id).length > NoneDiscussion) return;

            this.requestedCommentIds.push(comment.id);
            this.discussionSvc
                .GetDiscussions(comment.LessonId, comment.ClassRoomId, comment.id)
                .then(it=> {
                    if (it == null) return;
                    for (var index = 0; index < it.length; index++) {
                        this.discussions.push(it[index]);
                    }
                });
        }

        public CreateNewComment(message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return;

            this.commentSvc.CreateNewComment(this.userprofile.CurrentClassRoomId, this.userprofile.CurrentLessonId, message)
                .then(it=> {
                    if (it == null) {
                        return message;
                    }
                    else { 
                        var newComment = new app.shared.Comment(it.ActualCommentId, message, 0, 0, this.userprofile.ImageUrl, this.userprofile.FullName, this.userprofile.CurrentClassRoomId, this.userprofile.CurrentLessonId, this.userprofile.UserProfileId, 0 - this.MyComments.length);
                        this.MyComments.push(newComment);
                        return "";
                    }
                    
                });
        }

        public CreateNewDiscussion(commentId: string, message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return;

            var local = this.content.Comments.filter(it=> it.id == commentId)[0];
            this.discussionSvc.CreateDiscussion(local.ClassRoomId, local.LessonId, commentId, message)
                .then(it=> {
                    if (it == null) {
                        return message;
                    }

                    var newDiscussion = new app.shared.Discussion(it.ActualCommentId, commentId, message, 0, this.userprofile.ImageUrl, this.userprofile.FullName, this.userprofile.UserProfileId, 0 - this.discussions.length);
                    this.discussions.push(newDiscussion);
                    this.content.Comments.filter(it=> it.id == commentId)[0].TotalDiscussions++;
                    return "";
                });
        }

        public LikeComment(commentId: string, IsLike: number) {
            if (IsLike == -1)
                this.content.Comments.filter(it=> it.id == commentId)[0].TotalLikes++;
            else
                this.content.Comments.filter(it=> it.id == commentId)[0].TotalLikes--;

            var setIndex = this.likes.LikeCommentIds.indexOf(commentId);
            const ElementNotFound = -1;
            if (setIndex <= ElementNotFound) this.likes.LikeCommentIds.push(commentId);
            else this.likes.LikeCommentIds.splice(setIndex, 1);

            var local = this.content.Comments.filter(it=> it.id == commentId)[0];
            this.commentSvc.LikeComment(local.ClassRoomId, local.LessonId, commentId);

        }

        public LikeDiscussion(commentId: string, discussionId: string, IsLike: number) {
            if (IsLike == -1)
                this.discussions.filter(it=> it.id == discussionId)[0].TotalLikes++;
            else
                this.discussions.filter(it=> it.id == discussionId)[0].TotalLikes--;


            var setIndex = this.likes.LikeDiscussionIds.indexOf(discussionId);
            const ElementNotFound = -1;
            if (setIndex <= ElementNotFound) this.likes.LikeDiscussionIds.push(discussionId);
            else this.likes.LikeDiscussionIds.splice(setIndex, 1);

            var local = this.content.Comments.filter(it=> it.id == commentId)[0];
            this.discussionSvc.LikeDiscussion(local.ClassRoomId, local.LessonId, commentId, discussionId);
        }

        public DeleteComment() {
            var comment = this.targetComment; 
            var removeIndex = this.content.Comments.indexOf(comment);
            if (removeIndex > -1) this.content.Comments.splice(removeIndex, 1);
            else {
                removeIndex = this.MyComments.indexOf(comment);
                if (removeIndex > -1) this.MyComments.splice(removeIndex, 1);
            }
            this.commentSvc.UpdateComment(comment.ClassRoomId, comment.LessonId, comment.id, true, null);
        }

        public DeleteDiscussion() {
            var commentId = this.targetComment.id;
            var discussion = this.targetDiscussion;
            var removeIndex = this.discussions.indexOf(discussion);
            if (removeIndex > -1) this.discussions.splice(removeIndex, 1);
            this.content.Comments.filter(it=> it.id == commentId)[0].TotalDiscussions--;
            var local = this.content.Comments.filter(it=> it.id == commentId)[0];
            this.discussionSvc.UpdateDiscussion(local.ClassRoomId, local.LessonId, commentId, discussion.id, true, null);
        }

        public EditComment(commentId: string, message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return;

            var local = this.content.Comments.filter(it=> it.id == commentId)[0];
            this.commentSvc.UpdateComment(local.ClassRoomId, local.LessonId, commentId, false, message);
        }

        public EditDiscussion(commentId: string, discussionId: string, message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return;

            var local = this.content.Comments.filter(it=> it.id == commentId)[0];
            this.discussionSvc.UpdateDiscussion(local.ClassRoomId, local.LessonId, commentId, discussionId, false, message);
        }

        public EditOpen(message: string, open: boolean) {
            this.message = message;
            return !open;
        }

        public SaveEdit(messageId: number, save: boolean) {
            this.content.Comments.filter(it=> it.id == messageId)[0].Description = this.message;
            this.EditComment(this.content.Comments.filter(it=> it.id == messageId)[0].id, this.message);
            return !save;
        }

        public SaveEditDiscus(commentId: string, messageId: number, save: boolean) {
            this.discussions.filter(it=> it.id == messageId)[0].Description = this.message;
            this.EditDiscussion(commentId, this.discussions.filter(it=> it.id == messageId)[0].id, this.message);
            return !save;
        }

        public CancelEdit(save: boolean) {
            return !save;
        }

        public deleteComfirm(comment: string) {
            this.targetComment = comment;
            this.targetDiscussion = null;
            this.deleteComment = true;
        }

        public deleteDisComfirm(comment: string, discussion: string) {
            this.targetComment = comment;
            this.targetDiscussion = discussion;
            this.deleteComment = false;
        }
    }

    angular
        .module('app.journals')
        .controller('app.journals.JournalController', JournalController);
}