
var device_newjob = (function ($) {
    var pub = {};

    pub.init = function () {
        $('#datepicker').datepicker({
            autoclose: true,
            format: 'dd-mm-yyyy',
            language: 'tr'
        })
        $('.timepicker').timepicker({
            showInputs: false,
            showMeridian: false,
        })
       
    }

    return pub;
}(jQuery));
