module app.calendar {
    'use strict';

    class CarlendarController {
        public date = new Date();
        public year;
        public monthdays = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
        public monthAllNames = new Array("Jan", "Feb", "Mar", "Apl", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
        public today = new Date();
        public selected = new Array(this.today);
        public firstClick = true;
        public fisrtSelected: Date;
        public lastSelected: Date;
        public selectDay = new Array("NO", "NO", "NO", "NO", "NO", "NO", "NO");
        public Sifht: boolean;
        public month; // month
        public allweeks = []; // array for push week
        public monthNames: string;
        private courseInformation: any;
        private isWaittingForGetCourseScheduleContent: boolean;
        private isPrepareCourseScheduleContentComplete: boolean;

        static $inject = ['$scope', 'waitRespondTime', 'app.calendar.CourseScheduleService', 'app.shared.ClientUserProfileService'];
        constructor(private $scope, private waitRespondTime, private courseScheduleService: app.calendar.CourseScheduleService, private clientProfileSvc: app.shared.ClientUserProfileService) {
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            if (!this.clientProfileSvc.IsPrepareAllUserProfileCompleted()) {
                setTimeout(it => this.prepareUserprofile(), this.waitRespondTime);
                return;
            }

            this.prepareCourseScheduleContents();
        }

        private prepareCourseScheduleContents(): void {
            var shouldRequestCourseScheduleContent = !this.isPrepareCourseScheduleContentComplete && !this.isWaittingForGetCourseScheduleContent;
            if (shouldRequestCourseScheduleContent) {
                this.isWaittingForGetCourseScheduleContent = true;
                this.courseScheduleService.GetCourseSchedule().then(respond => {
                    if (respond != null) {
                        this.courseInformation = respond;
                        this.prepareSchedule();
                    }
                    this.isWaittingForGetCourseScheduleContent = false;
                    this.isPrepareCourseScheduleContentComplete = true;
                }, error => {
                    console.log('Load course schedule content failed, retrying ...');
                    this.isWaittingForGetCourseScheduleContent = false;
                    setTimeout(it=> this.prepareCourseScheduleContents(), this.waitRespondTime);
                });
            }
        }

        private prepareSchedule(): void {
            this.month = this.today.getMonth();
            this.year = this.today.getFullYear();
            this.setCalendar(this.month, this.year);
            this.fisrtSelected = new Date(this.today.toDateString());
            //-----------------null Begin date---------------------
            //this.courseInformation.BeginDate = null;
        }

        public getStartDate(): Date {
            alert(this.courseInformation.BeginDate);
            if (this.courseInformation.BeginDate == null) return new Date();
            else return new Date(this.courseInformation.BeginDate);
        }

        public setCalendar(month: number, year: number) {
            this.monthNames = this.monthAllNames[this.month];
            var firstDay = new Date(year,month,1);
            var days = this.monthdays[month]; // total day
            if (month == 1) {
                if (year % 4 == 0) days = 29;
            }
            var start = firstDay.getDay(); // start day off week
            if (start < 0) start += 7;
            var weeks = Math.floor((start + days) / 7); // total week on month
            if ((start + days) % 7 != 0) weeks++;

            var jumped = 0;
            var inserted = 1;

            var allWeeks = [];
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
                allWeeks.push(allday);
            }
            this.allweeks = allWeeks;
        }

        public nextMount() {
            this.month++;
            if (this.month > 11) {
                this.month = 0;
                this.year++;
            }
            this.allweeks = [];
            this.setCalendar(this.month, this.year);
        }

        public prevMount() {
            this.month--;
            if (this.month < 0) {
                this.month = 11;
                this.year--;
            }
            this.allweeks = [];
            this.setCalendar(this.month, this.year);
        }

        public CheckToday(day: Date) {
            return this.today.toDateString() == day.toDateString();
        }

        public IsHoliday(day: Date) {
            if (!this.isPrepareCourseScheduleContentComplete) return false;
            for (var i = 0; i < this.courseInformation.Holidays.length; i++) {
                if (new Date(this.courseInformation.Holidays[i]).toDateString() == day.toDateString()) return true;
            }
            return false;
        }

        public IsStartLesson(day: Date) {
            if (!this.isPrepareCourseScheduleContentComplete) return "";
            for (var i = 0; i < this.courseInformation.Lessons.length; i++) {
                if (day.toDateString() == new Date(this.courseInformation.Lessons[i].BeginDate).toDateString()) return this.courseInformation.Lessons[i].Name;
            }
            return "";
        }

