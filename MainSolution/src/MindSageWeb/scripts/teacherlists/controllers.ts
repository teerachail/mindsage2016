module app.teacherlists {
    'use strict';

    class teacherlistsController {
        static $inject = ['$scope', 'list', 'classRoomId', 'app.teacherlists.TeacherListService'];
        constructor(private $scope, public list, public classRoomId: string, private teacherlistsSvc: app.teacherlists.TeacherListService) {
        }

        public RemoveStd(removeId: string) {
            this.teacherlistsSvc.RemoveStudent(this.classRoomId, removeId);
        }

    }

    angular
        .module('app.teacherlists')
        .controller('app.teacherlists.teacherlistsController', teacherlistsController);
}