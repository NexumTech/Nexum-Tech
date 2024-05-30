$(document).ready(function () {
    $('#devicesModal').on('hidden.bs.modal', function () {
        window.location.reload();
    });

    $('#btnCreateDevice').click(function (event) {
        if ($('#devicesForm')[0].checkValidity() && ValidateName()) {
            CreateDevice();
        } else {
            event.preventDefault();
            $('#devicesForm').addClass("was-validated");
        }
    });

    function ValidateName() {
        const input = $('#txtName').val();
        const pattern = /^[a-zA-Z0-9]+$/;

        if (pattern.test(input)) return true;
        else {
            let timerInterval;
            Swal.fire({
                title: $('#lblInvalidDeviceName').text(),
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

            return false;
        }     
    }

    function CreateDevice() {
        $.ajax({
            type: 'POST',
            url: '/Devices/CreateDevice',
            data:
            {
                name: $('#txtName').val(),
                companyId: $('#companyId').val(),
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