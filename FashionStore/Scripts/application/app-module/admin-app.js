
(function () {
    'use strict';
    var nameHeaderCsrf = 'RequestVerificationToken';
    $(document).scroll(function (e) {
        $('nav').css({
            'left': -$(document).scrollLeft()
        });
    });
    var app = angular.module("adminApp", ['ui.router', 'azSuggestBox', 'adminHttpApp', "pagingApp", "spinnerGlobalApp",
        'ui.bootstrap',
        "lazyLoadApp", "vesparny.fancyModal", 'ngAnimate', 'ngTooltips',
        'toaster', "confirmApp", "spinnerApp", "serializeApp", "slick"
    ]);

    app.injectRequires = function (arr) {
        Array.prototype.push.apply(this.requires, arr);
    }
    app.config(configProvider);
    configProvider.$inject = ['adminHttpSvcProvider','$httpProvider'];

    function configProvider(adminProvider, $httpProvider) {
        adminProvider.initUrl(
            '/api/Admin/GoodsByPage',
            '/api/Admin/GoodsFullInfo',
            '/api/Admin/Log',
            '/api/Admin/LogDelete',
            '/api/Photo/AddPhoto',
            '/api/Good/Update',
            '/api/Good/Delete',
            '/api/Admin/UserByPage',
            '/api/Admin/UserUpdateRole');

        $httpProvider.defaults.headers.common[nameHeaderCsrf] = document.getElementById("csrf-token").value;
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
    app.value("csrfV", {
        name: nameHeaderCsrf,
        value: ''
    });
})()

