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

    $('.btn-remove-device').click(function () {
        var deviceId = $(this).data('device-id');
        var deviceName = $(this).data('device-name');

        $.ajax({
            type: 'DELETE',
            url: '/Devices/RemoveDevice',
            data:
            {
                id: deviceId,
                name: deviceName,
            },
            success: function (response) {
                let timerInterval;
                Swal.fire({
                    title: response.value,
                    icon: 'success',
                    timer: 3000,
                    didOpen: () => {
                        const timer = Swal.getPopup().querySelector("b");
                        timerInterval = setInterval(() => {
                            timer.textContent = `${Swal.getTimerLeft()}`;
                        }, 100);
                    },
                    willClose: () => {
                        clearInterval(timerInterval);
                        window.location.reload();
                    }
                });
            },
            error: function (xhr) {
                let timerInterval;
                Swal.fire({
                    title: xhr.responseText,
                    icon: 'error',
                    timer: 3000,
                    didOpen: () => {
                        const timer = Swal.getPopup().querySelector("b");
                        timerInterval = setInterval(() => {
                            timer.textContent = `${Swal.getTimerLeft()}`;
                        }, 100);
                    },
                    willClose: () => {
                        clearInterval(timerInterval);
                    }
                });
            }
        });
    });

    var $tooltipTriggerList = $('[data-bs-toggle="tooltip"]');

    $tooltipTriggerList.each(function () {
        var $tooltipTriggerEl = $(this);
        $tooltipTriggerEl.tooltip();
    });
});