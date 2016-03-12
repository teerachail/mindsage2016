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
            public userProfileId: string,
            public classRoomId: string,
            public beginDate: Date)
         {
        }
    }
    export class SetCourseScheduleRangeRequest {
        constructor(
            public userProfileId: string,
            public classRoomId: string,
            public isHoliday: boolean,
            public isShift: boolean,
            public fromDate : Date,
            public toDate: Date) {
        }
    }
    export class SetCourseScheduleWeekRequest {
        constructor(
            public userProfileId: string,
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
    export class ApplyToAllCourseRequest {
        constructor(
            public userProfileId: string,
            public classRoomId: string) {
        }
    }

    export class Calendar {
        constructor(
            public IsComplete: boolean ,
            public BeginDate: Date,
            public EndDate: Date,
            public Lessons: LessonSchedule[],
            public Holidays = []) {
        }
    }

    export class LessonSchedule {
        constructor(
            public Name: string,
            public BeginDate: Date) {
        }
    }
}