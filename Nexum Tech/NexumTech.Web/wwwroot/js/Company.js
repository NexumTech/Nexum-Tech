$(document).ready(function () {
    $('#companyModal').on('show.bs.modal', function () {
        $.ajax({
            type: 'GET',
            url: '/Company/PartialCreateCompany',
            data: {},
            success: function (data) {
                $('#companyModalContent').html(data);
            },
        });
    });
});