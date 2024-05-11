$(document).ready(function () {
    $(document).ajaxStart(function () {
        $('#loadingSpinner').show();
    });

    $(document).ajaxStop(function () {
        $('#loadingSpinner').hide();
    });

    $.ajax({
        type: 'GET',
        url: '/Layout/GetUserInfo',
        data: {},
        success: function (data) {
            $('#profilePicture').attr('src', data.base64Photo);
        },
    });

    $('#profileModal').on('show.bs.modal', function () {
        $.ajax({
            type: 'GET',
            url: '/Profile/PartialProfileSettings',
            data: {},
            success: function (data) {
                $('#profileModalContent').html(data);
            },
        });
    });

    $('.language-icon').click(function () {
        var culture = $(this).data('culture');

        $.ajax({
            type: 'POST',
            url: '/Layout/SetLanguage',
            data: {
                culture: culture
            },
            success: function (response) {
                window.location.href = '/Home'
            },
        });
    });
});