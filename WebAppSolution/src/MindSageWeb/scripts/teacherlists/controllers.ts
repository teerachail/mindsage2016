module app.teacherlists {
    'use strict';

    class teacherlistsController {
        public targetId: any;

        static $inject = ['$scope', 'list', 'classRoomId', 'app.teacherlists.TeacherListService'];
        constructor(private $scope, public list: any[], public classRoomId: string, private teacherlistsSvc: app.teacherlists.TeacherListService) {
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