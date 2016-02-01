Array.prototype.remove = function (item, field) {

    var array = this;
    for (var i = 0; i < array.length; i++) {

        if (array[i][field] == item) {
            array.splice(i, 1);
            break;
        }
    }

};
(function() {
    'use strict';

    var global = angular.module("app", []);

    global.injectRequires = function(arr) {
        Array.prototype.push.apply(this.requires, arr);
    }
})();