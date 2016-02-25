module app.studentlists {
    'use strict';

    class studentlistsController {
        static $inject = ['$scope', 'students'];
        constructor(private $scope, public student) {
        }

    }

    angular
        .module('app.studentlists')
        .controller('app.studentlists.studentlistsController', studentlistsController);
}