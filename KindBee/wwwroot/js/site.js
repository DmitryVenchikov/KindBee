// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/*© Un Sstrennen,2020*/function getCookie(e, t = !1) { if (!e) return; let n = document.cookie.match(new RegExp("(?:^|; )" + e.replace(/([.$?*|{}()\[\]\\\/+^])/g, "\\$1") + "=([^;]*)")); if (n) { let e = decodeURIComponent(n[1]); if (t) try { return JSON.parse(e) } catch (e) { } return e } } function setCookie(e, t, n = { path: "/" }) { if (!e) return; (n = n || {}).expires instanceof Date && (n.expires = n.expires.toUTCString()), t instanceof Object && (t = JSON.stringify(t)); let o = encodeURIComponent(e) + "=" + encodeURIComponent(t); for (let e in n) { o += "; " + e; let t = n[e]; !0 !== t && (o += "=" + t) } document.cookie = o } function deleteCookie(e) { setCookie(e, null, { expires: new Date, path: "/" }) }
$.get("/Account/IsAuthorized")
    .done(function (data) {
        if (data) {
            $("#LoginButton").remove();
        }
        else {
            $("#Logout").remove();
        }
    }).fail(function (data) {
        //если что то не так - выводим сообщение об ошибке
        alert("error");
        alert(data);
    });

window.onload = function () {
    window.scrollTo(0, +localStorage.getItem('page_scroll'));


    document.addEventListener('scroll', function () {
        localStorage.setItem('page_scroll', window.pageYOffset);
    });

   





    //alert("норм");
    //alert(document.cookie);
    //setCookie('name', 'valuescvgbsdfgdf');
    //смотрим авторизован ли пользователь и убираем либо оставляем ему кнопку <Выйти>
    //отправляем запрос для подтверждения на сервер каждые 5 минут 


    //var nameFromCookie = getCookie('name');
    //alert(Object.keys(nameFromCookie)[0]);
    //alert(nameFromCookie.name);

    //nameFromCookie = getCookie('.AspNetCore.Cookies');
    //alert(Object.keys(nameFromCookie)[0]);
    //alert(nameFromCookie['.AspNetCore.Cookies']);

    //if (getCookie('.AspNetCore.Cookies') === null || getCookie('.AspNetCore.Cookies') === '') {
    //    alert("неавторизован");
    //}
}
//function getCookie() {
//    return document.cookie.split('; ').reduce((acc, item) => {
//        const [name, value] = item.split('=')
//        acc[name] = value
//        return acc
//    }, {})
//}
