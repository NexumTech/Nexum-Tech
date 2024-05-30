$(document).ready(function () {
    var culture = GetCulture('Culture');

    $('#employeesTable').DataTable({
        language: {
            url: `../lib/datatables/i18n/default-${culture}.json`
        }
    });

    function GetCulture(name) {
        let cookieValue = null;
        if (document.cookie && document.cookie !== '') {
            const cookies = document.cookie.split(';');
            for (let i = 0; i < cookies.length; i++) {
                const cookie = cookies[i].trim();
                if (cookie.substring(0, name.length + 1) === (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }

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