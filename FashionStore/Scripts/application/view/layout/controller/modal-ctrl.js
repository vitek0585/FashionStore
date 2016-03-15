var global = angular.module("globalApp");


global.controller("modalCtrl", ["$scope", '$fancyModal', function (scope, modal) {
    scope.openModal = function (templateId, ctrl) {
        modal.open({
            template: angular.element(document.querySelector('#' + templateId)).html(),
            //openingClass: 'animated zoomIn',
            //closingClass: 'animated hinge',
            //openingOverlayClass: 'animated fadeIn',
            //closingOverlayClass: 'animated fadeOut',
            //plain: true,
            //closeByEscape: false,
            controller: ctrl
        });
    }
    scope.isOpen = function(isOpen) {
        
        return isOpen == 'true';
    };
}]);

global.controller("loginCtrl", ["$scope", "httpService", "formToObject", "$compile", "toaster", "$timeout", function (scope, http, ser, compile, toaster, t) {
    var tmpl = "<div >" +
         "<div class='label-error' ng-repeat='mess in responseHandler.errorMessages track by $index'>{{::mess}}</div>" +
         "</div>";
    var blursArray = [];
    scope.blurs = function (e) {
        scope[e] = false;
        blursArray.push(e);
    }
    scope.responseHandler = {
        errorMessages: [],
        isBusy: false,
        returnUrl: angular.isDefined(document.location.href) ? document.location.href : ''
    };
    scope.submit = function () {
        scope.responseHandler.isBusy = true;
        var form = ser.serializeObject(loginForm);
        console.log(loginForm);
        form.returnUrl = scope.responseHandler.returnUrl;
        var url = "/Account/Login";

        http.formRequest(url, form).then(function (d) {
            console.log(d.data);
            document.location = d.data;
        }, function (d) {
            if (d.status == 500) {
                toaster.pop('error', '', "<span class='label-error'>error!</span>", 5000, 'trustedHtml');
                return;
            }

            scope.responseHandler.errorMessages = d.data;
            var element = angular.element(tmpl);
            var fn = compile(element);
            fn(scope);
            t(function () {
                toaster.pop('error', '', element[0].innerHTML, 5000, 'trustedHtml');
            }, 0, true);

            angular.forEach(blursArray, function (value) {
                scope[value] = true;
            });
        }).finally(function () {
            scope.responseHandler.isBusy = false;
        });
    }

}]);

global.controller("regCtrl", ["$scope", "httpService", "formToObject", "$compile", "toaster", "$timeout", function (scope, http, ser, compile, toaster, t) {
    var tmpl = "<div >" +
        "<div class='label-error' ng-repeat='mess in responseHandler.errorMessages'>{{::mess}}</div>" +
        "</div>";
    var blursArray = [];
    scope.blurs = function (e) {
        scope[e] = false;
        blursArray.push(e);
    }

    scope.responseHandler = {
        errorMessages: [],
        succesMessage: undefined,
        isBusy: false,
        isSuccededRegister: false
    }

    scope.submit = function () {
        scope.responseHandler.isBusy = true;
        var form = ser.serializeObject(registerForm);
        var url = "/Account/Register";

        http.formRequest(url, form).then(function (d) {
            scope.responseHandler.isSuccededRegister = true;
            scope.responseHandler.succesMessage = d.data;
        }, function (d) {
            if (d.status == 500) {
                toaster.pop('error', '', "<span class='label-error'>error!</span>", 5000, 'trustedHtml');
                return;
            }

            scope.responseHandler.errorMessages = d.data;
            var element = angular.element(tmpl);
            var fn = compile(element);
            fn(scope);
            t(function () {
                toaster.pop('error', '', element[0].innerHTML, 5000, 'trustedHtml');
            }, 0, true);

            angular.forEach(blursArray, function (value) {
                scope[value] = true;
            });
        }).finally(function () {
            scope.responseHandler.isBusy = false;
        });

    }
}]);