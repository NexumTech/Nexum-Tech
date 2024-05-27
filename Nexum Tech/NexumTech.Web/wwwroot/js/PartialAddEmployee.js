$(document).ready(function () {
    $('#employeesModal').on('hidden.bs.modal', function () {
        window.location.reload();
    });

    $('#btnAddEmployee').click(function (event) {
        if ($('#employeesForm')[0].checkValidity()) {
            AddEmployee();
        } else {
            event.preventDefault();
            $('#employeesForm').addClass("was-validated");
        }
    });

    function AddEmployee() {
        $.ajax({
            type: 'POST',
            url: '/Employees/AddEmployee',
            data:
            {
                email: $('#txtEmail').val(),
                companyId: $('#companyId').val(),
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