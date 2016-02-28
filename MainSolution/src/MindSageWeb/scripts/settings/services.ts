module app.settings {
    'use strict';
    
    interface IUpdateProfileContentResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        UpdateProfile(data: T): T;
    }
    export class ProfileService {

        private svc: IUpdateProfileContentResourceClass<any>;

        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
            this.svc = <IUpdateProfileContentResourceClass<any>>$resource(appConfig.UpdateProfileUrl, {
                'id': '@id', 'Name': '@Name', 'SchoolName': '@SchoolName', 'IsPrivate': '@IsPrivate', 'IsReminderOnceTime': '@IsReminderOnceTime'
            }, { UpdateProfile: { method: 'PUT' } });
        }

        public UpdateProfile(name: string, schoolName: string, isPrivate: boolean, isReminderOnceTime: boolean): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.svc.UpdateProfile(new UpdateProfileRequest(userId, name, schoolName, isPrivate, isReminderOnceTime)).$promise;
        }
    }

    angular
        .module('app.settings')
        .service('app.settings.ProfileService', ProfileService);
}