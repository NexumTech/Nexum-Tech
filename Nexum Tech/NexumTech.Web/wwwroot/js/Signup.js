$(document).ready(function () {
    let currentPasswordStrength;
    let passwordInput = $("#txtPassword");
    let strengthBar = $("#strength-bar");
    let strengthText = $(".strength-text");

    function setStrength(value) {
        currentPasswordStrength = value;
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

    passwordInput.keyup(checkPasswordStrength);

    function checkPasswordStrength() {
        let strength = 0;

        if (passwordInput.val() === "") {
            clearStrength();
            return false;
        }

        if (passwordInput.val().match(/\s/)) {
            strength = 10;
            setColorAndText("red", $('#lblWhiteSpace').text());
            return false;
        }

        if (passwordInput.val().length < 8) {
            strength = 20;
            setColorAndText("red", $('#lblTooShort').text());
        } else {

            let lowerCase = passwordInput.val().match(/[a-z]/);
            let upperCase = passwordInput.val().match(/[A-Z]/);
            let numbers = passwordInput.val().match(/[0-9]/);
            let specialCharacters = passwordInput.val().match(/[\!\~\@\&\#\$\%\^\&\*\(\)\{\}\?\-\_\+\=]/);

            if (lowerCase || upperCase || numbers || specialCharacters) {
                strength = 40;
                setColorAndText("red", $('#lblWeak').text());
            }

            if (
                (lowerCase && upperCase) || (lowerCase && numbers) || (lowerCase && specialCharacters) ||
                (upperCase && numbers) || (upperCase && specialCharacters) || (numbers && specialCharacters)
            ) {
                strength = 60;
                setColorAndText("orange", $('#lblMedium').text());	
            }

            if ((lowerCase && upperCase && numbers) || (lowerCase && upperCase && specialCharacters) ||
                (lowerCase && numbers && specialCharacters) || (upperCase && numbers && specialCharacters)
            ) {
                strength = 80;
                setColorAndText("#088f08", $('#lblStrong').text());
            }

            if (lowerCase && upperCase && numbers && specialCharacters) {
                strength = 100;
                setColorAndText("green", $('#lblVeryStrong').text());
            }
        }
        setStrength(strength);
    }

    function CreateUser() {
        if (currentPasswordStrength < 60) {
            let timerInterval;
            Swal.fire({
                title: $('#lblPasswordStrength').text(),
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

            return;
        }

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
                    title: response,
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
    };

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

    $(document).keypress(function (e) {
        if (e.which == 13) {
            $('#btnSignup').click();
        }
    });
});
