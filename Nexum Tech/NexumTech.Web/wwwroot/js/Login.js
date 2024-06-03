window.onload = function () {
    google.accounts.id.initialize({
        client_id: "95046561902-b8j207ue9o4g58fpvqdmr9ergd4a24mo.apps.googleusercontent.com",
        ux_mode: "popup",
        callback: AuthenticateUserWithGoogle,
    });

    const createFakeGoogleWrapper = () => {
        const $googleLoginWrapper = $("<div>").addClass("custom-google-button").hide();
        $("body").append($googleLoginWrapper);

        google.accounts.id.renderButton($googleLoginWrapper[0], {
            type: "icon",
            width: "200",
        });

        const $googleLoginWrapperButton =
            $googleLoginWrapper.find("div[role=button]");

        return {
            click: () => {
                $googleLoginWrapperButton.click();
            },
        };
    };

    const googleButtonWrapper = createFakeGoogleWrapper();

    window.handleGoogleLogin = () => {
        googleButtonWrapper.click();
    };
};

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

    $('#btnGoogleLogin').click(function (event) {
        handleGoogleLogin();
    });

    $('#btnForgotPassword').click(function (event) {
        event.preventDefault();
        var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-outline-success",
            },
            buttonsStyling: false,
        });
        swalWithBootstrapButtons.fire({
            title: $('#lblForgotPassword').text(),
            input: "text",
            inputAttributes: {
                autocapitalize: "off",
                title: "",
                placeholder: "E-mail"
            },
            confirmButtonText: $('#lblSendPasswordResetLink').text(),
            preConfirm: async (email) => {
                if (email == "" || !emailRegex.test(email)) 
                    Swal.showValidationMessage($('#lblInvalidEmail').text());      
                else
                    RequestChangePassword(email);
            },
            allowOutsideClick: () => !Swal.isLoading()
        });
    });

    $(document).ajaxStart(function () {
        $('#loadingSpinner').show();
    });

    $(document).ajaxStop(function () {
        $('#loadingSpinner').hide();
    });

    $('#txtPassword').keypress(function (event) {
        if (event.keyCode === 13) { 
            event.preventDefault();
            $('#btnLogin').click();
        }
    });

    $(document).keypress(function (e) {
        if (e.which == 13) {
            $('#btnLogin').click();
        }
    });
});

function RequestChangePassword(email) {
     $.ajax({
        type: 'POST',
        url: '/Login/RequestToChangePassword',
        data: {
            email: email,
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
        error: function (xhr, status, error) {
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
};

function AuthenticateUserWithGoogle(response) {
    const data = jwtDecode(response.credential);

    $.ajax({
        type: 'POST',
        url: '/Login/LoginWithGoogle',
        data: {
            email: data.email,
            password: '',
            username: data.name,
            photo: data.picture
        },
        success: function (response) {
            window.location.href = '/Home';
        },
        error: function (xhr, status, error) {
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
};

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
        success: function (response) {
            window.location.href = '/Home';
        },
        error: function (xhr, status, error) {
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
};