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
});