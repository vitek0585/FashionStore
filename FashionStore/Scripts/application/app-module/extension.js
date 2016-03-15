Array.prototype.remove = function (item, field) {

    var array = this;
    for (var i = 0; i < array.length; i++) {

        if (array[i][field] == item) {
            array.splice(i, 1);
            break;
        }
    }

};
Array.prototype.removeRef = function (item) {

    var array = this;
    for (var i = 0; i < array.length; i++) {

        if (array[i] === item) {
            array.splice(i, 1);
            break;
        }
    }

};
