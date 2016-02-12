(function () {
    'use strict';
    angular.module("adminApp")
        .controller('logCtrl', logCtrl);

    logCtrl.$inject = ['adminHttpSvc', 'spinnerGlobalSvc', '$uibModal'];


    function logCtrl(adminHttp, spinner, modal) {
        var vm = this;
        var collapse = [];
        vm.logs = [];
        vm.info = {
            refresh: function () { },
            currentPage: 1,
            totalPages: 0,
            css: 'btn btn-pagging',
            cssActive: 'btn btn-primary active',
            rightPrev: '>>',
            leftPrev: '<<',
        };
        vm.clickPage = clickPage;
        vm.openR = openR;
        vm.addToCollapse = addToCollapse;
        vm.closeAllCollapse = closeAllCollapse;
        //setup all collapse
        function addToCollapse(coll) {
            collapse.push(coll);
        }
        //at the push pagging
        function clickPage(page) {
            spinner.begin();
            adminHttp.getLogsByPage({ page: page })
            .then(function (d) {
                vm.closeAllCollapse();
                vm.logs = d;
                vm.info.totalPages = parseInt(vm.logs.totalPagesCount);

            }).finally(function () {
                spinner.comleted();
            });
        }
        //closing all collapse
        function closeAllCollapse() {
            collapse.forEach(function (e, i) {
                for (var key in e) {
                    if (e.hasOwnProperty(key)) {
                        e[key] = true;
                    }
                }
            });
        }
        //dialog modal to remove log by id
        function openR(log) {

            var modalInstance = modal.open({
                animation: true,
                templateUrl: 'modalContent.html',
                controller: ['$scope', '$uibModalInstance', 'param', function (scope, modal, param) {
                    var id = param.logId;
                    var date = param.date;

                    var vm = this;
                    vm.title = "Remove log";
                    vm.body = "Do you want to delete log by id - " + id + " from at the date - " + date;
                    vm.ok = function () {
                        modal.close(id);
                    };
                    vm.cancel = function () {
                        modal.dismiss();
                    };
                }],
                controllerAs: 'modalVm',
                //size: size,
                resolve: {
                    param: function () {
                        return log;
                    }
                }
            });

            modalInstance.result.then(function (id) {
                adminHttp.deleteLog({ id: id }).then(function () {
                    vm.logs.items.remove(id, 'logId');
                });
            });

        }
        clickPage(vm.info.currentPage);
    }


})();