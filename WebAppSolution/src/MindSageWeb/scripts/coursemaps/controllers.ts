module app.coursemaps {
    'use strict';

    class CourseMapController {

        private content;
        private status;

        static $inject = ['$scope', '$state', '$q', '$stateParams', 'app.shared.ClientUserProfileService', 'app.coursemaps.CourseMapService'];
        constructor(private $scope, private $state, private $q, private $stateParams, private userSvc: app.shared.ClientUserProfileService, private coursemapSvc: app.coursemaps.CourseMapService) {
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            this.userSvc.PrepareAllUserProfile().then(() => {
                this.prepareCourseMapContents();
            });
        }

        private prepareCourseMapContents(): void {
            this.$q.all([
                this.coursemapSvc.GetContent(this.$stateParams.classRoomId),
                this.coursemapSvc.GetLessonStatus(this.$stateParams.classRoomId),
            ]).then(data => {
                this.content = data[0];
                this.status = data[1];
            }, error => {
                console.log('Load journal content failed, retrying ...');
            });
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