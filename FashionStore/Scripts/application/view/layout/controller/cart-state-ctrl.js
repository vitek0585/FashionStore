(function() {
    'use strict';
    angular.module("globalApp")
        .controller("CartStateController", cartStateController);

    cartStateController.$inject = ["cartSvc"];

    function cartStateController(cart) {
        var vm = this;
        cart.getCart().then(getCart);


        function getCart(data) {
            vm.cart = data;
        }
    }
})();