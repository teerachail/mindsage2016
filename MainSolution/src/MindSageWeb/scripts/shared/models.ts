module app.shared {
    'use strict';

    export class ClientUserProfile {
        public UserProfileId: string;
        public FullName: string;
        public ImageUrl: string;
        public CurrentClassRoomId: string;
        public CurrentLessonId: string;
    }

    export class GetCommentsRequest {
        constructor(
            public id: string,
            public classRoomId: string,
            public userId: string) {
        }
    }
}