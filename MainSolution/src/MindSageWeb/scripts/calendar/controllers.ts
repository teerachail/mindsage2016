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
        static $inject = ['$scope', '$state', 'courseSchedule', 'app.calendar.CourseScheduleService'];
        constructor(private $scope, private $state, private courseInformation: any, private courseScheduleService: app.calendar.CourseScheduleService) {
            this.month = this.today.getMonth();
            this.year = this.today.getFullYear();
            this.setCalendar(this.month, this.year);
            this.fisrtSelected = new Date(this.today.toDateString());
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
            for (var i = 0; i < this.courseInformation.Holidays.length; i++) {
                if (new Date(this.courseInformation.Holidays[i]).toDateString() == day.toDateString()) return true;
            }
            return false;
        }

        public IsStartLesson(day: Date) {
            for (var i = 0; i < this.courseInformation.Lessons.length; i++) {
                if (day.toDateString() == new Date(this.courseInformation.Lessons[i].BeginDate).toDateString()) return this.courseInformation.Lessons[i].Name;
            }
            return "";
        }

        public IsOdd(day: Date) {
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
            this.courseScheduleService.SetCourseStartDate(starDate);
        }
        public SetCourseScheduleRange(isHoliday: boolean, isShift: boolean) {
            this.courseScheduleService.SetCourseScheduleRange(isHoliday, isShift, this.fisrtSelected, this.lastSelected);
        }

    }
    angular
        .module('app.calendar')
        .controller('app.calendar.CarlendarController', CarlendarController);
}