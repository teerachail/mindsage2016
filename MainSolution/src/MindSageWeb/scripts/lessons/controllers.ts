module app.lessons {
    'use strict';

    class LessonController {
        public teacherView: boolean;
        static $inject = ['$scope', 'content'];
        constructor(private $scope, public content) {
            this.teacherView = this.content.IsTeacher;
        }

        public selectTeacherView(): void {
            this.teacherView = true;
        }

        public selectStdView(): void {
            this.teacherView = false;
        }

    }

    angular
        .module('app.lessons')
        .controller('app.lessons.LessonController', LessonController);
}