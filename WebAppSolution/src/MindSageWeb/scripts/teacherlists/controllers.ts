module app.teacherlists {
    'use strict';

    class teacherlistsController {

        public targetId: any;
        private list: any[] = [];
        private classRoomId: string;
        private isWaittingForGetTeacherListContent: boolean;
        private isPrepareTeacherListContentComplete: boolean;

        static $inject = ['$scope', 'waitRespondTime', '$stateParams', 'app.shared.ClientUserProfileService', 'app.teacherlists.TeacherListService'];
        constructor(private $scope, private waitRespondTime, private $stateParams, private userprofileSvc: app.shared.ClientUserProfileService, private teacherlistsSvc: app.teacherlists.TeacherListService) {
            this.classRoomId = $stateParams.classRoomId;
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            if (!this.userprofileSvc.IsPrepareAllUserProfileCompleted()) {
                setTimeout(it => this.prepareUserprofile(), this.waitRespondTime);
                return;
            }

            this.prepareTeacherListContents();
        }

        private prepareTeacherListContents(): void {
            var shouldRequestLessonContent = !this.isPrepareTeacherListContentComplete && !this.isWaittingForGetTeacherListContent;
            if (shouldRequestLessonContent) {
                this.isWaittingForGetTeacherListContent = true;
                this.teacherlistsSvc.GetStudentList(this.classRoomId).then(respond => {
                    if (respond != null) this.list = respond;
                    this.isWaittingForGetTeacherListContent = false;
                    this.isPrepareTeacherListContentComplete = true;
                }, error => {
                    console.log('Load TeacherList content failed, retrying ...');
                    this.isWaittingForGetTeacherListContent = false;
                    setTimeout(it => this.prepareTeacherListContents(), this.waitRespondTime);
                });
            }
        }

        public targetStd(Std: any) {
            this.targetId = Std;
        }

        public RemoveStd() {
            this.teacherlistsSvc.RemoveStudent(this.classRoomId, this.targetId.id);

            var removeIndex = this.list.indexOf(this.targetId);
            this.list.splice(removeIndex, 1);
        }

    }

    angular
        .module('app.teacherlists')
        .controller('app.teacherlists.teacherlistsController', teacherlistsController);
}