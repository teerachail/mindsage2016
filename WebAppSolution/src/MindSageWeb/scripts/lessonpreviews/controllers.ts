module app.lessonpreviews {
    'use strict';

    class LessonPreviewController {

        public teacherView;
        private content;
        private PrimaryVideoUrl;

        static $inject = ['$sce', '$q', '$stateParams', 'app.lessonpreviews.LessonService'];
        constructor(private $sce, private $q, private $stateParams, private svc: app.lessonpreviews.LessonService) {
            this.prepareLessonContents();
        }

        private prepareLessonContents(): void {
            this.svc.GetContent(this.$stateParams.lessonId).then(
                it => {
                    this.content = it;
                },
                error => {
                    console.log('Error: ' + error);
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