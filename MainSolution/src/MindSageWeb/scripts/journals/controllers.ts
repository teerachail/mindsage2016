module app.journals {
    'use strict';

    class JournalController {

        private userprofile: any;
        public openDiscussion: string;

        static $inject = ['$scope', 'content', 'app.shared.ClientUserProfileService', 'app.shared.DiscussionService'];
        constructor(private $scope, public content, private svc: app.shared.ClientUserProfileService, private discussionSvc: app.shared.DiscussionService) {
            this.userprofile = this.svc.GetClientUserProfile();
        }

        public GetWeeks() {
            var usedWeekNo = {};
            var lessonWeeks = [];
            for (var index = 0; index < this.content.Comments.length; index++) {
                var lessonWeekNo = this.content.Comments[index].LessonWeek;
                if (usedWeekNo.hasOwnProperty(lessonWeekNo)) continue;

                lessonWeeks.push(lessonWeekNo);
                usedWeekNo[lessonWeekNo] = 1;
            }
            return lessonWeeks;
        }

        public GetComments(week: number) {
            var qry = this.content.Comments.filter(it=> it.LessonWeek == week);
            return qry;
        }

        public showDiscussion(item: any): void {
            this.openDiscussion = item.id;
        }

        public hideDiscussion(): void {
            this.openDiscussion = "";
        }

    }

    angular
        .module('app.journals')
        .controller('app.journals.JournalController', JournalController);
}