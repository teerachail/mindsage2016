module app.calendar {
    'use strict';
    
    class CarlendarController {

        public date = new Date();
        public year;
        public monthdays = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
        public today = new Date();
        public selected = new Date();
        public monthAllNames = new Array("Jan", "Feb", "Mar", "Apl", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
        //public today = new Date('8/18/2016');
        public month; // month
        public allweeks = []; // array for push week
        public monthNames;
        private holiday = [];
        static $inject = ['$scope', '$state'];
        constructor(private $scope, private $state) {
            this.month = this.today.getMonth();
            this.year = this.today.getFullYear();
            this.setCalendar(this.month, this.year);
            this.holiday.push(new Date('3/18/2016'));
            this.holiday.push(new Date('3/8/2016'));
            this.holiday.push(new Date('3/1/2016'));
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

        public Selected(sdate: Date) {
            this.selected = sdate;
        }

        public OnSelected(sdate: Date) {
            if (this.selected.toDateString() == sdate.toDateString()) return true;
        }

        public CheckToday(day: Date) {
            return this.today.toDateString() == day.toDateString();
        }

        public IsHoliday(sdate: Date) {
            for (var i = 0; i < this.holiday.length; i++){
                if (this.holiday[i].toDateString() == sdate.toDateString()) return true;
            }
            return false;
        }

    }
    angular
        .module('app.calendar')
        .controller('app.calendar.CarlendarController', CarlendarController);
}