// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.onload = function () {
    window.scrollTo(0, +localStorage.getItem('page_scroll'));


    document.addEventListener('scroll', function () {
        localStorage.setItem('page_scroll', window.pageYOffset);
    });
}
