module app.lessons {
    'use strict';

    class LessonController {

        public teacherView: boolean;
        public currentUser: any;
        public message: string;
        public discussions = [];
        private requestedCommentIds = [];

        static $inject = ['$scope', 'content', 'classRoomId', 'lessonId', 'comment', 'app.shared.ClientUserProfileService', 'app.shared.DiscussionService', 'app.shared.CommentService', 'app.lessons.LessonService'];
        constructor(private $scope, public content, public classRoomId: string, public lessonId: string, public comment, private userprofileSvc: app.shared.ClientUserProfileService, private discussionSvc: app.shared.DiscussionService, private commentSvc: app.shared.CommentService, private lessonSvc: app.lessons.LessonService) {
            this.teacherView = false;
            this.currentUser = this.userprofileSvc.GetClientUserProfile();
            this.userprofileSvc.Advertisments = this.content.Advertisments;
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
                        this.discussions.push(it[index]);
                    }
                });
        }

        public CreateNewComment(message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return;

            this.commentSvc.CreateNewComment(this.classRoomId, this.lessonId, message);
        }

        public CreateNewDiscussion(commentId: string, message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return;

            this.discussionSvc.CreateDiscussion(this.classRoomId, this.lessonId, commentId, message);
        }

        public LikeComment(commentId: string) {
            this.commentSvc.LikeComment(this.classRoomId, this.lessonId, commentId);
        }

        public LikeDiscussion(commentId: string, discussionId: string) {
            this.discussionSvc.LikeDiscussion(this.classRoomId, this.lessonId, commentId, discussionId);
        }

        public DeleteComment(commentId: string) {
            this.commentSvc.UpdateComment(this.classRoomId, this.lessonId, commentId, true, null);
        }

        public DeleteDiscussion(commentId: string, discussionId: string) {
            this.discussionSvc.UpdateDiscussion(this.classRoomId, this.lessonId, commentId, discussionId, true, null);
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
            this.lessonSvc.LikeLesson(this.classRoomId, this.lessonId);
        }

        public ReadNote() {
            this.lessonSvc.ReadNote(this.classRoomId);
        }

        public EditOpen(message: string, open: boolean) {
            this.message = message;
            return !open;
        }

        public SaveEdit(messageId: number, save: boolean) {
            this.comment.Comments.filter(it=> it.id == messageId)[0].Description = this.message;
            this.EditComment(this.comment.Comments.filter(it=> it.id == messageId)[0].id, this.message);
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

    } 

    angular
        .module('app.lessons')
        .controller('app.lessons.LessonController', LessonController);
}