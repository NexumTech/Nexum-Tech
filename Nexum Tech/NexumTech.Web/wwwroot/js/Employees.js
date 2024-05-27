$(document).ready(function () {
    $('#employeesTable').DataTable();

    $('#employeesModal').on('show.bs.modal', function () {
        $.ajax({
            type: 'GET',
            url: '/Employees/PartialAddEmployee',
            data: {},
            success: function (data) {
                $('#employeesModalContent').html(data);
            },
        });
    });

    $('.btn-remove-employee').click(function () {
        var employeeId = $(this).data('employee-id');

        $.ajax({
            type: 'DELETE',
            url: '/Employees/RemoveEmployee',
            data:
            {
                id: employeeId,
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
    });
});