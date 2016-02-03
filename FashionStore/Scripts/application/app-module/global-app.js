Array.prototype.remove = function (item, field) {

    var array = this;
    for (var i = 0; i < array.length; i++) {

        if (array[i][field] == item) {
            array.splice(i, 1);
            break;
        }
    }

};
(function () {
    'use strict';

    var global = angular.module("globalApp", ["lazyLoadApp", "vesparny.fancyModal", 'ngAnimate', 'ngTooltips',
    'toaster', "confirmApp", "spinnerApp", "serializeApp", "httpApp", "cartApp", "slick"]);
  
    global.injectRequires = function (arr) {
        Array.prototype.push.apply(this.requires, arr);
    }
    global.value("culture", {
        symbol: ""
    });
    global.value("kindsRate", {
        ukr: {
            en: "grn",
            ru: "грн"
        }
    });

    global.config(configCart);
    configCart.$inject = ["cartSvcProvider"];

    function configCart(cartSvcProvider) {
        cartSvcProvider.initUrl("/Cart/Add", "/Cart/GetCart", "/Cart/Update", "/Cart/Details", "Cart/Remove", "/Cart/DoOrderReg", "/Cart/DoOrder");
    };

    global.controller("globalCtrl", [
        "$scope", "culture", 'lazyService', function (scope, culture, lazyLoad) {

            lazyLoad.setupElement("up");
            scope.initSymbol = function (value) {
                culture.symbol = value;
            };
        }
    ]);
    global.controller('collapseCtrl', [
        "$scope", function (scope) {
            scope.collapse = {
                lang: true,
                curr: true
            };
            scope.collapseLang = function (e) {

                scope.collapse.lang = !scope.collapse.lang;
                e.stopPropagation();

            }
            scope.collapseCurr = function (e) {

                scope.collapse.curr = !scope.collapse.curr;
                e.stopPropagation();

            }
        }
    ]);
    global.filter("currencyExtend", [
        "$filter", "culture", function (filter, culture) {

            return function (data, num) {

                var res = filter("number")(data, num);
                return res + " " + culture.symbol;
            }
        }
    ]);

    global.filter("currencyExtendConvertToGrn", [
        "$filter", "culture", "kindsRate", function (filter, culture, rate) {

            return function (data, curs, num) {
                if (culture.symbol == rate.ukr.en || culture.symbol == rate.ukr.ru)
                    data = data * curs;

                var res = filter("number")(data, num);
                return res + " " + culture.symbol;
            }
        }
    ]);
})();