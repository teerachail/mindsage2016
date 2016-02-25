module app.journals {
    'use strict';
    
    export class GetJournalCommentsRequest {
        constructor(public targetUserId: string, public requestByUserId: string, public classRoomId: string) { }
    }
}