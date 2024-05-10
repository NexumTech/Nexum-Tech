$(document).ready(function () {
    $('#btnSignup').click(function (event) {
        if ($('#signupForm')[0].checkValidity()) {
            CreateUser();
        } else {
            event.preventDefault();
            $('#signupForm').addClass("was-validated");
        }
    });

    $(document).ajaxStart(function () {
        $('#loadingSpinner').show();
    });

    $(document).ajaxStop(function () {
        $('#loadingSpinner').hide();
    });
});

function CreateUser() {
    var username = $('#txtUsername').val();
    var email = $('#txtEmail').val();
    var password = $('#txtPassword').val();

    $.ajax({
        type: 'POST',
        url: '/Signup/Signup',
        data: {
            username: username,
            email: email,
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
};