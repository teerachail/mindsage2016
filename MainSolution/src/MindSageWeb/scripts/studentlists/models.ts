module app.studentlists {
    'use strict';

    export class GetFriendListRequest {
        constructor(
            public userId: string,
            public classRoomId: string) {
        }
    }

    export class SendFriendRequest {
        constructor(
            public FromUserProfileId: string,
            public ToUserProfileId: string,
            public RequestId: string,
            public IsAccept: boolean) {
        }
    }

}