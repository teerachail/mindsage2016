module app.shared {
    'use strict';

    export class ClientUserProfileService {
        private clientUserProfile: ClientUserProfile;

        constructor() {
            // HACK: User profile information
            this.clientUserProfile = new ClientUserProfile();
            this.clientUserProfile.UserProfileId = 'sakul@mindsage.com';
        }

        public GetClientUserProfile(): ClientUserProfile {
            return this.clientUserProfile;
        }
    }

    angular
        .module('app.shared')
        .service('app.shared.ClientUserProfileService', ClientUserProfileService);
}