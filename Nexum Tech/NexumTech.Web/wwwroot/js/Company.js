$(document).ready(function () {
    $('#companyModal').on('show.bs.modal', function () {
        var hasCompany = $('#hasCompany').val();

        $.ajax({
            type: 'GET',
            url: '/Company/PartialCreateCompany',
            data: {},
            success: function (data) {
                $('#companyModalContent').html(data);

                if (hasCompany == 'true') {
                    $('#btnCreateCompany').html('Update');
                    $('#logo').prop('required', false);
                }
                else {
                    $('#btnCreateCompany').html('Create');
                    $('#logo').prop('required', true);
                }
            },
        });
    });

    $('#btnCloseCompany').click(function () {
        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-outline-success mx-2",
                denyButton: "btn btn-outline-danger mx-2"
            },
            buttonsStyling: false,
        });
        swalWithBootstrapButtons.fire({
            title: 'Are you sure?',
            text: "You will not be able to undo this action",
            showDenyButton: true,
            confirmButtonText: 'Yes',
            denyButtonText: 'Cancel',
        }).then((result) => {
            if (result.isConfirmed) {
                CloseCompany();
            }
        });
    });

    function CloseCompany() {
        $.ajax({
            type: 'DELETE',
            url: '/Company/CloseCompany',
            data: {},
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
                        window.location.reload();
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