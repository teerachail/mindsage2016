module app.teacherlists {
    'use strict';

    class teacherlistsController {
        static $inject = ['$scope', 'list', 'classRoomId', 'app.teacherlists.TeacherListService'];
        constructor(private $scope, public list: any[], public classRoomId: string, private teacherlistsSvc: app.teacherlists.TeacherListService) {
        }

        public RemoveStd(removeObj: any) {
            this.teacherlistsSvc.RemoveStudent(this.classRoomId, removeObj.id);

            var removeIndex = this.list.indexOf(removeObj);
            this.list.splice(removeIndex, 1);
        }

    }

    angular
        .module('app.teacherlists')
        .controller('app.teacherlists.teacherlistsController', teacherlistsController);
}