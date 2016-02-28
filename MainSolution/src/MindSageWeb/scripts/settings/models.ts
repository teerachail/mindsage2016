module app.settings {
    'use strict';

    export class UpdateProfileRequest {
        constructor(
            public id: string,
            public Name: string,
            public SchoolName: string,
            public IsPrivate: boolean,
            public IsReminderOnceTime: boolean) { }
    }

    export class UpdateCourseRequest {
        constructor(
            public id: string,
            public classRoomId: string,
            public ClassName: string,
            public ChangedStudentCode: string) { }
    }

    export class DeleteCourseRequest {
        constructor(
            public ClassRoomId: string,
            public UserProfileId: string) { }
    }
}