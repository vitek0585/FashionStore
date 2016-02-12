(function () {
    'use strict';

    var app = angular.module("adminApp", ['ui.router', 'azSuggestBox', 'adminHttpApp', "pagingApp", "spinnerGlobalApp",
        'ui.bootstrap',
        "lazyLoadApp", "vesparny.fancyModal", 'ngAnimate', 'ngTooltips',
        'toaster', "confirmApp", "spinnerApp", "serializeApp", "slick"
    ]);

    app.injectRequires = function (arr) {
        Array.prototype.push.apply(this.requires, arr);
    }
    app.config(configProvider);
    configProvider.$inject = ['adminHttpSvcProvider'];

    function configProvider(adminProvider) {
        adminProvider.initUrl('/api/Admin/Goods/ByPage', '/api/Admin/Goods/FullInfo', '/api/Admin/Goods/Log',
            '/api/Admin/Goods/LogDelete');
    }

    app.run(["$rootScope","spinnerGlobalSvc",function ($rootScope,spinner) {

        $rootScope
            .$on('$stateChangeStart',
                function (event, toState, toParams, fromState, fromParams) {
                    spinner.begin();
                });

        $rootScope
            .$on('$stateChangeSuccess',
                function (event, toState, toParams, fromState, fromParams) {
                    spinner.comleted();
                });


    }]);
})()