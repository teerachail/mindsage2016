module app.lessons {
    'use strict';

    class LessonController {

        static $inject = ['$scope', 'content'];
        constructor(private $scope, public content) {
        }
        
    }

    angular
        .module('app.lessons')
        .controller('app.lessons.LessonController', LessonController);
}