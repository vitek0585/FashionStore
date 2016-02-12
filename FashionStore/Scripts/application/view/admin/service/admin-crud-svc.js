(function () {
    'use strict';
    angular.module("adminHttpApp", [])
        .provider("adminHttpSvc", adminHttpSvc);

    function adminHttpSvc() {
        var NO_CONTENT = 204;
        var urlGetGoodsForTable = '';
        var urlGetFullInfo = '';
        var urlGetLogsForTable = '';
        var urlDeleteLogs = '';
        return {
            initUrl: initUrl,
            $get: ['$q', '$http', 'toaster', get]
        }

        function get($q, $http, toaster) {
            var dfd;
            return {
                getGoodsByPage: getGoodsByPage,
                fullInfo: fullInfo,
                getLogsByPage: getLogsByPage,
                deleteLog: deleteLog
            }
            //GET
            function getGoodsByPage(paramsUri) {
                return getRequest(paramsUri, urlGetGoodsForTable);
            }

            function fullInfo(paramsUri) {
                return getRequest(paramsUri, urlGetFullInfo);
            }

            function getLogsByPage(paramsUri) {
                return getRequest(paramsUri, urlGetLogsForTable);
            }
            //DELETE
            function deleteLog(paramsUri) {
                return deleteRequest(paramsUri, urlDeleteLogs);
            }
            //implement METHODS
            function getRequest(paramsUri, url) {
                dfd = $q.defer();
                var config = {
                    params: paramsUri
                };
                $http.get(url, config).then(success, error);
                return dfd.promise;
            }
            function deleteRequest(paramsUri, url) {
                dfd = $q.defer();
                var config = {
                    params: paramsUri
                };
                $http.delete(url, config).then(successAndMsg, error);
                return dfd.promise;

            }
            //handler success and errors
            function success(d) {
                dfd.resolve(d.data);
            }

            function successAndMsg(d) {
                if (d.status == NO_CONTENT)
                    toaster.pop('success', '', 'Success!', 3000);
                dfd.resolve(d.data);
            }
            function error(d) {
                dfd.reject();
                if (d.status == 500) {
                    toaster.pop('error', '', 'Occured error!', 3000);
                    return;
                }
                if (angular.isArray(d.data)) {
                    var e = angular.element('<div>').append(d.data.map(function (e) {
                        return angular.element("<div>").addClass("label-error").text(e)[0];
                    }));
                    toaster.pop('error', '', e.html(), 4000, "trustedHtml");
                    return;
                }
                if (d.data)
                    toaster.pop('error', '', d.data, 4000);
                else
                    toaster.pop('error', '', 'Error', 4000);
            }
        }


        //-------------------functions for provider
        function initUrl(pathGetGoodsForTable, pathGetFullInfo, pathGetLogsForTable, pathDeleteLogs) {

            if (isDefinedPath(pathGetGoodsForTable))
                urlGetGoodsForTable = pathGetGoodsForTable;

            if (isDefinedPath(pathGetFullInfo))
                urlGetFullInfo = pathGetFullInfo;

            if (isDefinedPath(pathGetLogsForTable))
                urlGetLogsForTable = pathGetLogsForTable;
            if (isDefinedPath(pathDeleteLogs))
                urlDeleteLogs = pathDeleteLogs;
        };

        function isDefinedPath(path) {
            return path != null || path !== undefined;
        }
    }
})();