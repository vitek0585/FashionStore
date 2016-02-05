(function () {
    'use strict';
    angular.module('globalApp')
        .controller('goodsDetailsCtrl', goodsDetailsCtrl);

    angular.module('globalApp').injectRequires(['selectBotstrApp']);

    goodsDetailsCtrl.$inject = ['$scope','cartSvc', '$filter', '$fancyModal', '$compile'];


    function goodsDetailsCtrl($scope,cart, filter, modal, $compile) {
   
        var VIEW_DET = 'det';
        var VIEW_DET_CORR = 'det-corr';
        var originItem = {};
        var idToRemove;
        var tmpData = {};
        var vm = this;
        vm.isWaiter = false;
        vm.details = [];
        vm.current = {};
        vm.view = VIEW_DET;

        vm.initCurrent = initCurrent;
        vm.getDetails = getDetails;
        vm.saveDetail = save;
        vm.cancel = cancel;
        vm.remove = remove;
        vm.openModal = open;
        vm.select = selectPicker;
        vm.isCorrectCount = isCorrectCount;

        vm.open = function () {
            //TODO this function is opening the modal window
        };
        //functions
        function initCurrent(item) {
            vm.current = clone(item);
            originItem = item;
        }

        function isCorrectCount(count) {
            return vm.current[count] > filter('filterBy')(vm.details, vm.current, count);
        }
        function getDetails(id) {
            vm.isWaiter = true;

            cart.getDetails(id).then(function (d) {
                vm.details = d;
                if (vm.details[0]) {

                    vm.view = VIEW_DET_CORR;
                    tmpData = clone(vm.current);
                }
            }).finally(dispose);
        }

        function save(size, color, clsId,count) {
            vm.isWaiter = true;

            cart.update(vm.current).then(function (d) {
                vm.current[size] = filter('filterBy')(vm.details, vm.current, size);
                vm.current[color] = filter('filterBy')(vm.details, vm.current, color);
                vm.current[clsId] = filter('filterBy')(vm.details, vm.current, clsId);
                vm.view = VIEW_DET;
                originItem[count] = vm.current[count];
            }).finally(dispose);
        }

        function selectPicker(item) {
            for (var i = 1; i < arguments.length; i++) {
                vm.current[arguments[i]] = item[arguments[i]];
            }

        }
        function cancel() {
            vm.view = VIEW_DET;
            vm.current = clone(tmpData);
        }

        function clone(item) {
            return JSON.parse(JSON.stringify(item));
        }
        function dispose(d) {
            vm.isWaiter = false;
        }

        function open(id) {
            idToRemove = id;
            vm.open();

        }
        function remove() {
          
            cart.remove(idToRemove).then(function (d) {
                cart.cart.remove(idToRemove, 'classificationId');
                modal.close();
            });
        }

    };



})();