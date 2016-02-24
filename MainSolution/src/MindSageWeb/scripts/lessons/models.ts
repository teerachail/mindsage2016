module app.lessons {
    'use strict';
    
    export class LessonContentRequest {
        
        constructor(
            public id: string,
            public classRoomId: string, 
            public userId: string){
        }
        
    }
    
    export class LessonContentRespond{
        
    }
}