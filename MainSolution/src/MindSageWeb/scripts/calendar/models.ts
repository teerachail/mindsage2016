module app.calendar {
    'use strict';
    export class CourseScheduleRequest {
        constructor(
            public id: string,
            public classRoomId: string) {
        }
    }
    export class SetCourseStartDateRequest {
        constructor(
            public id: string,
            public classRoomId: string,
            public beginDate: Date)
         {
        }
    }
    export class SetCourseScheduleRangeRequest {
        constructor(
            public id: string,
            public classRoomId: string,
            public isHoliday: boolean,
            public isShift: boolean,
            public fromDate : Date,
            public toDate: Date) {
        }
    }
    export class SetCourseScheduleWeekRequest {
        constructor(
            public id: string,
            public classRoomId: string,
            public isHoliday: boolean,
            public isShift: boolean,
            public isSunday: boolean,
            public isMonday: boolean,
            public isTuesday: boolean,
            public isWednesday: boolean,
            public isTursday: boolean,
            public isFriday: boolean,
            public isSaturday: boolean
        ) {
        }
    }

    
}