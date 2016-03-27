module app.lessonpreviews {
    'use strict';

    class LessonPreviewController {

        public teacherView;
        private content;
        private PrimaryVideoUrl;

        static $inject = ['$sce', '$q', '$stateParams', 'defaultUrl', 'app.lessonpreviews.LessonService'];
        constructor(private $sce, private $q, private $stateParams, private defaultUrl, private svc: app.lessonpreviews.LessonService) {
            this.prepareLessonContents();
        }

        private prepareLessonContents(): void {
            var lessonId = this.$stateParams.lessonId;
            this.svc.GetContent(lessonId).then(
                it => {
                    this.content = it;
                    this.PrimaryVideoUrl = this.$sce.trustAsResourceUrl(it.PrimaryContentURL);
                }                    ,
                error => console.log('Error: ' + error));
            (<any>angular.element(".owl-carousel")).owlCarousel({
                autoPlay: true,
                slideSpeed: 300,
                jsonPath: this.defaultUrl + '/api/lesson/' + lessonId + '/lessonpreviewads',
                singleItem: true
            });
        }
        
        public selectTeacherView(): void {
            this.teacherView = true;
        }

        public selectStdView(): void {
            this.teacherView = false;
        }
    } 

    angular
        .module('app.lessonpreviews')
        .controller('app.lessonpreviews.LessonPreviewController', LessonPreviewController);
}