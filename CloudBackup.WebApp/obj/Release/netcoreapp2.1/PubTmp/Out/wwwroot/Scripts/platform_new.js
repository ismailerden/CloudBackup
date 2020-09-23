
var platform_new = (function ($) {
    var pub = {};

    pub.init = function (data, type) {


        $("#Type").change(function () {
            var value = $(this).val();
            if (value == "0") {
                $("#dvContent").empty();
            }
            if (value == "1") {
                $("#dvContent").load("/Platform/_GoogleDrive?id=" + data);
            }
        });
        $("#Type").change();
    }

    return pub;
}(jQuery));
