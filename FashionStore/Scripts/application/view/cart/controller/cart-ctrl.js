
(function () {
    'use strict';

    var module = angular.module("globalApp");
    module.injectRequires(['angular.filter']);
    //config
    module.config(cartUrlInit);
    cartUrlInit.$inject = ["cartSvcProvider"];
    //controllers
    module.controller("exclusiveCtrl", actionExclusiveWidget);
    module.controller("cartController", cartController);
    module.controller("userMenuController", userMenuController);
    module.controller("orderCalculateController", orderCalculateController);

    actionExclusiveWidget.$inject = ["$scope", 'httpService'];
    cartController.$inject = ["$scope", "cartSvc"];

    userMenuController.$inject = ["$scope", "httpService"];
    orderCalculateController.$inject = ["$scope", "httpService"];

    //Config
    function cartUrlInit(cartSvcProvider) {
        cartSvcProvider.initUrl(null, undefined, null, null, null);
    }
    //Controllers
    function actionExclusiveWidget(scope, http) {
        scope.exclusiveGoods = [];
        scope.isWaiter = true;
        http.getRequest({ count: 10 }, "/api/Good/RandomGood").then(function (d) {
            scope.exclusiveGoods = d.data;
            scope.isWaiter = false;
        });
    }
    function cartController($scope, cart) {
        var vm = this;
        var isAuth = false;
        vm.isBusy = false;
        vm.initModel = initModel;
        vm.auth = auth;
        vm.doOrder = doOrder;
        vm.items = [];

        function initModel(model) {
            vm.items = model;
            cart.cart = vm.items;
        }

        function auth(authUser) {
            isAuth = authUser;
        }

        function doOrder() {
            
            if (isAuth) {
                vm.isBusy = true;
                cart.doOrderR().finally(function() {
                    vm.isBusy = false;
                    vm.items.length = 0;
                });
            } else {
                angular.element(document.getElementById('login')).scope().openModal('loginTmpl', 'loginCtrl');
            }
        }

    }
    function userMenuController(scope, http) {

        scope.view = {
            current: 'cart',
            history: [],
            userInfo: {}
        };

        var selectView = function (e) {

            var elem = $(e.target);
            var data = elem.data("view");

            scope.view.current = data;
            scope.$apply();

        };
        scope.getHistory = function (name) {

            http.requestOneDataApi("/api/Sale/GetHistory", name).
                then(function (d) {
                    scope.view.history = JSON.parse(d.data);

                });
        };
        scope.getUserInfo = function (name) {

            http.requestFormSimpleData("/Account/UserInfo", { name: name }).
                then(function (d) {
                    scope.view.userInfo = d.data;
                });
        };
        $(".menu-user").on("click", "a", selectView);
    }
    function orderCalculateController(scope, root) {
        scope.obj = {
            count: 0,
            price: 0
        };

        scope.$on("calculate", function () {
            console.log("calc");
            scope.items.totalAmount = scope.items.totalAmount + scope.obj.price * scope.obj.count;
        });
        scope.$watch("obj.count", function () {
            scope.items.totalAmount = 0;
            root.$broadcast("calculate");
        });
    }

})();