module app.calendar {
    'use strict';

    class CarlendarController {
        public BeginDate: Date;
        public date = new Date();
        public year;
        public monthdays = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
        public monthAllNames = new Array("Jan", "Feb", "Mar", "Apl", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
        public today = new Date();
        public selected = new Array();
        public firstClick = true;
        public fisrtSelected: Date;
        public lastSelected: Date;
        public selectDay = new Array("NO", "NO", "NO", "NO", "NO", "NO", "NO");
        public Sifht: boolean;
        public month; // month
        public allweeks = []; // array for push week
        public monthNames: string;
        private courseInformation: any;
        private isPrepareCourseScheduleContentComplete: boolean;
        private sun: boolean = false;
        private mon: boolean = false;
        private tue: boolean = false;
        private wed: boolean = false;
        private thu: boolean = false;
        private fri: boolean = false;
        private sat: boolean = false;
        private IsHolidayRange: boolean = false;
        private ShifhtRange: boolean = false;
        private IsHolidayWeek: boolean = false;
        private ShifhtWeek: boolean = false;


        static $inject = ['$scope', 'app.calendar.CourseScheduleService', 'app.shared.ClientUserProfileService'];
        constructor(private $scope, private courseScheduleService: app.calendar.CourseScheduleService, private clientProfileSvc: app.shared.ClientUserProfileService) {
            this.prepareUserprofile();
        }

        private prepareUserprofile(): void {
            this.clientProfileSvc.PrepareAllUserProfile().then(() => {
                this.prepareCourseScheduleContents();
            });
        }

        private prepareCourseScheduleContents(): void {
            this.courseScheduleService.GetCourseSchedule().then(respond => {
                if (respond != null) {
                    this.courseInformation = respond;
                    this.isPrepareCourseScheduleContentComplete = true;
                    this.prepareSchedule();
                }
            }, error => {
                console.log('Load course schedule content failed');
            });
        }

        private prepareSchedule(): void {
            this.setStartDate();
            this.month = this.today.getMonth();
            this.year = this.today.getFullYear();
            this.setCalendar(this.month, this.year);
            this.fisrtSelected = new Date(this.today.toDateString());
            //-----------------null Begin date---------------------
            //this.courseInformation.BeginDate = null;
        }

        public setStartDate(): void {
            if (this.courseInformation.BeginDate == null) this.BeginDate =  new Date();
            else this.BeginDate = new Date(this.courseInformation.BeginDate);
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
                        day <= new Date(this.courseInformation.EndDate)
                    ) return true;
                }
                else {
                    if (day >= new Date(this.courseInformation.Lessons[i].BeginDate) &&
                        day < new Date(this.courseInformation.Lessons[i + 1].BeginDate) 
                    ) return true;
                }
            }
        }

        public IsEven(day: Date) {
            if (!this.isPrepareCourseScheduleContentComplete) return false;
            for (var i = 1; i < this.courseInformation.Lessons.length; i += 2) {
                if (i == this.courseInformation.Lessons.length - 1) {
                    if (day >= new Date(this.courseInformation.Lessons[i].BeginDate) &&
                        day <= new Date(this.courseInformation.EndDate) 
                    ) return true;
                }
                else {
                    if (day >= new Date(this.courseInformation.Lessons[i].BeginDate) &&
                        day < new Date(this.courseInformation.Lessons[i + 1].BeginDate) 
                    ) return true;
                }
            }
        }

        public IsShift(day: Date) {
            if (!this.isPrepareCourseScheduleContentComplete) return false;
            for (var i = 0; i < this.courseInformation.ShiftDays.length; i++) {
                if (new Date(this.courseInformation.ShiftDays[i]).toDateString() == day.toDateString()) return true;
            }
            return false;
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
            if (this.selected == null) return false;
            for (var i = 0; i < this.selected.length; i++) {
                if (this.selected[i].toDateString() == day.toDateString()) return true;
            }
            return false;
        }

        public cancelSelected() {
            this.selected = [];
            this.fisrtSelected = null;
            this.lastSelected = null;
            this.firstClick = true;
            this.sun = false;
            this.mon = false;
            this.tue = false;
            this.wed = false;
            this.thu = false;
            this.fri = false;
            this.sat = false;
            this.IsHolidayRange = false;
            this.ShifhtRange = false;
            this.IsHolidayWeek = false;
            this.ShifhtWeek = false;
        }

        public SetStartDate(starDate: Date) {
            if (!this.isPrepareCourseScheduleContentComplete) return;
            this.courseScheduleService.SetCourseStartDate(starDate)
                .then(it=> {
                    it.Lessons.BeginDate
                    this.courseInformation = new app.calendar.Calendar(it.IsComplete, it.BeginDate, it.EndDate, it.Lessons, it.Holidays, it.ShiftDays);
                });
            this.cancelSelected();
        }

        public SetCourseScheduleRange() {
            if (!this.isPrepareCourseScheduleContentComplete) return;
            this.courseScheduleService.SetCourseScheduleRange(this.IsHolidayRange, this.ShifhtRange, this.fisrtSelected, this.lastSelected).then(it=> {
                it.Lessons.BeginDate
                this.courseInformation = new app.calendar.Calendar(it.IsComplete, it.BeginDate, it.EndDate, it.Lessons, it.Holidays, it.ShiftDays);
            });
            this.cancelSelected();
        }

        public SetCourseScheduleWeek() {
            if (!this.isPrepareCourseScheduleContentComplete) return;
            this.courseScheduleService.SetCourseScheduleWeek(this.IsHolidayWeek, this.ShifhtWeek, this.sun, this.mon, this.tue, this.wed, this.thu, this.fri, this.sat).then(it=> {
                it.Lessons.BeginDate
                this.courseInformation = new app.calendar.Calendar(it.IsComplete, it.BeginDate, it.EndDate, it.Lessons, it.Holidays, it.ShiftDays);
            });
            this.cancelSelected();
        }

        public ApplyToAllCourse() {
            this.courseScheduleService.ApplyToAllCourse()
        }

    }
    angular
        .module('app.calendar')
        .controller('app.calendar.CarlendarController', CarlendarController);
}