$(document).ready(function () {
    setTimeout(function () {
        $("#sessionAlert").alert('close');
    }, 6000);

    $('#btnLogin').click(function (event) {
        if ($('#loginForm')[0].checkValidity()) {
            AuthenticateUser();
        } else {
            event.preventDefault();
            $('#loginForm').addClass("was-validated");
        }
    });

    $(document).ajaxStart(function () {
        $('#loadingSpinner').show();
    });

    $(document).ajaxStop(function () {
        $('#loadingSpinner').hide();
    });
});

function AuthenticateUser() {
    var email = $('#txtEmail').val();
    var password = $('#txtPassword').val();

    $.ajax({
        type: 'POST',
        url: '/Login/Login',
        data: {
            email: email,
            password: password,
        },
        success: function(response) {
            window.location.href = '/Home';
        },
        error: function (xhr, status, error) {
            let timerInterval;
            Swal.fire({
                title: 'Error',
                html: xhr.responseText,
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
};