        public IsOdd(day: Date) {
            if (!this.isPrepareCourseScheduleContentComplete) return false;
            for (var i = 0; i < this.courseInformation.Lessons.length; i += 2) {
                if (i == this.courseInformation.Lessons.length - 1) {
                    if (day >= new Date(this.courseInformation.Lessons[i].BeginDate) &&
                        day <= new Date(this.courseInformation.EndDate) &&
                        !this.OnSelected(day) &&
                        !this.IsHoliday(day)
                    ) return true;
                }
                else {
                    if (day >= new Date(this.courseInformation.Lessons[i].BeginDate) &&
                        day < new Date(this.courseInformation.Lessons[i + 1].BeginDate) &&
                        !this.OnSelected(day) &&
                        !this.IsHoliday(day)
                    ) return true;
                }
            }
        }

        public IsEven(day: Date) {
            if (!this.isPrepareCourseScheduleContentComplete) return false;
            for (var i = 1; i < this.courseInformation.Lessons.length; i += 2) {
                if (i == this.courseInformation.Lessons.length - 1) {
                    if (day >= new Date(this.courseInformation.Lessons[i].BeginDate) &&
                        day <= new Date(this.courseInformation.EndDate) &&
                        !this.OnSelected(day) &&
                        !this.IsHoliday(day)
                    ) return true;
                }
                else {
                    if (day >= new Date(this.courseInformation.Lessons[i].BeginDate) &&
                        day < new Date(this.courseInformation.Lessons[i + 1].BeginDate) &&
                        !this.OnSelected(day) &&
                        !this.IsHoliday(day)
                    ) return true;
                }
            }
        }

        public Selected(day: Date) {
            if (this.firstClick) {
                this.selected = [];
                this.selected.push(day);
                this.firstClick = !this.firstClick
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
                    var daySelect = new Date(daySelect.setDate(daySelect.getDate() + 1))
                }
                this.firstClick = !this.firstClick
                this.fisrtSelected = new Date(this.selected[0].toDateString());
                this.lastSelected = new Date(this.selected[this.selected.length - 1].toDateString());
            }
        }

        public OnSelected(day: Date) {
            for (var i = 0; i < this.selected.length; i++) {
                if (this.selected[i].toDateString() == day.toDateString()) return true;
            }
            return false;
        }

        public SetStartDate(starDate: Date) {
            if (!this.isPrepareCourseScheduleContentComplete) return;
            this.courseScheduleService.SetCourseStartDate(starDate)
                .then(it=> {
                    it.Lessons.BeginDate
                    this.courseInformation = new app.calendar.Calendar(it.IsComplete, it.BeginDate, it.EndDate, it.Lessons, it.Holidays);
                });
        }

        public SetCourseScheduleRange(isHoliday: boolean, isShift: boolean) {
            if (!this.isPrepareCourseScheduleContentComplete) return;
            this.courseInformation = this.courseScheduleService.SetCourseScheduleRange(isHoliday, isShift, this.fisrtSelected, this.lastSelected).then(it=> {
                it.Lessons.BeginDate
                this.courseInformation = new app.calendar.Calendar(it.IsComplete, it.BeginDate, it.EndDate, it.Lessons, it.Holidays);
            });
        }

        public SetCourseScheduleWeek(isHoliday: boolean, isShift: boolean, isSunday: boolean, isMonday: boolean, isTuesday: boolean, isWednesday: boolean, isTursday: boolean, isFriday: boolean, isSaturday: boolean) {
            if (!this.isPrepareCourseScheduleContentComplete) return;
            this.courseScheduleService.SetCourseScheduleWeek(isHoliday, isShift, isSunday, isMonday, isTuesday, isWednesday, isTursday, isFriday, isSaturday).then(it=> {
                it.Lessons.BeginDate
                this.courseInformation = new app.calendar.Calendar(it.IsComplete, it.BeginDate, it.EndDate, it.Lessons, it.Holidays);
            });
        }

        public ApplyToAllCourse() {
            this.courseScheduleService.ApplyToAllCourse()
        }

    }
    angular
        .module('app.calendar')
        .controller('app.calendar.CarlendarController', CarlendarController);
}