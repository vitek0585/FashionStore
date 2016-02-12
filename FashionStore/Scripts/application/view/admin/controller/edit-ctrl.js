(function() {
    'use strict';
    angular.module("adminApp")
        .controller("editCtrl",editCtrl);
    editCtrl.$inject = ['$scope', 'adminHttpSvc', '$state', 'spinnerGlobalSvc','goodsInfo'];

    function editCtrl($scope, adminHttp, $state, spinner, goodsInfo) {
        var vm = this;
        vm.types = [];
        vm.select = {
            type: [],
            category: []
        }
        var blursArray = [];
        //for error form
        vm.blurs = function (e) {
            scope[e] = false;
            blursArray.push(e);
        }
        //rosolve in route
        vm.product = goodsInfo;



        //edit
        //function edit(id) {
        //    spinner.begin();
        //    adminHttp.fullInfo({ id: id }).then(function () {
                
        //    }).finally(function () {
        //        spinner.comleted();
        //    });
        //}
    }
})();