module app.calendar {
    'use strict';
    export class CourseScheduleRequest {
        constructor(
            public id: string,
            public classRoomId: string) {
        }
    }
}