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
        public EditId: string;
        private requestedCommentIds = [];


        private content: any;
        private targetUserId: any;
        private likes: any;
        private isWaittingForGetJournalContent: boolean;
        private isPrepareJournalContentComplete: boolean;

        static $inject = ['$scope', '$q', 'waitRespondTime', '$stateParams', 'app.shared.ClientUserProfileService', 'app.shared.DiscussionService', 'app.shared.CommentService', 'app.lessons.LessonService', 'app.journals.JournalService', 'app.shared.GetProfileService'];
        constructor(private $scope, private $q, private waitRespondTime, private $stateParams, private svc: app.shared.ClientUserProfileService, private discussionSvc: app.shared.DiscussionService, private commentSvc: app.shared.CommentService, private lessonSvc: app.lessons.LessonService, private journalSvc: app.journals.JournalService, private profileSvc: app.shared.GetProfileService) {
            this.targetUserId = this.$stateParams.targetUserId;
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            if (!this.svc.IsPrepareAllUserProfileCompleted()) {
                setTimeout(it => this.prepareUserprofile(), this.waitRespondTime);
                return;
            }

            this.userprofile = this.svc.GetClientUserProfile();
            this.prepareJournalContents();
        }
        private prepareJournalContents(): void {
            var shouldRequestJournalContent = !this.isPrepareJournalContentComplete && !this.isWaittingForGetJournalContent;
            if (shouldRequestJournalContent) {
                this.isWaittingForGetJournalContent = true;
                this.$q.all([
                    this.profileSvc.GetAllLike(),
                    this.journalSvc.GetComments(this.$stateParams.classRoomId, this.$stateParams.targetUserId),
                ]).then(data => {
                    this.likes = data[0];
                    this.content = data[1];
                    this.isWaittingForGetJournalContent = false;
                    this.isPrepareJournalContentComplete = true;
                }, error => {
                    console.log('Load journal content failed, retrying ...');
                    this.isWaittingForGetJournalContent = false;
                    setTimeout(it=> this.prepareJournalContents(), this.waitRespondTime);
                });
            }
        }

        public GetWeeks() {
            var usedWeekNo = {};
            var lessonWeeks = [];

            if (this.MyComments.length != 0) {
                var NewCommentlessonWeekNo = this.userprofile.CurrentLessonNo;
                lessonWeeks.push(NewCommentlessonWeekNo);
                usedWeekNo[NewCommentlessonWeekNo] = 1;
            }

            var totalComments = this.isPrepareJournalContentComplete ? this.content.Comments.length : 0;
            for (var index = 0; index < totalComments; index++) {

                var lessonWeekNo = this.content.Comments[index].LessonWeek;
                if (usedWeekNo.hasOwnProperty(lessonWeekNo)) continue;
                lessonWeeks.push(lessonWeekNo);
                usedWeekNo[lessonWeekNo] = 1;
            }
            return lessonWeeks;
        }

        public GetComments(week: number) {
            var comments = this.isPrepareJournalContentComplete ? this.content.Comments : [];
            var qry = comments.filter(it=> it.LessonWeek == week);
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
                        if (this.discussions.filter(dis => dis.id == it[index].id).length == 0)
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

            var local
            if (this.content.Comments.filter(it=> it.id == commentId).length == 0)
                local = this.MyComments.filter(it=> it.id == commentId)[0];
            else
                local = this.content.Comments.filter(it=> it.id == commentId)[0];

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
            if (this.content.Comments.filter(it=> it.id == commentId).length == 0) {
                if (IsLike == -1)
                    this.MyComments.filter(it=> it.id == commentId)[0].TotalLikes++;
                else
                    this.MyComments.filter(it=> it.id == commentId)[0].TotalLikes--;
            }
            else {
                if (IsLike == -1)
                    this.content.Comments.filter(it=> it.id == commentId)[0].TotalLikes++;
                else
                    this.content.Comments.filter(it=> it.id == commentId)[0].TotalLikes--;
            }

            var setIndex = this.likes.LikeCommentIds.indexOf(commentId);
            const ElementNotFound = -1;
            if (setIndex <= ElementNotFound) this.likes.LikeCommentIds.push(commentId);
            else this.likes.LikeCommentIds.splice(setIndex, 1);

            var ClassRoomId = this.userprofile.CurrentClassRoomId;
            var LessonId = this.userprofile.CurrentLessonId;
            
            this.commentSvc.LikeComment(ClassRoomId, LessonId, commentId);

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

        public EditOpen(message: any) {
            this.message = message.Description;
            this.EditId = message.id;
        }

        public SaveEdit(messageId: number) {
            this.content.Comments.filter(it=> it.id == messageId)[0].Description = this.message;
            this.EditComment(this.content.Comments.filter(it=> it.id == messageId)[0].id, this.message);
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
        .module('app.journals')
        .controller('app.journals.JournalController', JournalController);
}