(function() {
    'use strict';
    var m = angular.module("globalApp");

    m.controller("ctrl", [
        "$scope", "httpService", function(scope, http) {
            scope.items = [];
            http.getRequest(null, "/api/Good/NewGoods").then(function(d) {
                scope.items = d.data;
            });

        }
    ]);
})();;
