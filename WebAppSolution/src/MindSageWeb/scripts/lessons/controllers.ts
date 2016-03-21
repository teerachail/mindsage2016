module app.lessons {
    'use strict';

    class LessonController {

        public teacherView: boolean;
        public currentUser: any;
        public message: string;
        public targetComment: any;
        public targetDiscussion: any;
        public deleteComment: boolean;
        public discussions = [];
        public EditId: string;
        private requestedCommentIds = [];
        private likes: any;

        private content;
        private comment;
        private lessonId;
        private classRoomId;
        private isWaittingForGetLessonContent: boolean;
        private isPrepareLessonContentComplete: boolean;

        static $inject = ['$q', '$scope', '$stateParams', 'waitRespondTime', 'app.shared.ClientUserProfileService', 'app.shared.DiscussionService', 'app.shared.CommentService', 'app.lessons.LessonService', 'app.shared.GetProfileService'];
        constructor(private $q, private $scope, private $stateParams, private waitRespondTime, private userprofileSvc: app.shared.ClientUserProfileService, private discussionSvc: app.shared.DiscussionService, private commentSvc: app.shared.CommentService, private lessonSvc: app.lessons.LessonService, private getProfileSvc: app.shared.GetProfileService) {
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            if (!this.userprofileSvc.IsPrepareAllUserProfileCompleted()) {
                setTimeout(it => this.prepareUserprofile(), this.waitRespondTime);
                return;
            }

            this.lessonId = this.$stateParams.lessonId;
            this.classRoomId = this.$stateParams.classRoomId;
            this.currentUser = this.userprofileSvc.GetClientUserProfile();
            this.currentUser.CurrentDisplayLessonId = this.lessonId;
            this.userprofileSvc.UpdateUserProfile(this.currentUser);
            this.teacherView = false;

            this.prepareLessonContents();
        }

        private prepareLessonContents(): void {
            var shouldRequestLessonContent = !this.isPrepareLessonContentComplete && !this.isWaittingForGetLessonContent;
            if (shouldRequestLessonContent) {
                this.isWaittingForGetLessonContent = true;
                this.$q.all([
                    this.getProfileSvc.GetLike(),
                    this.lessonSvc.GetContent(this.lessonId, this.classRoomId),
                    this.commentSvc.GetComments(this.lessonId, this.classRoomId)
                ]).then(data => {
                    this.likes = data[0];
                    this.content = data[1];
                    this.comment = data[2];
                    this.userprofileSvc.Advertisments = this.content.Advertisments;
                    this.isWaittingForGetLessonContent = false;
                    this.isPrepareLessonContentComplete = true;
                }, error => {
                    console.log('Load lesson content failed, retrying ...');
                    this.isWaittingForGetLessonContent = false;
                    setTimeout(it=> this.prepareLessonContents(), this.waitRespondTime);
                });
            }
        }

        public selectTeacherView(): void {
            this.teacherView = true;
        }

        public selectStdView(): void {
            this.teacherView = false;
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
                .GetDiscussions(this.lessonId, this.classRoomId, comment.id)
                .then(it=> {
                    if (it == null) return;
                    for (var index = 0; index < it.length; index++) {
                        if (this.discussions.filter(dis => dis.id == it[index].id).length == 0)
                            this.discussions.push(it[index]);
                    }
                });
        }

        public CreateNewComment(message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return message;

            this.commentSvc.CreateNewComment(this.classRoomId, this.lessonId, message)
                .then(it=> {
                    if (it == null) {
                        return message;
                    }
                    else {
                        var userprofile = this.userprofileSvc.GetClientUserProfile();
                        var newComment = new app.shared.Comment(it.ActualCommentId, message, 0, 0, userprofile.ImageUrl, userprofile.FullName, this.classRoomId, this.lessonId, userprofile.UserProfileId, 0 - this.comment.Comments.length);
                        this.comment.Comments.push(newComment);
                        return "";
                    }
                });
        }

        public CreateNewDiscussion(commentId: string, message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return message;

            this.discussionSvc.CreateDiscussion(this.classRoomId, this.lessonId, commentId, message)
                .then(it=> {
                    if (it == null) {
                        return message;
                    }
                    else {
                        var userprofile = this.userprofileSvc.GetClientUserProfile();
                        var newDiscussion = new app.shared.Discussion(it.ActualCommentId, commentId, message, 0, userprofile.ImageUrl, userprofile.FullName, userprofile.UserProfileId, 0 - this.discussions.length);
                        this.discussions.push(newDiscussion);
                        this.comment.Comments.filter(it=> it.id == commentId)[0].TotalDiscussions++;
                        return "";
                    }
                });
        }

        public LikeComment(commentId: string, IsLike: number) {
            if (IsLike == -1)
                this.comment.Comments.filter(it=> it.id == commentId)[0].TotalLikes++;
            else
                this.comment.Comments.filter(it=> it.id == commentId)[0].TotalLikes--;

            this.commentSvc.LikeComment(this.classRoomId, this.lessonId, commentId);

            var removeIndex = this.likes.LikeCommentIds.indexOf(commentId);
            const ElementNotFound = -1;
            if (removeIndex <= ElementNotFound) this.likes.LikeCommentIds.push(commentId);
            else this.likes.LikeCommentIds.splice(removeIndex, 1);
        }

        public LikeDiscussion(commentId: string, discussionId: string, IsLike: number) {
            if (IsLike == -1)
                this.discussions.filter(it=> it.id == discussionId)[0].TotalLikes++;
            else
                this.discussions.filter(it=> it.id == discussionId)[0].TotalLikes--;

            this.discussionSvc.LikeDiscussion(this.classRoomId, this.lessonId, commentId, discussionId);

            var removeIndex = this.likes.LikeDiscussionIds.indexOf(discussionId);
            const ElementNotFound = -1;
            if (removeIndex <= ElementNotFound) this.likes.LikeDiscussionIds.push(discussionId);
            else this.likes.LikeDiscussionIds.splice(removeIndex, 1);
        }

        public DeleteComment() {
            var comment = this.targetComment;
            var removeIndex = this.comment.Comments.indexOf(comment);
            if (removeIndex > -1) this.comment.Comments.splice(removeIndex, 1);
            this.commentSvc.UpdateComment(this.classRoomId, this.lessonId, comment.id, true, null);
        }

        public DeleteDiscussion() {
            var commentId = this.targetComment.id;
            var discussion = this.targetDiscussion;
            var removeIndex = this.discussions.indexOf(discussion);
            if (removeIndex > -1) this.discussions.splice(removeIndex, 1);
            this.comment.Comments.filter(it=> it.id == commentId)[0].TotalDiscussions--;
            this.discussionSvc.UpdateDiscussion(this.classRoomId, this.lessonId, commentId, discussion.id, true, null);
        }

        public EditComment(commentId: string, message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return;

            this.commentSvc.UpdateComment(this.classRoomId, this.lessonId, commentId, false, message);
        }

        public EditDiscussion(commentId: string, discussionId: string, message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return;

            this.discussionSvc.UpdateDiscussion(this.classRoomId, this.lessonId, commentId, discussionId, false, message);
        }

        public LikeLesson() {
            this.likes.IsLikedLesson = !this.likes.IsLikedLesson;
            this.content.TotalLikes++;
            this.lessonSvc.LikeLesson(this.classRoomId, this.lessonId);
        }

        public DisLikeLesson() {
            this.likes.IsLikedLesson = !this.likes.IsLikedLesson;
            this.content.TotalLikes--;
            this.lessonSvc.LikeLesson(this.classRoomId, this.lessonId);
        }

        public ReadNote() {
            this.lessonSvc.ReadNote(this.classRoomId);
        }

        public EditOpen(message: any) {
            this.message = message.Description;
            this.EditId = message.id;
        }

        public SaveEdit(messageId: number) {
            this.comment.Comments.filter(it=> it.id == messageId)[0].Description = this.message;
            this.EditComment(this.comment.Comments.filter(it=> it.id == messageId)[0].id, this.message);
            this.EditId = null;
        }

        public SaveEditDiscus(commentId: string, messageId: number) {

            this.discussions.filter(it=> it.id == messageId)[0].Description = this.message;
            this.EditDiscussion(commentId, this.discussions.filter(it=> it.id == messageId)[0].id, this.message);
            this.EditId = null;
        }

        public CancelEdit() {
            this.EditId = null;
        }

        public IsEdit(id: string) {
            return this.EditId == id;
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
        .module('app.lessons')
        .controller('app.lessons.LessonController', LessonController);
}