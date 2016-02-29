angular.module('app', ['ui.router', 'app.shared', 'app.lessons', 'app.studentlists', 'app.coursemaps', 'app.journals', 'app.teacherlists', 'appDirectives', 'app.sidemenus'])
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
                        'comment': ['$stateParams', 'app.shared.CommentService',
                            function (params, svc) { return svc.GetComments(params.lessonId, params.classRoomId); }],
                        'classRoomId': ['$stateParams', function (params) { return params.classRoomId; }],
                        'lessonId': ['$stateParams', function (params) { return params.lessonId; }]
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
var module = angular.module('appDirectives', [])
    .directive('onFinishRender', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            if (scope.$last === true) {
                $timeout(function () {
                    scope.$emit('ngRepeatFinished');
                    $(document).foundation();
                }, 100);
            }
        }
    };
});
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
        .module('app.sidemenus', [
        "ngResource",
        'app.shared'
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
            this.GetDiscussionUrl = apiUrl + '/lesson/:id/:classRoomId/:commentId/discussions/:userId';
            this.CreateCommentUrl = apiUrl + '/comment';
            this.CreateDiscussionUrl = apiUrl + '/discussion';
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
            function CourseMapController($scope, content, status, userSvc) {
                this.$scope = $scope;
                this.content = content;
                this.status = status;
                this.userSvc = userSvc;
                this.userProfile = userSvc.GetClientUserProfile();
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
            CourseMapController.$inject = ['$scope', 'content', 'status', 'app.shared.ClientUserProfileService'];
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
            function JournalController($scope, content, svc, discussionSvc) {
                this.$scope = $scope;
                this.content = content;
                this.svc = svc;
                this.discussionSvc = discussionSvc;
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
            JournalController.$inject = ['$scope', 'content', 'app.shared.ClientUserProfileService', 'app.shared.DiscussionService'];
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
            function LessonController($scope, content, classRoomId, lessonId, comment, userprofileSvc, discussionSvc, commentSvc) {
                this.$scope = $scope;
                this.content = content;
                this.classRoomId = classRoomId;
                this.lessonId = lessonId;
                this.comment = comment;
                this.userprofileSvc = userprofileSvc;
                this.discussionSvc = discussionSvc;
                this.commentSvc = commentSvc;
                this.discussions = [];
                this.requestedCommentIds = [];
                this.teacherView = false;
                this.currentUser = this.userprofileSvc.GetClientUserProfile();
            }
            LessonController.prototype.selectTeacherView = function () {
                this.teacherView = true;
            };
            LessonController.prototype.selectStdView = function () {
                this.teacherView = false;
            };
            LessonController.prototype.showDiscussion = function (item) {
                this.openDiscussion = item.id;
                this.GetDiscussions(item);
            };
            LessonController.prototype.hideDiscussion = function () {
                this.openDiscussion = "";
            };
            LessonController.prototype.GetDiscussions = function (comment) {
                var _this = this;
                if (comment == null)
                    return;
                var NoneDiscussion = 0;
                if (comment.TotalDiscussions <= NoneDiscussion)
                    return;
                if (this.requestedCommentIds.filter(function (it) { return it == comment.id; }).length > NoneDiscussion)
                    return;
                this.requestedCommentIds.push(comment.id);
                this.discussionSvc
                    .GetDiscussions(this.lessonId, this.classRoomId, comment.id)
                    .then(function (it) {
                    if (it == null)
                        return;
                    for (var index = 0; index < it.length; index++) {
                        _this.discussions.push(it[index]);
                    }
                });
            };
            LessonController.prototype.CreateNewComment = function (message) {
                var NoneContentLength = 0;
                if (message.length <= NoneContentLength)
                    return;
                this.commentSvc.CreateNewComment(this.classRoomId, this.lessonId, message);
            };
            LessonController.prototype.CreateNewDiscussion = function (commentId, message) {
                var NoneContentLength = 0;
                if (message.length <= NoneContentLength)
                    return;
                this.discussionSvc.CreateDiscussion(this.classRoomId, this.lessonId, commentId, message);
            };
            LessonController.$inject = ['$scope', 'content', 'classRoomId', 'lessonId', 'comment', 'app.shared.ClientUserProfileService', 'app.shared.DiscussionService', 'app.shared.CommentService'];
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
        var GetCommentsRequest = (function () {
            function GetCommentsRequest(id, classRoomId, userId) {
                this.id = id;
                this.classRoomId = classRoomId;
                this.userId = userId;
            }
            return GetCommentsRequest;
        })();
        shared.GetCommentsRequest = GetCommentsRequest;
        var GetDiscussionRequest = (function () {
            function GetDiscussionRequest(id, classRoomId, commentId, userId) {
                this.id = id;
                this.classRoomId = classRoomId;
                this.commentId = commentId;
                this.userId = userId;
            }
            return GetDiscussionRequest;
        })();
        shared.GetDiscussionRequest = GetDiscussionRequest;
        var CreateCommentRequest = (function () {
            function CreateCommentRequest(ClassRoomId, LessonId, UserProfileId, Description) {
                this.ClassRoomId = ClassRoomId;
                this.LessonId = LessonId;
                this.UserProfileId = UserProfileId;
                this.Description = Description;
            }
            return CreateCommentRequest;
        })();
        shared.CreateCommentRequest = CreateCommentRequest;
        var CreateDiscussionRequest = (function () {
            function CreateDiscussionRequest(ClassRoomId, LessonId, CommentId, UserProfileId, Description) {
                this.ClassRoomId = ClassRoomId;
                this.LessonId = LessonId;
                this.CommentId = CommentId;
                this.UserProfileId = UserProfileId;
                this.Description = Description;
            }
            return CreateDiscussionRequest;
        })();
        shared.CreateDiscussionRequest = CreateDiscussionRequest;
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
                this.clientUserProfile.CurrentClassRoomId = "ClassRoom01";
                this.clientUserProfile.CurrentLessonId = "Lesson01";
            }
            ClientUserProfileService.prototype.GetClientUserProfile = function () {
                return this.clientUserProfile;
            };
            return ClientUserProfileService;
        })();
        shared.ClientUserProfileService = ClientUserProfileService;
        var CommentService = (function () {
            function CommentService(appConfig, $resource, userprofileSvc) {
                this.$resource = $resource;
                this.userprofileSvc = userprofileSvc;
                this.getCommentSvc = $resource(appConfig.LessonCommentUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'userId': '@userId' });
                this.createCommentSvc = $resource(appConfig.CreateCommentUrl, {
                    'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'UserProfileId': '@UserProfileId', 'Description': '@Description' });
            }
            CommentService.prototype.GetComments = function (lessonId, classRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.getCommentSvc.get(new shared.GetCommentsRequest(lessonId, classRoomId, userId)).$promise;
            };
            CommentService.prototype.CreateNewComment = function (classRoomId, lessonId, description) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.createCommentSvc.save(new shared.CreateCommentRequest(classRoomId, lessonId, userId, description)).$promise;
            };
            CommentService.$inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
            return CommentService;
        })();
        shared.CommentService = CommentService;
        var DiscussionService = (function () {
            function DiscussionService(appConfig, $resource, userprofileSvc) {
                this.$resource = $resource;
                this.userprofileSvc = userprofileSvc;
                this.getDiscussionSvc = $resource(appConfig.GetDiscussionUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'commentId': '@commentId', 'userId': '@userId' });
                this.createDiscussionSvc = $resource(appConfig.CreateDiscussionUrl, {
                    'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'CommentId': '@CommentId', 'UserProfileId': '@UserProfileId', 'Description': '@Description' });
            }
            DiscussionService.prototype.GetDiscussions = function (lessonId, classRoomId, commentId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.getDiscussionSvc.query(new shared.GetDiscussionRequest(lessonId, classRoomId, commentId, userId)).$promise;
            };
            DiscussionService.prototype.CreateDiscussion = function (classRoomId, lessonId, commentId, message) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.createDiscussionSvc.save(new shared.CreateDiscussionRequest(classRoomId, lessonId, commentId, userId, message)).$promise;
            };
            DiscussionService.$inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
            return DiscussionService;
        })();
        shared.DiscussionService = DiscussionService;
        angular
            .module('app.shared')
            .service('app.shared.ClientUserProfileService', ClientUserProfileService)
            .service('app.shared.CommentService', CommentService)
            .service('app.shared.DiscussionService', DiscussionService);
    })(shared = app.shared || (app.shared = {}));
})(app || (app = {}));
var app;
(function (app) {
    var sidemenus;
    (function (sidemenus) {
        'use strict';
        var SideMenuController = (function () {
            function SideMenuController($scope, userSvc) {
                this.$scope = $scope;
                this.userSvc = userSvc;
                this.userProfile = userSvc.GetClientUserProfile();
            }
            SideMenuController.prototype.GetUserProfileId = function () {
                return encodeURI(this.userProfile.UserProfileId);
            };
            SideMenuController.$inject = ['$scope', 'app.shared.ClientUserProfileService'];
            return SideMenuController;
        })();
        angular
            .module('app.sidemenus')
            .controller('app.sidemenus.SideMenuController', SideMenuController);
    })(sidemenus = app.sidemenus || (app.sidemenus = {}));
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
