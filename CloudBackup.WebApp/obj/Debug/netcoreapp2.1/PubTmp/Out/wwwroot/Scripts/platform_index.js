
var platform_index = (function ($) {
    var pub = {};

    pub.init = function () {

        loadFromData();

       
        $('#modal-').on('hidden.bs.modal', function () {
            $('#dtDevices').DataTable().search('').draw();
        });
    }
    pub.DeleteDevice = function (id) {
        bootbox.confirm({
            message: "Planı Silmek İstediğinize Emin misiniz ? " ,

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
                        url: "/Platform/DeletePlatform",
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
            "sAjaxSource": "/Platform/GetPlatformDatatable",
            "bProcessing": true,
            paging: true,
            "aoColumns": [
                { "sName": "PlanName", "orderable": false },
                { "sName": "PlanType", "orderable": false },
                {
                    "sName": "Id",
                    "bSearchable": false,
                    "bSortable": false,
                    "render":
                        function (data, type, row) {

                            return ' <a class="btn btn-success ModalPopup" data-post-url="/Platform/New?id=' + data + '"  data-height="420px"><i class="fa fa-edit"></i></a> ' +
                                '<a class="btn btn-danger" onclick="platform_index.DeleteDevice(\'' + data + '\')"><i class="fa fa-remove"></i></a> ';

                        }
                }
            ]
        });
    }

    return pub;
}(jQuery));
