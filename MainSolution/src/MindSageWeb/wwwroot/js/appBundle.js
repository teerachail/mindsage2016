angular.module('app', ['ui.router', 'app.shared', 'app.lessons'])
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
            url: '/lesson/:lessonId/:classRoomId',
            views: {
                'lessonContent': {
                    templateUrl: 'tmpl/lesson_student.html',
                    controller: 'app.lessons.LessonController as cx',
                    resolve: {
                        'content': ['$stateParams', 'app.lessons.LessonService',
                            function (params, svc) { svc.GetContent(params.lessonId, params.classRoomId); }]
                    }
                }
            }
        })
            .state('app.main.details', {
            url: '/details',
            views: {
                'lessonContent': {
                    templateUrl: 'tmpl/lesson_teacher.html'
                }
            }
        })
            .state('app.course', {
            url: '/course/:courseId',
            views: {
                'mainContent': {
                    templateUrl: 'tmpl/course_layout.html'
                }
            }
        })
            .state('app.course.nitification', {
            url: '/nitification',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/notification.html'
                }
            }
        })
            .state('app.course.journal', {
            url: '/journal/:userid',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/journal.html'
                }
            }
        })
            .state('app.course.coursemap', {
            url: '/coursemap',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/coursemap.html'
                }
            }
        })
            .state('app.course.friendlist', {
            url: '/friendlist',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/friendlist.html'
                }
            }
        })
            .state('app.course.studentlist', {
            url: '/studentlist',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/studentlist.html'
                }
            }
        })
            .state('app.course.config', {
            url: '/setting',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/setting.html'
                }
            }
        });
        $urlRouterProvider.otherwise('/app/main/lesson/Lesson01/ClassRoom01');
    }]);
(function () {
    'use strict';
    angular
        .module('app.lessons', [
        "ngResource",
        'app.shared'
    ]);
})();
(function () {
    'use strict';
    angular
        .module('app.shared', [
        "ngResource"
    ]);
})();
var app;
(function (app) {
    'use strict';
    var AppConfig = (function () {
        function AppConfig(defaultUrl) {
            var apiUrl = defaultUrl + '/api';
            this.LessonUrl = apiUrl + '/lesson/:id/:classRoomId/:userId';
        }
        AppConfig.$inject = ['defaultUrl'];
        return AppConfig;
    })();
    app.AppConfig = AppConfig;
    // HACK: Change the host Url
    angular
        .module('app')
        .constant('defaultUrl', 'http://localhost:4147')
        .service('appConfig', AppConfig);
})(app || (app = {}));
var app;
(function (app) {
    var lessons;
    (function (lessons) {
        'use strict';
        var LessonController = (function () {
            function LessonController($scope, content) {
                this.$scope = $scope;
                this.content = content;
            }
            LessonController.$inject = ['$scope', 'content'];
            return LessonController;
        })();
        angular
            .module('app.lessons')
            .controller('app.lessons.LessonController', LessonController);
    })(lessons = app.lessons || (app.lessons = {}));
})(app || (app = {}));
var app;
(function (app) {
    var lessons;
    (function (lessons) {
        'use strict';
        var LessonContentRequest = (function () {
            function LessonContentRequest(id, classRoomId, userId) {
                this.id = id;
                this.classRoomId = classRoomId;
                this.userId = userId;
            }
            return LessonContentRequest;
        })();
        lessons.LessonContentRequest = LessonContentRequest;
        var LessonContentRespond = (function () {
            function LessonContentRespond() {
            }
            return LessonContentRespond;
        })();
        lessons.LessonContentRespond = LessonContentRespond;
    })(lessons = app.lessons || (app.lessons = {}));
})(app || (app = {}));
var app;
(function (app) {
    var lessons;
    (function (lessons) {
        'use strict';
        var LessonService = (function () {
            function LessonService(appConfig, $resource, userprofileSvc) {
                this.$resource = $resource;
                this.userprofileSvc = userprofileSvc;
                this.svc = $resource(appConfig.LessonUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'userId': '@userId' }, {
                    GetContent: { method: 'GET' }
                });
            }
            LessonService.prototype.GetContent = function (id, classRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.svc.GetContent(new lessons.LessonContentRequest(id, classRoomId, userId)).$promise;
            };
            LessonService.$inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
            return LessonService;
        })();
        lessons.LessonService = LessonService;
        angular
            .module('app.lessons')
            .service('app.lessons.LessonService', LessonService);
    })(lessons = app.lessons || (app.lessons = {}));
})(app || (app = {}));
var app;
(function (app) {
    var shared;
    (function (shared) {
        'use strict';
        var ClientUserProfile = (function () {
            function ClientUserProfile() {
            }
            return ClientUserProfile;
        })();
        shared.ClientUserProfile = ClientUserProfile;
    })(shared = app.shared || (app.shared = {}));
})(app || (app = {}));
var app;
(function (app) {
    var shared;
    (function (shared) {
        'use strict';
        var ClientUserProfileService = (function () {
            function ClientUserProfileService() {
                // HACK: User profile information
                this.clientUserProfile = new shared.ClientUserProfile();
                this.clientUserProfile.UserProfileId = 'sakul@mindsage.com';
            }
            ClientUserProfileService.prototype.GetClientUserProfile = function () {
                return this.clientUserProfile;
            };
            return ClientUserProfileService;
        })();
        shared.ClientUserProfileService = ClientUserProfileService;
        angular
            .module('app.shared')
            .service('app.shared.ClientUserProfileService', ClientUserProfileService);
    })(shared = app.shared || (app.shared = {}));
})(app || (app = {}));
//# sourceMappingURL=appBundle.js.map