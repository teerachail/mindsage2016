module app.journals {
    'use strict';

    class JournalController {

        static $inject = ['$scope', 'content'];
        constructor(private $scope, public content) {
        }

    }

    angular
        .module('app.journals')
        .controller('app.journals.JournalController', JournalController);
}