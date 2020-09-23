
var device_alljoblog = (function ($) {
    var pub = {};

    pub.init = function () {

        loadFromData();

    }
    function loadFromData() {
        $('#dtDevices').DataTable({
            "language": {
                "url": "/datatableTurkish.json"
            },
            "fnDrawCallback": function (oSettings) {
                common.init();
            },
            "bServerSide": true,
            "sAjaxSource": "/Device/GetLogDatatableWithDevicePlanId",
            "bProcessing": true,
            paging: true,
            "aoColumns": [
                { "sName": "ProcessTime", "orderable": false },
                { "sName": "DeviceName", "orderable": false },
                { "sName": "PlanName", "orderable": false },
                { "sName": "LogText", "orderable": false },
               
            ]
        });
    }

   

return pub;
}(jQuery));
