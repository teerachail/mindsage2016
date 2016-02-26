angular.module('app', ['ui.router', 'app.shared', 'app.lessons', 'app.studentlists', 'app.coursemaps', 'app.journals', 'app.teacherlists'])
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
                    templateUrl: 'tmpl/lesson.html',
                    controller: 'app.lessons.LessonController as cx',
                    resolve: {
                        'content': ['$stateParams', 'app.lessons.LessonService',
                            function (params, svc) { return svc.GetContent(params.lessonId, params.classRoomId); }],
                        'comment': ['$stateParams', 'app.lessons.LessonCommentService',
                            function (params, svc) { return svc.GetComments(params.lessonId, params.classRoomId); }]
                    }
                }
            }
        })
            .state('app.course', {
            url: '/course/:classRoomId',
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
            url: '/journal/:targetUserId',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/journal.html',
                    controller: 'app.journals.JournalController as cx',
                    resolve: {
                        'content': ['$stateParams', 'app.journals.JournalService',
                            function (params, svc) { return svc.GetComments(params.classRoomId, params.targetUserId); }]
                    }
                }
            }
        })
            .state('app.course.coursemap', {
            url: '/coursemap',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/coursemap.html',
                    controller: 'app.coursemaps.CourseMapController as cx',
                    resolve: {
                        'content': ['$stateParams', 'app.coursemaps.CourseMapService',
                            function (params, svc) { return svc.GetContent(params.classRoomId); }],
                        'status': ['$stateParams', 'app.coursemaps.CourseMapService',
                            function (params, svc) { return svc.GetLessonStatus(params.classRoomId); }]
                    }
                }
            }
        })
            .state('app.course.studentlist', {
            url: '/studentlist/:classRoomId',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/studentlist.html',
                    controller: 'app.studentlists.studentlistsController as cx',
                    resolve: {
                        'list': ['$stateParams', 'app.studentlists.StudentListService',
                            function (params, svc) { return svc.GetStudentList(params.classRoomId); }]
                    }
                }
            }
        })
            .state('app.course.teacherlist', {
            url: '/teacherlist/:classRoomId',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/teacherlist.html',
                    controller: 'app.teacherlists.teacherlistsController as cx',
                    resolve: {
                        'list': ['$stateParams', 'app.teacherlists.TeacherListService',
                            function (params, svc) { return svc.GetStudentList(params.classRoomId); }]
                    }
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
        .module('app.coursemaps', [
        "ngResource",
        'app.shared'
    ]);
})();
(function () {
    'use strict';
    angular
        .module('app.journals', [
        "ngResource",
        'app.shared'
    ]);
})();
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
(function () {
    'use strict';
    angular
        .module('app.studentlists', [
        "ngResource",
        'app.shared'
    ]);
})();
(function () {
    'use strict';
    angular
        .module('app.teacherlists', [
        "ngResource",
        'app.shared'
    ]);
})();
var app;
(function (app) {
    'use strict';
    var AppConfig = (function () {
        function AppConfig(defaultUrl) {
            var apiUrl = defaultUrl + '/api';
            this.LessonUrl = apiUrl + '/lesson/:id/:classRoomId/:userId';
            this.LessonCommentUrl = apiUrl + '/lesson/:id/:classRoomId/comments/:userId';
            this.CourseMapContentUrl = apiUrl + '/mycourse/:id/:classRoomId/:action';
            this.LessonDiscussionUrl = apiUrl + '/lesson/:id/:classRoomId/discussions/:userId';
            this.StudentListUrl = apiUrl + '/friend/:userId/:classRoomId';
            this.JournalCommentUrl = apiUrl + '/journal/:targetUserId/:requestByUserId/:classRoomId';
            this.TeacherListUrl = apiUrl + '/mycourse/:userId/:classRoomId/students';
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
    var coursemaps;
    (function (coursemaps) {
        'use strict';
        var CourseMapController = (function () {
            function CourseMapController($scope, content, status) {
                this.$scope = $scope;
                this.content = content;
                this.status = status;
            }
            CourseMapController.prototype.HaveAnyComments = function (lessonId) {
                var qry = this.status.filter(function (it) { return it.LessonId == lessonId; });
                if (qry.length <= 0)
                    return false;
                else
                    return qry[0].HaveAnyComments;
            };
            CourseMapController.prototype.HaveReadAllContents = function (lessonId) {
                var qry = this.status.filter(function (it) { return it.LessonId == lessonId; });
                if (qry.length <= 0)
                    return false;
                else
                    return qry[0].IsReadedAllContents;
            };
            CourseMapController.$inject = ['$scope', 'content', 'status'];
            return CourseMapController;
        })();
        angular
            .module('app.coursemaps')
            .controller('app.coursemaps.CourseMapController', CourseMapController);
    })(coursemaps = app.coursemaps || (app.coursemaps = {}));
})(app || (app = {}));
var app;
(function (app) {
    var coursemaps;
    (function (coursemaps) {
        'use strict';
        var GetCourseMapContentRequest = (function () {
            function GetCourseMapContentRequest(id, classRoomId) {
                this.id = id;
                this.classRoomId = classRoomId;
            }
            return GetCourseMapContentRequest;
        })();
        coursemaps.GetCourseMapContentRequest = GetCourseMapContentRequest;
    })(coursemaps = app.coursemaps || (app.coursemaps = {}));
})(app || (app = {}));
var app;
(function (app) {
    var coursemaps;
    (function (coursemaps) {
        'use strict';
        var CourseMapService = (function () {
            function CourseMapService(appConfig, $resource, userprofileSvc) {
                this.$resource = $resource;
                this.userprofileSvc = userprofileSvc;
                this.svc = $resource(appConfig.CourseMapContentUrl, { 'id': '@id', 'classRoomId': '@classRoomId' }, {
                    GetContent: { method: 'GET', isArray: true, },
                    GetLessonStatus: { method: 'GET', isArray: true, params: { 'action': 'status' } },
                });
            }
            CourseMapService.prototype.GetContent = function (classRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.svc.GetContent(new coursemaps.GetCourseMapContentRequest(userId, classRoomId)).$promise;
            };
            CourseMapService.prototype.GetLessonStatus = function (classRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.svc.GetLessonStatus(new coursemaps.GetCourseMapContentRequest(userId, classRoomId)).$promise;
            };
            CourseMapService.$inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
            return CourseMapService;
        })();
        coursemaps.CourseMapService = CourseMapService;
        angular
            .module('app.coursemaps')
            .service('app.coursemaps.CourseMapService', CourseMapService);
    })(coursemaps = app.coursemaps || (app.coursemaps = {}));
})(app || (app = {}));
var app;
(function (app) {
    var journals;
    (function (journals) {
        'use strict';
        var JournalController = (function () {
            function JournalController($scope, content, svc) {
                this.$scope = $scope;
                this.content = content;
                this.svc = svc;
                this.userprofile = this.svc.GetClientUserProfile();
            }
            JournalController.prototype.GetWeeks = function () {
                var usedWeekNo = {};
                var lessonWeeks = [];
                for (var index = 0; index < this.content.Comments.length; index++) {
                    var lessonWeekNo = this.content.Comments[index].LessonWeek;
                    if (usedWeekNo.hasOwnProperty(lessonWeekNo))
                        continue;
                    lessonWeeks.push(lessonWeekNo);
                    usedWeekNo[lessonWeekNo] = 1;
                }
                return lessonWeeks;
            };
            JournalController.prototype.GetComments = function (week) {
                var qry = this.content.Comments.filter(function (it) { return it.LessonWeek == week; });
                return qry;
            };
            JournalController.$inject = ['$scope', 'content', 'app.shared.ClientUserProfileService'];
            return JournalController;
        })();
        angular
            .module('app.journals')
            .controller('app.journals.JournalController', JournalController);
    })(journals = app.journals || (app.journals = {}));
})(app || (app = {}));
var app;
(function (app) {
    var journals;
    (function (journals) {
        'use strict';
        var GetJournalCommentsRequest = (function () {
            function GetJournalCommentsRequest(targetUserId, requestByUserId, classRoomId) {
                this.targetUserId = targetUserId;
                this.requestByUserId = requestByUserId;
                this.classRoomId = classRoomId;
            }
            return GetJournalCommentsRequest;
        })();
        journals.GetJournalCommentsRequest = GetJournalCommentsRequest;
    })(journals = app.journals || (app.journals = {}));
})(app || (app = {}));
var app;
(function (app) {
    var journals;
    (function (journals) {
        'use strict';
        var JournalService = (function () {
            function JournalService(appConfig, $resource, userprofileSvc) {
                this.$resource = $resource;
                this.userprofileSvc = userprofileSvc;
                this.svc = $resource(appConfig.JournalCommentUrl, {
                    'targetUserId': '@targetUserId', 'requestByUserId': '@requestByUserId', 'classRoomId': '@classRoomId' }, {
                    GetComments: { method: 'GET' },
                });
            }
            JournalService.prototype.GetComments = function (classRoomId, targetUserId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.svc.GetComments(new journals.GetJournalCommentsRequest(targetUserId, userId, classRoomId)).$promise;
            };
            JournalService.$inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
            return JournalService;
        })();
        journals.JournalService = JournalService;
        angular
            .module('app.journals')
            .service('app.journals.JournalService', JournalService);
    })(journals = app.journals || (app.journals = {}));
})(app || (app = {}));
var app;
(function (app) {
    var lessons;
    (function (lessons) {
        'use strict';
        var LessonController = (function () {
            function LessonController($scope, content, comment, userprofileSvc) {
                this.$scope = $scope;
                this.content = content;
                this.comment = comment;
                this.userprofileSvc = userprofileSvc;
                this.teacherView = false;
                this.currentUser = this.userprofileSvc.GetClientUserProfile();
            }
            LessonController.prototype.selectTeacherView = function () {
                this.teacherView = true;
            };
            LessonController.prototype.selectStdView = function () {
                this.teacherView = false;
            };
            LessonController.prototype.showDiscussion = function (id) {
                this.openDiscussion = id;
            };
            LessonController.prototype.hideDiscussion = function (id) {
                this.openDiscussion = "";
            };
            LessonController.$inject = ['$scope', 'content', 'comment', 'app.shared.ClientUserProfileService'];
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
        var LessonCommentRequest = (function () {
            function LessonCommentRequest(id, classRoomId, userId) {
                this.id = id;
                this.classRoomId = classRoomId;
                this.userId = userId;
            }
            return LessonCommentRequest;
        })();
        lessons.LessonCommentRequest = LessonCommentRequest;
        var LessonDiscussionRequest = (function () {
            function LessonDiscussionRequest(id, classRoomId, commentId, userId) {
                this.id = id;
                this.classRoomId = classRoomId;
                this.commentId = commentId;
                this.userId = userId;
            }
            return LessonDiscussionRequest;
        })();
        lessons.LessonDiscussionRequest = LessonDiscussionRequest;
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
                this.svc = $resource(appConfig.LessonUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'userId': '@userId' });
            }
            LessonService.prototype.GetContent = function (id, classRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.svc.get(new lessons.LessonContentRequest(id, classRoomId, userId)).$promise;
            };
            LessonService.$inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
            return LessonService;
        })();
        lessons.LessonService = LessonService;
        var LessonCommentService = (function () {
            function LessonCommentService(appConfig, $resource, userprofileSvc) {
                this.$resource = $resource;
                this.userprofileSvc = userprofileSvc;
                this.svc = $resource(appConfig.LessonCommentUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'userId': '@userId' });
            }
            LessonCommentService.prototype.GetComments = function (id, classRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.svc.get(new lessons.LessonCommentRequest(id, classRoomId, userId)).$promise;
            };
            LessonCommentService.$inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
            return LessonCommentService;
        })();
        lessons.LessonCommentService = LessonCommentService;
        angular
            .module('app.lessons')
            .service('app.lessons.LessonService', LessonService)
            .service('app.lessons.LessonCommentService', LessonCommentService);
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
                this.clientUserProfile.ImageUrl = 'http://placehold.it/100x100';
                this.clientUserProfile.FullName = 'Sakul Jaruthanaset';
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
var app;
(function (app) {
    var studentlists;
    (function (studentlists) {
        'use strict';
        var studentlistsController = (function () {
            function studentlistsController($scope, list) {
                this.$scope = $scope;
                this.list = list;
            }
            studentlistsController.$inject = ['$scope', 'list'];
            return studentlistsController;
        })();
        angular
            .module('app.studentlists')
            .controller('app.studentlists.studentlistsController', studentlistsController);
    })(studentlists = app.studentlists || (app.studentlists = {}));
})(app || (app = {}));
var app;
(function (app) {
    var studentlists;
    (function (studentlists) {
        'use strict';
        var GetFriendListRequest = (function () {
            function GetFriendListRequest(userId, classRoomId) {
                this.userId = userId;
                this.classRoomId = classRoomId;
            }
            return GetFriendListRequest;
        })();
        studentlists.GetFriendListRequest = GetFriendListRequest;
    })(studentlists = app.studentlists || (app.studentlists = {}));
})(app || (app = {}));
var app;
(function (app) {
    var studentlists;
    (function (studentlists) {
        'use strict';
        var StudentListService = (function () {
            function StudentListService(appConfig, $resource, userprofileSvc) {
                this.$resource = $resource;
                this.userprofileSvc = userprofileSvc;
                this.svc = $resource(appConfig.StudentListUrl, { 'userId': '@userId', 'classRoomId': '@classRoomId' });
            }
            StudentListService.prototype.GetStudentList = function (classRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.svc.query(new studentlists.GetFriendListRequest(userId, classRoomId)).$promise;
            };
            StudentListService.$inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
            return StudentListService;
        })();
        studentlists.StudentListService = StudentListService;
        angular
            .module('app.studentlists')
            .service('app.studentlists.StudentListService', StudentListService);
    })(studentlists = app.studentlists || (app.studentlists = {}));
})(app || (app = {}));
var app;
(function (app) {
    var teacherlists;
    (function (teacherlists) {
        'use strict';
        var teacherlistsController = (function () {
            function teacherlistsController($scope, list) {
                this.$scope = $scope;
                this.list = list;
            }
            teacherlistsController.$inject = ['$scope', 'list'];
            return teacherlistsController;
        })();
        angular
            .module('app.teacherlists')
            .controller('app.teacherlists.teacherlistsController', teacherlistsController);
    })(teacherlists = app.teacherlists || (app.teacherlists = {}));
})(app || (app = {}));
var app;
(function (app) {
    var teacherlists;
    (function (teacherlists) {
        'use strict';
        var GetStudentListRequest = (function () {
            function GetStudentListRequest(userId, classRoomId) {
                this.userId = userId;
                this.classRoomId = classRoomId;
            }
            return GetStudentListRequest;
        })();
        teacherlists.GetStudentListRequest = GetStudentListRequest;
    })(teacherlists = app.teacherlists || (app.teacherlists = {}));
})(app || (app = {}));
var app;
(function (app) {
    var teacherlists;
    (function (teacherlists) {
        'use strict';
        var TeacherListService = (function () {
            function TeacherListService(appConfig, $resource, userprofileSvc) {
                this.$resource = $resource;
                this.userprofileSvc = userprofileSvc;
                this.svc = $resource(appConfig.TeacherListUrl, { 'userId': '@userId', 'classRoomId': '@classRoomId' });
            }
            TeacherListService.prototype.GetStudentList = function (classRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.svc.query(new teacherlists.GetStudentListRequest(userId, classRoomId)).$promise;
            };
            TeacherListService.$inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
            return TeacherListService;
        })();
        teacherlists.TeacherListService = TeacherListService;
        angular
            .module('app.teacherlists')
            .service('app.teacherlists.TeacherListService', TeacherListService);
    })(teacherlists = app.teacherlists || (app.teacherlists = {}));
})(app || (app = {}));
//# sourceMappingURL=appBundle.js.map