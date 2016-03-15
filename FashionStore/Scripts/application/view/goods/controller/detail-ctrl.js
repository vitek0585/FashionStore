(function () {
    'use strict';

    var m = angular.module("globalApp");
    var modules = ['imageZoomApp', 'angular.filter', 'selectBotstrApp'];
    m.injectRequires(modules);

    m.controller("randomCtrl", ["$scope", "httpService", function (scope, http) {
        scope.randomItem = {};
        scope.isWaiter = true;
        http.getRequest({ count: 10 }, "/api/Good/RandomGood").then(function (d) {
            scope.randomItem = d.data;
            scope.isWaiter = false;
        });
    }]);

    m.controller("ctrlDetail", [
        "$scope", "cartSvc", "httpService", "$timeout",
        function (scope, cart, http, timeout) {

            scope.item = {};
            scope.current = {};

            scope.initCurrent = function (item) {
                angular.copy(item, scope.current);
             };

            scope.initialize = function (item) {
                scope.item = item;

                if (angular.isDefined(scope.item.types) && scope.item.types.length > 0) {
                    angular.copy(scope.item.types[0], scope.current);
                }

            };
            scope.add = function (count) {
                var item = scope.current;
                var tmp = {};
                angular.copy(item, tmp);
                tmp.goodId = scope.item.goodId;
                tmp.countGood = count;
                cart.add(tmp);
            }

        }]);
})();