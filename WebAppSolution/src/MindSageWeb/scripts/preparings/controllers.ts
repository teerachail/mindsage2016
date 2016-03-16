module preparings.main {
    'use strict';

    class PreparingController {

        static $inject = ['$state', 'app.shared.ClientUserProfileService'];
        constructor(private $state, private userSvc: app.shared.ClientUserProfileService) {
            this.prepareUserProfile();
        }

        private prepareUserProfile(): void {
            var isCompleted = this.userSvc.IsPrepareAllUserProfileCompleted();
            if (!isCompleted) {
                setTimeout(it => this.prepareUserProfile(), 777);
                return;
            }

            var userprofile = this.userSvc.GetClientUserProfile();
            var lessonId = userprofile.CurrentLessonId;
            var classRoomId = userprofile.CurrentClassRoomId;
            this.$state.go('app.main.lesson', { 'lessonId': lessonId, 'classRoomId': classRoomId }, { 'location': 'replace' });
        }
    }

    angular
        .module('app.preparings')
        .controller('app.preparings.PreparingController', PreparingController);
}