Array.prototype.remove = function (item, field) {

    var array = this;
    for (var i = 0; i < array.length; i++) {

        if (array[i][field] == item) {
            array.splice(i, 1);
            break;
        }
    }

};

$(document).scroll(function (e) {
    $('nav').css({
        'left': -$(document).scrollLeft()
    });
});