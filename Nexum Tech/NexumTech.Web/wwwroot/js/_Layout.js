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

    $.ajax({
        type: 'GET',
        url: '/Layout/GetCompanies',
        data: {},
        success: function (data) {
            var companiesList = $('#companiesList');
            companiesList.empty();

            data.forEach(function (company, index, array) {
                var listItem = `
                                <li>
                                    <a class="dropdown-item" href="#" data-name="${company.name}" data-logo="${company.base64Logo}" data-company-id="${company.id}">
                                        ${company.name}
                                    </a>
                                </li>
                               `;

                companiesList.append(listItem);

                if (index < array.length - 1) {
                    var divider = '<li><hr class="dropdown-divider"></li>';
                    companiesList.append(divider);
                }
            });

            $('#companiesList').on('click', 'a', function (event) {
                event.preventDefault();

                var companyId = $(this).data('company-id');
                var companyName = $(this).data('name');
                var companyLogo = $(this).data('logo');

                $('#dropdownCompanies').attr('data-company-id', companyId);
                $('#dropdownCompanies img').attr('src', companyLogo);
                $('#dropdownCompanies strong').text(companyName);
            });

            if (data.length > 0) {
                var firstCompany = data[0];

                $('#dropdownCompanies').data('company-id', firstCompany.id);
                $('#dropdownCompanies img').attr('src', firstCompany.base64Logo);
                $('#dropdownCompanies strong').text(firstCompany.name);
            } else {
                $('#dropdownCompanies img').addClass('d-none');
                $('#dropdownCompanies strong').text($('#lblDontHaveCompanies').text());
                $('#dropdownCompanies').attr('disabled', true);
            }
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