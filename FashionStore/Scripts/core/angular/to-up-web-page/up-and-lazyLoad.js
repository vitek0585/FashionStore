//lazyLoad.setupElement("/api/Good/GetByPage", "loading", "up", scope.items);
angular.module("lazyLoadApp", ["httpApp"]).
    service("lazyService", ["httpService", function (http) {

        var loadingElem;
        var upButton;
        var elemContainer;
        var page = 1;
        var isLazy;
        var url;
        var isLoaded = false;
        var lazyRun = 0;
        var loadEmployee = function () {

            if (!isLoaded) {
                
                loadingElem.fadeTo("slow", 1).css("display", "block");
                isLoaded = true;
                http.getRequest({ id: page }, url).then(function(d) {
                    page++;
                    Array.prototype.push.apply(elemContainer, JSON.parse(d.data));
                }, function() {

                }).finally(function() {
                    loadingElem.fadeTo("slow", 0).css("display", "none");
                    isLoaded = false;
                    elem.appendTo(".container-empl");//.end().filter("div[class^=col]").fadeTo("slow", 1);
                });
            }

        };
        this.setupElementLazy = function (urlTo, loading, upBut, container, run) {

            if (angular.isDefined(run))
                lazyRun = run;

            isLazy = true;
            loadingElem = $('#'+loading);
            upButton = $('#' + upBut);
            elemContainer = container;
            url = urlTo;
            loadEmployee();

            upButton.fadeOut("slow").click(function () {
                $("body").animate({ scrollTop: 0 }, "slow");
            });
            
        };
        this.setupElement = function (upBut) {
            isLazy = false;
            
            upButton = $('#' + upBut);

            upButton.fadeOut("slow");
            upButton.on("click", function () {
                //console.log("UP");
                $("body").animate({ scrollTop: 0 }, "slow");
            });

        };

        $(window).scroll(function () {
            var wHeight = $(window).height();
            var dHeight = $(document).height();

            var posScrl = $(document).scrollTop();
            var pos = Math.ceil(posScrl + wHeight);

            if (pos >= dHeight - lazyRun && isLazy) {
                loadEmployee();
            }
            if (pos > wHeight + 100) {
                $("#up").fadeIn("slow");
            } else {
                $("#up").fadeOut("slow");
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