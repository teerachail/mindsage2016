module app.shared {
    'use strict';

    class LessonLayoutController {
        public RunningVideoUrl: string;

        static $inject = ['$sce'];
        constructor(private $sce) {
        }

        public ChangeVideo(url: string) {
            this.RunningVideoUrl = this.$sce.trustAsResourceUrl(url);
        }
    }

    class CourseLayoutController {
    }

    class MainController {
        static $inject = ['userInfo', 'app.shared.ClientUserProfileService'];
        constructor(private userInfo, private userProfileSvc: app.shared.ClientUserProfileService) {
            var userProfile = this.userProfileSvc.GetClientUserProfile();
            userProfile.UserProfileId = this.userInfo.UserProfileId;
            userProfile.FullName = this.userInfo.FullName;
            userProfile.ImageUrl = this.userInfo.ImageUrl;
            userProfile.SchoolName = this.userInfo.SchoolName;
            userProfile.IsPrivateAccout = this.userInfo.IsPrivateAccout;
            userProfile.IsReminderOnceTime = this.userInfo.IsReminderOnceTime;
            userProfile.CurrentClassRoomId = this.userInfo.CurrentClassRoomId;
            userProfile.CurrentLessonId = this.userInfo.CurrentLessonId;
            this.userProfileSvc.UpdateUserProfile(userProfile);
        }
    }

    angular
        .module('app.shared')
        .controller('app.shared.LessonLayoutController', LessonLayoutController)
        .controller('app.shared.CourseLayoutController', CourseLayoutController)
        .controller('app.shared.MainController', MainController);
}