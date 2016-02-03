var http = angular.module("httpApp", []);
http.service("httpService", ["$http", function (http) {

    this.xmlhttp = function (type, url, data, success, error) {
        var xhr = new XMLHttpRequest;
        xhr.open(type, url, true);
        xhr.send(data);
    };
    this.requestWithConfig = function (item, url, config) {

        var promise = http.post(url, item, config);
        return promise;
    }
    //post запрос
    this.postRequest = function (paramUri,body,url,headers) {
        var config = {
            headers: headers,
            params: paramUri
        };
        var promise = http.post(url, body, config);
        return promise;
    }
    //запрос для данных которые находятся в теле запроса (принимает обычный объект)
    this.requestFormSimpleData = function (url, item, headers) {
        var form = '';
        for (var k in item) {
            if (Array.isArray(item[k])) {
                for (var i = 0; i < item[k].length; i++) {
                    form += '&' + k + "=" + item[k][i];
                }
            } else
                form += k + "=" + item[k] + '&';
        }
        var config = {
            url: url,
            method: "POST",
            headers: headers || { 'Content-Type': 'application/x-www-form-urlencoded' },
            data: form.substring(0, form.length-1)
        };
        var promise = http(config);
        return promise;
    }
    //для web api, запрос для одного элемента данных которые находятся в теле запроса (принимает число или строку)
    this.requestOneDataApi = function (url, item, headers) {
        var form = '=' + item;
        
        var config = {
            url: url,
            method: "POST",
            headers: headers || { 'Content-Type': 'application/x-www-form-urlencoded' },
            data: form
        };
        var promise = http(config);
        return promise;
    }
    //для запроса данные передаются с строке адреса (принимает число или строку)
    this.getRequest = function (param, url, headers) {
        var config = {
            headers: headers,
            params: param
        };
        var promise = http.get(url, config);
        return promise;
    }
    //для запроса данные передаются с строке адреса (принимает число или строку)кешируется
    this.getByCache = function (param, url, headers) {
        var config = {
            headers: headers,
            params: param,
            cache:true
        };
        var promise = http.get(url, config);
        return promise;
    }
    //load Files принимает файл (один) номер id продукта к которому добавить фото
    this.fileRequest = function (url,item,param,headers) {
        var form = new FormData();
        form.append("file", item);
        var config = {
            params: param,
            url: url,
            method: "POST",
            data: form,
            transformRequest: angular.identity,
            headers: headers|| { 'Content-Type': undefined }
        };
        var promise = http(config);
        return promise;
    }
    //Form request
    this.formRequest = function (url, item, headers) {
        var form = new FormData();
        for (var k in item) {
            if (Array.isArray(item[k])) {
                for (var i = 0; i < item[k].length; i++) {
                    form.append(k + i, item[k][i]);
                }

            } else
                form.append(k, item[k]);
        }

        var config = {
            url: url,
            method: "POST",
            data: form,
            transformRequest: angular.identity,
            headers: headers || { 'Content-type': undefined }
        };
        var promise = http(config);
        return promise;
    }
}]);