angular.module('app', ['ui.router', 'app.shared', 'app.lessons', 'app.studentlists'])
    .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider: angular.ui.IStateProvider, $urlRouterProvider: angular.ui.IUrlRouterProvider) {
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
                                (params, svc) => { return svc.GetContent(params.lessonId, params.classRoomId) }],
                            'comment': ['$stateParams', 'app.lessons.LessonCommentService',
                                (params, svc) => { return svc.GetComments(params.lessonId, params.classRoomId) }]
                        }
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

            .state('app.course.studentlist', {
                url: '/studentlist/:classRoomId',
                views: {
                    'courseContent': {
                        templateUrl: 'tmpl/studentlist.html',
                        controller: 'app.studentlists.studentlistsController as cx',
                        resolve: {
                            'students': ['$stateParams', 'app.studentlists.StudentListService',
                                (params, svc) => { return svc.GetStudentList(params.classRoomId) }]
                        }
                    }
                }
            })

            .state('app.course.teacherlist', {
                url: '/teacherlist',
                views: {
                    'courseContent': {
                        templateUrl: 'tmpl/teacherlist.html'
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
            })
            ;
        $urlRouterProvider.otherwise('/app/course//studentlist/ClassRoom01');
    }]);