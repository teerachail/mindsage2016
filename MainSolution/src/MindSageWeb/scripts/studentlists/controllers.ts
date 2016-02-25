module app.studentlists {
    'use strict';

    class studentlistsController {
        static $inject = ['$scope', 'list'];
        constructor(private $scope, public list) {
        }

    }

    angular
        .module('app.studentlists')
        .controller('app.studentlists.studentlistsController', studentlistsController);
}