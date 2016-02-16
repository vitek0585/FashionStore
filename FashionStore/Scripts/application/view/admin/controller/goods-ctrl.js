(function () {
    'use strict';

    angular.module("adminApp").
        controller('goodsCtrl', goodsCtrl);
    goodsCtrl.$inject = ['$scope', 'adminHttpSvc', '$state', 'spinnerGlobalSvc'];

    function goodsCtrl($scope, adminHttp, $state, spinner) {
        var vm = this;
        vm.goods = [];
        //for edit goods
        vm.types = [];
        vm.colors = [];
        vm.sizes = [];

        vm.select = {
            type: [],
            category: []
        }
        vm.isEdit = false;
        vm.info = {
            refresh: function () { },
            currentPage: undefined,
            totalPages: 0,
            css: 'btn btn-pagging',
            cssActive: 'btn btn-primary active',
            rightPrev: '>>',
            leftPrev: '<<',
        };
        //functions
        vm.init = init;
        vm.remove = remove;
        vm.clickPage = clickPage;
        vm.stretch = stretch;
        //implement functions--------------------------------
        //at the start page
        function init(cat,col,siz) {
            vm.types = cat;
            vm.colors = col;
            vm.sizes = siz;

            refreshTableGoods();
        }
        //at the push pagging
        function clickPage(page) {
            spinner.begin();
            adminHttp.getGoodsByPage({ page: page, category: vm.select.category[0].categoryId })
            .then(function (d) {
                vm.goods = d;
                vm.info.totalPages = parseInt(vm.goods.totalPagesCount);
            }).finally(function () {
                spinner.comleted();
            });
        }
        //remove
        function remove(id) {
            spinner.begin();
            adminHttp.deleteGoods({ id: id }).finally(function () {
                spinner.comleted();
            });
        }
        //update the table when you select a category or type in the combo box
        function refreshTableGoods() {
            $scope.$watch(function () {
                if (angular.isDefined(vm.select.category[0])) {
                    return vm.select.category[0].categoryId;
                }
            }, function (o, n) {
                if (angular.isDefined(vm.select.category[0])) {
                    vm.info.currentPage = 1;
                    clickPage(vm.info.currentPage);
                }
            });
        }

        function stretch() {

            return document.getElementById('goods-edit').children[0]?'col-xs-8':'col-xs-12';
        }
    }

})();