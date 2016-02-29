angular.module('app', ['ui.router', 'app.shared', 'app.lessons', 'app.studentlists', 'app.coursemaps', 'app.notification', 'app.journals', 'app.teacherlists', 'appDirectives', 'app.sidemenus', 'app.settings'])    
    .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider: angular.ui.IStateProvider, $urlRouterProvider: angular.ui.IUrlRouterProvider) {
        $stateProvider

            .state('app', {
                url: '/app',
                templateUrl: 'tmpl/layout.html',
                controller: 'app.shared.MainController as appcx',
                resolve: {
                    'userInfo': ['app.shared.GetProfileService', svc => { return svc.GetProfile() }],
                    'allCourse': ['app.shared.GetProfileService', svc => { return svc.GetAllCourse() }]
                }
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
                                (params, svc) => { return svc.GetContent(params.lessonId, params.classRoomId) }],
                            'comment': ['$stateParams', 'app.shared.CommentService',
                                (params, svc) => { return svc.GetComments(params.lessonId, params.classRoomId) }],
                            'classRoomId': ['$stateParams', params => { return params.classRoomId }],
                            'lessonId': ['$stateParams', params => { return params.lessonId }],
                            'likes': ['app.shared.GetProfileService', svc => { return svc.GetLike() }]
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
                        templateUrl: 'tmpl/notification.html'
                        //controller: 'app.notification.NotificationController as cx',
                        //resolve: {
                        //    'notification': ['app.shared.GetProfileService', svc => { return svc.GetNotificationContent() }]
                        //}
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
                                (params, svc) => { return svc.GetComments(params.classRoomId, params.targetUserId) }],
                            'targetUserId': ['$stateParams', params => { return params.targetUserId }],
                            'likes': ['app.shared.GetProfileService', svc => { return svc.GetAllLike() }]
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
                                (params, svc) => { return svc.GetContent(params.classRoomId) }],
                            'status': ['$stateParams', 'app.coursemaps.CourseMapService',
                                (params, svc) => { return svc.GetLessonStatus(params.classRoomId) }]
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
                            'list': ['$stateParams', 'app.studentlists.StudentListService',
                                (params, svc) => { return svc.GetStudentList(params.classRoomId) }],
                            'classRoomId': ['$stateParams', params => { return params.classRoomId }]
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
                                (params, svc) => { return svc.GetStudentList(params.classRoomId) }],
                            'classRoomId': ['$stateParams', params => { return params.classRoomId }]
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
                            'courseInfo': ['app.shared.GetProfileService', svc => { return svc.GetCourse() }]
                        }
                    }
                }
            })
            ;

        $urlRouterProvider.otherwise('/app/main/lesson/Lesson01/ClassRoom01');
    }]);