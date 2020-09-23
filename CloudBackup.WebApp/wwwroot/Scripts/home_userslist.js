
var home_userslist = (function ($) {
    var pub = {};

    pub.init = function (organizationId) {

        loadFromData(organizationId);

       
        $('#modal-').on('hidden.bs.modal', function () {
            $('#dtDevices').DataTable().search('').draw();
        });
    }
    pub.ChangeActiveStatus = function (id) {
        bootbox.confirm({
            message: "Kullanıcı aktifliğini değiştirmek istediğinize emin misiniz ? " ,

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
                        url: "/Home/UserChangeStatus",
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
    function loadFromData(organizationId) {
        $('#dtDevices').DataTable({
            "language": {
                "url": "/datatableTurkish.json"
            },
            "fnDrawCallback": function (oSettings) {
                common.init();
            },
            "bServerSide": true,
            "sAjaxSource": "/Home/GetUsersDatatable",
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "organizationId", "value": organizationId });
            },
            "bProcessing": true,
            paging: true,
            "aoColumns": [
                { "sName": "UserName", "orderable": false },
                { "sName": "FullName", "orderable": false },
                { "sName": "Email", "orderable": false },
                { "sName": "ActiveStatus", "orderable": false },
                {
                    "sName": "Id",
                    "bSearchable": false,
                    "bSortable": false,
                    "render":
                        function (data, type, row) {

                            return ' <a class="btn btn-success ModalPopup" data-post-url="/Home/UsersNew?id=' + data + '"  data-height="320px"><i class="fa fa-edit"></i></a> ' +
                                '<a class="btn btn-danger" onclick="home_userslist.ChangeActiveStatus(\'' + data + '\')"><i class="fa fa-refresh"></i></a> ';

                        }
                }
            ]
        });
    }

    return pub;
}(jQuery));
