
var device_index = (function ($) {
    var pub = {};

    pub.init = function () {

        loadFromData();

       
        $('#modal-').on('hidden.bs.modal', function () {
            $('#dtDevices').DataTable().search('').draw();
        });
    }
    pub.DeleteDevice = function (id) {
        bootbox.confirm({
            message: "Cihazı Silmek İstediğinize Emin misiniz ? " ,

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
                        url: "/Device/DeleteDevice",
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
    function loadFromData() {
        $('#dtDevices').DataTable({
            "language": {
                "url": "/datatableTurkish.json"
            },
            "fnDrawCallback": function (oSettings) {
                common.init();
            },
            "bServerSide": true,
            "sAjaxSource": "/Device/GetDeviceDatatable",
            //"fnServerParams": function (aoData) {
            //    aoData.push({ "name": "formCorporationID", "value": $("#CorporationIDSelect").val() });
            //},
            "bProcessing": true,
            paging: true,
            "aoColumns": [
                { "sName": "DeviceName", "orderable": false },
                { "sName": "LastOperationTime", "orderable": false },
                { "sName": "CreatedDate", "orderable": false },
                { "sName": "Status", "orderable": false },
                {
                    "sName": "Id",
                    "bSearchable": false,
                    "bSortable": false,
                    "render":
                        function (data, type, row) {

                            return ' <a class="btn btn-success ModalPopup" data-post-url="/Device/New?id=' + data + '"  data-height="320px"><i class="fa fa-edit"></i></a> ' +
                                '<a class="btn btn-warning ModalPopup" data-post-url="/Device/Information?id=' + data + '" data-height="310px" > <i class="fa fa-eye"></i></a > ' +
                                '<a class="btn btn-warning" href = "/Device/JobList?Id=' + data + '" > <i class="fa fa-list-ol"></i></a > ' +
                                '<a class="btn btn-danger" onclick="device_index.DeleteDevice(\'' + data + '\')"><i class="fa fa-remove"></i></a> ';

                        }
                }
            ]
        });
    }

    return pub;
}(jQuery));
