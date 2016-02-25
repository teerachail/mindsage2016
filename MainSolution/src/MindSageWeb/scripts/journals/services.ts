module app.journals {
    'use strict';
    
    interface IJournalResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetComments(data: T): T;
        GetDiscussion(data: T): T;
    }

    export class JournalService {

        private svc: IJournalResourceClass<any>;
        static $inject = ['appConfig', '$resource', 'app.shared.ClientUserProfileService'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService, private userprofileSvc: app.shared.ClientUserProfileService) {
        this.svc = <IJournalResourceClass<any>>$resource(appConfig.JournalCommentUrl, {
            'targetUserId': '@targetUserId', 'requestByUserId': '@requestByUserId', 'classRoomId': '@classRoomId' }, {
                GetComments: { method: 'GET' },
            });
        }

        public GetComments(classRoomId: string, targetUserId: string): ng.IPromise<any> {
            var userId = this.userprofileSvc.GetClientUserProfile().UserProfileId;
            return this.svc.GetComments(new GetJournalCommentsRequest(targetUserId, userId, classRoomId)).$promise;
        }
    }

    angular
        .module('app.journals')
        .service('app.journals.JournalService', JournalService);
}