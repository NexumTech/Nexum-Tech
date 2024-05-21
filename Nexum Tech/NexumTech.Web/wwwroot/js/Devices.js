$(document).ready(function () {
    $('#devicesTable').DataTable();

    $('#devicesModal').on('show.bs.modal', function () {
        $.ajax({
            type: 'GET',
            url: '/Devices/PartialCreateDevice',
            data: {},
            success: function (data) {
                $('#devicesModalContent').html(data);
            },
        });
    });
});