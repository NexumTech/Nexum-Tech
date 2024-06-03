$(document).ready(function () {
    $('#companyModal').on('hidden.bs.modal', function () {
        window.location.reload();
    });

    $('#logo').change(function (event) {
        const file = event.target.files[0];
        const maxSizeInMB = 4;
        const maxSizeInBytes = maxSizeInMB * 1024 * 1024;

        if (file && file.size > maxSizeInBytes) {
            let timerInterval;
            Swal.fire({
                title: $('#lblImageSize').text(),
                icon: 'warning',
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
            
            $(this).val('');
        }
    });

    $('#btnCreateCompany').click(function (event) {
        if ($('#companyForm')[0].checkValidity()) {
            var hasCompany = $('#hasCompany').val();

            if (hasCompany == 'true') UpdateCompany();
            else CreateCompany();
        } else {
            event.preventDefault();
            $('#companyForm').addClass("was-validated");
        }
    });

    $(document).keypress(function (e) {
        if (e.which == 13) {
            $('#btnCreateCompany').click();
        }
    });

    function GetBase64StringFromFile(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onload = function (e) {
                const base64String = e.target.result;
                resolve(base64String);
            };
            reader.onerror = function (e) {
                reject(e);
            };
            reader.readAsDataURL(file);
        });
    }

    async function UpdateCompany() {
        let $fileInput = $('#logo');
        let file = $fileInput[0].files[0];
        let base64Logo;

        var formData = new FormData();
        formData.append('name', $('#txtName').val());
        formData.append('description', $('#txtDescription').val());

        if (file) {
            base64Logo = await GetBase64StringFromFile(file);
            formData.append('base64Logo', base64Logo);
        }

        $.ajax({
            type: 'PUT',
            url: '/Company/UpdateCompany',
            data: formData,
            processData: false,
            contentType: false,
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
                        $('#companyModal').modal('hide');
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

    async function CreateCompany() {
        let $fileInput = $('#logo');
        let file = $fileInput[0].files[0];
        let base64Logo = await GetBase64StringFromFile(file);

        var formData = new FormData();
        formData.append('name', $('#txtName').val());
        formData.append('description', $('#txtDescription').val());
        formData.append('base64Logo', base64Logo);

        if ($('#txtName').val().trim() !== '' && $('#txtDescription').val().trim() !== '') {
            $.ajax({
                type: 'POST',
                url: '/Company/CreateCompany',
                data: formData,
                processData: false,
                contentType: false,
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
                            $('#companyModal').modal('hide');
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
    }
});