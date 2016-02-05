angular.module("lazyLoadSimpleApp", ["httpApp"]).
    service("lazyLoadSvc", ["httpService", "$timeout", function (http, $timeout) {

        var loadingElem;
        var lazyRunDistance = 100;
        var page = 1;
        var url;
        var isLoaded = false;
        var list = [];
        var conditional = { isTrust: false };

        var loadEmployee = function () {
            if (!isLoaded) {

                loadingElem.fadeTo("slow", 1).css("display", "block");
                isLoaded = true;
                http.getRequest({ page: page }, url).then(function (d) {
                    page++;
                    d.data.forEach(function (e) {
                        list.push(e);
                    });

                }).finally(function () {
                    isLoaded = false;
                    loadingElem.fadeTo("slow", 0).css("display", "none");
                });
            }

        };
        this.setConditional = function (cond) {
            conditional = cond;
        }
        this.setupLazy = function (urlTo, loadingE, distance, cond) {
            loadingElem = $('#' + loadingE);
            if (angular.isDefined(distance))
                lazyRunDistance = distance;

            if (angular.isDefined(cond))
                conditional = cond;

            url = urlTo;

            loadEmployee();
            return list;
        };


        $(window).scroll(function () {
            if (!conditional.isTrust)
                return;

            var wHeight = $(window).height();
            var dHeight = $(document).height();

            var posScrl = $(document).scrollTop();
            var pos = Math.ceil(posScrl + wHeight);

            if (pos >= dHeight - lazyRunDistance) {
                loadEmployee();
            }

        });
    }]);

//#loading {
//    position: absolute;
//    bottom: -4em;
//    text-align: center;
//    width: 100%;
//    z-index: 1;
//}

//#loading > img {
//    height: 15%;
//    width: 15%;

//}
//#up {
//    position: fixed;
//    bottom: 20%;
//    right: 10%;
//    z-index: 1000;

//}
//#up > div {
//    padding: 2em;

//}
//<div id="loading" style="position: absolute">
//       <img src="~/Image/load.gif" alt="loading">
//   </div>
//   <div id="up">
//       <div class="btn btn-primary"><span class="glyphicon glyphicon-menu-up"></span></div>
//   </div>