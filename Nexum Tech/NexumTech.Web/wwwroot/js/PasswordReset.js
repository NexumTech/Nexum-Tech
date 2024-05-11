$(document).ready(function () {
    let password = $("#txtPassword");
    let strengthBar = $("#strength-bar");
    let strengthText = $(".strength-text");

    function setStrength(value) {
        strengthBar.width(value + "%");
    }

    function setColorAndText(color, text) {
        strengthBar.css("background-color", color);
        strengthText.html(text);
        strengthText.css("color", color);
    }

    function clearStrength() {
        strengthBar.width(0);
        strengthBar.css("background-color", "");
        strengthText.html("");
    }

    password.keyup(checkPasswordStrength);

    function checkPasswordStrength() {
        let strength = 0;

        if (password.val() === "") {
            clearStrength();
            return false;
        }

        if (password.val().match(/\s/)) {
            setColorAndText("red", "White space is not allowed");
            return false;
        }

        if (password.val().length < 8) {
            strength = 20;
            setColorAndText("red", "Too short");
        } else {

            let lowerCase = password.val().match(/[a-z]/);
            let upperCase = password.val().match(/[A-Z]/);
            let numbers = password.val().match(/[0-9]/);
            let specialCharacters = password.val().match(/[\!\~\@\&\#\$\%\^\&\*\(\)\{\}\?\-\_\+\=]/);

            if (lowerCase || upperCase || numbers || specialCharacters) {
                strength = 40;
                setColorAndText("red", "Weak");
            }

            if (
                (lowerCase && upperCase) || (lowerCase && numbers) || (lowerCase && specialCharacters) ||
                (upperCase && numbers) || (upperCase && specialCharacters) || (numbers && specialCharacters)
            ) {
                strength = 60;
                setColorAndText("orange", "Medium");
            }

            if ((lowerCase && upperCase && numbers) || (lowerCase && upperCase && specialCharacters) ||
                (lowerCase && numbers && specialCharacters) || (upperCase && numbers && specialCharacters)
            ) {
                strength = 80;
                setColorAndText("#088f08", "Strong");
            }

            if (lowerCase && upperCase && numbers && specialCharacters) {
                strength = 100;
                setColorAndText("green", "Very Strong");
            }
        }
        setStrength(strength);
    }

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
                title: response.value,
                icon: 'success',
                didClose: () => {
                    window.location.href = '/Login';
                },
            });
        },
        error: function (xhr, status, error) {
            let timerInterval;
            Swal.fire({
                title: xhr.responseText,
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