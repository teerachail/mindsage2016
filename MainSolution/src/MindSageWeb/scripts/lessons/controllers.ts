module app.lessons {
    'use strict';

    class LessonController {

        public teacherView: boolean;
        public currentUser: any;
        public openDiscussion: string;
        public discussions = [];

        static $inject = ['$scope', 'content', 'classRoomId', 'lessonId', 'comment', 'app.shared.ClientUserProfileService', 'app.shared.DiscussionService', 'app.shared.CommentService'];
        constructor(private $scope, public content, public classRoomId: string, public lessonId: string, public comment, private userprofileSvc: app.shared.ClientUserProfileService, private discussionSvc: app.shared.DiscussionService, private commentSvc: app.shared.CommentService) {
            this.teacherView = false;
            this.currentUser = this.userprofileSvc.GetClientUserProfile();
        }

        public selectTeacherView(): void {
            this.teacherView = true;
        }

        public selectStdView(): void {
            this.teacherView = false;
        }

        public showDiscussion(item: any): void {
            this.openDiscussion = item.id;
            this.GetDiscussions(item.id);
        }

        public hideDiscussion(): void {
            this.openDiscussion = "";
        }

        public GetDiscussions(commentId: string) {
            this.discussionSvc
                .GetDiscussions(this.lessonId, this.classRoomId, commentId)
                .then(it=> this.discussions = it);
        }

        public CreateNewComment(message: string) {
            this.commentSvc.CreateNewComment(this.classRoomId, this.lessonId, message);
        }

        public CreateNewDiscussion(commentId: string, message: string) {
            alert('CommentId:' + commentId + ', Message:' + message);
        }

    }

    angular
        .module('app.lessons')
        .controller('app.lessons.LessonController', LessonController);
}