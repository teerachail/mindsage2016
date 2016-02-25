module app.courses {
    'use strict';

    class CourseMapController {

        static $inject = ['$scope', 'content'];
        constructor(private $scope, public content) {
        }

    }

    angular
        .module('app.courses')
        .controller('app.courses.CourseMapController', CourseMapController);
}