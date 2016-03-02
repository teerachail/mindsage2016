module app.shared {
    'use strict';

    class LessonLayoutController {
        public RunningVideoUrl: string;

        static $inject = ['$sce'];
        constructor(private $sce) {
        }

        public ChangeVideo(url: string) {
            this.RunningVideoUrl = this.$sce.trustAsResourceUrl(url);
        }
    }

    class CourseLayoutController {

        private ads: any;

        static $inject = ['app.shared.ClientUserProfileService'];
        constructor(private clientUserProfileSvc: app.shared.ClientUserProfileService) {
            this.ads = this.clientUserProfileSvc.Advertisments;
        }
    }

    angular
        .module('app.shared')
        .controller('app.shared.LessonLayoutController', LessonLayoutController)
        .controller('app.shared.CourseLayoutController', CourseLayoutController);
}