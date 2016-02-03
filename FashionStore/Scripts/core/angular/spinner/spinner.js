
angular.module('spinnerApp',[]).
    directive('spinWait', function() {

        return {
            link:function(scope,elem,attr) {
                

            },
            templateUrl: "/Scripts/core/angular/spinner/Spinner.html",
            rerestrict: 'E',
            scope: {
                visible:'=showWhen'
            }
        }
    });

//<spin-wait show-when="responseHandler.isBusy" />