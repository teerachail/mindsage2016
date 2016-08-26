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

    export class LessonAnswerRequest {
        constructor(
            public id: string,
            public classRoomId: string,
            public userId: string) {
        }

    }

    export class AnswerModel {
        constructor(
            public AssessmentId: string,
            public Answer: string) {
        }

    }

    export class LessonAnswers {
        constructor(
            public id: string,
            public UserProfileId: string,
            public ClassRoomId: string,
            public LessonId: string,
            public CreatedDate: any,
            public DeletedDate: any,
            public Answer: AnswerModel[]) {
        }

    }

    export class LessonAnswersRequest {
        constructor(
            public UserProfileId: string,
            public ClassRoomId: string,
            public LessonId: string,
            public AssessmentId: string,
            public Answer: string) {
        }

    }

}