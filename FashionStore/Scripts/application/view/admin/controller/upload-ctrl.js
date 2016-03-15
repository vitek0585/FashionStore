(function () {
    'use strict';
    angular.module('adminApp')
        .controller("ctrlUpload", ["$scope", "$timeout", "$q", function (scope, timeout, q) {

            var upload = $("#uploadImage");
            //if last file == before file then upload.change not work
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
            scope.selectFile = function (e) {

                $("#uploadImage").trigger('click');

            };
            scope.uploadAll = function () {
                var e = document.querySelectorAll(".button-upload:not([disabled])");

                timeout(function () {
                    for(var i=0;i<e.length;i++)
                    angular.element(e[i]).triggerHandler('click');
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
})();