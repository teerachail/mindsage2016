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
    }

    angular
        .module('app.shared')
        .controller('app.shared.LessonLayoutController', LessonLayoutController)
        .controller('app.shared.CourseLayoutController', CourseLayoutController);
}