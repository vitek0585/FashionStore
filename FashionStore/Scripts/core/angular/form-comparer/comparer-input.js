angular.module("confirmApp", []).
    directive("compareTo", function () {
    return {
        require: "ngModel",
        link: function (scope, element, attr, ngModel) {

            ngModel.$validators.compareTo = function (modelValue) {
                return modelValue == scope.otherValue;
            };

            scope.$watch("otherValue", function () {
                ngModel.$validate();
            });
        },
        scope: {
            otherValue: "=source"
        }
    }

})