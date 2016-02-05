(function () {
    'use strict';
    var prod = angular.module("globalApp");
    var arr = ["httpRouteApp", "pagingApp", 'ui.bootstrap', 'angularRangeSlider', 'azSuggestBox'];
    prod.injectRequires(arr);
    //Array.prototype.push.apply(prod.requires, arr);
    //prod.config(configPagging);
    //configPagging.$inject = ['routeServiceProvider', '$locationProvider'];

    //function configPagging(routeServiceProvider) {
    //    routeServiceProvider.startRoute(true);

    //}
    prod.controller("prodCtrl", ["$scope", "cartSvc",
        function (scope, cart) {

            scope.cart = [];
            scope.colors = [];
            scope.sizes = [];
            scope.filter = {
                colorsSelect: [],
                sizesSelect: [],
                sortBySelect: {},
                priceMin: null,
                priceMax: null
            };

            scope.add = cart.add;


            scope.initFilterData = function (min, max, colors, sizes) {
                scope.colors = colors;
                scope.sizes = sizes;
                scope.filter.priceMin = min;
                scope.filter.priceMax = max;
            }

            scope.filterAccept = function () {

                scope.$broadcast("acceptFilterEvent",
                {
                    c: scope.filter.colorsSelect.map(function (e) { return e.id; }),
                    s: scope.filter.sizesSelect.map(function (e) { return e.id; }),
                    min: scope.filter.priceMin,
                    max: scope.filter.priceMax

                });
            }
            var currentSort = 'none';
            scope.$watch('filter.sortBySelect', function () {

                var sort = null;
                if (scope.filter.sortBySelect[0]) {
                    sort = scope.filter.sortBySelect[0].sort;
                }
                if (sort == null || currentSort == sort) {
                    return;
                }
                currentSort = sort;
                scope.$broadcast("filterSortByEvent",
                {
                    sortBy: sort
                });

            }, true);


        }
    ]);
    var actionExclusiveWidget = function (scope, http) {
        scope.exclusiveGoods = [];
        http.getRequest({ count: 10 }, "/api/Good/RandomGood").then(function (d) {
            scope.exclusiveGoods = d.data;
        });
    }
    prod.controller("exclusiveCtrl", ["$scope", 'httpService', actionExclusiveWidget]);
    //paging goods
    prod.controller("pageCtrl", ["$scope", "routeService", "httpService", '$location', '$window', '$rootScope',
        function (s, route, http, $location, $window, $rootScope) {

            var NO_CONTENT_STATUS_CODE= 204;

            s.items = [];
            //filter
            var colorsSelect = [];
            var sizesSelect = [];
            var sortBySelect = [];
            var price = {
                min: '',
                max: ''
            };
            //wait load
            s.isWait = false;
            //for paging
            s.info = {
                refresh: function () { },
                currentPage: undefined,
                totalPages: 0,
                css: 'btn btn-pagging',
                cssActive: 'btn btn-primary active',
                rightPrev: '>>',
                leftPrev: '<<',
            };

            s.init = function () {
                $rootScope.$on('$locationChangeStart', function (event, next, current) {
                    getGoods(getPageFromUrl());
                    s.info.currentPage = getPageFromUrl();
                    s.info.remoteClick(s.info.currentPage);
                });

            };
            s.clickPage = function (page) {
                $location.search('page', page);
            };

            //filters handler
            var refreshPage = function () {
                if (!s.isWait) {
                    s.info.currentPage = 1;
                    s.clickPage(s.info.currentPage);
                    s.$emit('$locationChangeStart');
                    //s.info.refresh();
                }
            };;
            s.$on("filterSortByEvent", function (event, args) {
                console.log("filterSortByEvent");
                sortBySelect = args.sortBy;
                refreshPage();
            });
            s.$on("acceptFilterEvent", function (event, args) {
                console.log("acceptFilterEvent");

                colorsSelect = args.c;
                sizesSelect = args.s;
                price.min = args.min;
                price.max = args.max;

                refreshPage();
            });
            //get items from a remote source
            function getGoods(page) {
                s.isWait = true;
                http.getRequest(
                {
                    page: page,
                    category: s.category,
                    colors: colorsSelect.toString(),
                    sizes: sizesSelect.toString(),
                    sort: sortBySelect,
                    priceMin: price.min,
                    priceMax: price.max

                }, "/api/Good/GetByPage").then(function (d) {
                    if (d.status == NO_CONTENT_STATUS_CODE) {
                        s.info.totalPages = 0;
                        s.items.length = 0;
                    } else {
                        s.items = d.data.items;
                        s.info.totalPages = d.data.totalPagesCount;
                    }
                    s.isWait = false;


                }, function () {

                    s.info.totalPages = 0;
                    s.items.length = 0;
                    s.isWait = false;

                });
            }
            //get current page from url
            function getPageFromUrl() {
                var page = 1;
                if ($location.search() && $location.search().page) {
                    page = $location.search().page;
                }
                return page;
            }
        }
    ]);
})();
