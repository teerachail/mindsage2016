module app.teacherlists {
    'use strict';

    class teacherlistsController {

        public targetId: any;
        private list: any[] = [];
        private classRoomId: string;

        static $inject = ['$scope', '$stateParams', 'app.shared.ClientUserProfileService', 'app.teacherlists.TeacherListService'];
        constructor(private $scope, private $stateParams, private userprofileSvc: app.shared.ClientUserProfileService, private teacherlistsSvc: app.teacherlists.TeacherListService) {
            this.classRoomId = $stateParams.classRoomId;
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            this.userprofileSvc.PrepareAllUserProfile().then(() => {
                this.prepareTeacherListContents();
            });
        }

        private prepareTeacherListContents(): void {
            this.teacherlistsSvc.GetStudentList(this.classRoomId).then(respond => {
                if (respond != null) this.list = respond;
            }, error => {
                console.log('Load TeacherList content failed');
            });
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