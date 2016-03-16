module app.coursemaps {
    'use strict';

    class CourseMapController {

        private userProfile: any;

        static $inject = ['$scope','$state', 'content', 'status', 'app.shared.ClientUserProfileService'];
        constructor(private $scope, private $state, public content, public status, private userSvc: app.shared.ClientUserProfileService) {
            this.userProfile = userSvc.GetClientUserProfile();
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