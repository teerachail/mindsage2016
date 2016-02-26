module app.lessons {
    'use strict';

    class LessonController {

        public teacherView: boolean;
        public currentUser: any;

        static $inject = ['$scope', 'content', 'comment', 'app.shared.ClientUserProfileService'];
        constructor(private $scope, public content, public comment, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.teacherView = this.content.IsTeacher;
            this.currentUser = this.userprofileSvc.GetClientUserProfile();
        }

        public selectTeacherView(): void {
            this.teacherView = true;
        }

        public selectStdView(): void {
            this.teacherView = false;
        }

        //public showDiscussion(commentId: string): Array<DiscussionList> {
        //    return new DiscussionList[];
        //}

    }

    angular
        .module('app.lessons')
        .controller('app.lessons.LessonController', LessonController);
}