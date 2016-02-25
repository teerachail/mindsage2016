module app.teacherlists {
    'use strict';

    class teacherlistsController {
        static $inject = ['$scope', 'list'];
        constructor(private $scope, public list) {
        }

    }

    angular
        .module('app.teacherlists')
        .controller('app.teacherlists.teacherlistsController', teacherlistsController);
}