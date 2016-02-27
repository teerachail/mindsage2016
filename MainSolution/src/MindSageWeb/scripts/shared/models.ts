module app.shared {
    'use strict';

    export class ClientUserProfile {
        public UserProfileId: string;
        public FullName: string;
        public ImageUrl: string;
        public CurrentClassRoomId: string;
        public CurrentLessonId: string;
        public IsTeacher: boolean;
    }

    export class GetCommentsRequest {
        constructor(
            public id: string,
            public classRoomId: string,
            public userId: string) {
        }
    }

    export class GetDiscussionRequest {
        constructor(
            public id: string,
            public classRoomId: string,
            public commentId: string,
            public userId: string) {
        }
    }

    export class CreateCommentRequest {
        constructor(
            public ClassRoomId: string,
            public LessonId: string,
            public UserProfileId: string,
            public Description: string) {
        }
    }

    export class CreateDiscussionRequest {
        constructor(
            public ClassRoomId: string,
            public LessonId: string,
            public CommentId: string,
            public UserProfileId: string,
            public Description: string) {
        }
    }

    export class LikeCommentRequest {
        constructor(
            public ClassRoomId: string,
            public LessonId: string,
            public CommentId: string,
            public UserProfileId: string) {
        }
    }
}