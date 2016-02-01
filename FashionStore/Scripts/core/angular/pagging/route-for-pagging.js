var httpRoute = angular.module("httpRouteApp", []);
httpRoute.provider("routeService", function () {
    return {
        startRoute: function (start) {

            if (angular.isDefined(start))
                window.onpopstate = function (e) {
                    try {
                        console.log(e.state.previous_url);
                        if (e.state)
                            window.location = e.state.previous_url;

                    } catch (e) {
                        console.log("error onpopstate");
                    }
                }
        },
        $get: function () {
            return {
                route: function (url, isReplace) {
                   
                        if (isReplace) {
                            history.replaceState({ 'previous_url': url }, '', url);
                        } else {
                            history.pushState({ 'previous_url': url }, '', url);
                        }
  
                }
            }
        }
    };

}
);