
var device_devicejoblog = (function ($) {
    var pub = {};

    pub.init = function (devicePlanId) {
        loadFromData(devicePlanId);
    }
    function loadFromData(devicePlanId) {
        $('#dtDevices').DataTable({
            "language": {
                "url": "/datatableTurkish.json"
            },
            "fnDrawCallback": function (oSettings) {
                common.init();
            },
            "bServerSide": true,
            "sAjaxSource": "/Device/GetLogDatatableWithDevicePlanId",
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "devicePlanId", "value": devicePlanId });
            },
            "bProcessing": true,
            paging: true,
            "aoColumns": [
                { "sName": "ProccessTime", "orderable": false },
                { "sName": "LogText", "orderable": false }
            ]
        });
    }


return pub;
}(jQuery));
