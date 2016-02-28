module app.shared {
    'use strict';

    export class ClientUserProfile {
        public UserProfileId: string;
        public FullName: string;
        public ImageUrl: string;
        public SchoolName: string;
        public IsPrivateAccout: boolean;
        public IsReminderOnceTime: boolean;
        public CurrentClassRoomId: string;
        public CurrentLessonId: string;
        public IsTeacher: boolean;
        public ClassName: string;
        public CurrentStudentCode: string;
        public NumberOfStudents: number;
        public StartDate: any;
    }

    export class Advertisment {
        public ImageUrl: string;
        public LinkUrl: string;
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

    export class LikeDiscussionRequest {
        constructor(
            public ClassRoomId: string,
            public LessonId: string,
            public CommentId: string,
            public DiscussionId: string,
            public UserProfileId: string) {
        }
    }

    export class UpdateCommentRequest {
        constructor(
            public id: string,
            public ClassRoomId: string,
            public LessonId: string,
            public UserProfileId: string,
            public IsDelete: boolean,
            public Description: string) {
        }
    }

    export class UpdateDiscussionRequest {
        constructor(
            public id: string,
            public ClassRoomId: string,
            public LessonId: string,
            public CommentId: string,
            public UserProfileId: string,
            public IsDelete: boolean,
            public Description: string) {
        }
    }
    export class GetUserProfileRequest {
        constructor(
            public id: string) {
        }
    }
    export class GetCourseRequest {
        constructor(
            public id: string,
            public classRoomId: string) {
        }
    }
    
    


}