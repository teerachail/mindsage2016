module app.studentlists {
    'use strict';

    export class GetFriendListRequest {
        constructor(
            public userId: string,
            public classRoomId: string) {
        }
    }

}