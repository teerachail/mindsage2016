module app.teacherlists {
    'use strict';

    export class GetStudentListRequest {
        constructor(
            public userId: string,
            public classRoomId: string) {
        }
    }

}