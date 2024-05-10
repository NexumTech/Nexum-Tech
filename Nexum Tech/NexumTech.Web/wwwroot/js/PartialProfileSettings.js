$(document).ready(function () {
    $('#image-container').click(function () {
        $('#file-upload').click();
    });

    $('#file-upload').change(function (event) {
        var input = event.target;
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#profile-image').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    });

    $('#profileModal').on('hidden.bs.modal', function () {
        window.location.reload();
    });

    $('#btnEditUsername').click(function () {
        var username = $(this).parent().text().trim(); 
        var input = $("<form class='mt-4' novalidate id='profileForm'>" +
                        `<input type='text' id='txtUsername' class='form-control fs-6 p-12' placeholder='Username' value='${username}' required >` +
                        "<div class='invalid-feedback'>Digite um nome</div>" +
                       "</form>").val(username); 
        $(this).parent().replaceWith(input);
    })

    $('#btnSaveProfile').click(function (event) {
        if ($('#profileForm')[0] != null) {
            if ($('#profileForm')[0]?.checkValidity()) {
                UpdateProfile();
            } else {
                event.preventDefault();
                $('#profileForm').addClass("was-validated");
            }
        } else UpdateProfile();
    });

    function UpdateProfile() {
        var id = $('#txtId').val();
        var username = $('#txtUsername').val();
        var base64Photo = $('#profile-image').attr('src');

        $.ajax({
            type: 'PUT',
            url: '/Profile/UpdateProfile',
            data: {
                user: {
                    username: username,
                    id: id,
                },
                base64Photo: base64Photo,
            },
            success: function (response) {
                $("#profileModal").trigger('show.bs.modal');
                let timerInterval;
                Swal.fire({
                    title: 'Success',
                    html: response,
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
    }
});