(function () {
    'use strict';
    angular.module("adminApp")
        .config(configRoute);

    configRoute.$inject = ['$stateProvider'];

    function configRoute($stateProvider) {

        $stateProvider.state('goods-state', {
            url: '/Goods',
            controller: 'goodsCtrl',
            controllerAs: 'gVm',
            templateUrl: "/Admin/Goods"

        }).state('goods-state.edit', {
            url: '/Edit/:id',
            views: {
                "goods-edit": {
                    controller: 'editCtrl',
                    controllerAs: 'eVm',
                    templateUrl: "/Admin/GoodsEdit",
                    resolve: {
                        goodsInfo: function (adminHttpSvc, $stateParams) {
                            return adminHttpSvc.fullInfo({ id: $stateParams.id });
                        }
                    },
                }
            },
            onEnter: function ($stateParams) {
                
            },
            onExit: function () {
                
            }

        }).state('log-state', {
            url: '/Logs',
            controller: 'logCtrl',
            controllerAs: 'lVm',
            templateUrl: "/Admin/Log"

        });
    }
})();

