
var home_organizationlist = (function ($) {
    var pub = {};

    pub.init = function () {

        loadFromData();

       
        $('#modal-').on('hidden.bs.modal', function () {
            $('#dtDevices').DataTable().search('').draw();
        });
    }
    pub.ChangeActiveStatus = function (id) {
        bootbox.confirm({
            message: "Kurum aktifliğini değiştirmek istediğinize emin misiniz ? " ,

            buttons: {
                confirm: {
                    label: 'Değiştir',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Hayır',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result == "1") {

                    $.ajax({
                        url: "/Home/OrganizationChangeStatus",
                        data: { id: id },
                        success: function (data) {
                            if (data == "1") {
                                $('#dtDevices').DataTable().search('').draw();
                                bootbox.alert({ message: "Değiştirildi", size: 'small' });
                            }
                            else if (data == "-1") {
                                window.location.reload();
                            }
                            else { bootbox.alert({ message: "Değiştirme işlemi sırasında bir hata oluştu", size: 'small' });}
                        },
                        error: function (error) {
                            bootbox.alert({ message: "Değiştirme işlemi sırasında bir hata oluştu", size: 'small' });
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
            "sAjaxSource": "/Home/GetOrganizationDatatable",
            //"fnServerParams": function (aoData) {
            //    aoData.push({ "name": "formCorporationID", "value": $("#CorporationIDSelect").val() });
            //},
            "bProcessing": true,
            paging: true,
            "aoColumns": [
                { "sName": "Name", "orderable": false },
                { "sName": "PersonName", "orderable": false },
                { "sName": "PersonEmail", "orderable": false },
                { "sName": "AddressBinding", "orderable": false },
                { "sName": "ActiveStatus", "orderable": false },
                {
                    "sName": "Id",
                    "bSearchable": false,
                    "bSortable": false,
                    "render":
                        function (data, type, row) {

                            return ' <a class="btn btn-success ModalPopup" data-post-url="/Home/OrganizationNew?id=' + data + '"  data-height="320px"><i class="fa fa-edit"></i></a> ' +
                                ' <a class="btn btn-success" href="/Home/UsersList?organizationId=' + data + '"  data-height="320px"><i class="fa fa-users"></i></a> ' +
                                '<a class="btn btn-danger" onclick="home_organizationlist.ChangeActiveStatus(\'' + data + '\')"><i class="fa fa-refresh"></i></a> ';

                        }
                }
            ]
        });
    }

    return pub;
}(jQuery));
