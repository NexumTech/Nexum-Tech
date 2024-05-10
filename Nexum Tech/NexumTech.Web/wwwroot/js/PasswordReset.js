$(document).ready(function () {
    $('#btnChangePassword').click(function (event) {
        if ($('#passwordResetForm')[0].checkValidity()) {
            ChangePassword();
        } else {
            event.preventDefault();
            $('#passwordResetForm').addClass("was-validated");
        }
    });
});

function ChangePassword() {
    var password = $('#txtPassword').val();

    $.ajax({
        type: 'PUT',
        url: '/PasswordReset/UpdatePassword',
        data: {
            password: password,
        },
        success: function (response) {
            Swal.fire({
                title: 'Success',
                html: response,
                icon: 'success',
                didClose: () => {
                    window.location.href = '/Login';
                },
            });
        },
        error: function (xhr, status, error) {
            let timerInterval;
            Swal.fire({
                title: 'Error',
                html: xhr.responseText,
                icon: 'error',
                timer: 3000,
                showClass: {
                    popup: `
                        animate__animated
                        animate__fadeInUp
                        animate__faster
                    `
                },
                hideClass: {
                    popup: `
                        animate__animated
                        animate__fadeOutDown
                        animate__faster
                    `
                },
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
    });
}