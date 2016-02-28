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
}