module app.sidemenus {
    'use strict';

    export class SideMenuService {

        public CurrentTabName: string;

        constructor() {
            this.CurrentTabName = "Home";
        }

    }

    angular
        .module('app.sidemenus')
        .service('app.sidemenus.SideMenuService', SideMenuService);
}