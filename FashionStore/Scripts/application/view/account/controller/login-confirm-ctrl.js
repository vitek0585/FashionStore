(function () {
    'use strict';
    var app = angular.module('globalApp');
    app.injectRequires();
    var confirmRegister = function (scope, http, ser, compile, toaster, t) {
        var tmpl = "<div >" +
            "<div class='label-error' ng-repeat='mess in responseHandler.errorMessages'>{{::mess}}</div>" +
            "</div>";

        scope.responseHandler = {
            errorMessages: [],
            succesMessage: undefined,
            isBusy: false,
            isSuccededRegister: false
        }

        var blursArray = [];

        scope.blurs = function (e) {
            scope[e] = false;
            blursArray.push(e);
        }
        scope.submit = function () {
            scope.responseHandler.isBusy = true;
            var form = ser.serializeObject(confirmForm);
            var url = "/Account/ExternalLoginConfirmation";

            http.formRequest(url, form).then(function (d) {
                window.location=d.data;
            }, function (d) {
                if (d.status == 500) {
                    toaster.pop('error', '', 'Error', 4000);
                    return;
                }
                scope.responseHandler.errorMessages = d.data;
                var element = angular.element(tmpl);
                var fn = compile(element);
                fn(scope);
                if (angular.isArray(d.data)) {
                    var e = angular.element('<div>').append(d.data.map(function (e) {
                        return angular.element("<div>").addClass("label-error").text(e)[0];
                    }));
                    toaster.pop('error', '', e.html(), 4000, "trustedHtml");

                } else {

                    toaster.pop('error', '', d.data, 4000);
                }
              

                angular.forEach(blursArray, function (value) {
                    scope[value] = true;
                });
            })
                .finally(function () {
                    scope.responseHandler.isBusy = false;
                });
        };
    }
    app.controller('extConfCtrl', ["$scope", "httpService", "formToObject", "$compile", "toaster", "$timeout", confirmRegister]);
})();