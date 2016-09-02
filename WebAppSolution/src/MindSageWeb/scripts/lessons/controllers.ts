module app.lessons {
    'use strict';

    class LessonController {

        private content: any;
        private comment: any;
        public discussions: any;
        private requestedCommentIds: any;

        public message: string;
        public targetComment: any;
        public targetDiscussion: any;
        public deleteComment: boolean;
        public EditId: string;
        private likes: any;

        private ItemSelect: any;
        private QuestionSelect: any;
        private QuestionIndex: number;
        private LessonAnswer: any;
        private Answer: string;
        private AnswerSend: boolean;

        private IsTeacher: boolean;
        private currentUserId: string;
        private notification: number;

        private lessonId: string;
        private classRoomId: string;
        static $inject = ['$sce', '$scope', '$state', 'app.shared.ClientUserProfileService', 'app.shared.GetProfileService', '$q', '$stateParams', 'defaultUrl', 'app.shared.DiscussionService', 'app.shared.CommentService', 'app.lessons.LessonService'];
        constructor(private $sce, private $scope, private $state, private userprofileSvc: app.shared.ClientUserProfileService, private getProfileSvc: app.shared.GetProfileService, private $q, private $stateParams, private defaultUrl, private discussionSvc: app.shared.DiscussionService, private commentSvc: app.shared.CommentService, private lessonSvc: app.lessons.LessonService) {
            this.requestedCommentIds = ['', ''];
            this.discussions = [];
            this.LessonAnswer = {
                id: '01',
                Answers: [
                ]
            }

            this.lessonId = this.$stateParams.lessonId;
            this.classRoomId = this.$stateParams.classRoomId;
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            this.userprofileSvc.PrepareAllUserProfile().then(() => {
                this.userprofileSvc.ClientUserProfile.CurrentDisplayLessonId = this.lessonId;
                this.currentUserId = encodeURI(this.userprofileSvc.ClientUserProfile.UserProfileId);
                this.IsTeacher = this.userprofileSvc.ClientUserProfile.IsTeacher;
                this.prepareLessonContents();
                this.loadNotifications();
            });
        }

        private prepareLessonContents(): void {
            this.$q.all([
                this.getProfileSvc.GetLike(),
                this.lessonSvc.GetContent(this.lessonId, this.classRoomId),
                this.commentSvc.GetComments(this.lessonId, this.classRoomId),
                this.lessonSvc.LessonAnswer(this.lessonId, this.classRoomId)
            ]).then(data => {
                this.likes = data[0];
                this.content = data[1];
                this.userprofileSvc.PrimaryVideoUrl = this.$sce.trustAsResourceUrl(data[1].PrimaryContentURL);
                this.comment = data[2];
                this.userprofileSvc.Advertisments = this.content.Advertisments;
                this.LessonAnswer = data[3];
                this.SelectItem(this.content.StudentItems[0]);
                this.SelectQuestion(null);
            }, error => {
                console.log('Load lesson content failed');
                this.prepareLessonContents();
            });
        }

        private loadNotifications(): void {
            this.getProfileSvc.GetNotificationNumber()
                .then(respond => {
                    if (respond == null) this.notification = 0;
                    else this.notification = respond.notificationTotal;
                }, error => {
                    console.log('Load notifications content failed');
                });
        }

        public ChangeCourse(classRoomId: string, lessonId: string, classCalendarId: string) {
            this.userprofileSvc.ChangeCourse(classRoomId).then(respond => {
                this.userprofileSvc.UpdateCourseInformation(respond);
                this.userprofileSvc.ClientUserProfile.CurrentLessonId = lessonId;
                this.userprofileSvc.ClientUserProfile.CurrentClassCalendarId = classCalendarId;
                this.$state.go("app.main.lesson", { 'classRoomId': classRoomId, 'lessonId': lessonId }, { inherit: false });
            }, error => {
                console.log('Change course failed');
            });
        }

        private SelectItem(item: any): void {
            this.ItemSelect = item;
            this.CheckPreAssessments();
        }

        private CheckPreAssessments(): void {
            if (this.content.PreAssessments != null)
                for (var index = 0; index < this.content.PreAssessments.length; index++)
                    for (var quest = 0; quest < this.content.PreAssessments[index].Assessments.length; quest++)
                        if (this.CheckQuestion(this.content.PreAssessments[index]) < this.content.PreAssessments[index].Assessments.length) {
                            this.ItemSelect = this.content.PreAssessments[index];
                            this.SelectQuestion(this.content.PreAssessments[index]);
                            return;
                        }
        }

        private SelectQuestion(item: any): void {
            if (item == null) {
                this.QuestionSelect = null;
                this.CheckPreAssessments();
                return;
            }
            this.QuestionIndex = this.CheckQuestion(item);
            if (this.QuestionIndex >= item.Assessments.length) this.QuestionIndex--;
            this.AnswerSend = this.HaveAnswer(item.Assessments[this.QuestionIndex]);
            this.QuestionSelect = item.Assessments[this.QuestionIndex];

            var answer = this.LessonAnswer.Answers.filter(it => it.AssessmentId == this.QuestionSelect.id)[0];
            if (answer == null)
                this.Answer = null;
            else
                this.Answer = answer.Answer;

            var IsPreTest = this.content.PreAssessments.filter(it => it.id == item.id)[0];
            if (IsPreTest == null)
                this.CheckPreAssessments();
        }

        private NextQuestion(): void {
            this.QuestionIndex = this.CheckQuestion(this.ItemSelect);
            if (this.QuestionIndex < this.ItemSelect.Assessments.length) {
                this.QuestionSelect = this.ItemSelect.Assessments[this.QuestionIndex];
                var answer = this.LessonAnswer.Answers.filter(it => it.AssessmentId == this.ItemSelect.Assessments[this.QuestionIndex].id)[0];
                if (answer == null) {
                    this.Answer = null;
                    this.AnswerSend = false;
                    return;
                }
                this.Answer = answer.Answer;
                this.AnswerSend = true;
            }
        }

        private NextPage(): void {
            var index = 0;
            if (this.content.PreAssessments.filter(it => it.id == this.ItemSelect.id)[0] != null) {
                index = this.content.PreAssessments.indexOf(this.ItemSelect);
                if (++index < this.content.PreAssessments.length) {
                    this.SelectItem(this.content.PreAssessments[index]);
                    this.SelectQuestion(this.content.PreAssessments[index]);
                    return;
                }
            }
            if (this.content.PostAssessments.filter(it => it.id == this.ItemSelect.id)[0] != null) {
                index = this.content.PostAssessments.indexOf(this.ItemSelect);
                if (++index < this.content.PostAssessments.length) {
                    this.SelectItem(this.content.PostAssessments[index]);
                    this.SelectQuestion(this.content.PostAssessments[index]);
                    return;
                }
            }

            if (this.IsTeacher)
                this.SelectItem(this.content.TeacherItems[0]);
            else
                this.SelectItem(this.content.StudentItems[0]);
            this.SelectQuestion(null);
        }

        private CheckQuestion(item: any): number {
            if (item.Assessments != null) {
                for (var index = 0; index < item.Assessments.length; index++) {
                    if (this.LessonAnswer.Answers.filter(it => it.AssessmentId == item.Assessments[index].id)[0] == null)
                        return index;
                }
                return item.Assessments.length;
            } else return 0;
        }

        private MenuOnSelect(item: string, IsSelect: boolean) {
            if (IsSelect)
                return item.replace('.png', '_SELECTED.png');
            else
                return item;
        }

        private SendAnswer(): void {
            var item = {
                AssessmentId: this.QuestionSelect.id,
                Answer: this.Answer
            }
            this.LessonAnswer.Answers.push(item);
            this.lessonSvc.CreateNewAnswer(this.LessonAnswer, item);
            this.AnswerSend = true;
        }

        private HaveAnswer(item: any): boolean {
            if (this.LessonAnswer.Answers.filter(it => it.AssessmentId == item.id)[0])
                return true;
            else
                return false;
        }

        private IsAnswerCorrect(item: any): boolean {
            var answer = this.LessonAnswer.Answers.filter(it => it.AssessmentId == item.id)[0];
            if (answer == null) return null;
            if (item.Choices == null || item.Choices.length == 0) return true;
            return item.Choices.filter(it => it.id == answer.Answer)[0].IsCorrect;
        }

        private ChangeQuestionButton(): boolean {
            if (this.ItemSelect != null) {
                if (this.ItemSelect.Assessments != null) {
                    return this.AnswerSend && this.CheckQuestion(this.ItemSelect) < this.ItemSelect.Assessments.length;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private FinishButton(): boolean {
            if (this.ItemSelect != null) {
                if (this.ItemSelect.Assessments != null) {
                    return this.AnswerSend && this.CheckQuestion(this.ItemSelect) >= this.ItemSelect.Assessments.length;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private HideChoice(item: string): boolean {
            var HaveString = item;
            HaveString = HaveString.replace('<!DOCTYPE html>', '');
            HaveString = HaveString.replace(' ', '');
            HaveString = HaveString.replace('<html>', '');
            HaveString = HaveString.replace('<head>', '');
            HaveString = HaveString.replace('</head>', '');
            HaveString = HaveString.replace('<body>', '');
            HaveString = HaveString.replace('</body>', '');
            HaveString = HaveString.replace('</html>', '');
            HaveString = HaveString.trim();
            return HaveString.trim() == '';
        }

        public HtmlReplace(item: string) {
            if (item == null) return;
            item = item.replace('<p>', '<span>');
            item = item.replace('</p>', '</span>');
            return item;
        }

        //comment & discussions
        public showDiscussion(item: any, open: boolean) {
            this.GetDiscussions(item);
            return !open;
        }

        public GetDiscussions(comment) {
            if (comment == null) return;

            const NoneDiscussion = 0;
            if (comment.TotalDiscussions <= NoneDiscussion) return;
            if (this.requestedCommentIds == null) this.requestedCommentIds.push(comment.id);
            if (this.requestedCommentIds.filter(it => it == comment.id).length > NoneDiscussion) return;
            else this.requestedCommentIds.push(comment.id);


            this.discussionSvc
                .GetDiscussions(this.lessonId, this.classRoomId, comment.id)
                .then(it => {
                    if (it == null) return;
                    if (this.discussions == null) this.discussions = it;
                    else
                        for (var index = 0; index < it.length; index++) {
                            if (this.discussions.filter(dis => dis.id == it[index].id).length == 0)
                                this.discussions.push(it[index]);
                        }
                });
        }

        public CreateNewComment(message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return message;

            this.commentSvc.CreateNewComment(this.classRoomId, this.lessonId, message)
                .then(it => {
                    if (it == null) {
                        return message;
                    }
                    else {
                        var userprofile = this.userprofileSvc.ClientUserProfile;
                        var newComment = new app.shared.Comment(it.ActualCommentId, message, 0, 0, userprofile.ImageUrl, userprofile.FullName, this.classRoomId, this.lessonId, userprofile.UserProfileId, 0 - this.comment.Comments.length);
                        this.comment.Comments.push(newComment);
                        return "";
                    }
                });
        }

        public CreateNewDiscussion(commentId: string, message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return message;

            this.discussionSvc.CreateDiscussion(this.classRoomId, this.lessonId, commentId, message)
                .then(it => {
                    if (it == null) {
                        return message;
                    }
                    else {
                        var userprofile = this.userprofileSvc.ClientUserProfile;
                        var discussionOrder = 0;
                        if (this.discussions != null) discussionOrder = this.discussions.length;

                        var newDiscussion = new app.shared.Discussion(it.ActualCommentId, commentId, message, 0, userprofile.ImageUrl, userprofile.FullName, userprofile.UserProfileId, 0 - discussionOrder);
                        this.discussions.push(newDiscussion);
                        this.comment.Comments.filter(it => it.id == commentId)[0].TotalDiscussions++;
                        return "";
                    }
                });
        }

        public LikeComment(commentId: string, IsLike: number) {
            if (IsLike == -1)
                this.comment.Comments.filter(it => it.id == commentId)[0].TotalLikes++;
            else if (this.comment.Comments.filter(it => it.id == commentId)[0].TotalLikes > 0)
                this.comment.Comments.filter(it => it.id == commentId)[0].TotalLikes--;

            this.commentSvc.LikeComment(this.classRoomId, this.lessonId, commentId);

            var removeIndex = this.likes.LikeCommentIds.indexOf(commentId);
            const ElementNotFound = -1;
            if (removeIndex <= ElementNotFound) this.likes.LikeCommentIds.push(commentId);
            else this.likes.LikeCommentIds.splice(removeIndex, 1);
        }

        public LikeDiscussion(commentId: string, discussionId: string, IsLike: number) {
            if (IsLike == -1)
                this.discussions.filter(it => it.id == discussionId)[0].TotalLikes++;
            else if (this.discussions.filter(it => it.id == discussionId)[0].TotalLikes > 0)
                this.discussions.filter(it => it.id == discussionId)[0].TotalLikes--;

            this.discussionSvc.LikeDiscussion(this.classRoomId, this.lessonId, commentId, discussionId);

            var removeIndex = this.likes.LikeDiscussionIds.indexOf(discussionId);
            const ElementNotFound = -1;
            if (removeIndex <= ElementNotFound) this.likes.LikeDiscussionIds.push(discussionId);
            else this.likes.LikeDiscussionIds.splice(removeIndex, 1);
        }

        public DeleteComment() {
            var comment = this.targetComment;
            var removeIndex = this.comment.Comments.indexOf(comment);
            if (removeIndex > -1) this.comment.Comments.splice(removeIndex, 1);
            this.commentSvc.UpdateComment(this.classRoomId, this.lessonId, comment.id, true, null);
        }

        public DeleteDiscussion() {
            var commentId = this.targetComment.id;
            var discussion = this.targetDiscussion;
            var removeIndex = this.discussions.indexOf(discussion);
            if (removeIndex > -1) this.discussions.splice(removeIndex, 1);
            this.comment.Comments.filter(it => it.id == commentId)[0].TotalDiscussions--;
            this.discussionSvc.UpdateDiscussion(this.classRoomId, this.lessonId, commentId, discussion.id, true, null);
        }

        public EditComment(commentId: string, message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return;

            this.commentSvc.UpdateComment(this.classRoomId, this.lessonId, commentId, false, message);
        }

        public EditDiscussion(commentId: string, discussionId: string, message: string) {
            const NoneContentLength = 0;
            if (message.length <= NoneContentLength) return;

            this.discussionSvc.UpdateDiscussion(this.classRoomId, this.lessonId, commentId, discussionId, false, message);
        }

        public LikeLesson() {
            this.likes.IsLikedLesson = !this.likes.IsLikedLesson;
            this.content.TotalLikes++;
            this.lessonSvc.LikeLesson(this.classRoomId, this.lessonId);
        }

        public DisLikeLesson() {
            this.likes.IsLikedLesson = !this.likes.IsLikedLesson;
            if (this.content.TotalLikes > 0)
                this.content.TotalLikes--;
            this.lessonSvc.LikeLesson(this.classRoomId, this.lessonId);
        }

        public EditOpen(message: any) {
            this.message = message.Description;
            this.EditId = message.id;
        }

        public SaveEdit(messageId: number) {
            this.comment.Comments.filter(it => it.id == messageId)[0].Description = this.message;
            this.EditComment(this.comment.Comments.filter(it => it.id == messageId)[0].id, this.message);
            this.EditId = null;
        }

        public SaveEditDiscus(commentId: string, messageId: number) {

            this.discussions.filter(it => it.id == messageId)[0].Description = this.message;
            this.EditDiscussion(commentId, this.discussions.filter(it => it.id == messageId)[0].id, this.message);
            this.EditId = null;
        }

        public CancelEdit() {
            this.EditId = null;
        }

        public IsEdit(id: string) {
            return this.EditId == id;
        }

        public deleteComfirm(comment: string) {
            this.targetComment = comment;
            this.targetDiscussion = null;
            this.deleteComment = true;
        }

        public deleteDisComfirm(comment: string, discussion: string) {
            this.targetComment = comment;
            this.targetDiscussion = discussion;
            this.deleteComment = false;
        }

    }

    angular
        .module('app.lessons')
        .controller('app.lessons.LessonController', LessonController);
}