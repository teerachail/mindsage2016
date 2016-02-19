angular.module('app', ['ui.router'])
    .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider: angular.ui.IStateProvider, $urlRouterProvider: angular.ui.IUrlRouterProvider) {
        $stateProvider.state('app', {
            url: '/app',
            template: '<h2>Hello?</h2>'
        });
        $urlRouterProvider.otherwise('/app');
    }]);