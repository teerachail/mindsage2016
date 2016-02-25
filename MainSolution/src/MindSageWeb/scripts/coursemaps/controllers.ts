module app.coursemaps {
    'use strict';

    class CourseMapController {

        static $inject = ['$scope', 'content', 'status'];
        constructor(private $scope, public content, public status) {
        }

        public HaveAnyComments(lessonId: string): boolean {
            var qry = this.status.filter(it=> it.LessonId == lessonId);
            if (qry.length <= 0) return false;
            else return qry[0].HaveAnyComments;
        }

        public HaveReadAllContents(lessonId: string): boolean {
            var qry = this.status.filter(it=> it.LessonId == lessonId);
            if (qry.length <= 0) return false;
            else return qry[0].IsReadedAllContents;
        }

    }

    angular
        .module('app.coursemaps')
        .controller('app.coursemaps.CourseMapController', CourseMapController);
}