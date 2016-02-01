
angular.module('spinnerApp',[]).
    directive('spinWait', function() {

        return {
            link:function(scope,elem,attr) {
                

            },
            templateUrl: "/Scripts/Angular/Common/Spinner.html",
            rerestrict: 'E',
            scope: {
                visible:'=showWhen'
            }
        }
    });

//<spin-wait show-when="responseHandler.isBusy" />