$(document).ready(function () {
    $(document).ajaxStart(function () {
        $('#loadingSpinner').show();
    });

    $(document).ajaxStop(function () {
        $('#loadingSpinner').hide();
    });
});