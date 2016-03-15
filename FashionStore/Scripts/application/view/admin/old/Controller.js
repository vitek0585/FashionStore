//var m = angular.module("adminApp", ["httpApp", "pagingApp", "httpRouteApp", "spinnerApp", "selectedApp", "notifyApp"]);

var m = angular.module("globalApp");
var arr = ["httpApp", "pagingApp", "httpRouteApp", "spinnerApp", "selectedApp", "notifyApp"];
Array.prototype.push.apply(m.requires, arr);

m.factory("slickService", ["$compile", "$timeout", function (compile, timeout) {
    var container = $(".container-slider");
    var init = {
        centerMode: true, //главная картинка переносится в центр и тогда по краям видно остальные картинки
        dots: true, //ставит точки для выбора фото
        // centerPadding: '60px', //отступы, если 0 то поместяться ровно сколько указано slidesToShow (по умолчанию 50px)
        //vertical: true,
        //slidesToScroll: 1,
        slidesToShow: 3,
        // cssEase: 'ease',
        focusOnSelect: true,
        //  variableWidth: true,//ширина берется по элементу (картинки будут разной ширины)
        asNavFor: '.slider-for',
        // adaptiveWidth: true,
        arrows: false
    };
    var timer;

    return {
        setup: function (scope) {

            timeout.cancel(timer);
            $('.slider').remove();
            container.addClass("hideShow");

            if (scope.currentItem.item.photos.length == 0)
                return;

            var elem = compile(
            "<div class='slider'>" +
                "<div ng-repeat='photo in currentItem.item.photos' style='display:inline-block'>" +
                    "<img ng-src='/GetPhoto/{{photo}}' alt='No image'/>" +
                    "<a href='#' >" +

                    "</a>" +
                "</div>" +
            "</div>")(scope);

            angular.element(container).append(elem);
            timer = timeout(function () {
                try {
                    $('.slider').slick(init);
                    container.removeClass("hideShow");
                } catch (e) { }
            }, 500);

        },
        removeSlick: function () {
            $('.slider').remove();
        }
    }
}]);
m.controller("ctrlGlobal", [
    "$scope", "slickService", "$timeout", "httpService", "notifyWindow", function (scope, slick, timeout, http, notifyWindow) {
        var factoryCurrent = function () {
            return angular.copy({ item: { photos: [] } });
        };
        scope.currentItem = factoryCurrent();
        scope.mode = {
            isEdit: true
        };
        $(".select-mode").change(function (e) {
            scope.mode.isEdit = this.checked;
            scope.currentItem = factoryCurrent();
            angular.element(document.querySelector("#editForm")).scope().editForm.$setPristine();
            clear();
            scope.$apply();
            slick.removeSlick();
        });

        scope.sliderSetup = function () {
            if (scope.mode.isEdit)
                slick.setup(scope);

        };
        scope.errorHandler = {
            message: ''
        };
        var clear = function () {
            scope.errorHandler.message = '';
        };
        var error = function (d) {
            clear();
            var newLine = '\n';
            if (angular.isDefined(d.data.ModelState)) {
                for (var key in d.data.ModelState) {
                    scope.errorHandler.message += d.data.ModelState[key][0] + newLine;
                }
            }
        };
        var errorNotify = function (d) {
            notifyWindow.notifyError(d.data, 'notifyWin');

        };
        //Select Item
        scope.selectItem = function (item) {

            if (scope.mode.isEdit) {
                scope.currentItem.item = item;
            }
            else {
                scope.currentItem.item = factoryCurrent();
            }
            scope.sliderSetup();
            clear();
        }
        //Remove items
        var event;
        var idItem;
        var removeImg = function () {
            http.postRequest(idItem, "/api/Photo/RemovePhoto").then(function (d) {
                notifyWindow.notifySuccess(d.data, 'notifyWin');
                $(event.target).closest("div").remove();
                var index = scope.currentItem.item.photos.indexOf(idItem);
                scope.currentItem.item.photos.splice(index, 1);
                slick.removeSlick();
                clear();
            }, errorNotify);
        };
        var removeGood = function () {

            http.postRequest(idItem, "/api/Good/RemoveGood").then(function (d) {
                notifyWindow.notifySuccess(d.data, 'notifyWin');
                $(event.target).closest("div").remove();
                scope.currentItem = factoryCurrent();
                slick.setup(scope);
                clear();
            }, errorNotify);
        };
        //modal
        var messages = {
            removeImg: 'Do you want remove image?',
            removeGood: 'Do you want remove products?',

        };
        var actions = {
            removeImg: removeImg,
            removeGood: removeGood
        };
        scope.modal = {
            message: '',
            isModal: false,
            action: function () { }
        };

        scope.showModal = function (id, m, e) {
            event = e;
            idItem = id;

            scope.modal.isModal = true;
            scope.modal.action = actions[m];
            scope.modal.message = messages[m];
        };
        scope.hideModal = function () {
            scope.modal.isModal = false;
        };
        //Edit and New create
        scope.save = function (item) {
            var imageUpload = document.querySelector("#image-upload");

            var good = {
                goodId: item.goodId.$viewValue,
                goodName: angular.isDefined(item.goodName.$viewValue) ? item.goodName.$viewValue : '',
                goodCount: item.goodCount.$viewValue,
                priceUsd: item.priceUsd.$viewValue
            };
            var url = scope.mode.isEdit ? "/api/Good/Update" : "/api/Good/Create";

            if (scope.mode.isEdit === true) {
                http.postRequest(good, url).then(function (d) {
                    notifyWindow.notifySuccess(d.data, 'notifyWin');

                    clear();
                }, error);
            } else {
                var headers = {
                    'Content-Type': undefined,
                };
                good.files = angular.element(imageUpload).scope().files;
                http.formRequest(url, good, headers).then(function (d) {
                    console.log(d.data);
                    notifyWindow.notifySuccess(d.data, 'notifyWin');
                    clear();
                }, error);
            }

        };


    }
]);
//paging goods
m.controller("ctrlPaging", ["$scope", "routeService", "httpService", "$timeout", "$q", function (s, route, http, timeout, q) {
    s.info = {
        currentPage: 1,
        totalPages: 0,
        css: 'btn btn-primary',
        cssActive: 'btn btn-primary active',
        rightPrev: '>>',
        leftPrev: '<<',
    };
    s.items = [];

    s.clickPage = function (page) {

        if (!angular.isDefined(page)) {
            page = s.info.currentPage;
        }
        
        route.route("/Admin/Index/" + page, false);
        var defered = q.defer();

        http.getRequest({ id: page }, "/api/Good/GetByPage").then(function (d) {
            timeout(function () {
                s.items.length = 0;
                s.items = JSON.parse(d.data);
                defered.resolve();

            }, 0, true);
        });
        return defered;
    };
    s.refreshElement = function (id) {
        var d = s.clickPage(s.info.currentPage);
        d.promise.then(function() {
            var item = s.items.find(function(element, index, array) {
                return element.goodId === id;
            });
            console.log(item);
            s.selectItem(item);
        });
    };

}
]);
//paging users
m.controller("ctrlPagingUsers", ["$scope", "routeService", "httpService", "$timeout", "notifyWindow", function (s, route, http, timeout, notifyWindow) {
    s.info = {
        currentPage: 1,
        totalPages: 0,
        css: 'btn btn-primary',
        cssActive: 'btn btn-primary active',
        rightPrev: '>>',
        leftPrev: '<<',
    };
    s.items = [];

    s.clickPage = function (page) {

        //route.route("/Admin/GetUsers/" + page, false);
        http.getRequest({ id: page }, "/Admin/GetUsers").then(function (d) {
            timeout(function () {
                s.info.totalPages = d.data.totalPages;
                s.items.length = 0;
                s.items = d.data.items;

            }, 0, true);

        });
    };

}
]);
//upload image
m.controller("ctrlUpload", ["$scope", "$timeout", "$q", function (scope, timeout, q) {
    'use strict';
    var upload = $("#upload");
    upload.change(function (e) {
        $.makeArray(this.files).forEach(function (item) {
            var imageType = /image.*/;

            if (item.type.match(imageType)) {
                var file = item;
                var reader = new FileReader();
                try {
                    reader.readAsDataURL(file);
                } catch (e) {
                    reader.abort();
                }
                reader.onloadend = function () {
                    var result = reader.result;
                    file.result = result;
                    file.upload = false;
                    scope.files.push(file);
                    scope.$apply();
                }
            }
        });

    });
    scope.selectFile = function () {
        upload.trigger("click");
    };
    scope.uploadAll = function () {
        var e = document.querySelectorAll(".button-upload:not([disabled])");

        timeout(function () {
            angular.element(e).triggerHandler('click');
        }, 0, true);
    };
    scope.clear = function () {

        scope.files.splice(0, scope.files.length);

    };
    scope.isShow = function () {
        return angular.isDefined(currentItem.item.goodId) || !mode.isEdit;
    }
    scope.files = [];

}]);
//ctrlEditUser
m.controller("ctrlEditUser", ["$scope", "httpService", "notifyWindow",
     function (scope, http, notifyWindow) {
         'use strict';
         scope.user = {};
         var items;
         scope.currentUser = function (item, list) {
             scope.user = item;
             items = list;
         };
         scope.remove = function () {
             http.postRequest({ id: scope.user.id }, "/Admin/RemoveUser").then(function (d) {
                 notifyWindow.notifySuccess(d.statusText, 'notifyWin');

                 var rem = items.indexOf(scope.user);
                 items.splice(rem, 1);
             }, function (d) {
                 notifyWindow.notifyError(d.statusText, 'notifyWin');

             });
         };
         scope.save = function (id, value) {

             http.postRequest({ id: id, roles: value }, "/Admin/SetRoles").then(function (d) {
                 notifyWindow.notifySuccess(d.statusText, 'notifyWin');
             }, function (d) {
                 notifyWindow.notifyError(d.statusText, 'notifyWin');
             });
         };
         scope.modal = {
             isModal: false,

         };

         scope.hideModal = function () {
             scope.modal.isModal = false;
         };
         scope.showModal = function () {
             scope.modal.isModal = true;
         };



     }
]);








