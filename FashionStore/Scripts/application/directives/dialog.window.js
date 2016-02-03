(function () {
    'use strict';
    angular
        .module('globalApp')
        .directive('modalWindow', modalWindow);

    exampleController.$inject = ['$scope', '$fancyModal', '$compile'];

    function modalWindow() {
        var directive = {
            restrict: 'EA',

            scope: {
                msg: '=',
                open: '=',
                okTxt: '@',
                cancelTxt: '@',
                okHadler: '&',
                spinerWaiter: '='
            },
            link: linkFunc,
            controller: exampleController,
            controllerAs: 'vm',
            bindToController: true // because the scope is isolated
        };

        return directive;

        function linkFunc(scope, elem, attr, ctrl) {

        }


    }
    function exampleController($scope, modal, $compile) {
        var vm = this;
        vm.cancel = modal.close;
        var e = angular.element(template());
        $compile(e)($scope);
        vm.open = function () {

            modal.open({
                template: e,
                type: 'html'
            });
            e = angular.element(template());
            $compile(e)($scope);
        }
        
    }

    function template() {
        return "<div class='row row-nomargin dialog-modal'>" +
                "<div class='col-xs-12' ng-cloak>" +
                    "<div class='text-uppercase message-info'>{{vm.msg}}</div>" +
                    "<div style='margin-top: 80px;text-align: right;'>" +
                        "<button class='btn btn-primary btn-buy' style='margin-right: 10px;' ng-click='vm.okHadler()'>{{vm.okTxt}}</button>" +
                        "<a class='btn btn-primary btn-buy' ng-click='vm.cancel()'>{{vm.cancelTxt}}</a>" +
                    "</div>" +
                "</div>" +
            "<spin-wait show-when='vm.spinerWaiter' />" +
            "</div>";
    }


})();
