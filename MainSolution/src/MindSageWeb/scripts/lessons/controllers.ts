module app.lessons {
    'use strict';

    class LessonController {

        public teacherView: boolean;
        public currentUser: any;
        public openDiscussion: string;

        static $inject = ['$scope', 'content', 'comment', 'app.shared.ClientUserProfileService', 'app.shared.DiscussionService'];
        constructor(private $scope, public content, public comment, private userprofileSvc: app.shared.ClientUserProfileService, private discussionSvc: app.shared.DiscussionService) {
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
        }

        public hideDiscussion(): void {
            this.openDiscussion = "";
        }
        
        public GetDiscussions(commentId: string) {
            return [];
            //return this.discussionSvc
            //    .GetDiscussions('Lesson01', 'ClassRoom01', commentId)
            //    .then(it=> it);
        }

        public CreateAComment(message: string) {
           // TODO: Create a comment
        }

    }

    angular
        .module('app.lessons')
        .controller('app.lessons.LessonController', LessonController);
}