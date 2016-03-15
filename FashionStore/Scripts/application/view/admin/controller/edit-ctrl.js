(function () {
    'use strict';
    angular.module("adminApp")
        .controller("editCtrl", editCtrl);
    editCtrl.$inject = ['$scope', 'adminHttpSvc', '$state', 'spinnerGlobalSvc', 'goodsInfo', '$timeout', "formToObject"];

    function editCtrl($scope, adminHttp, $state, spinner, goodsInfo, $timeout, ser) {
        var vm = this;
        var uniqueId = -1;
        vm.types = [];
        vm.select = {
            type: [],
            category: []
        }
        //for update draw all elem that need it
        vm.refresh = false;
        var blursArray = [];
        //for error form
        vm.blurs = function (e) {
            scope[e] = false;
            blursArray.push(e);
        }
        //rosolve in route
        vm.product = goodsInfo;
        //update all elements (slick new image has added)
        $scope.$on("refresh", function () {
            vm.refresh = true;

            $timeout(function () {
                vm.refresh = false;
            });

        });

        vm.submit = submit;
        vm.newCls = newCls;
        vm.removeCls = removeCls;

        function submit(form) {
            spinner.begin();
            var res = convertFormToobject(form);
            adminHttp.updateGoods(res).then(function() {
                
            }).finally(function() {
                spinner.comleted();
            });
        }

        function newCls() {
            vm.product.clsn.push({
                classificationId: uniqueId--,
                colorId: -1,
                sizeId: -1,
                countGood: 0
            });
        }

        function removeCls(item) {

            vm.product.clsn.remove(item.classificationId, 'classificationId');

        }
        //edit
        function convertFormToobject(form) {
            var result = {
                goodId: form.goodId.$modelValue,
                goodNameEn: form.goodNameEn.$modelValue,
                goodNameRu: form.goodNameRu.$modelValue,
                priceUsd: form.priceUsd.$modelValue,
                discount: form.discount.$modelValue,
                categoryId: form.selectCat.$modelValue[0].categoryId,
                classificationGoods: []
            }
            for (var i = 0; ; i++) {

                if (angular.isDefined(form["classification" + i])) {
                    var cls = {
                        classificationId: form["classification" + i].$modelValue.classificationId,
                        countGood: form["classification" + i].$modelValue.countGood,
                        colorId: form["classification" + i].$modelValue.colors[0].colorId,
                        sizeId: form["classification" + i].$modelValue.sizes[0].sizeId,
                        goodId: result.goodId
                    }
                    result.classificationGoods.push(cls);
                } else {
                    break;
                }
            }
            return result;
        }
    }
})();