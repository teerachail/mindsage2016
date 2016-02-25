module app.lessons {
    'use strict';

    class LessonController {
        public teacherView: boolean;
        static $inject = ['$scope', 'content', 'comment'];
        constructor(private $scope, public content, public comment) {
            this.teacherView = this.content.IsTeacher;
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