(function () {
    'use strict';
    angular.module("cartApp", [])
        .provider("cartSvc", cartExecute);

    function cartExecute() {
        var tmpl = "<div >" +
           "<div class='label-error' ng-repeat='mess in errors track by $index'>{{::mess}}</div>" +
           "</div>";
        var cart = [];
        var urlAdd = '';
        var urlCart = '';
        var urlUpdate = '';
        var urlGetTypes = '';
        var urlRemove = '';
        var urlDoOrder = '';
        var urlDoOrderReg = '';
        return {
            initUrl: initUrl,
            $get: ["$q", "toaster", "httpService", "culture", get]
        }
        //-------------------functions $get
        function get($q, toaster, http, culture) {

            return {
                cart: cart,
                getCart: getCart,
                add: add,
                update: update,
                getDetails: getDetails,
                remove: remove,
                doOrderR: doOrderR,
                doOrder: doOrder
            }
            //-------------------functions $get
            function getCart() {
                var dfd = $q.defer();

                if (urlCart == undefined)
                    end();
                else remoteCart();

                return dfd.promise;
                //func
                function remoteCart() {
                    try {

                        http.getRequest({}, urlCart).then(function (d) {
                            cart = d.data;
                        }).finally(end);

                    } catch (e) {
                        end();
                    }
                }
                function end() {
                    dfd.resolve(cart);
                };
            };
            function add(item) {
                item.isActive = false;
                http.postRequest({}, item, urlAdd).then(function (d) {
                    toaster.pop('success', '', d.data);

                    if (!contains(cart, item))
                        cart.push(item);

                }).catch(error).finally(function () {
                    item.isActive = true;
                });
                function error(d) {
                    notify(d);
                }

            };
            function update(item) {
                var dfd = $q.defer();

                http.postRequest({}, item, urlUpdate).then(function (d) {
                    dfd.resolve(d.data);
                }).catch(error);

                return dfd.promise;
                function error(d) {
                    dfd.reject(d);
                    notify(d);
                }

            }
            function getDetails(id) {
                var dfd = $q.defer();
                http.getByCache({ id: id }, urlGetTypes)
                    .then(function (d) {
                        dfd.resolve(d.data);
                    }).catch(error);
                return dfd.promise;
                function error(d) {
                    dfd.reject(d);
                    notify(d);
                }
            }
            function remove(id) {
                var dfd = $q.defer();

                http.postRequest({ id: id }, {}, urlRemove).then(function (d) {
                    dfd.resolve(d.data);
                }).catch(error);

                return dfd.promise;
                function error(d) {
                    dfd.reject(d);
                    notify(d);
                }

            }

            function doOrderR() {
                var dfd = $q.defer();

                http.postRequest({}, {}, urlDoOrderReg).then(function (d) {
                    dfd.resolve(d.data);
                    cart.length = 0;
                    toaster.pop('success', '', d.data, 6000);
                }).catch(error);

                return dfd.promise;
                function error(d) {
                    dfd.reject(d);
                    notify(d);
                }
            }
            function doOrder(user) {
                var dfd = $q.defer();

                http.requestFormSimpleData(urlDoOrder, user, undefined).then(function (d) {
                    dfd.resolve(d.data);
                    cart.length = 0;
                    toaster.pop('success', '', d.data, 6000);
                }).catch(function (d) {
                    dfd.reject(d.data);
                    notify(d);
                });

                return dfd.promise;

            }
            function notify(d) {
                if (d.status == 500) {
                    toaster.pop('error', '',culture=='ru'?'Произошла ошибка!':'Occured error!', 3000);
                    return;
                }
                if (angular.isArray(d.data)) {
                    var e = angular.element('<div>').append(d.data.map(function(e) {
                        return angular.element("<div>").addClass("label-error").text(e)[0];
                    }));
                    toaster.pop('error', '', e.html(), 4000, "trustedHtml");
                    return;
                } 
                toaster.pop('error', '', d.data, 4000);
            }

        }
        //-------------------functions for provider
        function initUrl(pathToAdd, pathToCart, pathUpdate, pathGetTypes, pathRemove, pathDoOrderReg, pathDoOrder) {
            if (pathToAdd != null)
                urlAdd = pathToAdd;
            if (pathToCart != null || pathToCart === undefined)
                urlCart = pathToCart;
            if (pathUpdate != null)
                urlUpdate = pathUpdate;
            if (pathGetTypes != null)
                urlGetTypes = pathGetTypes;
            if (pathRemove != null)
                urlRemove = pathRemove;
            if (pathDoOrderReg != null)
                urlDoOrderReg = pathDoOrderReg;
            if (pathDoOrder != null)
                urlDoOrder = pathDoOrder;
        };
        //-------------------functions for additional
        function contains(arr, item) {
            return arr.filter(function (elem) {
                return elem.goodId == item.goodId &&
                    elem.sizeId == item.sizeId &&
                    elem.colorId == item.colorId;
            }).length > 0;
        }

    }
})();