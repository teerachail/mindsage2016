module app.coursemaps {
    'use strict';

    export class GetCourseMapContentRequest {
        constructor(public id: string, public classRoomId: string) {
        }
    }
}