module app.lessonpreviews {
    'use strict';

    class LessonPreviewController {

        private content: any;

        public message: string;

        private ItemSelect: any;
        private QuestionSelect: any;
        private QuestionIndex: number;
        private LessonAnswer: any;
        private Answer: string;
        private AnswerSend: boolean;

        private IsTeacher: boolean;

        static $inject = ['$sce', '$scope', '$state', 'defaultUrl', 'app.lessonpreviews.LessonService', '$stateParams'];
        constructor(private $sce, private $scope, private $state, private defaultUrl, private svc: app.lessonpreviews.LessonService, private $stateParams) {
            this.IsTeacher = true;
            
            this.LessonAnswer = {
                id: '01',
                Answer: [
                ]
            }
            this.prepareLessonContents();
        }

        private prepareLessonContents(): void {
            var lessonId = this.$stateParams.lessonId;
            this.svc.GetContent(lessonId).then(
                it => {
                    this.content = it;
                    console.log(it);
                    this.SelectItem(this.content.StudentItems[0]);
                    this.SelectQuestion(null);
                }                    ,
                error => console.log('Error: ' + error));
            (<any>angular.element(".owl-carousel")).owlCarousel({
                autoPlay: true,
                slideSpeed: 300,
                jsonPath: this.defaultUrl + '/api/lesson/' + lessonId + '/lessonpreviewads',
                singleItem: true
            });
        }

        private SelectItem(item: any): void {
            this.ItemSelect = item;
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
                return;
            }
            this.QuestionIndex = this.CheckQuestion(item);
            if (this.QuestionIndex >= item.Assessments.length) this.QuestionIndex--;
            this.AnswerSend = this.HaveAnswer(item.Assessments[this.QuestionIndex]);
            this.QuestionSelect = item.Assessments[this.QuestionIndex];

            var answer = this.LessonAnswer.Answer.filter(it => it.AssessmentId == this.QuestionSelect.id)[0];
            if (answer == null)
                this.Answer = null;
            else
                this.Answer = answer.Answer;
        }

        private NextQuestion(): void {
            this.QuestionIndex = this.CheckQuestion(this.ItemSelect);
            if (this.QuestionIndex < this.ItemSelect.Assessments.length) {
                this.QuestionSelect = this.ItemSelect.Assessments[this.QuestionIndex];
                var answer = this.LessonAnswer.Answer.filter(it => it.AssessmentId == this.ItemSelect.Assessments[this.QuestionIndex].id)[0];
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
                    if (this.LessonAnswer.Answer.filter(it => it.AssessmentId == item.Assessments[index].id)[0] == null)
                        return index;
                }
                return item.Assessments.length;
            } else return 0;
        }

        private SendAnswer(): void {
            var item = {
                AssessmentId: this.QuestionSelect.id,
                Answer: this.Answer
            }
            this.LessonAnswer.Answer.push(item);
            this.AnswerSend = true;
        }

        private HaveAnswer(item: any): boolean {
            if (this.LessonAnswer.Answer.filter(it => it.AssessmentId == item.id)[0])
                return true;
            else
                return false;
        }

        private IsAnswerCorrect(item: any): boolean {
            var answer = this.LessonAnswer.Answer.filter(it => it.AssessmentId == item.id)[0];
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

        //static $inject = ['$sce', '$q', '$stateParams', 'defaultUrl', 'app.lessonpreviews.LessonService'];
        //constructor(private $sce, private $q, private $stateParams, private defaultUrl, private svc: app.lessonpreviews.LessonService) {
        //    this.primaryContentIconUrl = defaultUrl + "/assets/img/iconic/media/video.png";
        //    this.prepareLessonContents();
        //}

        //private prepareLessonContents(): void {
        //    var lessonId = this.$stateParams.lessonId;
        //    this.svc.GetContent(lessonId).then(
        //        it => {
        //            this.content = it;
        //            this.PrimaryVideoUrl = this.$sce.trustAsResourceUrl(it.PrimaryContentURL);
        //        }                    ,
        //        error => console.log('Error: ' + error));
        //    (<any>angular.element(".owl-carousel")).owlCarousel({
        //        autoPlay: true,
        //        slideSpeed: 300,
        //        jsonPath: this.defaultUrl + '/api/lesson/' + lessonId + '/lessonpreviewads',
        //        singleItem: true
        //    });
        //}

        //public selectTeacherView(): void {
        //    this.teacherView = true;
        //}

        //public selectStdView(): void {
        //    this.teacherView = false;
        //}
    }

    angular
        .module('app.lessonpreviews')
        .controller('app.lessonpreviews.LessonPreviewController', LessonPreviewController);
}