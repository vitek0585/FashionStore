(function() {
    'use strict';
    var m = angular.module("globalApp");
    var arr = ['slick'];
    Array.prototype.push.apply(m.requires, arr);
})();