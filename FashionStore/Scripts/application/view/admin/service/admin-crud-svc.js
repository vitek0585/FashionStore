(function () {
    'use strict';
    angular.module("adminHttpApp", [])
        .provider("adminHttpSvc", adminHttpSvc);

    function adminHttpSvc() {
        var NO_CONTENT = 204;
        var FORBIDDEN = 403;
        var SERVER_ERROR = 500;
        var NOT_FOUND = 404;
        var urlGetGoodsForTable = '';
        var urlGetFullInfo = '';
        var urlGetLogsForTable = '';
        var urlDeleteLogs = '';
        var urlUploadFile = '';
        var urlUpdateGoods = '';
        var urlDeleteGoods = '';
        var urlGetUsers = '';
        var urlUpdateUser = '';
        return {
            initUrl: initUrl,
            $get: ['$q', '$http', 'toaster', get]
        }

        function get($q, $http, toaster) {

            return {
                getGoodsByPage: getGoodsByPage,
                fullInfo: fullInfo,
                getLogsByPage: getLogsByPage,
                deleteLog: deleteLog,
                uploadFile: uploadFile,
                updateGoods: updateGoods,
                deleteGoods: deleteGoods,
                getUsersByPage: getUsersByPage,
                updateUser: updateUser
            }
            //GET
            function getGoodsByPage(paramsUri) {
                return getRequest(paramsUri, urlGetGoodsForTable);
            }
            function getUsersByPage(paramsUri) {
                return getRequest(paramsUri, urlGetUsers);
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

            function deleteGoods(paramsUri) {
                return deleteRequest(paramsUri, urlDeleteGoods);
            }
            //POST
            //load Files принимает файл (один) номер id продукта к которому добавить фото
            function uploadFile(item, param) {
                var form = new FormData();
                form.append("file", item);
                var config = {
                    params: param,
                    url: urlUploadFile,
                    method: "POST",
                    data: form,
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                };
                return customRequest(config);

            }
            //PUT
            function updateGoods(data) {
                var config = {
                    url: urlUpdateGoods,
                    method: "PUT",
                    data: data,
                    headers: { 'Content-Type': 'application/json' }
                };
                return customRequest(config);
            }

            function updateUser(data) {
                var config = {
                    url: urlUpdateUser,
                    method: "PUT",
                    data: convertObjToXWwwFormData(data),
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                };
                return customRequest(config);
            }
            //implement METHODS
            function getRequest(paramsUri, url) {
                var dfd = $q.defer();
                var config = {
                    params: paramsUri
                };
                $http.get(url, config).then(success(dfd), error(dfd));
                return dfd.promise;
            }
            function deleteRequest(paramsUri, url) {
                var dfd = $q.defer();
                var config = {
                    params: paramsUri,
                    transformResponse: transform,
                    transformRequest: transformReq
            };
                $http.delete(url, config).then(successAndMsg(dfd), error(dfd));
                return dfd.promise;

                function transform(data, headersGetter, status) {
                    if (status == 302) {
                        
                    }
                }

                function transformReq(data,headers) {
                    
                }

            }
            function customRequest(config) {
                var dfd = $q.defer();

                var promise = $http(config);
                promise.then(success(dfd), error(dfd));
                promise.success(function ()
                { toaster.pop('success', '', 'Success!', 3000); });

                return dfd.promise;

            }
            //handler success and errors
            function success(dfd) {

                return function (d) {

                    dfd.resolve(d.data);
                }
            }
            function successAndMsg(dfd) {

                return function (d) {
                    if (d.status == NO_CONTENT)
                        toaster.pop('success', '', 'Success!', 3000);
                    dfd.resolve(d.data);
                }
            }
            function error(dfd) {

                return function (d) {
                    dfd.reject();
                    if (d.status == FORBIDDEN) {
                        toaster.pop('error', '', 'Access forbidden!', 3000);
                        return;
                    }
                    if (d.status == NOT_FOUND) {
                        toaster.pop('error', '', 'Access forbidden!', 3000);
                        return;
                    }
                    if (d.status == SERVER_ERROR) {
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
        }


        //-------------------functions for provider
        function initUrl(pathGetGoodsForTable, pathGetFullInfo, pathGetLogsForTable,
            pathDeleteLogs, pathUploadFile, pathUpdateGoods, pathDeleteGoods, pathGetUsers, pathUpdateUser) {

            if (isDefinedPath(pathGetGoodsForTable))
                urlGetGoodsForTable = pathGetGoodsForTable;

            if (isDefinedPath(pathGetFullInfo))
                urlGetFullInfo = pathGetFullInfo;

            if (isDefinedPath(pathGetLogsForTable))
                urlGetLogsForTable = pathGetLogsForTable;

            if (isDefinedPath(pathDeleteLogs))
                urlDeleteLogs = pathDeleteLogs;

            if (isDefinedPath(pathUploadFile))
                urlUploadFile = pathUploadFile;

            if (isDefinedPath(pathUpdateGoods))
                urlUpdateGoods = pathUpdateGoods;

            if (isDefinedPath(pathDeleteGoods))
                urlDeleteGoods = pathDeleteGoods;

            if (isDefinedPath(pathGetUsers))
                urlGetUsers = pathGetUsers;

            if (isDefinedPath(pathUpdateUser))
                urlUpdateUser = pathUpdateUser;
        };

        function isDefinedPath(path) {
            return path != null || path !== undefined;
        }

        function convertObjToXWwwFormData(obj) {
            var form = '';
            for (var k in obj) {

                if (Array.isArray(obj[k])) {
                    form += k + "=";
                    for (var i = 0; i < obj[k].length; i++) {
                        form += obj[k][i] + ',';

                        if (i === obj[k].length - 1)
                            form = form.slice(0, -1);
                    }

                } else
                    form += k + "=" + obj[k];

                form += '&';
            }
            return form.slice(0, -1);
        }
    }
})();