angular.module('app', ['ui.router', 'app.shared', 'app.lessons', 'app.studentlists', 'app.coursemaps', 'app.journals', 'app.teacherlists', 'appDirectives', 'app.sidemenus'])    
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
                                (params, svc) => { return svc.GetComments(params.classRoomId, params.targetUserId) }]
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
                url: '/studentlist/:classRoomId',
                views: {
                    'courseContent': {
                        templateUrl: 'tmpl/studentlist.html',
                        controller: 'app.studentlists.studentlistsController as cx',
                        resolve: {
                            'list': ['$stateParams', 'app.studentlists.StudentListService',
                                (params, svc) => { return svc.GetStudentList(params.classRoomId) }]
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
                                (params, svc) => { return svc.GetStudentList(params.classRoomId) }]
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
            })
            ;

        $urlRouterProvider.otherwise('/app/main/lesson/Lesson01/ClassRoom01');
    }]);