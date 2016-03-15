(function() {
    'use strict';
    angular.module("spinnerGlobalApp", [])
        .factory("spinnerGlobalSvc", spinnerGlobalSvc);

    function spinnerGlobalSvc() {
        return {
        
            begin: begin,
            comleted: comleted
        };

        function begin() {

            $("#loader-content").clearQueue().fadeIn(100);
        }

        function comleted() {
            $("#loader-content").fadeOut("slow");
        }
    }
    
})();
