var paging = angular.module("pagingApp", []);
//info = {
//    currentPage: 1,
//    totalPages: 150,
//    css: 'btn btn-primary',
//    cssActive: 'btn btn-primary active',
//    rightPrev: '..>',
//    leftPrev: '<..',
//}
paging.directive("pagingSetup", [function () {
    var visibility = {
        visible: { 'visibility': 'visible' },
        nonVisible: { 'visibility': 'collapse' }
    }
    var initialize = function (buttons) {
        for (var i = 0; i < 9; i++) {
            buttons.push({});
        };
    }
    return {
        template:
            "<a ng-repeat='item in buttons' " +
            "ng-class='item.css' " +
            "ng-click='click(item)' " +
            "ng-style='item.display'>{{item.value}}</>",

        link: function (scope, element, attributes) {
            var buttons = [];
            var info = scope.info;
            initialize(buttons);

            var paging = function () {

                //удаление данных с кнопок 
                var clearAttrDataFromButton = function () {
                    buttons.forEach(function (item) {
                        item.css = info.css;
                        item.currentPage = 0;
                        item.isRefresh = true;
                        item.isActive = false;
                    });
                }
                //регинирация кнопки
                var setupButton = function (elem, page) {

                    elem.currentPage = page;
                    elem.display = visibility.visible;
                    elem.value = page;

                }
                //генерация кнопок
                //генерация левой стороны
                var leftButtonGenerate = function (opjForButton) {

                    if (opjForButton.numberButton > 2) {
                        setupButton(buttons[opjForButton.currentItem++], 1);
                        setupButton(buttons[opjForButton.currentItem], opjForButton.numberButton - 1);
                        buttons[opjForButton.currentItem++].value = info.leftPrev;
                    }
                };
                //генерация средних 
                var middleButtonGenerate = function (opjForButton, current) {

                    for (; opjForButton.numberButton < current + 3 && opjForButton.numberButton <= info.totalPages;
                        opjForButton.currentItem++, opjForButton.numberButton++) {

                        if (opjForButton.numberButton == current) {
                            var e = buttons[opjForButton.currentItem];
                            e.css = info.cssActive;
                            e.isActive = true;
                            e.isRefresh = false;
                            if (opjForButton.currentItem - 1 >= 0)
                                buttons[opjForButton.currentItem - 1].isRefresh = false;
                            if (opjForButton.currentItem + 1 < buttons.length)
                                buttons[opjForButton.currentItem + 1].isRefresh = false;
                        }

                        setupButton(buttons[opjForButton.currentItem], opjForButton.numberButton);
                    }
                }
                //генерация правой стороны
                var rightButtonGenerate = function (opjForButton) {
                    if (opjForButton.numberButton + 2 <= info.totalPages) {
                        setupButton(buttons[opjForButton.currentItem], opjForButton.numberButton);
                        buttons[opjForButton.currentItem++].value = info.rightPrev;

                        setupButton(buttons[opjForButton.currentItem++], info.totalPages);

                    } else {

                        while (opjForButton.numberButton <= info.totalPages) {
                            setupButton(buttons[opjForButton.currentItem++], opjForButton.numberButton++);
                        }
                    }

                }
                //скрывает кнопки которые не используются
                var hideButtons = function (opjForButton) {
                    if (opjForButton.currentItem == 1) {
                        opjForButton.currentItem = 0;
                    }
                    while (opjForButton.currentItem < buttons.length) {
                        buttons[opjForButton.currentItem++].display = visibility.nonVisible;
                    }
                }
                var replaceActive = function (to) {
                    try {

                        var currentActive = buttons.filter(function (item) {
                            return item.isActive;
                        })[0];
                        currentActive.isActive = false;
                        currentActive.css = info.css;
                        to.isActive = true;
                        to.css = info.cssActive;

                    } catch (e) {
                        console.log(e);
                    }
                }
                //генерация кнопок
                this.calculatePaging = function (item) {
                    if (item) {

                        if (!item.isRefresh) {
                            replaceActive(item);
                            return;
                        }
                    }

                    var opjForButton = {
                        numberButton: info.currentPage - 2 > 3 ? info.currentPage - 2 : 1,
                        currentItem: 0
                    };

                    clearAttrDataFromButton();
                    leftButtonGenerate(opjForButton);
                    middleButtonGenerate(opjForButton, info.currentPage);
                    rightButtonGenerate(opjForButton);
                    hideButtons(opjForButton);
                }
            }
            var g = new paging();
            g.calculatePaging(null);

            scope.buttons = buttons;
            scope.click = function (item) {

                // console.log(item.currentPage, item.isRefresh);
                if (info.currentPage == item.currentPage)
                    return;

                info.currentPage = item.currentPage;
                scope.clickPage(info.currentPage);
                g.calculatePaging(item);

            }
            scope.$watch(function () {
                return info.totalPages;
            }, function () {
                g.calculatePaging();
            });

            info.refresh = g.calculatePaging;

        },
        restrict: "A",
        scope: {
            info: "=info",
            clickPage: "=clickPage"
        }
    }
}]);