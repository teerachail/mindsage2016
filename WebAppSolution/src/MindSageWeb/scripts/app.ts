(function () {
    'use strict';

    angular.module('app', [
        'ui.router',
        'ngAnimate',

        //foundation
        'foundation',
        'foundation.dynamicRouting',
        'foundation.dynamicRouting.animations',

        'foundation.accordion',

        //custom controllers
        'app.shared',
        'app.preparings',
        'app.lessons',
        'app.studentlists',
        'app.coursemaps',
        'app.notification',
        'app.journals',
        'app.teacherlists',
        'app.sidemenus',
        'app.settings',
        'app.main',
        'app.calendar'
    ])
        .config(config)
        .run(run)
        ;

    config.$inject = ['$urlRouterProvider', '$locationProvider'];

    function config($urlProvider, $locationProvider) {
        $urlProvider.otherwise('/');

        $locationProvider.html5Mode({
            enabled: false,
            requireBase: false
        });

        $locationProvider.hashPrefix('!');
    }

    function run() {
        FastClick.attach(document.body);
    }

})();
