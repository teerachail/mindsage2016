var module = angular.module('appDirectives', [])
    .directive('onFinishRender', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope: any, element, attr) {
                if (scope.$last === true) {
                    $timeout(function () {
                        scope.$emit('ngRepeatFinished');
                        (<any>$(document)).foundation();
                    }, 100);
                }
            }
        }
    });