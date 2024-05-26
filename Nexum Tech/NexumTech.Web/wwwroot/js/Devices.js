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

    var $tooltipTriggerList = $('[data-bs-toggle="tooltip"]');

    $tooltipTriggerList.each(function () {
        var $tooltipTriggerEl = $(this);
        $tooltipTriggerEl.tooltip();
    });
});