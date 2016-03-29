module app.studentlists {
    'use strict';

    export class SearchFriendRequest {
        constructor(
            public userId: string,
            public classRoomId: string,
            public key: string) {
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