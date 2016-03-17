module app.coursemaps {
    'use strict';

    class CourseMapController {

        private userProfile: any;
        private content;
        private status;
        private isWaittingForGetCourseMapContent: boolean;
        private isPrepareCourseMapContentComplete: boolean;

        static $inject = ['$scope', '$state', '$q', '$stateParams', 'waitRespondTime', 'app.shared.ClientUserProfileService', 'app.coursemaps.CourseMapService'];
        constructor(private $scope, private $state, private $q, private $stateParams, private waitRespondTime, private userSvc: app.shared.ClientUserProfileService, private coursemapSvc: app.coursemaps.CourseMapService) {
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            if (!this.userSvc.IsPrepareAllUserProfileCompleted()) {
                setTimeout(it => this.prepareUserprofile(), this.waitRespondTime);
                return;
            }

            this.userProfile = this.userSvc.GetClientUserProfile();
            this.prepareCourseMapContents();
        }
        private prepareCourseMapContents(): void {
            var shouldRequestJournalContent = !this.isPrepareCourseMapContentComplete && !this.isWaittingForGetCourseMapContent;
            if (shouldRequestJournalContent) {
                this.isWaittingForGetCourseMapContent = true;
                this.$q.all([
                    this.coursemapSvc.GetContent(this.$stateParams.classRoomId),
                    this.coursemapSvc.GetLessonStatus(this.$stateParams.classRoomId),
                ]).then(data => {
                    this.content = data[0];
                    this.status = data[1];
                    this.isWaittingForGetCourseMapContent = false;
                    this.isPrepareCourseMapContentComplete = true;
                }, error => {
                    console.log('Load journal content failed, retrying ...');
                    this.isWaittingForGetCourseMapContent = false;
                    setTimeout(it=> this.prepareCourseMapContents(), this.waitRespondTime);
                });
            }
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