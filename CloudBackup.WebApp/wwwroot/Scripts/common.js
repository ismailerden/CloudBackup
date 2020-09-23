
var common = (function ($) {
    var pub = {};

    pub.init = function () {


        $(".ModalPopup").click(function () {


            var iframeUrl = $(this).data("post-url");

            var dataHeight = $(this).data("height");


            $('#modal-').modal({ show: true });
            $('#iframeModalBox').attr('src', iframeUrl);
            $('#iframeModalBox').attr('height', dataHeight);

        });
    }
    pub.openModal = function (porturl , height ) {
        var iframeUrl = porturl;

        var dataHeight = height;


        $('#modal-').modal({ show: true });
        $('#iframeModalBox').attr('src', iframeUrl);
        $('#iframeModalBox').attr('height', dataHeight);

    }
    pub.GetUrlVars = function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }
    return pub;
}(jQuery));
