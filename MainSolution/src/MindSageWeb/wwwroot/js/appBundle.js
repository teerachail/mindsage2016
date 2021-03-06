angular.module('app', ['ui.router', 'app.shared', 'app.lessons', 'app.studentlists', 'app.coursemaps', 'app.notification', 'app.journals', 'app.teacherlists', 'appDirectives', 'app.sidemenus', 'app.settings', 'app.main', 'app.calendar'])
    .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('app', {
            url: '/app',
            templateUrl: 'tmpl/layout.html',
            controller: 'app.main.MainController as acx'
        })
            .state('app.main', {
            url: '/main',
            views: {
                'mainContent': {
                    templateUrl: 'tmpl/lesson_layout.html',
                    controller: 'app.shared.LessonLayoutController as mcx'
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
                    templateUrl: 'tmpl/course_layout.html',
                    controller: 'app.shared.CourseLayoutController as mcx'
                }
            }
        })
            .state('app.course.notification', {
            url: '/notification',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/notification.html',
                    controller: 'app.notification.NotificationController as cx',
                    resolve: {
                        'notification': ['app.shared.GetProfileService', function (svc) { return svc.GetNotificationContent(); }]
                    }
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
                            function (params, svc) { return svc.GetComments(params.classRoomId, params.targetUserId); }],
                        'targetUserId': ['$stateParams', function (params) { return params.targetUserId; }],
                        'likes': ['app.shared.GetProfileService', function (svc) { return svc.GetAllLike(); }]
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
            url: '/studentlist',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/studentlist.html',
                    controller: 'app.studentlists.studentlistsController as cx',
                    resolve: {
                        'classRoomId': ['$stateParams', function (params) { return params.classRoomId; }]
                    }
                }
            }
        })
            .state('app.course.teacherlist', {
            url: '/teacherlist',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/teacherlist.html',
                    controller: 'app.teacherlists.teacherlistsController as cx',
                    resolve: {
                        'list': ['$stateParams', 'app.teacherlists.TeacherListService',
                            function (params, svc) { return svc.GetStudentList(params.classRoomId); }],
                        'classRoomId': ['$stateParams', function (params) { return params.classRoomId; }]
                    }
                }
            }
        })
            .state('app.course.config', {
            url: '/setting',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/setting.html',
                    controller: 'app.settings.SettingController as cx',
                    resolve: {
                        'courseInfo': ['app.shared.GetProfileService', function (svc) { return svc.GetCourse(); }]
                    }
                }
            }
        })
            .state('app.course.calendar', {
            url: '/calendar',
            views: {
                'courseContent': {
                    templateUrl: 'tmpl/calendar.html',
                    controller: 'app.calendar.CarlendarController as cx'
                }
            }
        });
        $urlRouterProvider.otherwise('/app/main/lesson/Lesson01/ClassRoom01');
    }]);
