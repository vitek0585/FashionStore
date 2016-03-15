(function () {
    'use strict';
    angular.module('imgOnLoadDir', [])
    .directive('ngImgOnload', function () {

        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                scope.$watch(function() {
                     return attrs.ngImgOnload;
                }, function() {

                    var path = attrs.ngImgOnload;
                    var img = new Image();
                    img.src = path;

                    img.onload = function(evt) {
                        // scope.$apply(function() {
                        element.css({
                            'background': 'url(' + this.src + ') no-repeat center',
                            'background-size': 'cover',

                        }).addClass('anim-show');
                        //$(element).width='300px';
                   
                        //   });

                        //scope.$apply(function() {
                        //    scope.$eval(attrs.ngImageOnload, { $element: evt.target });
                        //});
                 
                        evt.preventDefault();

                    };
                });
            }
            
        };
    });
})();