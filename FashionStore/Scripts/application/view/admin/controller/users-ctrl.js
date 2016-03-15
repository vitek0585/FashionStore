(function () {
    'use strict';
    angular.module("adminApp")
        .controller("usersCtrl", usersCtrl);

    usersCtrl.$inject = ['$scope', 'adminHttpSvc', 'spinnerGlobalSvc'];

    function usersCtrl($scope, adminHttp, spinner) {
        var vm = this;
        vm.users = {};
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
        vm.save = save;


        //at the push pagging
        function clickPage(page) {
            spinner.begin();
            adminHttp.getUsersByPage({ page: page })
            .then(function (d) {
                vm.users = d.users;
                vm.info.totalPages = parseInt(d.totalPagesCount);
            }).finally(function () {
                spinner.comleted();
            });
        }
        //at the push pagging
        function save(user) {
            var actual = {
                id: user.id,
                roles: user.roles.map(function (e) {
                    return e.name;
                })
            }
            console.log(actual);
            spinner.begin();
            adminHttp.updateUser(actual)
            .finally(function () {
                spinner.comleted();
            });
        }

        vm.clickPage(vm.info.currentPage);
    }

})();