(function () {
    'use strict';
    angular
        .module('app.calendar', [
        "ngResource",
        'app.shared'
    ]);
})();
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
        .module('app.main', [
        "ngResource",
        'app.shared'
    ]);
})();
(function () {
    'use strict';
    angular
        .module('app.notification', [
        "ngResource",
        'app.shared'
    ]);
})();
(function () {
    'use strict';
    angular
        .module('app.settings', [
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
            this.SendFriendRequestUrl = apiUrl + '/friend';
            this.JournalCommentUrl = apiUrl + '/journal/:targetUserId/:requestByUserId/:classRoomId';
            this.TeacherListUrl = apiUrl + '/mycourse/:userId/:classRoomId/students';
            this.TeacherRemoveStdUrl = apiUrl + '/mycourse/removestud';
            this.GetDiscussionUrl = apiUrl + '/lesson/:id/:classRoomId/:commentId/discussions/:userId';
            this.CreateCommentUrl = apiUrl + '/comment';
            this.CreateDiscussionUrl = apiUrl + '/discussion';
            this.LikeCommentUrl = apiUrl + '/comment/like';
            this.LikeDiscussionUrl = apiUrl + '/discussion/like';
            this.UpdateCommentUrl = apiUrl + '/comment/:id';
            this.UpdateDiscussionUrl = apiUrl + '/discussion/:id';
            this.LikeLessonUrl = apiUrl + '/lesson/like';
            this.ReadNoteUrl = apiUrl + '/lesson/readnote';
            this.UpdateProfileUrl = apiUrl + '/profile/:id';
            this.GetCourserofileUrl = apiUrl + '/mycourse/:id/:classRoomId/info';
            this.UpdateCourseUrl = apiUrl + '/mycourse/:id';
            this.DeleteCourseUrl = apiUrl + '/mycourse/leave';
            this.StudenMessageEditUrl = apiUrl + '/mycourse/message';
            this.GetAllCourserofileUrl = apiUrl + '/mycourse/:id/courses';
            this.GetNotificationNumberUrl = apiUrl + '/notification/:id/:classRoomId';
            this.GetNotificationContentUrl = apiUrl + '/notification/:id/:classRoomId/content';
            this.GetLiketUrl = apiUrl + '/mycourse/:id/:classRoomId/:lessonId';
            this.GetAllLiketUrl = apiUrl + '/mycourse/:id/:classRoomId/alllikes';
            this.GetUserProfileUrl = apiUrl + '/profile';
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
    var calendar;
    (function (calendar) {
        'use strict';
        var CourseInformation = (function () {
            function CourseInformation() {
            }
            return CourseInformation;
        })();
        var LessonInfo = (function () {
            function LessonInfo() {
            }
            return LessonInfo;
        })();
        var CarlendarController = (function () {
            function CarlendarController($scope, $state) {
                this.$scope = $scope;
                this.$state = $state;
                this.date = new Date();
                this.monthdays = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
                this.monthAllNames = new Array("Jan", "Feb", "Mar", "Apl", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
                this.today = new Date();
                this.selected = new Array(this.today);
                this.firstClick = true;
                this.holiday = [];
                this.Lesson = [];
                this.Lesson2 = [];
                this.Lesson3 = [];
                this.Lessons = [];
                this.allweeks = []; // array for push week
                this.month = this.today.getMonth();
                this.year = this.today.getFullYear();
                this.setCalendar(this.month, this.year);
                this.fisrtSelected = new Date(this.today.toDateString());
                //-----------Mock Data ----------------------
                this.holiday.push(new Date('3/26/2016'), new Date('3/1/2016'), new Date('4/1/2016'));
                this.Lesson.push("Lesson01", 1, new Date('3/24/2016'));
                this.Lesson2.push("Lesson02", 2, new Date('3/30/2016'));
                this.Lesson3.push("Lesson03", 3, new Date('4/5/2016'));
                this.Lessons.push(this.Lesson, this.Lesson2, this.Lesson3);
                this.EndCourseDate = new Date('4/9/2016');
                //---------------------------------
                this.courseInformation = {
                    BeginDate: new Date('3/24/2016'),
                    EndDate: new Date('4/9/2016'),
                    Lessons: [
                        {
                            Name: 'Lesson01',
                            Order: 1,
                            BeginDate: new Date('3/24/2016')
                        },
                        {
                            Name: 'Lesson02',
                            Order: 2,
                            BeginDate: new Date('3/30/2016')
                        },
                        {
                            Name: 'Lesson03',
                            Order: 3,
                            BeginDate: new Date('4/5/2016')
                        },
                    ],
                    Holidays: [
                        new Date('3/26/2016'), new Date('3/1/2016'), new Date('4/1/2016'),
                    ]
                };
            }
            CarlendarController.prototype.setCalendar = function (month, year) {
                this.monthNames = this.monthAllNames[this.month];
                var firstDay = new Date((month + 1) + '/1/' + year);
                var days = this.monthdays[month]; // total day
                if (month == 1) {
                    if (year % 4 == 0)
                        days = 29;
                }
                var start = firstDay.getDay(); // start day off week
                if (start < 0)
                    start += 7;
                var weeks = Math.floor((start + days) / 7); // total week on month
                if ((start + days) % 7 != 0)
                    weeks++;
                var jumped = 0;
                var inserted = 1;
                for (var j = 1; j <= weeks; j++) {
                    var allday = [];
                    for (var i = 1; i <= 7; i++) {
                        if (jumped < start || inserted > days) {
                            var pDate = new Date(firstDay.toDateString());
                            pDate.setDate(pDate.getDate() - start + jumped);
                            var preDate = new Date();
                            allday.push(pDate);
                            jumped++;
                        }
                        else {
                            var date = new Date(firstDay.toDateString());
                            firstDay.setDate(firstDay.getDate() + 1);
                            allday.push(date);
                            inserted++;
                        }
                    }
                    this.allweeks.push(allday);
                }
            };
            CarlendarController.prototype.nextMount = function () {
                this.month++;
                if (this.month > 11) {
                    this.month = 0;
                    this.year++;
                }
                this.allweeks = [];
                this.setCalendar(this.month, this.year);
            };
            CarlendarController.prototype.prevMount = function () {
                this.month--;
                if (this.month < 0) {
                    this.month = 11;
                    this.year--;
                }
                this.allweeks = [];
                this.setCalendar(this.month, this.year);
            };
            CarlendarController.prototype.CheckToday = function (day) {
                return this.today.toDateString() == day.toDateString();
            };
            CarlendarController.prototype.IsHoliday = function (day) {
                for (var i = 0; i < this.holiday.length; i++) {
                    if (this.holiday[i].toDateString() == day.toDateString())
                        return true;
                }
                return false;
            };
            CarlendarController.prototype.IsStartLesson = function (day) {
                for (var i = 0; i < this.courseInformation.Lessons.length; i++) {
                    if (day.toDateString() == this.Lessons[i][2].toDateString())
                        return this.Lessons[i][0];
                }
                return "";
            };
            CarlendarController.prototype.IsOdd = function (day) {
                for (var i = 0; i < this.courseInformation.Lessons.length; i += 2) {
                    if (i == this.courseInformation.Lessons.length - 1) {
                        if (day >= this.courseInformation.Lessons[i].BeginDate &&
                            day <= this.courseInformation.EndDate &&
                            !this.OnSelected(day) &&
                            !this.IsHoliday(day))
                            return true;
                    }
                    else {
                        if (day >= this.courseInformation.Lessons[i].BeginDate &&
                            day < this.courseInformation.Lessons[i + 1].BeginDate &&
                            !this.OnSelected(day) &&
                            !this.IsHoliday(day))
                            return true;
                    }
                }
            };
            CarlendarController.prototype.IsEven = function (day) {
                for (var i = 1; i < this.courseInformation.Lessons.length; i += 2) {
                    if (i == this.courseInformation.Lessons.length - 1) {
                        if (day >= this.courseInformation.Lessons[i].BeginDate &&
                            day <= this.courseInformation.EndDate &&
                            !this.OnSelected(day) &&
                            !this.IsHoliday(day))
                            return true;
                    }
                    else {
                        if (day >= this.courseInformation.Lessons[i].BeginDate &&
                            day < this.courseInformation.Lessons[i + 1].BeginDate &&
                            !this.OnSelected(day) &&
                            !this.IsHoliday(day))
                            return true;
                    }
                }
            };
            CarlendarController.prototype.IsOddEven = function (day) {
                for (var i = 0; i < this.courseInformation.Lessons.length; i++) {
                    if (i == this.courseInformation.Lessons.length - 1) {
                        if (day >= this.courseInformation.Lessons[i].BeginDate &&
                            day <= this.courseInformation.EndDate &&
                            !this.OnSelected(day) &&
                            !this.IsHoliday(day))
                            return true;
                    }
                    else {
                        if (day >= this.Lessons[i][2] &&
                            day < this.courseInformation.Lessons[i + 1].BeginDate &&
                            !this.OnSelected(day) &&
                            !this.IsHoliday(day))
                            return true;
                    }
                }
            };
            CarlendarController.prototype.Selected = function (day) {
                if (this.firstClick) {
                    this.selected = [];
                    this.selected.push(day);
                    this.firstClick = !this.firstClick;
                    this.fisrtSelected = new Date(this.selected[0].toDateString());
                    this.lastSelected = null;
                }
                else {
                    var countDaySelect = (+day - +this.selected[0]) / 1000 / 60 / 60 / 24;
                    if (day < this.selected[0]) {
                        var dayTemp = day;
                        day = this.selected[0];
                        this.selected[0] = dayTemp;
                        countDaySelect = countDaySelect * -1;
                    }
                    var daySelect = new Date(this.selected[0].toDateString());
                    for (var i = 0; i < countDaySelect; i++) {
                        this.selected.push(daySelect);
                        var daySelect = new Date(daySelect.setDate(daySelect.getDate() + 1));
                    }
                    this.firstClick = !this.firstClick;
                    this.fisrtSelected = new Date(this.selected[0].toDateString());
                    this.lastSelected = new Date(this.selected[this.selected.length - 1].toDateString());
                }
            };
            CarlendarController.prototype.OnSelected = function (day) {
                for (var i = 0; i < this.selected.length; i++) {
                    if (this.selected[i].toDateString() == day.toDateString())
                        return true;
                }
                return false;
            };
            CarlendarController.$inject = ['$scope', '$state'];
            return CarlendarController;
        })();
        angular
            .module('app.calendar')
            .controller('app.calendar.CarlendarController', CarlendarController);
    })(calendar = app.calendar || (app.calendar = {}));
})(app || (app = {}));
var app;
(function (app) {
    var calendar;
    (function (calendar) {
        'use strict';
    })(calendar = app.calendar || (app.calendar = {}));
})(app || (app = {}));
var app;
(function (app) {
    var calendar;
    (function (calendar) {
        'use strict';
        angular
            .module('app.calendar');
    })(calendar = app.calendar || (app.calendar = {}));
})(app || (app = {}));
var app;
(function (app) {
    var coursemaps;
    (function (coursemaps) {
        'use strict';
        var CourseMapController = (function () {
            function CourseMapController($scope, $state, content, status, userSvc) {
                this.$scope = $scope;
                this.$state = $state;
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
            CourseMapController.$inject = ['$scope', '$state', 'content', 'status', 'app.shared.ClientUserProfileService'];
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
            function JournalController($scope, content, targetUserId, likes, svc, discussionSvc, commentSvc, lessonSvc) {
                this.$scope = $scope;
                this.content = content;
                this.targetUserId = targetUserId;
                this.likes = likes;
                this.svc = svc;
                this.discussionSvc = discussionSvc;
                this.commentSvc = commentSvc;
                this.lessonSvc = lessonSvc;
                this.MyComments = [];
                this.discussions = [];
                this.requestedCommentIds = [];
                this.userprofile = this.svc.GetClientUserProfile();
            }
            JournalController.prototype.GetWeeks = function () {
                var usedWeekNo = {};
                var lessonWeeks = [];
                if (this.MyComments.length != 0) {
                    var NewCommentlessonWeekNo = this.userprofile.CurrentLessonNo;
                    lessonWeeks.push(NewCommentlessonWeekNo);
                    usedWeekNo[NewCommentlessonWeekNo] = 1;
                }
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
            JournalController.prototype.showDiscussion = function (item, open) {
                this.GetDiscussions(item);
                return !open;
            };
            JournalController.prototype.GetDiscussions = function (comment) {
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
                    .GetDiscussions(comment.LessonId, comment.ClassRoomId, comment.id)
                    .then(function (it) {
                    if (it == null)
                        return;
                    for (var index = 0; index < it.length; index++) {
                        _this.discussions.push(it[index]);
                    }
                });
            };
            JournalController.prototype.CreateNewComment = function (message) {
                var _this = this;
                var NoneContentLength = 0;
                if (message.length <= NoneContentLength)
                    return;
                this.commentSvc.CreateNewComment(this.userprofile.CurrentClassRoomId, this.userprofile.CurrentLessonId, message)
                    .then(function (it) {
                    if (it == null) {
                        return message;
                    }
                    else {
                        var newComment = new app.shared.Comment(it.ActualCommentId, message, 0, 0, _this.userprofile.ImageUrl, _this.userprofile.FullName, _this.userprofile.CurrentClassRoomId, _this.userprofile.CurrentLessonId, _this.userprofile.UserProfileId, 0 - _this.MyComments.length);
                        _this.MyComments.push(newComment);
                        return "";
                    }
                });
            };
            JournalController.prototype.CreateNewDiscussion = function (commentId, message) {
                var _this = this;
                var NoneContentLength = 0;
                if (message.length <= NoneContentLength)
                    return;
                var local = this.content.Comments.filter(function (it) { return it.id == commentId; })[0];
                this.discussionSvc.CreateDiscussion(local.ClassRoomId, local.LessonId, commentId, message)
                    .then(function (it) {
                    if (it == null) {
                        return message;
                    }
                    var newDiscussion = new app.shared.Discussion(it.ActualCommentId, commentId, message, 0, _this.userprofile.ImageUrl, _this.userprofile.FullName, _this.userprofile.UserProfileId, 0 - _this.discussions.length);
                    _this.discussions.push(newDiscussion);
                    _this.content.Comments.filter(function (it) { return it.id == commentId; })[0].TotalDiscussions++;
                    return "";
                });
            };
            JournalController.prototype.LikeComment = function (commentId, IsLike) {
                if (IsLike == -1)
                    this.content.Comments.filter(function (it) { return it.id == commentId; })[0].TotalLikes++;
                else
                    this.content.Comments.filter(function (it) { return it.id == commentId; })[0].TotalLikes--;
                var setIndex = this.likes.LikeCommentIds.indexOf(commentId);
                var ElementNotFound = -1;
                if (setIndex <= ElementNotFound)
                    this.likes.LikeCommentIds.push(commentId);
                else
                    this.likes.LikeCommentIds.splice(setIndex, 1);
                var local = this.content.Comments.filter(function (it) { return it.id == commentId; })[0];
                this.commentSvc.LikeComment(local.ClassRoomId, local.LessonId, commentId);
            };
            JournalController.prototype.LikeDiscussion = function (commentId, discussionId, IsLike) {
                if (IsLike == -1)
                    this.discussions.filter(function (it) { return it.id == discussionId; })[0].TotalLikes++;
                else
                    this.discussions.filter(function (it) { return it.id == discussionId; })[0].TotalLikes--;
                var setIndex = this.likes.LikeDiscussionIds.indexOf(discussionId);
                var ElementNotFound = -1;
                if (setIndex <= ElementNotFound)
                    this.likes.LikeDiscussionIds.push(discussionId);
                else
                    this.likes.LikeDiscussionIds.splice(setIndex, 1);
                var local = this.content.Comments.filter(function (it) { return it.id == commentId; })[0];
                this.discussionSvc.LikeDiscussion(local.ClassRoomId, local.LessonId, commentId, discussionId);
            };
            JournalController.prototype.DeleteComment = function () {
                var comment = this.targetComment;
                var removeIndex = this.content.Comments.indexOf(comment);
                if (removeIndex > -1)
                    this.content.Comments.splice(removeIndex, 1);
                else {
                    removeIndex = this.MyComments.indexOf(comment);
                    if (removeIndex > -1)
                        this.MyComments.splice(removeIndex, 1);
                }
                this.commentSvc.UpdateComment(comment.ClassRoomId, comment.LessonId, comment.id, true, null);
            };
            JournalController.prototype.DeleteDiscussion = function () {
                var commentId = this.targetComment.id;
                var discussion = this.targetDiscussion;
                var removeIndex = this.discussions.indexOf(discussion);
                if (removeIndex > -1)
                    this.discussions.splice(removeIndex, 1);
                this.content.Comments.filter(function (it) { return it.id == commentId; })[0].TotalDiscussions--;
                var local = this.content.Comments.filter(function (it) { return it.id == commentId; })[0];
                this.discussionSvc.UpdateDiscussion(local.ClassRoomId, local.LessonId, commentId, discussion.id, true, null);
            };
            JournalController.prototype.EditComment = function (commentId, message) {
                var NoneContentLength = 0;
                if (message.length <= NoneContentLength)
                    return;
                var local = this.content.Comments.filter(function (it) { return it.id == commentId; })[0];
                this.commentSvc.UpdateComment(local.ClassRoomId, local.LessonId, commentId, false, message);
            };
            JournalController.prototype.EditDiscussion = function (commentId, discussionId, message) {
                var NoneContentLength = 0;
                if (message.length <= NoneContentLength)
                    return;
                var local = this.content.Comments.filter(function (it) { return it.id == commentId; })[0];
                this.discussionSvc.UpdateDiscussion(local.ClassRoomId, local.LessonId, commentId, discussionId, false, message);
            };
            JournalController.prototype.EditOpen = function (message, open) {
                this.message = message;
                return !open;
            };
            JournalController.prototype.SaveEdit = function (messageId, save) {
                this.content.Comments.filter(function (it) { return it.id == messageId; })[0].Description = this.message;
                this.EditComment(this.content.Comments.filter(function (it) { return it.id == messageId; })[0].id, this.message);
                return !save;
            };
            JournalController.prototype.SaveEditDiscus = function (commentId, messageId, save) {
                this.discussions.filter(function (it) { return it.id == messageId; })[0].Description = this.message;
                this.EditDiscussion(commentId, this.discussions.filter(function (it) { return it.id == messageId; })[0].id, this.message);
                return !save;
            };
            JournalController.prototype.CancelEdit = function (save) {
                return !save;
            };
            JournalController.prototype.deleteComfirm = function (comment) {
                this.targetComment = comment;
                this.targetDiscussion = null;
                this.deleteComment = true;
            };
            JournalController.prototype.deleteDisComfirm = function (comment, discussion) {
                this.targetComment = comment;
                this.targetDiscussion = discussion;
                this.deleteComment = false;
            };
            JournalController.$inject = ['$scope', 'content', 'targetUserId', 'likes', 'app.shared.ClientUserProfileService', 'app.shared.DiscussionService', 'app.shared.CommentService', 'app.lessons.LessonService'];
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
            function LessonController($scope, content, classRoomId, lessonId, comment, userprofileSvc, discussionSvc, commentSvc, lessonSvc, getProfileSvc) {
                var _this = this;
                this.$scope = $scope;
                this.content = content;
                this.classRoomId = classRoomId;
                this.lessonId = lessonId;
                this.comment = comment;
                this.userprofileSvc = userprofileSvc;
                this.discussionSvc = discussionSvc;
                this.commentSvc = commentSvc;
                this.lessonSvc = lessonSvc;
                this.getProfileSvc = getProfileSvc;
                this.discussions = [];
                this.requestedCommentIds = [];
                this.teacherView = false;
                this.currentUser = this.userprofileSvc.GetClientUserProfile();
                this.currentUser.CurrentDisplayLessonId = lessonId;
                this.userprofileSvc.UpdateUserProfile(this.currentUser);
                this.userprofileSvc.Advertisments = this.content.Advertisments;
                this.getProfileSvc.GetLike().then(function (it) { return _this.likes = it; });
            }
            LessonController.prototype.selectTeacherView = function () {
                this.teacherView = true;
            };
            LessonController.prototype.selectStdView = function () {
                this.teacherView = false;
            };
            LessonController.prototype.showDiscussion = function (item, open) {
                this.GetDiscussions(item);
                return !open;
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
                var _this = this;
                var NoneContentLength = 0;
                if (message.length <= NoneContentLength)
                    return message;
                this.commentSvc.CreateNewComment(this.classRoomId, this.lessonId, message)
                    .then(function (it) {
                    if (it == null) {
                        return message;
                    }
                    else {
                        var userprofile = _this.userprofileSvc.GetClientUserProfile();
                        var newComment = new app.shared.Comment(it.ActualCommentId, message, 0, 0, userprofile.ImageUrl, userprofile.FullName, _this.classRoomId, _this.lessonId, userprofile.UserProfileId, 0 - _this.comment.Comments.length);
                        _this.comment.Comments.push(newComment);
                        return "";
                    }
                });
            };
            LessonController.prototype.CreateNewDiscussion = function (commentId, message) {
                var _this = this;
                var NoneContentLength = 0;
                if (message.length <= NoneContentLength)
                    return message;
                this.discussionSvc.CreateDiscussion(this.classRoomId, this.lessonId, commentId, message)
                    .then(function (it) {
                    if (it == null) {
                        return message;
                    }
                    else {
                        var userprofile = _this.userprofileSvc.GetClientUserProfile();
                        var newDiscussion = new app.shared.Discussion(it.ActualCommentId, commentId, message, 0, userprofile.ImageUrl, userprofile.FullName, userprofile.UserProfileId, 0 - _this.discussions.length);
                        _this.discussions.push(newDiscussion);
                        _this.comment.Comments.filter(function (it) { return it.id == commentId; })[0].TotalDiscussions++;
                        return "";
                    }
                });
            };
            LessonController.prototype.LikeComment = function (commentId, IsLike) {
                if (IsLike == -1)
                    this.comment.Comments.filter(function (it) { return it.id == commentId; })[0].TotalLikes++;
                else
                    this.comment.Comments.filter(function (it) { return it.id == commentId; })[0].TotalLikes--;
                this.commentSvc.LikeComment(this.classRoomId, this.lessonId, commentId);
                var removeIndex = this.likes.LikeCommentIds.indexOf(commentId);
                var ElementNotFound = -1;
                if (removeIndex <= ElementNotFound)
                    this.likes.LikeCommentIds.push(commentId);
                else
                    this.likes.LikeCommentIds.splice(removeIndex, 1);
            };
            LessonController.prototype.LikeDiscussion = function (commentId, discussionId, IsLike) {
                if (IsLike == -1)
                    this.discussions.filter(function (it) { return it.id == discussionId; })[0].TotalLikes++;
                else
                    this.discussions.filter(function (it) { return it.id == discussionId; })[0].TotalLikes--;
                this.discussionSvc.LikeDiscussion(this.classRoomId, this.lessonId, commentId, discussionId);
                var removeIndex = this.likes.LikeDiscussionIds.indexOf(discussionId);
                var ElementNotFound = -1;
                if (removeIndex <= ElementNotFound)
                    this.likes.LikeDiscussionIds.push(discussionId);
                else
                    this.likes.LikeDiscussionIds.splice(removeIndex, 1);
            };
            LessonController.prototype.DeleteComment = function () {
                var comment = this.targetComment;
                var removeIndex = this.comment.Comments.indexOf(comment);
                if (removeIndex > -1)
                    this.comment.Comments.splice(removeIndex, 1);
                this.commentSvc.UpdateComment(this.classRoomId, this.lessonId, comment.id, true, null);
            };
            LessonController.prototype.DeleteDiscussion = function () {
                var commentId = this.targetComment.id;
                var discussion = this.targetDiscussion;
                var removeIndex = this.discussions.indexOf(discussion);
                if (removeIndex > -1)
                    this.discussions.splice(removeIndex, 1);
                this.comment.Comments.filter(function (it) { return it.id == commentId; })[0].TotalDiscussions--;
                this.discussionSvc.UpdateDiscussion(this.classRoomId, this.lessonId, commentId, discussion.id, true, null);
            };
            LessonController.prototype.EditComment = function (commentId, message) {
                var NoneContentLength = 0;
                if (message.length <= NoneContentLength)
                    return;
                this.commentSvc.UpdateComment(this.classRoomId, this.lessonId, commentId, false, message);
            };
            LessonController.prototype.EditDiscussion = function (commentId, discussionId, message) {
                var NoneContentLength = 0;
                if (message.length <= NoneContentLength)
                    return;
                this.discussionSvc.UpdateDiscussion(this.classRoomId, this.lessonId, commentId, discussionId, false, message);
            };
            LessonController.prototype.LikeLesson = function () {
                this.likes.IsLikedLesson = !this.likes.IsLikedLesson;
                this.content.TotalLikes++;
                this.lessonSvc.LikeLesson(this.classRoomId, this.lessonId);
            };
            LessonController.prototype.DisLikeLesson = function () {
                this.likes.IsLikedLesson = !this.likes.IsLikedLesson;
                this.content.TotalLikes--;
                this.lessonSvc.LikeLesson(this.classRoomId, this.lessonId);
            };
            LessonController.prototype.ReadNote = function () {
                this.lessonSvc.ReadNote(this.classRoomId);
            };
            LessonController.prototype.EditOpen = function (message, open) {
                this.message = message;
                return !open;
            };
            LessonController.prototype.SaveEdit = function (messageId, save) {
                this.comment.Comments.filter(function (it) { return it.id == messageId; })[0].Description = this.message;
                this.EditComment(this.comment.Comments.filter(function (it) { return it.id == messageId; })[0].id, this.message);
                return !save;
            };
            LessonController.prototype.SaveEditDiscus = function (commentId, messageId, save) {
                this.discussions.filter(function (it) { return it.id == messageId; })[0].Description = this.message;
                this.EditDiscussion(commentId, this.discussions.filter(function (it) { return it.id == messageId; })[0].id, this.message);
                return !save;
            };
            LessonController.prototype.CancelEdit = function (save) {
                return !save;
            };
            LessonController.prototype.deleteComfirm = function (comment) {
                this.targetComment = comment;
                this.targetDiscussion = null;
                this.deleteComment = true;
            };
            LessonController.prototype.deleteDisComfirm = function (comment, discussion) {
                this.targetComment = comment;
                this.targetDiscussion = discussion;
                this.deleteComment = false;
            };
            LessonController.$inject = ['$scope', 'content', 'classRoomId', 'lessonId', 'comment', 'app.shared.ClientUserProfileService', 'app.shared.DiscussionService', 'app.shared.CommentService', 'app.lessons.LessonService', 'app.shared.GetProfileService'];
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
        var LikeLessonRequest = (function () {
            function LikeLessonRequest(ClassRoomId, LessonId, UserProfileId) {
                this.ClassRoomId = ClassRoomId;
                this.LessonId = LessonId;
                this.UserProfileId = UserProfileId;
            }
            return LikeLessonRequest;
        })();
        lessons.LikeLessonRequest = LikeLessonRequest;
        var ReadNoteRequest = (function () {
            function ReadNoteRequest(ClassRoomId, UserProfileId) {
                this.ClassRoomId = ClassRoomId;
                this.UserProfileId = UserProfileId;
            }
            return ReadNoteRequest;
        })();
        lessons.ReadNoteRequest = ReadNoteRequest;
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
                this.getLessonSvc = $resource(appConfig.LessonUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'userId': '@userId' });
                this.likeLessonSvc = $resource(appConfig.LikeLessonUrl, {
                    'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'UserProfileId': '@UserProfileId'
                });
                this.readNoteSvc = $resource(appConfig.ReadNoteUrl, {
                    'ClassRoomId': '@ClassRoomId', 'UserProfileId': '@UserProfileId'
                });
            }
            LessonService.prototype.GetContent = function (lessonId, classRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.getLessonSvc.get(new lessons.LessonContentRequest(lessonId, classRoomId, userId)).$promise;
            };
            LessonService.prototype.LikeLesson = function (classRoomId, lessonId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.likeLessonSvc.save(new lessons.LikeLessonRequest(classRoomId, lessonId, userId)).$promise;
            };
            LessonService.prototype.ReadNote = function (classRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.readNoteSvc.save(new lessons.ReadNoteRequest(classRoomId, userId)).$promise;
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
    var main;
    (function (main) {
        'use strict';
        var MainController = (function () {
            function MainController(listsSvc, userSvc) {
                this.listsSvc = listsSvc;
                this.userSvc = userSvc;
                this.classRoomId = this.userSvc.GetClientUserProfile().CurrentClassRoomId;
            }
            MainController.prototype.FriendsStatus = function (friendId) {
                if (this.userSvc.GetClientUserProfile().UserProfileId == friendId)
                    return 2;
                return this.userSvc.GetFriendLists().filter(function (it) { return it.UserProfileId == friendId; })[0].Status;
            };
            MainController.prototype.targetData = function (friendId) {
                var targetObj = this.userSvc.GetFriendLists().filter(function (it) { return it.UserProfileId == friendId; })[0];
                if (targetObj == null)
                    return;
                else
                    this.targetUser = targetObj;
            };
            MainController.prototype.SendFriendRequest = function (friendObj) {
                this.userSvc.GetFriendLists().filter(function (it) { return it == friendObj; })[0].Status = 0;
                this.listsSvc.SendFriendRequest(friendObj.UserProfileId, null, false);
            };
            MainController.prototype.ConfirmFriendRequest = function (friendObj) {
                this.userSvc.GetFriendLists().filter(function (it) { return it == friendObj; })[0].Status = 2;
                this.listsSvc.SendFriendRequest(friendObj.UserProfileId, friendObj.RequestId, true);
            };
            MainController.prototype.DeleteFriendRequest = function (friendObj) {
                this.userSvc.GetFriendLists().filter(function (it) { return it == friendObj; })[0].Status = 3;
                this.listsSvc.SendFriendRequest(friendObj.UserProfileId, friendObj.RequestId, false);
            };
            MainController.$inject = ['app.studentlists.StudentListService', 'app.shared.ClientUserProfileService'];
            return MainController;
        })();
        angular
            .module('app.main')
            .controller('app.main.MainController', MainController);
    })(main = app.main || (app.main = {}));
})(app || (app = {}));
var app;
(function (app) {
    var notification;
    (function (notification_1) {
        'use strict';
        var NotificationController = (function () {
            function NotificationController($scope, $state, userSvc, notification, getProfile, sideMenuSvc) {
                this.$scope = $scope;
                this.$state = $state;
                this.userSvc = userSvc;
                this.notification = notification;
                this.getProfile = getProfile;
                this.sideMenuSvc = sideMenuSvc;
                this.userInfo = userSvc.GetClientUserProfile();
                this.displayNpotifications = this.notification.filter(function (it) { return it.FromUserProfiles != null; });
            }
            NotificationController.prototype.OpenJournalPage = function (name, userId) {
                this.sideMenuSvc.CurrentTabName = name;
                this.$state.go("app.course.teacherlist", { 'classRoomId': this.userInfo.CurrentClassRoomId, 'targetUserId': userId }, { inherit: false });
            };
            NotificationController.prototype.GetFirstLiker = function (name) {
                return name[0];
            };
            NotificationController.$inject = ['$scope', '$state', 'app.shared.ClientUserProfileService', 'notification', 'app.shared.GetProfileService', 'app.sidemenus.SideMenuService'];
            return NotificationController;
        })();
        angular
            .module('app.notification')
            .controller('app.notification.NotificationController', NotificationController);
    })(notification = app.notification || (app.notification = {}));
})(app || (app = {}));
var app;
(function (app) {
    var notification;
    (function (notification) {
        'use strict';
    })(notification = app.notification || (app.notification = {}));
})(app || (app = {}));
var app;
(function (app) {
    var notification;
    (function (notification) {
        'use strict';
        angular
            .module('app.notification');
    })(notification = app.notification || (app.notification = {}));
})(app || (app = {}));
var app;
(function (app) {
    var settings;
    (function (settings) {
        'use strict';
        var SettingController = (function () {
            function SettingController($scope, $state, profileSvc, courseInfo, clientProfileSvc, getProfile, sideMenuSvc) {
                this.$scope = $scope;
                this.$state = $state;
                this.profileSvc = profileSvc;
                this.courseInfo = courseInfo;
                this.clientProfileSvc = clientProfileSvc;
                this.getProfile = getProfile;
                this.sideMenuSvc = sideMenuSvc;
                this.userInfo = this.clientProfileSvc.GetClientUserProfile();
                this.ClassName = this.courseInfo.ClassName;
                this.CurrentStudentCode = this.courseInfo.CurrentStudentCode;
            }
            SettingController.prototype.OpenStudentListPage = function (name) {
                this.sideMenuSvc.CurrentTabName = name;
                this.$state.go("app.course.teacherlist", { 'classRoomId': this.userInfo.CurrentClassRoomId }, { inherit: false });
            };
            SettingController.prototype.UpdateProfile = function (name, schoolName, isPrivate, isReminderOnceTime) {
                if (name != null && name != "")
                    this.profileSvc.UpdateProfile(name, schoolName, isPrivate, isReminderOnceTime);
            };
            SettingController.prototype.UpdateCoursee = function (ClassName, ChangedStudentCode, BeginDate) {
                if (this.courseInfo.ClassName == ClassName)
                    ClassName = null;
                if (this.courseInfo.CurrentStudentCode == ChangedStudentCode)
                    ChangedStudentCode = null;
                if (ClassName != null || ChangedStudentCode != null)
                    this.profileSvc.UpdateCourse(ClassName, ChangedStudentCode, BeginDate);
            };
            SettingController.prototype.DeleteCourse = function () {
                this.profileSvc.DeleteCourse(this.courseInfo.ClassRoomId);
            };
            SettingController.prototype.StudenMessageEdit = function (Message) {
                this.profileSvc.StudenMessageEdit(Message);
            };
            SettingController.prototype.GetAllCourse = function () {
                this.getProfile.GetAllCourse();
            };
            SettingController.prototype.GetNotificationNumber = function () {
                this.getProfile.GetNotificationNumber();
            };
            SettingController.prototype.GetNotificationContent = function () {
                this.getProfile.GetNotificationContent();
            };
            SettingController.prototype.GetLike = function () {
                this.getProfile.GetLike();
            };
            SettingController.$inject = ['$scope', '$state', 'app.settings.ProfileService', 'courseInfo', 'app.shared.ClientUserProfileService', 'app.shared.GetProfileService', 'app.sidemenus.SideMenuService'];
            return SettingController;
        })();
        angular
            .module('app.settings')
            .controller('app.settings.SettingController', SettingController);
    })(settings = app.settings || (app.settings = {}));
})(app || (app = {}));
var app;
(function (app) {
    var settings;
    (function (settings) {
        'use strict';
        var UpdateProfileRequest = (function () {
            function UpdateProfileRequest(id, Name, SchoolName, IsPrivate, IsReminderOnceTime) {
                this.id = id;
                this.Name = Name;
                this.SchoolName = SchoolName;
                this.IsPrivate = IsPrivate;
                this.IsReminderOnceTime = IsReminderOnceTime;
            }
            return UpdateProfileRequest;
        })();
        settings.UpdateProfileRequest = UpdateProfileRequest;
        var UpdateCourseRequest = (function () {
            function UpdateCourseRequest(id, classRoomId, ClassName, ChangedStudentCode, BeginDate) {
                this.id = id;
                this.classRoomId = classRoomId;
                this.ClassName = ClassName;
                this.ChangedStudentCode = ChangedStudentCode;
                this.BeginDate = BeginDate;
            }
            return UpdateCourseRequest;
        })();
        settings.UpdateCourseRequest = UpdateCourseRequest;
        var DeleteCourseRequest = (function () {
            function DeleteCourseRequest(ClassRoomId, UserProfileId) {
                this.ClassRoomId = ClassRoomId;
                this.UserProfileId = UserProfileId;
            }
            return DeleteCourseRequest;
        })();
        settings.DeleteCourseRequest = DeleteCourseRequest;
        var StudenMessageEditRequest = (function () {
            function StudenMessageEditRequest(ClassRoomId, UserProfileId, Message) {
                this.ClassRoomId = ClassRoomId;
                this.UserProfileId = UserProfileId;
                this.Message = Message;
            }
            return StudenMessageEditRequest;
        })();
        settings.StudenMessageEditRequest = StudenMessageEditRequest;
    })(settings = app.settings || (app.settings = {}));
})(app || (app = {}));
var app;
(function (app) {
    var settings;
    (function (settings) {
        'use strict';
        var ProfileService = (function () {
            function ProfileService(appConfig, $resource, userprofileSvc) {
                this.$resource = $resource;
                this.userprofileSvc = userprofileSvc;
                this.svc = $resource(appConfig.UpdateProfileUrl, {
                    'id': '@id', 'Name': '@Name', 'SchoolName': '@SchoolName', 'IsPrivate': '@IsPrivate', 'IsReminderOnceTime': '@IsReminderOnceTime'
                }, { UpdateProfile: { method: 'PUT' } });
                this.updateCoursesvc = $resource(appConfig.UpdateCourseUrl, {
                    'id': '@id', 'classRoomId': '@classRoomId', 'ClassName': '@ClassName', 'ChangedStudentCode': '@ChangedStudentCode', 'BeginDate': '@BeginDate'
                }, { UpdateCourse: { method: 'PUT' } });
                this.deleteCoursesvc = $resource(appConfig.DeleteCourseUrl, {
                    'ClassRoomId': '@ClassRoomId', 'UserProfileId': '@UserProfileId'
                });
                this.studenMessageEditsvc = $resource(appConfig.StudenMessageEditUrl, {
                    'ClassRoomId': '@ClassRoomId', 'UserProfileId': '@UserProfileId', 'Message': '@Message'
                });
            }
            ProfileService.prototype.UpdateProfile = function (name, schoolName, isPrivate, isReminderOnceTime) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.svc.UpdateProfile(new settings.UpdateProfileRequest(userId, name, schoolName, isPrivate, isReminderOnceTime)).$promise;
            };
            ProfileService.prototype.UpdateCourse = function (ClassName, ChangedStudentCode, BeginDate) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
                return this.updateCoursesvc.UpdateCourse(new settings.UpdateCourseRequest(userId, classroomId, ClassName, ChangedStudentCode, BeginDate)).$promise;
            };
            ProfileService.prototype.DeleteCourse = function (ClassRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.deleteCoursesvc.save(new settings.DeleteCourseRequest(ClassRoomId, userId)).$promise;
            };
            ProfileService.prototype.StudenMessageEdit = function (Message) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
                return this.studenMessageEditsvc.save(new settings.StudenMessageEditRequest(classroomId, userId, Message)).$promise;
            };
            ProfileService.$inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
            return ProfileService;
        })();
        settings.ProfileService = ProfileService;
        angular
            .module('app.settings')
            .service('app.settings.ProfileService', ProfileService);
    })(settings = app.settings || (app.settings = {}));
})(app || (app = {}));
var app;
(function (app) {
    var shared;
    (function (shared) {
        'use strict';
        var LessonLayoutController = (function () {
            function LessonLayoutController($sce) {
                this.$sce = $sce;
            }
            LessonLayoutController.prototype.ChangeVideo = function (url) {
                this.RunningVideoUrl = this.$sce.trustAsResourceUrl(url);
            };
            LessonLayoutController.$inject = ['$sce'];
            return LessonLayoutController;
        })();
        var CourseLayoutController = (function () {
            function CourseLayoutController(clientUserProfileSvc) {
                this.clientUserProfileSvc = clientUserProfileSvc;
                this.ads = this.clientUserProfileSvc.Advertisments;
            }
            CourseLayoutController.$inject = ['app.shared.ClientUserProfileService'];
            return CourseLayoutController;
        })();
        angular
            .module('app.shared')
            .controller('app.shared.LessonLayoutController', LessonLayoutController)
            .controller('app.shared.CourseLayoutController', CourseLayoutController);
    })(shared = app.shared || (app.shared = {}));
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
        var Advertisment = (function () {
            function Advertisment() {
            }
            return Advertisment;
        })();
        shared.Advertisment = Advertisment;
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
        var LikeCommentRequest = (function () {
            function LikeCommentRequest(ClassRoomId, LessonId, CommentId, UserProfileId) {
                this.ClassRoomId = ClassRoomId;
                this.LessonId = LessonId;
                this.CommentId = CommentId;
                this.UserProfileId = UserProfileId;
            }
            return LikeCommentRequest;
        })();
        shared.LikeCommentRequest = LikeCommentRequest;
        var LikeDiscussionRequest = (function () {
            function LikeDiscussionRequest(ClassRoomId, LessonId, CommentId, DiscussionId, UserProfileId) {
                this.ClassRoomId = ClassRoomId;
                this.LessonId = LessonId;
                this.CommentId = CommentId;
                this.DiscussionId = DiscussionId;
                this.UserProfileId = UserProfileId;
            }
            return LikeDiscussionRequest;
        })();
        shared.LikeDiscussionRequest = LikeDiscussionRequest;
        var UpdateCommentRequest = (function () {
            function UpdateCommentRequest(id, ClassRoomId, LessonId, UserProfileId, IsDelete, Description) {
                this.id = id;
                this.ClassRoomId = ClassRoomId;
                this.LessonId = LessonId;
                this.UserProfileId = UserProfileId;
                this.IsDelete = IsDelete;
                this.Description = Description;
            }
            return UpdateCommentRequest;
        })();
        shared.UpdateCommentRequest = UpdateCommentRequest;
        var UpdateDiscussionRequest = (function () {
            function UpdateDiscussionRequest(id, ClassRoomId, LessonId, CommentId, UserProfileId, IsDelete, Description) {
                this.id = id;
                this.ClassRoomId = ClassRoomId;
                this.LessonId = LessonId;
                this.CommentId = CommentId;
                this.UserProfileId = UserProfileId;
                this.IsDelete = IsDelete;
                this.Description = Description;
            }
            return UpdateDiscussionRequest;
        })();
        shared.UpdateDiscussionRequest = UpdateDiscussionRequest;
        var GetUserProfileRequest = (function () {
            function GetUserProfileRequest(id) {
                this.id = id;
            }
            return GetUserProfileRequest;
        })();
        shared.GetUserProfileRequest = GetUserProfileRequest;
        var GetCourseRequest = (function () {
            function GetCourseRequest(id, classRoomId) {
                this.id = id;
                this.classRoomId = classRoomId;
            }
            return GetCourseRequest;
        })();
        shared.GetCourseRequest = GetCourseRequest;
        var Comment = (function () {
            function Comment(id, Description, TotalLikes, TotalDiscussions, CreatorImageUrl, CreatorDisplayName, ClassRoomId, LessonId, CreatedByUserProfileId, Order) {
                this.id = id;
                this.Description = Description;
                this.TotalLikes = TotalLikes;
                this.TotalDiscussions = TotalDiscussions;
                this.CreatorImageUrl = CreatorImageUrl;
                this.CreatorDisplayName = CreatorDisplayName;
                this.ClassRoomId = ClassRoomId;
                this.LessonId = LessonId;
                this.CreatedByUserProfileId = CreatedByUserProfileId;
                this.Order = Order;
            }
            return Comment;
        })();
        shared.Comment = Comment;
        var Discussion = (function () {
            function Discussion(id, CommentId, Description, TotalLikes, CreatorImageUrl, CreatorDisplayName, CreatedByUserProfileId, Order) {
                this.id = id;
                this.CommentId = CommentId;
                this.Description = Description;
                this.TotalLikes = TotalLikes;
                this.CreatorImageUrl = CreatorImageUrl;
                this.CreatorDisplayName = CreatorDisplayName;
                this.CreatedByUserProfileId = CreatedByUserProfileId;
                this.Order = Order;
            }
            return Discussion;
        })();
        shared.Discussion = Discussion;
        var GetAllCourseRequest = (function () {
            function GetAllCourseRequest(id) {
                this.id = id;
            }
            return GetAllCourseRequest;
        })();
        shared.GetAllCourseRequest = GetAllCourseRequest;
        var GetNotificationNumberRequest = (function () {
            function GetNotificationNumberRequest(id, classRoomId) {
                this.id = id;
                this.classRoomId = classRoomId;
            }
            return GetNotificationNumberRequest;
        })();
        shared.GetNotificationNumberRequest = GetNotificationNumberRequest;
        var CourseCatalog = (function () {
            function CourseCatalog() {
            }
            return CourseCatalog;
        })();
        shared.CourseCatalog = CourseCatalog;
        var GetNotificationContentRequest = (function () {
            function GetNotificationContentRequest(id, classRoomId) {
                this.id = id;
                this.classRoomId = classRoomId;
            }
            return GetNotificationContentRequest;
        })();
        shared.GetNotificationContentRequest = GetNotificationContentRequest;
        var GetLikeRequest = (function () {
            function GetLikeRequest(id, classRoomId, lessonId) {
                this.id = id;
                this.classRoomId = classRoomId;
                this.lessonId = lessonId;
            }
            return GetLikeRequest;
        })();
        shared.GetLikeRequest = GetLikeRequest;
        var GetAllLikeRequest = (function () {
            function GetAllLikeRequest(id, classRoomId) {
                this.id = id;
                this.classRoomId = classRoomId;
            }
            return GetAllLikeRequest;
        })();
        shared.GetAllLikeRequest = GetAllLikeRequest;
        var GetFriendListRequest = (function () {
            function GetFriendListRequest(userId, classRoomId) {
                this.userId = userId;
                this.classRoomId = classRoomId;
            }
            return GetFriendListRequest;
        })();
        shared.GetFriendListRequest = GetFriendListRequest;
    })(shared = app.shared || (app.shared = {}));
})(app || (app = {}));
var app;
(function (app) {
    var shared;
    (function (shared) {
        'use strict';
        var ClientUserProfileService = (function () {
            function ClientUserProfileService(appConfig, $resource) {
                this.$resource = $resource;
                this.getUserProfileSvc = $resource(appConfig.GetUserProfileUrl, {});
                this.getAllCourseSvc = $resource(appConfig.GetAllCourserofileUrl, { 'id': '@id' });
                this.getStudentListsvc = $resource(appConfig.StudentListUrl, { 'userId': '@userId', 'classRoomId': '@classRoomId' });
            }
            ClientUserProfileService.prototype.UpdateUserProfile = function (userProfile) {
                if (userProfile == null)
                    return;
                this.clientUserProfile = userProfile;
            };
            ClientUserProfileService.prototype.GetClientUserProfile = function () {
                var _this = this;
                if (this.clientUserProfile == null || this.clientUserProfile.UserProfileId == null) {
                    if (this.isWaittingForUserProfileRespond)
                        return;
                    else {
                        this.isWaittingForUserProfileRespond = true;
                        this.getUserProfileSvc.get().$promise.then(function (respond) {
                            if (respond == null)
                                return _this.GetClientUserProfile();
                            else {
                                _this.isWaittingForUserProfileRespond = false;
                                _this.clientUserProfile = respond;
                                return _this.clientUserProfile;
                            }
                        });
                    }
                }
                else
                    return this.clientUserProfile;
            };
            ClientUserProfileService.prototype.GetAllAvailableCourses = function () {
                var _this = this;
                if (this.allAvailableCourses == null) {
                    if (this.isWaittingForAllCourses)
                        return;
                    else {
                        this.isWaittingForAllCourses = true;
                        this.getAllCourseSvc.get().$promise.then(function (respond) {
                            if (respond == null)
                                return _this.GetAllAvailableCourses();
                            else {
                                _this.isWaittingForAllCourses = false;
                                _this.allAvailableCourses = respond;
                                return _this.allAvailableCourses;
                            }
                        });
                    }
                }
                else
                    return this.allAvailableCourses;
            };
            ClientUserProfileService.prototype.GetFriendLists = function () {
                var _this = this;
                if (this.friendList == null) {
                    if (this.isWaittingForFriendList)
                        return;
                    else {
                        this.isWaittingForFriendList = true;
                        var userId = this.clientUserProfile.UserProfileId;
                        var classRoomId = this.clientUserProfile.CurrentClassRoomId;
                        this.getStudentListsvc.query(new shared.GetFriendListRequest(userId, classRoomId)).$promise.then(function (respond) {
                            if (respond == null)
                                return _this.GetFriendLists();
                            else {
                                _this.isWaittingForFriendList = false;
                                _this.friendList = respond;
                                return _this.friendList;
                            }
                        });
                    }
                }
                else
                    return this.friendList;
            };
            ClientUserProfileService.$inject = ['appConfig', '$resource'];
            return ClientUserProfileService;
        })();
        shared.ClientUserProfileService = ClientUserProfileService;
        var CommentService = (function () {
            function CommentService(appConfig, $resource, userprofileSvc) {
                this.$resource = $resource;
                this.userprofileSvc = userprofileSvc;
                this.getCommentSvc = $resource(appConfig.LessonCommentUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'userId': '@userId' });
                this.createCommentSvc = $resource(appConfig.CreateCommentUrl, {
                    'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'UserProfileId': '@UserProfileId', 'Description': '@Description'
                });
                this.likeCommentSvc = $resource(appConfig.LikeCommentUrl, {
                    'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'CommentId': '@CommentId', 'UserProfileId': '@UserProfileId'
                });
                this.updateCommentSvc = $resource(appConfig.UpdateCommentUrl, {
                    'id': '@id', 'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'UserProfileId': '@UserProfileId', 'IsDelete': '@IsDelete', 'Description': '@Description'
                }, { UpdateComment: { method: 'PUT' } });
            }
            CommentService.prototype.GetComments = function (lessonId, classRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.getCommentSvc.get(new shared.GetCommentsRequest(lessonId, classRoomId, userId)).$promise;
            };
            CommentService.prototype.CreateNewComment = function (classRoomId, lessonId, description) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.createCommentSvc.save(new shared.CreateCommentRequest(classRoomId, lessonId, userId, description)).$promise;
            };
            CommentService.prototype.LikeComment = function (classRoomId, lessonId, commentId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.likeCommentSvc.save(new shared.LikeCommentRequest(classRoomId, lessonId, commentId, userId)).$promise;
            };
            CommentService.prototype.UpdateComment = function (classRoomId, lessonId, commentId, isDelete, message) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.updateCommentSvc.UpdateComment(new shared.UpdateCommentRequest(commentId, classRoomId, lessonId, userId, isDelete, message)).$promise;
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
                    'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'CommentId': '@CommentId', 'UserProfileId': '@UserProfileId', 'Description': '@Description'
                });
                this.likeDiscussionSvc = $resource(appConfig.LikeDiscussionUrl, {
                    'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'CommentId': '@CommentId', 'DiscussionId': '@DiscussionId', 'UserProfileId': '@UserProfileId', 'Description': '@Description'
                });
                this.updateDiscussionSvc = $resource(appConfig.UpdateDiscussionUrl, {
                    'id': '@id', 'ClassRoomId': '@ClassRoomId', 'LessonId': '@LessonId', 'CommentId': '@CommentId', 'UserProfileId': '@UserProfileId', 'IsDelete': '@IsDelete', 'Description': '@Description'
                }, { UpdateDiscussion: { method: 'PUT' } });
            }
            DiscussionService.prototype.GetDiscussions = function (lessonId, classRoomId, commentId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.getDiscussionSvc.query(new shared.GetDiscussionRequest(lessonId, classRoomId, commentId, userId)).$promise;
            };
            DiscussionService.prototype.CreateDiscussion = function (classRoomId, lessonId, commentId, message) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.createDiscussionSvc.save(new shared.CreateDiscussionRequest(classRoomId, lessonId, commentId, userId, message)).$promise;
            };
            DiscussionService.prototype.LikeDiscussion = function (classRoomId, lessonId, commentId, discussionId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.likeDiscussionSvc.save(new shared.LikeDiscussionRequest(classRoomId, lessonId, commentId, discussionId, userId)).$promise;
            };
            DiscussionService.prototype.UpdateDiscussion = function (classRoomId, lessonId, commentId, discussionId, isDelete, message) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.updateDiscussionSvc.UpdateDiscussion(new shared.UpdateDiscussionRequest(discussionId, classRoomId, lessonId, commentId, userId, isDelete, message)).$promise;
            };
            DiscussionService.$inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
            return DiscussionService;
        })();
        shared.DiscussionService = DiscussionService;
        var GetProfileService = (function () {
            function GetProfileService(appConfig, $resource, userprofileSvc) {
                this.$resource = $resource;
                this.userprofileSvc = userprofileSvc;
                this.getCourseSvc = $resource(appConfig.GetCourserofileUrl, { 'id': '@id', 'classRoomId': '@classRoomId' });
                this.getAllCourseSvc = $resource(appConfig.GetAllCourserofileUrl, { 'id': '@id' });
                this.getNotificationNumberSvc = $resource(appConfig.GetNotificationNumberUrl, { 'id': '@id', 'classRoomId': '@classRoomId' });
                this.getNotificationContentSvc = $resource(appConfig.GetNotificationContentUrl, { 'id': '@id', 'classRoomId': '@classRoomId' });
                this.getLikeSvc = $resource(appConfig.GetLiketUrl, { 'id': '@id', 'classRoomId': '@classRoomId', 'lessonId': '@lessonId' });
                this.getAllLikeSvc = $resource(appConfig.GetAllLiketUrl, { 'id': '@id', 'classRoomId': '@classRoomId' });
                this.getUserProfileSvc = $resource(appConfig.GetUserProfileUrl, {});
            }
            GetProfileService.prototype.GetUserProfile = function () {
                return this.getUserProfileSvc.get().$promise;
            };
            GetProfileService.prototype.GetCourse = function () {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
                return this.getCourseSvc.get(new shared.GetCourseRequest(userId, classroomId)).$promise;
            };
            GetProfileService.prototype.GetAllCourse = function () {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.getAllCourseSvc.query(new shared.GetAllCourseRequest(userId)).$promise;
            };
            GetProfileService.prototype.GetNotificationNumber = function () {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
                return this.getNotificationNumberSvc.get(new shared.GetNotificationNumberRequest(userId, classroomId)).$promise;
            };
            GetProfileService.prototype.GetNotificationContent = function () {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
                return this.getNotificationContentSvc.query(new shared.GetNotificationContentRequest(userId, classroomId)).$promise;
            };
            GetProfileService.prototype.GetLike = function () {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
                var lessonId = this.userprofileSvc.GetClientUserProfile().CurrentDisplayLessonId;
                return this.getLikeSvc.get(new shared.GetLikeRequest(userId, classroomId, lessonId)).$promise;
            };
            GetProfileService.prototype.GetAllLike = function () {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                var classroomId = this.userprofileSvc.GetClientUserProfile().CurrentClassRoomId;
                var lessonId = this.userprofileSvc.GetClientUserProfile().CurrentLessonId;
                return this.getAllLikeSvc.get(new shared.GetAllLikeRequest(userId, classroomId)).$promise;
            };
            GetProfileService.$inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
            return GetProfileService;
        })();
        shared.GetProfileService = GetProfileService;
        angular
            .module('app.shared')
            .service('app.shared.ClientUserProfileService', ClientUserProfileService)
            .service('app.shared.CommentService', CommentService)
            .service('app.shared.DiscussionService', DiscussionService)
            .service('app.shared.GetProfileService', GetProfileService);
    })(shared = app.shared || (app.shared = {}));
})(app || (app = {}));
var app;
(function (app) {
    var sidemenus;
    (function (sidemenus) {
        'use strict';
        var SideMenuController = (function () {
            function SideMenuController($scope, $state, userSvc, sideMenuSvc, notificationSvc) {
                var _this = this;
                this.$scope = $scope;
                this.$state = $state;
                this.userSvc = userSvc;
                this.sideMenuSvc = sideMenuSvc;
                this.notificationSvc = notificationSvc;
                this.userProfile = userSvc.GetClientUserProfile();
                this.notificationSvc.GetNotificationNumber()
                    .then(function (it) {
                    if (it == null) {
                        _this.notification = 0;
                    }
                    else
                        _this.notification = it.notificationTotal;
                });
                this.AllAvailableCourses = this.userSvc.GetAllAvailableCourses();
                this.userSvc.GetFriendLists();
            }
            SideMenuController.prototype.GetUserProfileId = function () {
                return encodeURI(this.userProfile.UserProfileId);
            };
            SideMenuController.prototype.ChangeTab = function (name) {
                this.sideMenuSvc.CurrentTabName = name;
            };
            SideMenuController.prototype.ChangeClassRoom = function (classRoomId, lessonId, className) {
                var userProfile = this.userSvc.GetClientUserProfile();
                userProfile.CurrentClassRoomId = classRoomId;
                userProfile.CurrentLessonId = lessonId;
                userProfile.ClassName = className;
                // TODO: Update user profile
                //userProfile.CurrentStudentCode
                //userProfile.IsTeacher
                //userProfile.NumberOfStudents
                //userProfile.StartDate
                this.userSvc.UpdateUserProfile(userProfile);
                this.$state.go("app.main.lesson", { 'classRoomId': classRoomId, 'lessonId': lessonId }, { inherit: false });
            };
            SideMenuController.$inject = ['$scope', '$state', 'app.shared.ClientUserProfileService', 'app.sidemenus.SideMenuService', 'app.shared.GetProfileService'];
            return SideMenuController;
        })();
        angular
            .module('app.sidemenus')
            .controller('app.sidemenus.SideMenuController', SideMenuController);
    })(sidemenus = app.sidemenus || (app.sidemenus = {}));
})(app || (app = {}));
var app;
(function (app) {
    var sidemenus;
    (function (sidemenus) {
        'use strict';
        var SideMenuService = (function () {
            function SideMenuService() {
                this.CurrentTabName = "Home";
            }
            return SideMenuService;
        })();
        sidemenus.SideMenuService = SideMenuService;
        angular
            .module('app.sidemenus')
            .service('app.sidemenus.SideMenuService', SideMenuService);
    })(sidemenus = app.sidemenus || (app.sidemenus = {}));
})(app || (app = {}));
var app;
(function (app) {
    var studentlists;
    (function (studentlists) {
        'use strict';
        var studentlistsController = (function () {
            function studentlistsController($scope, classRoomId, userSvc) {
                this.$scope = $scope;
                this.classRoomId = classRoomId;
                this.userSvc = userSvc;
            }
            studentlistsController.$inject = ['$scope', 'classRoomId', 'app.shared.ClientUserProfileService'];
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
        //export class GetFriendListRequest {
        //    constructor(
        //        public userId: string,
        //        public classRoomId: string) {
        //    }
        //}
        var SendFriendRequest = (function () {
            function SendFriendRequest(FromUserProfileId, ToUserProfileId, RequestId, IsAccept) {
                this.FromUserProfileId = FromUserProfileId;
                this.ToUserProfileId = ToUserProfileId;
                this.RequestId = RequestId;
                this.IsAccept = IsAccept;
            }
            return SendFriendRequest;
        })();
        studentlists.SendFriendRequest = SendFriendRequest;
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
                //this.GetStudentListsvc = <IStudentListResourceClass<any>>$resource(appConfig.StudentListUrl, { 'userId': '@userId', 'classRoomId': '@classRoomId' });
                this.SendFriendRequestsvc = $resource(appConfig.SendFriendRequestUrl, { 'FromUserProfileId': '@FromUserProfileId', 'ToUserProfileId': '@ToUserProfileId', 'RequestId': '@RequestId', 'IsAccept': '@IsAccept' });
            }
            //public GetStudentList(classRoomId: string): ng.IPromise<any> {
            //    var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            //    return this.GetStudentListsvc.query(new GetFriendListRequest(userId, classRoomId)).$promise;
            //}
            StudentListService.prototype.SendFriendRequest = function (ToUserProfileId, RequestId, IsAccept) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.SendFriendRequestsvc.save(new studentlists.SendFriendRequest(userId, ToUserProfileId, RequestId, IsAccept)).$promise;
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
            function teacherlistsController($scope, list, classRoomId, teacherlistsSvc) {
                this.$scope = $scope;
                this.list = list;
                this.classRoomId = classRoomId;
                this.teacherlistsSvc = teacherlistsSvc;
            }
            teacherlistsController.prototype.RemoveStd = function (removeObj) {
                this.teacherlistsSvc.RemoveStudent(this.classRoomId, removeObj.id);
                var removeIndex = this.list.indexOf(removeObj);
                this.list.splice(removeIndex, 1);
            };
            teacherlistsController.$inject = ['$scope', 'list', 'classRoomId', 'app.teacherlists.TeacherListService'];
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
        var RemoveStudentRequest = (function () {
            function RemoveStudentRequest(classRoomId, UserProfileId, RemoveUserProfileId) {
                this.classRoomId = classRoomId;
                this.UserProfileId = UserProfileId;
                this.RemoveUserProfileId = RemoveUserProfileId;
            }
            return RemoveStudentRequest;
        })();
        teacherlists.RemoveStudentRequest = RemoveStudentRequest;
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
                this.GetStudentListsvc = $resource(appConfig.TeacherListUrl, { 'userId': '@userId', 'classRoomId': '@classRoomId' });
                this.RemoveStudentsvc = $resource(appConfig.TeacherRemoveStdUrl, { 'classRoomId': '@classRoomId', 'UserProfileId': '@UserProfileId', 'RemoveUserProfileId': '@RemoveUserProfileId' });
            }
            TeacherListService.prototype.GetStudentList = function (classRoomId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.GetStudentListsvc.query(new teacherlists.GetStudentListRequest(userId, classRoomId)).$promise;
            };
            TeacherListService.prototype.RemoveStudent = function (classRoomId, removeId) {
                var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
                return this.RemoveStudentsvc.save(new teacherlists.RemoveStudentRequest(classRoomId, userId, removeId)).$promise;
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