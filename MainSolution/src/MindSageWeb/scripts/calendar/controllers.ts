module app.calendar {
    'use strict';
    
    class CourseInformation {
        public BeginDate: Date;
        public EndDate: Date;
        public Lessons: LessonInfo[];
        public Holidays: Date[];
    }
    class LessonInfo {
        public Name: string;
        public BeginDate: Date;
    }

    class CarlendarController {

        public courseInformation: CourseInformation;

        public date = new Date();
        public year;
        public monthdays = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
        public monthAllNames = new Array("Jan", "Feb", "Mar", "Apl", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
        public today = new Date();
        public selected = new Array(this.today);
        public firstClick = true;
        public fisrtSelected: Date;
        public lastSelected: Date;
        //-----------Mock Data ----------------------
        public selectDay = new Array("NO", "NO", "NO", "NO", "NO", "NO", "NO");
        public Sifht: boolean;
        public EndCourseDate: Date;
        private holiday = [];
        public LessOrder: number;
        public LessonName: string;
        public Lesson = [];
        public Lesson2 = [];
        public Lesson3 = [];
        public Lessons = [];
        public Name: string;
        public Order: number;
        public Start: Date;
        public BeginDate: Date;
        public TestCalendarData :any ;
        //---------------------------------
        //public today = new Date('8/18/2016');
        public month; // month
        public allweeks = []; // array for push week
        public monthNames: string;
        static $inject = ['$scope', '$state','app.calendar.CourseScheduleService'];
        constructor(private $scope, private $state, private courseInformationT: app.calendar.CourseScheduleService) {
            this.TestCalendarData = this.courseInformationT.GetCourseSchedule();
            this.month = this.today.getMonth();
            this.year = this.today.getFullYear();
            this.setCalendar(this.month, this.year);
            this.fisrtSelected = new Date(this.today.toDateString());
            //-----------Mock Data ----------------------
            this.holiday.push(new Date('3/26/2016'), new Date('3/1/2016'), new Date('4/1/2016'));
            this.Lesson.push("Lesson01",  1, new Date('3/24/2016'));
            this.Lesson2.push("Lesson02", 2, new Date('3/30/2016'));
            this.Lesson3.push("Lesson03", 3, new Date('4/5/2016'));
            this.Lessons.push(this.Lesson, this.Lesson2, this.Lesson3);
            this.EndCourseDate = new Date('4/9/2016');
            //---------------------------------
            this.courseInformation = {
                BeginDate: new Date('3/24/2016'),
                EndDate: new Date('4/9/2016'),
                Lessons: [
                    {
                        Name: 'Lesson01',
                        BeginDate: new Date('3/24/2016')
                    },
                    {
                        Name: 'Lesson02',
                        BeginDate: new Date('3/30/2016')
                    },
                    {
                        Name: 'Lesson03',
                        BeginDate: new Date('4/5/2016')
                    },
                ],
                Holidays: [
                    new Date('3/26/2016'), new Date('3/1/2016'), new Date('4/1/2016'),
                ]
            };
        }

        public setCalendar(month: number, year: number) {
            this.monthNames = this.monthAllNames[this.month];
            var firstDay = new Date((month + 1) + '/1/' + year);
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
            for (var i = 0; i < this.holiday.length; i++){
                if (this.holiday[i].toDateString() == day.toDateString()) return true;
            }
            return false;
        }

        public IsStartLesson(day: Date) {
            for (var i = 0; i < this.courseInformation.Lessons.length; i++) {
                if (day.toDateString() == this.Lessons[i][2].toDateString()) return this.Lessons[i][0];
            }
            return "";
        }

        public IsOdd(day: Date) {
            for (var i = 0; i < this.courseInformation.Lessons.length; i += 2) {
                if (i == this.courseInformation.Lessons.length - 1) {
                    if (day >= this.courseInformation.Lessons[i].BeginDate &&
                        day <= this.courseInformation.EndDate &&
                        !this.OnSelected(day) &&
                        !this.IsHoliday(day)
                    ) return true;
                }
                else {
                    if (day >= this.courseInformation.Lessons[i].BeginDate &&
                        day < this.courseInformation.Lessons[i+1].BeginDate &&
                        !this.OnSelected(day) &&
                        !this.IsHoliday(day)
                    ) return true;
                }
            }
        }

        public IsEven(day: Date) {
            for (var i = 1; i < this.courseInformation.Lessons.length; i += 2) {
                if (i == this.courseInformation.Lessons.length - 1) {
                    if (day >= this.courseInformation.Lessons[i].BeginDate &&
                        day <= this.courseInformation.EndDate &&
                        !this.OnSelected(day) &&
                        !this.IsHoliday(day)
                    ) return true;
                }
                else {
                    if (day >= this.courseInformation.Lessons[i].BeginDate &&
                        day < this.courseInformation.Lessons[i + 1].BeginDate &&
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
                this.lastSelected = new Date(this.selected[this.selected.length-1].toDateString());
            }
        }

        public OnSelected(day: Date) {
            for (var i = 0; i < this.selected.length; i++){
                if (this.selected[i].toDateString() == day.toDateString()) return true;
            }
            return false;
        }

    }
    angular
        .module('app.calendar')
        .controller('app.calendar.CarlendarController', CarlendarController);
}