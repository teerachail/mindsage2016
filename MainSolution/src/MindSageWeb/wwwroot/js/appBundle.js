angular.module('app', ['ui.router'])
    .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('app', {
            url: '/app',
            templateUrl: 'tmpl/layout.html'
        })
            .state('app.main', {
            url: '/main',
            views: {
                'mainContent': {
                    templateUrl: 'tmpl/lesson_layout.html'
                }
            }
        })
            .state('app.main.lesson', {
            url: '/lesson/:lessonid',
            views: {
                'lessonContent': {
                    templateUrl: 'tmpl/lesson.html'
                }
            }
        });
        $urlRouterProvider.otherwise('/app/main/lesson/111');
    }]);
var mod;
//# sourceMappingURL=appBundle.js.map