module app.shared {
    'use strict';

    export class ClientUserProfileService {
        private clientUserProfile: ClientUserProfile;

        constructor() {
            // HACK: User profile information
            this.clientUserProfile = new ClientUserProfile();
            this.clientUserProfile.UserProfileId = 'sakul@mindsage.com';
            this.clientUserProfile.ImageUrl = 'http://placehold.it/100x100';
            this.clientUserProfile.FullName = 'Sakul Jaruthanaset';
            this.clientUserProfile.CurrentClassRoomId = "ClassRoom01";
            this.clientUserProfile.CurrentLessonId = "Lesson01";
        }

        public GetClientUserProfile(): ClientUserProfile {
            return this.clientUserProfile;
        }
    }

    angular
        .module('app.shared')
        .service('app.shared.ClientUserProfileService', ClientUserProfileService);
}