(function() {
    'use strict';
    var m = angular.module("adminApp");
    m.directive("uploadImage", [
        "$q", "httpService", "$timeout", "$interval", function (q, http, timeout, interval) {

            var uploadAnim = function (pr) {
                pr.active = true;
                var i = 0;
                var start = interval(function () {
                    if (i == 60)
                        interval.cancel(start);
                    pr.width = { 'width': i + '%' };
                    i += 5;
                }, 250);
                return start;
            };
            return {
                link: function (scope, element, attr) {

                    scope.remove = function (index) {
                 
                        scope.files.splice(index, 1);

                    };
                
                    scope.upload = function (index, progress) {
                        if (progress.active)
                            return;

                        var inval = uploadAnim(progress);
                        var headers = { 'Content-Type': undefined };
                        http.fileRequest('/api/Photo/AddPhoto', scope.files[index],{ id: scope.currentItem.goodId }, headers)
                            .then(function () {
                                interval.cancel(inval);
                                progress.width = { 'width': '100%' };
                                scope.files[index].upload = true;

                                //обновление элемента
                                angular.element($("#controll-view-goods")).scope().refreshElement(scope.currentItem.goodId);
                            
                            }, function () {
                                interval.cancel(inval);
                                progress.width = { 'width': '0%' };

                            })
                            .then(function () {
                                timeout(function () {
                                    progress.active = false;
                                }, 2000);
                            });

                    };


                },
                restirect: "A",
                scope: {
                    files: "=files",
                    currentItem: "=currentItem"
              
                },
                templateUrl: "/Scripts/application/view/admin/template/FileTable.html"

            }
        }
    ]);

    m.filter("sizeFilter", ["$filter", function (filter) {

        return function (data) {

            var value = data / 1024;

            return filter("number")(value, 2) + " KB";
        }

    }]);
})();