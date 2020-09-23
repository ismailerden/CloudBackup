
var device_joblist = (function ($) {
    var pub = {};

    pub.init = function (deviceId) {
        loadFromData(deviceId);
        $('#modal-').on('hidden.bs.modal', function () {
            $('#dtDevices').DataTable().search('').draw();
        });
    }
    pub.DeleteJob = function (id) {
        bootbox.confirm({
            message: "Cihaz planını Silmek İstediğinize Emin misiniz ? ",

            buttons: {
                confirm: {
                    label: 'Sil',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Hayır',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result == true) {

                    $.ajax({
                        url: "/Device/DeleteDevicePlan",
                        data: { id: id },
                        success: function (data) {
                            if (data == true) {
                                $('#dtDevices').DataTable().search('').draw();
                                bootbox.alert({ message: "Silindi", size: 'small' });
                            }
                            else
                                bootbox.alert({ message: "Silme işlemi sırasında bir hata oluştu", size: 'small' });
                        },
                        error: function (error) {
                            bootbox.alert({ message: "Silme işlemi sırasında bir hata oluştu", size: 'small' });
                        }

                    });


                }
                else {

                }

            }
        });

    }

    function loadFromData(deviceId) {
        $('#dtDevices').DataTable({
            "language": {
                "url": "/datatableTurkish.json"
            },
            "fnDrawCallback": function (oSettings) {
                common.init();
            },
            "bServerSide": true,
            "sAjaxSource": "/Device/GetDeviceJobDatatable",
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "deviceId", "value": deviceId });
            },
            "bProcessing": true,
            paging: true,
            "aoColumns": [
                { "sName": "PlanName", "orderable": false },
                { "sName": "PlanDescription", "orderable": false },
                { "sName": "PlatformName", "orderable": false },
                { "sName": "RetryPlan", "orderable": false },
                { "sName": "LastProcessTime", "orderable": false },
                {
                    "sName": "Id",
                    "bSearchable": false,
                    "bSortable": false,
                    "render":
                        function (data, type, row) {

                            return '<a class="btn btn-success ModalPopup" data-post-url="/Device/NewJob?id=' + data + '&deviceId=' + deviceId + '"  data-height="610px"><i class="fa fa-edit"></i></a> ' +
                                '<a class="btn btn-warning" href="/Device/DeviceJobLog?id=' + data + '"><i class="fa fa-book"></i></a> ' +
                                '<a class="btn btn-danger" onclick="device_joblist.DeleteJob(\'' + data + '\')"><i class="fa fa-remove"></i></a> ';

                        }
                }
            ]
        });
    }


    return pub;
}(jQuery));
