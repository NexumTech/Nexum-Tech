$(document).ready(function () {
    $('#deviceModal').on('hidden.bs.modal', function () {
        window.location.reload();
    });

    $('#btnCreateDevice').click(function (event) {
        if ($('#devicesForm')[0].checkValidity()) {
            CreateDevice();
        } else {
            event.preventDefault();
            $('#devicesForm').addClass("was-validated");
        }
    });

    function CreateDevice() {
        $.ajax({
            type: 'POST',
            url: '/Devices/CreateDevice',
            data:
            {
                name: $('#txtName').val(),
                description: $('#txtDescription').val(),
                companyId: $('#companyId').val(),
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
    }
});