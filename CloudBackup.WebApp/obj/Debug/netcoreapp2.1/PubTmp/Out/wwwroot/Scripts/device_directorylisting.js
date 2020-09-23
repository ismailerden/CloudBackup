
var device_directorylisting = (function ($) {
    var pub = {};

    pub.init = function (htmlElement, deviceId, cloudId) {
        $('#btnChoose').click(function () {
            $.ajax({
                url: "/Device/ReturnDeviceDiretoryById",
                data: { id: $('#selectedDirectory').val() },
                success: function (data) {
                    parent.ChooseDirectory(data, htmlElement)
                }
            });

            parent.$('#modal-').modal('toggle');
        });

        $('.ClickRefresh').click(function () {
            bootbox.alert("Sayfa yenileme talebi oluşturulmuştur. Yeni liste güncellendiğinde bildirim gelecektir.");
        });

        $('#treeView').on('changed.jstree', function (e, data) {
            var i, j, r = [];
            for (i = 0, j = data.selected.length; i < j; i++) {
                r.push(data.instance.get_node(data.selected[i]).id);
            }
            $('#selectedDirectory').val(r.join(', '));
            //console.log(r.join(', '));
        }).jstree({
            'core': {
                'data': {
                    'url': function (node) {
                        return node.id === '#' ?
                            'GetRootDirectoryListing' :
                            'GetParentDirectoryListing';
                    },
                    'data': function (node) {
                        return { 'directoryId': node.id, deviceId: deviceId, cloudId : cloudId };
                    }
                }
            }
        });

    }

    return pub;
}(jQuery));
