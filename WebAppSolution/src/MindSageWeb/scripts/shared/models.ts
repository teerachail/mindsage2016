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
        public CurrentClassCalendarId: string;
        public CurrentLessonNo: number;

        public CurrentDisplayLessonId: string;
        public IsTeacher: boolean;
        public ClassName: string;
        public CurrentStudentCode: string;
        public NumberOfStudents: number;
        public StartDate: any;
        public IsSelfPurchase: boolean;
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

    export class Comment {
        constructor(
            public id: string,
            public Description: string,
            public TotalLikes: number,
            public TotalDiscussions: number,
            public CreatorImageUrl: string,
            public CreatorDisplayName: string,
            public ClassRoomId: string,
            public LessonId: string,
            public CreatedByUserProfileId: string,
            public Order: number) {
        }
    }

    export class Discussion {
        constructor(
            public id: string,
            public CommentId: string,
            public Description: string,
            public TotalLikes: number,
            public CreatorImageUrl: string,
            public CreatorDisplayName: string,
            public CreatedByUserProfileId: string,
            public Order: number) {
        }
    }
    export class GetAllCourseRequest {
        constructor(
            public id: string) {
        }
    }

    export class GetNotificationNumberRequest {
        constructor(
            public id: string,
            public classRoomId: string) {
        }
    }
    export class CourseCatalog {
        public id: string;
        public ImageUrl: string;
        public Name: string;
        public ClassRoomId: string;
        public LessonId: string;
        public GroupName: string;
        public Description: string;
    }
    export class GetNotificationContentRequest {
        constructor(
            public id: string,
            public classRoomId: string) {
        }
    }

    export class GetLikeRequest {
        constructor(
            public id: string,
            public classRoomId: string,
            public lessonId: string) {
        }
    }

    export class GetAllLikeRequest {
        constructor(
            public id: string,
            public classRoomId: string) {
        }
    }

    export class GetFriendListRequest {
        constructor(
            public userId: string,
            public classRoomId: string) {
        }
    }

    export class ChangeCourseRequest {
        constructor(
            public UserProfileId: string,
            public ClassRoomId: string) {
        }
    }

    export class SendContactRequest {
        constructor(
            public Name: string,
            public Email: string,
            public Message: string) {
        }
    }
}