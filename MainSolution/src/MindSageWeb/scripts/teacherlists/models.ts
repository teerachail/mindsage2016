module app.teacherlists {
    'use strict';

    export class GetStudentListRequest {
        constructor(
            public userId: string,
            public classRoomId: string) {
        }
    }

    export class RemoveStudentRequest {
        constructor(
            public classRoomId: string,
            public UserProfileId: string,
            public RemoveUserProfileId: string) {
        }
    }

}