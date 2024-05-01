$(document).ready(function () {
    var currentTheme;
    function getCookie(cookieName) {
        var cookies = document.cookie.split(';');

        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i].trim();

            if (cookie.startsWith(cookieName + '=')) {
                return cookie.substring(cookieName.length + 1);
            }
        }

        return null;
    }

    function setCookie(cookieName, cookieValue) {
        var expirationDate = new Date();
        expirationDate.setDate(expirationDate.getFullYear() + 1);

        var cookie = cookieName + '=' + cookieValue + '; expires=' + expirationDate.toUTCString() + '; path=/';
        document.cookie = cookie;

        return cookieValue;
    }

    function changeTheme(theme) {
        $('[data-bs-theme]').each(function () {
            $(this).attr('data-bs-theme', theme);
        });
    }

    var cookieTheme = getCookie('CurrentTheme');

    if (cookieTheme != null) {
        currentTheme = cookieTheme;
    } else {
        var systemTheme = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
        currentTheme = setCookie('CurrentTheme', systemTheme);
    }

    if (currentTheme == 'dark') {
        $('#SwitchTheme').prop('checked', true);
        $('#SwitchTheme').siblings('label').find('i').removeClass('fa-regular fa-sun').addClass('fa-regular fa-moon');
    }
    else {
        $('#SwitchTheme').prop('checked', false);
        $('#SwitchTheme').siblings('label').find('i').removeClass('fa-regular fa-moon').addClass('fa-regular fa-sun');
    }

    $('#SwitchTheme').change(function () {
        if ($(this).prop('checked')) {
            setCookie('CurrentTheme', 'dark');
            changeTheme('dark');
            $(this).siblings('label').find('i').removeClass('fa-regular fa-sun').addClass('fa-regular fa-moon');
        } else {
            setCookie('CurrentTheme', 'light');
            changeTheme('light');
            $(this).siblings('label').find('i').removeClass('fa-regular fa-moon').addClass('fa-regular fa-sun');
        }
    });

    $('[data-bs-theme]').each(function () {
        $(this).attr('data-bs-theme', currentTheme);
    });
});