$(document).ready(function () {
    $(document).ajaxStart(function () {
        $('#loadingSpinner').show();
    });

    $(document).ajaxStop(function () {
        $('#loadingSpinner').hide();
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
    })

    $.ajax({
        type: 'GET',
        url: '/Layout/GetUserInfo',
        data: {},
        success: function (data) {
            $("#lblUsername").text(data.username);
        },
    });
});