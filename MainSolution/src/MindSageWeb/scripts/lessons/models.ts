module app.lessons {
    'use strict';
    
    export class LessonContentRequest {
        constructor(
            public id: string,
            public classRoomId: string, 
            public userId: string){
        }   
    }

    export class LessonDiscussionRequest {
        constructor(
            public id: string,
            public classRoomId: string,
            public commentId: string,
            public userId: string) {
        }
    }

    export class LikeLessonRequest {
        constructor(
            public ClassRoomId: string,
            public LessonId: string,
            public UserProfileId: string) {
        }

    }
    export class ReadNoteRequest {
        constructor(
            public ClassRoomId: string,
            public UserProfileId: string) {
        }

    }

}