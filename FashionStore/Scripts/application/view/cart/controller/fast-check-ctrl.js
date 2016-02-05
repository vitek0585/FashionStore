(function () {
    'use strict';
    var global = angular.module("globalApp");

    global.controller("checkoutCtrl", ["$scope", "cartSvc", "formToObject", "$fancyModal", function (scope, cart, ser, modal) {

        var blursArray = [];
        scope.blurs = function (e) {
            scope[e] = false;
            blursArray.push(e);
        }
        scope.responseHandler = {
            isBusy: false
        };
        scope.submit = function () {
            scope.responseHandler.isBusy = true;
            var form = ser.serializeObject(checkoutForm);
            cart.doOrder(form).then(function () {
                modal.close();
            }).finally(function () {
                angular.forEach(blursArray, function (value) {
                    scope[value] = true;
                });
                scope.responseHandler.isBusy = false;
            });

        }

    }]);
})();