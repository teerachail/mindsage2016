angular.module('app', ['ui.router'])
    .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $stateProvider.state('app', {
            url: '/app',
            template: '<h2>Hello?</h2>'
        });
        $urlRouterProvider.otherwise('/app');
    }]);
var mod;
//# sourceMappingURL=appBundle.js.map