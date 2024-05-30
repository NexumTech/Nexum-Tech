$(document).ready(function () {
    var culture = GetCulture('Culture');

    $('#devicesTable').DataTable({
        language: {
            url: `../lib/datatables/i18n/default-${culture}.json`
        }
    });

    function GetCulture(name) {
        let cookieValue = null;
        if (document.cookie && document.cookie !== '') {
            const cookies = document.cookie.split(';');
            for (let i = 0; i < cookies.length; i++) {
                const cookie = cookies[i].trim();
                if (cookie.substring(0, name.length + 1) === (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }

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
                    title: response,
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
});