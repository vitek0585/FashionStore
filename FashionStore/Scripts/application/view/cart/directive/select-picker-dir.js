angular.module("selectBotstrApp", [])
.directive('selectByRadio', ['$timeout', '$compile', selectDirective]);

function selectDirective(timeout, compile) {
    var template = "<select ng-model='current.sizeId' class='selectpicker'" +
        "ng-options='type.sizeId as type.sizeName for type in details|filter:{colorId : current.colorId}'>" +
        "</select>";
    var template2 = "<select class='selectpicker' >" +
       "<option " +
       "ng-repeat='type in item.types|filter:{colorId : current.colorId}' value='{{type.sizeId}}'>{{type.sizeName}}</option>" +
       "</select>";
    return {
        restrict: 'A,E',

        link: function (scope, element, attrs) {
            var elem = angular.element(template);
            $(element).html(compile(elem)(scope));
            $(element).css('opacity', 0);
            timeout(function () {
                $(elem).selectpicker({

                });
                $(element).animate({ opacity: '1' }, 200);
            });
            scope.$watch('current.colorId', function refresh() {
                timeout(function () {
                    $(elem).selectpicker('refresh');
                });
            });
           
        },
        scope: {
            details: "=",
            current: "="
        }
    }

};
